using System.Collections.Concurrent;
using System.Text.Json;
using AuthSvc.BLL.Interfaces;
using AuthSvc.BLL.Models;
using AuthSvc.BLL.Utils;
using AuthSvc.DAL.Enums;
using SharedLibrary.Commons;
using SharedLibrary.Services.Interfaces;
using SharedLibrary.Utils;

namespace AuthSvc.BLL.Implements
{
    public class VerificationCacheService(
        ICacheService _cacheService,
        AppConfiguration _config) : IVerificationCacheService
    {
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> KeyLocks = new();

        public async Task<bool> TryBeginCooldownAsync(
            VerificationPurposeEnum purpose,
            VerificationDeliveryMethodEnum deliveryMethod,
            string identifier,
            TimeSpan cooldown)
        {
            var key = BuildCooldownKey(purpose, deliveryMethod, identifier);

            await using var keyLock = await AcquireKeyLockAsync(key);
            if (await _cacheService.ExistsAsync(key))
            {
                return false;
            }

            await _cacheService.SetAsync(key, "1", cooldown);
            return true;
        }

        public async Task<string> CreateEmailLinkTokenAsync(
            VerificationPurposeEnum purpose,
            string identifier,
            int authAccountId,
            TimeSpan expiration)
        {
            var accountKey = BuildAccountLockKey(purpose, authAccountId);
            await using var accountLock = await AcquireKeyLockAsync(accountKey);

            await ClearVerificationArtifactsAsync(purpose, authAccountId);

            var token = StringUtil.GenerateSecureToken();
            var payload = new VerificationCachePayload
            {
                AuthAccountId = authAccountId,
                Identifier = identifier,
                ExpiresAtUtc = DateTimeOffset.UtcNow.Add(expiration)
            };

            var json = JsonSerializer.Serialize(payload, JsonOptions);
            await _cacheService.SetAsync(BuildEmailTokenKey(purpose, token), json, expiration);
            await _cacheService.SetAsync(BuildEmailActiveKey(purpose, authAccountId), token, expiration);

            return token;
        }

        public async Task<string> CreateSmsOtpAsync(
            VerificationPurposeEnum purpose,
            string identifier,
            int authAccountId,
            TimeSpan expiration)
        {
            var accountKey = BuildAccountLockKey(purpose, authAccountId);
            await using var accountLock = await AcquireKeyLockAsync(accountKey);

            await ClearVerificationArtifactsAsync(purpose, authAccountId);

            var otp = OtpUtil.GenerateOtp();
            var payload = new OtpVerificationCachePayload
            {
                AuthAccountId = authAccountId,
                Identifier = identifier,
                OtpHash = OtpUtil.GenerateOtpHash(otp, identifier, _config.JwtConfig.SecretKey),
                AttemptCount = 0,
                ExpiresAtUtc = DateTimeOffset.UtcNow.Add(expiration)
            };

            var json = JsonSerializer.Serialize(payload, JsonOptions);
            await _cacheService.SetAsync(BuildSmsOtpKey(purpose, identifier), json, expiration);
            await _cacheService.SetAsync(BuildSmsActiveKey(purpose, authAccountId), identifier, expiration);

            return otp;
        }

        public async Task<VerificationCachePayload?> ConsumeEmailLinkTokenAsync(
            VerificationPurposeEnum purpose,
            string token)
        {
            var tokenKey = BuildEmailTokenKey(purpose, token);
            var json = await _cacheService.GetAsync(tokenKey);
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            var payload = JsonSerializer.Deserialize<VerificationCachePayload>(json, JsonOptions);
            if (payload == null)
            {
                await _cacheService.DeleteAsync(tokenKey);
                return null;
            }

            var accountKey = BuildAccountLockKey(purpose, payload.AuthAccountId);
            await using var accountLock = await AcquireKeyLockAsync(accountKey);

            json = await _cacheService.GetAsync(tokenKey);
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            payload = JsonSerializer.Deserialize<VerificationCachePayload>(json, JsonOptions);
            if (payload == null || payload.ExpiresAtUtc <= DateTimeOffset.UtcNow)
            {
                await _cacheService.DeleteAsync(tokenKey);
                if (payload != null)
                {
                    await _cacheService.DeleteAsync(BuildEmailActiveKey(purpose, payload.AuthAccountId));
                }
                return null;
            }

            var activeKey = BuildEmailActiveKey(purpose, payload.AuthAccountId);
            var activeToken = await _cacheService.GetAsync(activeKey);
            if (!string.Equals(activeToken, token, StringComparison.Ordinal))
            {
                return null;
            }

            await _cacheService.DeleteAsync(tokenKey);
            await _cacheService.DeleteAsync(activeKey);
            return payload;
        }

        public async Task<VerificationCachePayload?> VerifySmsOtpAsync(
            VerificationPurposeEnum purpose,
            string identifier,
            string otp,
            int maxAttempts,
            bool consumeOnSuccess = true)
        {
            var key = BuildSmsOtpKey(purpose, identifier);
            var json = await _cacheService.GetAsync(key);
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            var payload = JsonSerializer.Deserialize<OtpVerificationCachePayload>(json, JsonOptions);
            if (payload == null)
            {
                await _cacheService.DeleteAsync(key);
                return null;
            }

            var accountKey = BuildAccountLockKey(purpose, payload.AuthAccountId);
            await using var accountLock = await AcquireKeyLockAsync(accountKey);

            json = await _cacheService.GetAsync(key);
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            payload = JsonSerializer.Deserialize<OtpVerificationCachePayload>(json, JsonOptions);
            if (payload == null || payload.ExpiresAtUtc <= DateTimeOffset.UtcNow)
            {
                await DeleteSmsOtpArtifactsAsync(purpose, payload?.AuthAccountId ?? 0, identifier);
                return null;
            }

            var activeSmsIdentifier = await _cacheService.GetAsync(BuildSmsActiveKey(purpose, payload.AuthAccountId));
            if (!string.Equals(activeSmsIdentifier, identifier, StringComparison.Ordinal))
            {
                return null;
            }

            if (!OtpUtil.VerifyOtp(otp, identifier, payload.OtpHash, _config.JwtConfig.SecretKey))
            {
                var nextAttempts = payload.AttemptCount + 1;
                if (nextAttempts >= maxAttempts)
                {
                    await DeleteSmsOtpArtifactsAsync(purpose, payload.AuthAccountId, identifier);
                    return null;
                }

                var remainingTtl = payload.ExpiresAtUtc - DateTimeOffset.UtcNow;
                if (remainingTtl <= TimeSpan.Zero)
                {
                    await DeleteSmsOtpArtifactsAsync(purpose, payload.AuthAccountId, identifier);
                    return null;
                }

                var updatedPayload = new OtpVerificationCachePayload
                {
                    AuthAccountId = payload.AuthAccountId,
                    Identifier = payload.Identifier,
                    OtpHash = payload.OtpHash,
                    AttemptCount = nextAttempts,
                    ExpiresAtUtc = payload.ExpiresAtUtc
                };

                await _cacheService.SetAsync(
                    key,
                    JsonSerializer.Serialize(updatedPayload, JsonOptions),
                    remainingTtl);
                await _cacheService.SetAsync(
                    BuildSmsActiveKey(purpose, payload.AuthAccountId),
                    identifier,
                    remainingTtl);

                return null;
            }

            if (consumeOnSuccess)
            {
                await DeleteSmsOtpArtifactsAsync(purpose, payload.AuthAccountId, identifier);
            }

            return new VerificationCachePayload
            {
                AuthAccountId = payload.AuthAccountId,
                Identifier = payload.Identifier,
                ExpiresAtUtc = payload.ExpiresAtUtc
            };
        }

        public async Task<string> CreatePasswordResetSessionTokenAsync(
            int authAccountId,
            string identifier,
            TimeSpan expiration)
        {
            var token = StringUtil.GenerateSecureToken();
            var payload = new VerificationCachePayload
            {
                AuthAccountId = authAccountId,
                Identifier = identifier,
                ExpiresAtUtc = DateTimeOffset.UtcNow.Add(expiration)
            };

            await _cacheService.SetAsync(
                BuildPasswordResetSessionKey(token),
                JsonSerializer.Serialize(payload, JsonOptions),
                expiration);

            return token;
        }

        public async Task<VerificationCachePayload?> ConsumePasswordResetSessionTokenAsync(string token)
        {
            var sessionKey = BuildPasswordResetSessionKey(token);

            await using var keyLock = await AcquireKeyLockAsync(sessionKey);
            var json = await _cacheService.GetAsync(sessionKey);
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }

            var payload = JsonSerializer.Deserialize<VerificationCachePayload>(json, JsonOptions);
            if (payload == null || payload.ExpiresAtUtc <= DateTimeOffset.UtcNow)
            {
                await _cacheService.DeleteAsync(sessionKey);
                return null;
            }

            await _cacheService.DeleteAsync(sessionKey);
            return payload;
        }

