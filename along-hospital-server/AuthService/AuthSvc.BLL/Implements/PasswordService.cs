using AuthSvc.BLL.DTOs.Request;
using AuthSvc.BLL.DTOs.Response;
using AuthSvc.BLL.Interfaces;
using AuthSvc.BLL.Utils;
using AuthSvc.DAL.Enums;
using AuthSvc.DAL.Models;
using MessageBroker.Events.SendEmailEvents;
using MessageBroker.Events.SendSmsEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Services.Interfaces;
using SharedLibrary.Utils;

namespace AuthSvc.BLL.Implements
{
    public class PasswordService(
        IUnitOfWork unitOfWork,
        IMessageBus bus,
        AppConfiguration configuration,
        ICurrentUserService currentUserService,
        ITokenService tokenService,
        IVerificationCacheService verificationCacheService) : IPasswordService
    {
        private static readonly TimeSpan ResetEmailLinkTtl = TimeSpan.FromMinutes(15);
        private static readonly TimeSpan ResetSmsOtpTtl = TimeSpan.FromMinutes(5);
        private static readonly TimeSpan ResetTokenTtl = TimeSpan.FromMinutes(10);
        private static readonly TimeSpan VerificationCooldown = TimeSpan.FromSeconds(60);
        private const int MaxOtpAttempts = 5;

        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMessageBus _bus = bus;
        private readonly AppConfiguration _configuration = configuration;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IVerificationCacheService _verificationCacheService = verificationCacheService;
        private readonly IGenericRepository<AuthAccount> _authRepository = unitOfWork.Repository<AuthAccount>();

        public async Task<VerificationMethodOptionsResponseDTO> GetForgotPasswordOptionsAsync(
            LookupVerificationMethodsRequestDTO request)
        {
            var existingAuth = await this.FindAuthByIdentifierAsync(request.Identifier)
                ?? throw new DataNotFoundException("No account was found for that email or phone number");

            var methods = BuildVerificationMethodOptionsUtil.BuildVerificationMethodOptions(existingAuth);
            if (methods.Count == 0)
            {
                throw new InvalidDataException("No verification methods are available for this account");
            }

            var preferredDeliveryMethod = request.Identifier.Contains('@')
                ? VerificationDeliveryMethodEnum.Email.ToString()
                : VerificationDeliveryMethodEnum.Sms.ToString();

            var defaultDeliveryMethod = methods.Any(method =>
                string.Equals(method.DeliveryMethod, preferredDeliveryMethod, StringComparison.OrdinalIgnoreCase))
                ? preferredDeliveryMethod
                : methods[0].DeliveryMethod;

            return new VerificationMethodOptionsResponseDTO
            {
                Identifier = request.Identifier.Trim(),
                DefaultDeliveryMethod = defaultDeliveryMethod,
                Methods = methods
            };
        }

        public async Task ForgotPasswordRequestAsync(ForgotPasswordRequestDTO request)
        {
            var deliveryMethod = EnumUtil.ParseEnum<VerificationDeliveryMethodEnum>(request.DeliveryMethod);
            var existingAuth = await this.FindAuthByIdentifierAsync(request.Identifier)
                ?? throw new DataNotFoundException("No account was found for that email or phone number");

            switch (deliveryMethod)
            {
                case VerificationDeliveryMethodEnum.Email:
                    {
                        var deliveryEmail = StringUtil.NormalizeEmail(existingAuth.Email ?? string.Empty);
                        if (string.IsNullOrWhiteSpace(deliveryEmail))
                        {
                            throw new InvalidDataException("Email verification is not available for this account");
                        }

                        var cooldownStarted = await _verificationCacheService.TryBeginCooldownAsync(
                            VerificationPurposeEnum.PasswordReset,
                            deliveryMethod,
                            deliveryEmail,
                            VerificationCooldown);

                        if (!cooldownStarted)
                        {
                            return;
                        }

                        var token = await _verificationCacheService.CreateEmailLinkTokenAsync(
                            VerificationPurposeEnum.PasswordReset,
                            deliveryEmail,
                            existingAuth.Id,
                            ResetEmailLinkTtl);

                        var link = $"{_configuration.UrlsConfig.FrontendUrl}/auth/reset-password?deliveryMethod=Email&token={Uri.EscapeDataString(token)}";
                        await _bus.SendAsync(new SendVerificationLinkEmailEvent
                        {
                            Email = deliveryEmail,
                            Subject = "Reset your password",
                            Link = link
                        });
                        return;
                    }

                case VerificationDeliveryMethodEnum.Sms:
                    {
                        var phone = existingAuth.Phone ?? string.Empty;
                        if (string.IsNullOrWhiteSpace(phone))
                        {
                            throw new InvalidDataException("SMS verification is not available for this account");
                        }

                        var cooldownStarted = await _verificationCacheService.TryBeginCooldownAsync(
                            VerificationPurposeEnum.PasswordReset,
                            deliveryMethod,
                            phone,
                            VerificationCooldown);

                        if (!cooldownStarted)
                        {
                            return;
                        }

                        var otp = await _verificationCacheService.CreateSmsOtpAsync(
                            VerificationPurposeEnum.PasswordReset,
                            phone,
                            existingAuth.Id,
                            ResetSmsOtpTtl);

                        await _bus.SendAsync(new SendOtpSmsEvent
                        {
                            PhoneNumber = phone,
                            Otp = otp
                        });
                        return;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(deliveryMethod), deliveryMethod, null);
            }
        }

        public async Task<VerifyForgotPasswordResponseDTO> VerifyForgotPasswordAsync(VerifyForgotPasswordRequestDTO request)
        {
            var deliveryMethod = EnumUtil.ParseEnum<VerificationDeliveryMethodEnum>(request.DeliveryMethod);

            return deliveryMethod switch
            {
                VerificationDeliveryMethodEnum.Email => await this.VerifyForgotPasswordByEmailAsync(request.Token),
                VerificationDeliveryMethodEnum.Sms => await this.VerifyForgotPasswordBySmsAsync(request.Identifier, request.Otp),
                _ => throw new ArgumentOutOfRangeException(nameof(deliveryMethod), deliveryMethod, null)
            };
        }

        public async Task ResetPasswordAsync(ResetPasswordRequestDTO request)
        {
            var payload = await _verificationCacheService.ConsumePasswordResetSessionTokenAsync(request.ResetToken);

            if (payload == null)
            {
                throw new InvalidDataException("Password reset session is invalid or expired");
            }

            var existingAuth = await _authRepository.GetByIdAsync(payload.AuthAccountId)
                ?? throw new InvalidDataException("Invalid request");

            existingAuth.Password = CryptoUtil.EncryptPassword(request.NewPassword);
            _authRepository.Update(existingAuth);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task ChangePasswordAsync(ChangePasswordRequestDTO request, string accessToken)
        {
            var existingAuth = await _authRepository.GetByIdAsync(_currentUserService.AuthId)
                ?? throw new DataNotFoundException("User not found");

            if (!CryptoUtil.VerifyPassword(request.CurrentPassword, existingAuth.Password))
            {
                throw new ValidationFailureException(nameof(request.CurrentPassword), "Current password is incorrect");
            }

            if (request.CurrentPassword == request.NewPassword)
            {
                throw new ValidationFailureException(nameof(request.NewPassword), "New password must be different from current password");
            }

            existingAuth.Password = CryptoUtil.EncryptPassword(request.NewPassword);
            _authRepository.Update(existingAuth);

            await _tokenService.RevokeAllTokensAsync(existingAuth.Id);

            await _unitOfWork.SaveChangeAsync();
        }

        #region Helper Methods
        private async Task<VerifyForgotPasswordResponseDTO> VerifyForgotPasswordByEmailAsync(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new InvalidDataException("Invalid request");
            }

            var payload = await _verificationCacheService.ConsumeEmailLinkTokenAsync(
                VerificationPurposeEnum.PasswordReset,
                token);

            if (payload == null)
            {
                throw new InvalidDataException("The reset link is invalid or has already been used");
            }

            var existingAuth = await _authRepository.GetByIdAsync(payload.AuthAccountId)
                ?? throw new InvalidDataException("Invalid request");

            var resetToken = await _verificationCacheService.CreatePasswordResetSessionTokenAsync(
                existingAuth.Id,
                payload.Identifier,
                ResetTokenTtl);

            return new VerifyForgotPasswordResponseDTO
            {
                ResetToken = resetToken
            };
        }

        private async Task<VerifyForgotPasswordResponseDTO> VerifyForgotPasswordBySmsAsync(string? identifier, string? otp)
        {
            if (string.IsNullOrWhiteSpace(identifier) || string.IsNullOrWhiteSpace(otp))
            {
                throw new InvalidDataException("Invalid request");
            }

            var authAccountId = await this.VerifyForgotPasswordOtpInternalAsync(identifier, otp);
            var existingAuth = await _authRepository.GetByIdAsync(authAccountId)
                ?? throw new InvalidDataException("Invalid request");

            var resetToken = await _verificationCacheService.CreatePasswordResetSessionTokenAsync(
                existingAuth.Id,
                existingAuth.Phone ?? string.Empty,
                ResetTokenTtl);

            return new VerifyForgotPasswordResponseDTO
            {
                ResetToken = resetToken
            };
        }

        private async Task<int> VerifyForgotPasswordOtpInternalAsync(
            string identifier,
            string otp,
            bool consumeOnSuccess = true)
        {
            var existingAuth = await this.FindAuthByIdentifierAsync(identifier);

            if (existingAuth == null || string.IsNullOrWhiteSpace(existingAuth.Phone))
            {
                throw new InvalidDataException("Invalid or expired OTP");
            }

            var payload = await _verificationCacheService.VerifySmsOtpAsync(
                VerificationPurposeEnum.PasswordReset,
                existingAuth.Phone,
                otp,
                MaxOtpAttempts,
                consumeOnSuccess);

            if (payload == null || payload.AuthAccountId != existingAuth.Id)
            {
                throw new InvalidDataException("Invalid or expired OTP");
            }

            return payload.AuthAccountId;
        }

        private async Task<AuthAccount?> FindAuthByIdentifierAsync(string identifier)
        {
            if (identifier.Contains('@'))
            {
                var normalizedEmail = StringUtil.NormalizeEmail(identifier);
                return await _authRepository.GetByConditionAsync(u => u.Email == normalizedEmail);
            }


            return await _authRepository.GetByConditionAsync(u => u.Phone == identifier);
        }
        #endregion
    }
}