        private static string BuildCooldownKey(
            VerificationPurposeEnum purpose,
            VerificationDeliveryMethodEnum deliveryMethod,
            string identifier)
        {
            return $"verification:{purpose}:cooldown:{deliveryMethod}:{identifier}";
        }

        private static string BuildEmailActiveKey(VerificationPurposeEnum purpose, int authAccountId)
        {
            return $"verification:{purpose}:email:active:{authAccountId}";
        }

        private static string BuildEmailTokenKey(VerificationPurposeEnum purpose, string token)
        {
            return $"verification:{purpose}:email:token:{token}";
        }

        private static string BuildSmsOtpKey(VerificationPurposeEnum purpose, string identifier)
        {
            return $"verification:{purpose}:sms:otp:{identifier}";
        }

        private static string BuildSmsActiveKey(VerificationPurposeEnum purpose, int authAccountId)
        {
            return $"verification:{purpose}:sms:active:{authAccountId}";
        }

        private static string BuildAccountLockKey(VerificationPurposeEnum purpose, int authAccountId)
        {
            return $"verification:{purpose}:account:lock:{authAccountId}";
        }

        private static string BuildPasswordResetSessionKey(string token)
        {
            return $"verification:password-reset:verified:{token}";
        }

        private static async Task<IAsyncDisposable> AcquireKeyLockAsync(string key)
        {
            var semaphore = KeyLocks.GetOrAdd(key, static _ => new SemaphoreSlim(1, 1));
            await semaphore.WaitAsync();
            return new Releaser(semaphore);
        }

        private async Task ClearVerificationArtifactsAsync(
            VerificationPurposeEnum purpose,
            int authAccountId)
        {
            var activeEmailKey = BuildEmailActiveKey(purpose, authAccountId);
            var activeToken = await _cacheService.GetAsync(activeEmailKey);
            if (!string.IsNullOrWhiteSpace(activeToken))
            {
                await _cacheService.DeleteAsync(BuildEmailTokenKey(purpose, activeToken));
                await _cacheService.DeleteAsync(activeEmailKey);
            }

            var activeSmsKey = BuildSmsActiveKey(purpose, authAccountId);
            var activeSmsIdentifier = await _cacheService.GetAsync(activeSmsKey);
            if (!string.IsNullOrWhiteSpace(activeSmsIdentifier))
            {
                await _cacheService.DeleteAsync(BuildSmsOtpKey(purpose, activeSmsIdentifier));
                await _cacheService.DeleteAsync(activeSmsKey);
            }
        }

        private async Task DeleteSmsOtpArtifactsAsync(
            VerificationPurposeEnum purpose,
            int authAccountId,
            string identifier)
        {
            await _cacheService.DeleteAsync(BuildSmsOtpKey(purpose, identifier));
            if (authAccountId <= 0)
            {
                return;
            }

            await _cacheService.DeleteAsync(BuildSmsActiveKey(purpose, authAccountId));
        }

        private sealed class Releaser(SemaphoreSlim semaphore) : IAsyncDisposable
        {
            public ValueTask DisposeAsync()
            {
                semaphore.Release();
                return ValueTask.CompletedTask;
            }
        }
    }
}
