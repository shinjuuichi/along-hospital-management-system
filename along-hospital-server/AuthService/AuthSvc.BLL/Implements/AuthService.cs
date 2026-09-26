using AuthSvc.BLL.DTOs;
using AuthSvc.BLL.DTOs.Request;
using AuthSvc.BLL.DTOs.Response;
using AuthSvc.BLL.Interfaces;
using AuthSvc.BLL.Utils;
using AuthSvc.DAL.Enums;
using AuthSvc.DAL.Models;
using AutoMapper;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using MessageBroker.Events.SendEmailEvents;
using MessageBroker.Events.SendSmsEvents;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Enums;
using SharedLibrary.Services.Interfaces;
using SharedLibrary.Utils;

namespace AuthSvc.BLL.Implements
{
    public class AuthService(
        IUnitOfWork unitOfWork,
        IGoogleAuthService googleAuthService,
        ITokenService tokenService,
        ITokenBlacklistService tokenBlacklistService,
        IMessageBus bus,
        AppConfiguration configuration,
        ICurrentUserService currentUserService,
        IMapper mapper,
        IVerificationCacheService verificationCacheService)
        : BaseService<AuthAccount, CreateAuthDTO, UpdateAuthDTO, GetAuthDTO>(unitOfWork, mapper), IAuthService
    {
        private static readonly TimeSpan RegisterEmailLinkTtl = TimeSpan.FromMinutes(15);
        private static readonly TimeSpan RegisterSmsOtpTtl = TimeSpan.FromMinutes(5);
        private static readonly TimeSpan VerificationCooldown = TimeSpan.FromSeconds(60);
        private const int MaxOtpAttempts = 5;

        #region Register
        public async Task RegisterAsync(RegisterRequestDTO request)
        {
            var normalizedEmail = StringUtil.NormalizeEmail(request.Email);

            var existingAuth = await _repository.GetByConditionAsync(
                u => u.Phone == request.Phone || u.Email == normalizedEmail);

            if (existingAuth != null)
            {
                if (existingAuth.Status == AuthStatusEnum.PendingVerification)
                {
                    throw new InvalidDataException("An account is pending verification. Please verify your account");
                }

                throw new DataConflictException("An account with the same email or phone already exists");
            }

            var newAuth = _mapper.Map<AuthAccount>(request);
            newAuth.Password = CryptoUtil.EncryptPassword(request.Password);
            newAuth.Email = normalizedEmail;
            newAuth.Phone = request.Phone;
            newAuth.Stage = AuthStageEnum.PatientProfilePendingWithPhone;
            newAuth.Provider = ProviderEnum.Email;

            await _repository.AddAsync(newAuth);
            await _unitOfWork.SaveChangeAsync();

            await this.SendRegisterVerificationAsync(newAuth, VerificationDeliveryMethodEnum.Email);
        }

        public async Task<VerificationMethodOptionsResponseDTO> GetRegisterResendOptionsAsync(
            LookupVerificationMethodsRequestDTO request)
        {
            var existingAuth = await this.FindAuthByIdentifierAsync(request.Identifier);

            if (existingAuth == null || existingAuth.Status != AuthStatusEnum.PendingVerification)
            {
                throw new DataNotFoundException("No pending verification account was found");
            }

            var methods = BuildVerificationMethodOptionsUtil.BuildVerificationMethodOptions(existingAuth);

            var defaultDeliveryMethod = methods.Any(method =>
                string.Equals(
                    method.DeliveryMethod,
                    VerificationDeliveryMethodEnum.Email.ToString(),
                    StringComparison.OrdinalIgnoreCase))
                ? VerificationDeliveryMethodEnum.Email.ToString()
                : methods[0].DeliveryMethod;

            return new VerificationMethodOptionsResponseDTO
            {
                Identifier = request.Identifier.Trim(),
                DefaultDeliveryMethod = defaultDeliveryMethod,
                Methods = methods
            };
        }

        public async Task ResendRegisterAsync(ResendRegisterRequestDTO request)
        {
            var deliveryMethod = EnumUtil.ParseEnum<VerificationDeliveryMethodEnum>(request.DeliveryMethod);
            var existingAuth = await this.FindAuthByIdentifierAsync(request.Identifier);

            if (existingAuth == null || existingAuth.Status != AuthStatusEnum.PendingVerification)
            {
                throw new DataNotFoundException("No pending verification account was found");
            }

            await this.SendRegisterVerificationAsync(existingAuth, deliveryMethod);
        }

        public async Task<GetAuthResponseDTO> VerifyRegisterAsync(VerifyRegisterRequestDTO request)
        {
            var deliveryMethod = EnumUtil.ParseEnum<VerificationDeliveryMethodEnum>(request.DeliveryMethod);

            return deliveryMethod switch
            {
                VerificationDeliveryMethodEnum.Email => await this.VerifyRegisterByEmailAsync(request.Token),
                VerificationDeliveryMethodEnum.Sms => await this.VerifyRegisterBySmsAsync(request.Identifier, request.Otp),
                _ => throw new ArgumentOutOfRangeException(nameof(deliveryMethod), deliveryMethod, null)
            };
        }

        private async Task<GetAuthResponseDTO> VerifyRegisterByEmailAsync(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new InvalidDataException("Invalid request");
            }

            var payload = await verificationCacheService.ConsumeEmailLinkTokenAsync(
                VerificationPurposeEnum.Registration,
                token);

            if (payload == null)
            {
                throw new InvalidDataException("Invalid request");
            }

            var existingAuth = await _repository.GetByIdAsync(payload.AuthAccountId)
                ?? throw new InvalidDataException("Invalid request");

            if (existingAuth.Status != AuthStatusEnum.Verified)
            {
                existingAuth.Status = AuthStatusEnum.Verified;
                _repository.Update(existingAuth);
                await _unitOfWork.SaveChangeAsync();
            }

            return await tokenService.GenerateTokensAsync(existingAuth);
        }

        private async Task<GetAuthResponseDTO> VerifyRegisterBySmsAsync(string? identifier, string? otp)
        {
            if (string.IsNullOrWhiteSpace(identifier) || string.IsNullOrWhiteSpace(otp))
            {
                throw new InvalidDataException("Invalid request");
            }

            var existingAuth = await this.FindAuthByIdentifierAsync(identifier);

            if (existingAuth == null || string.IsNullOrWhiteSpace(existingAuth.Phone))
            {
                throw new InvalidDataException("Invalid request");
            }

            var payload = await verificationCacheService.VerifySmsOtpAsync(
                VerificationPurposeEnum.Registration,
                existingAuth.Phone ?? string.Empty,
                otp,
                MaxOtpAttempts);

            if (payload == null)
            {
                throw new InvalidDataException("Invalid request");
            }

            if (payload.AuthAccountId != existingAuth.Id)
            {
                throw new InvalidDataException("Invalid request");
            }

            existingAuth = await _repository.GetByIdAsync(payload.AuthAccountId)
                ?? throw new InvalidDataException("Invalid request");

            if (existingAuth.Status != AuthStatusEnum.Verified)
            {
                existingAuth.Status = AuthStatusEnum.Verified;
                _repository.Update(existingAuth);
                await _unitOfWork.SaveChangeAsync();
            }

            return await tokenService.GenerateTokensAsync(existingAuth);
        }

        private async Task SendRegisterVerificationAsync(
            AuthAccount auth,
            VerificationDeliveryMethodEnum deliveryMethod)
        {
            switch (deliveryMethod)
            {
                case VerificationDeliveryMethodEnum.Email:
                    {
                        var email = StringUtil.NormalizeEmail(auth.Email ?? string.Empty);
                        if (string.IsNullOrWhiteSpace(email))
                        {
                            throw new InvalidDataException("Email verification is not available for this account");
                        }

                        var cooldownStarted = await verificationCacheService.TryBeginCooldownAsync(
                            VerificationPurposeEnum.Registration,
                            deliveryMethod,
                            email,
                            VerificationCooldown);

                        if (!cooldownStarted)
                        {
                            return;
                        }

                        var token = await verificationCacheService.CreateEmailLinkTokenAsync(
                            VerificationPurposeEnum.Registration,
                            email,
                            auth.Id,
                            RegisterEmailLinkTtl);

                        var link = $"{configuration.UrlsConfig.FrontendUrl}/auth/verify?deliveryMethod=Email&token={Uri.EscapeDataString(token)}";
                        await bus.SendAsync(new SendVerificationLinkEmailEvent
                        {
                            Email = email,
                            Subject = "Verify your account",
                            Link = link
                        });
                        return;
                    }
                case VerificationDeliveryMethodEnum.Sms:
                    {
                        var phone = auth.Phone ?? string.Empty;
                        if (string.IsNullOrWhiteSpace(phone))
                        {
                            throw new InvalidDataException("SMS verification is not available for this account");
                        }

                        var cooldownStarted = await verificationCacheService.TryBeginCooldownAsync(
                            VerificationPurposeEnum.Registration,
                            deliveryMethod,
                            phone,
                            VerificationCooldown);

                        if (!cooldownStarted)
                        {
                            return;
                        }

                        var otp = await verificationCacheService.CreateSmsOtpAsync(
                            VerificationPurposeEnum.Registration,
                            phone,
                            auth.Id,
                            RegisterSmsOtpTtl);

                        await bus.SendAsync(new SendOtpSmsEvent
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
        #endregion

        #region Login
        public async Task<GetAuthResponseDTO> LoginAsync(LoginRequestDTO request)
        {
            var identifier = request.Identifier.Trim();

            AuthAccount? existingAuth;
            if (identifier.Contains('@'))
            {
                var normalizedEmail = StringUtil.NormalizeEmail(identifier);
                existingAuth = await _repository.GetByConditionAsync(u => u.Email == normalizedEmail);
            }
            else
            {
                existingAuth = await _repository.GetByConditionAsync(u => u.Phone == identifier);
            }

            if (existingAuth == null || !CryptoUtil.VerifyPassword(request.Password, existingAuth.Password))
            {
                throw new InvalidDataException("Invalid credentials");
            }

            if (existingAuth.Status == AuthStatusEnum.Terminated
                || existingAuth.Status == AuthStatusEnum.Suspended)
            {
                throw new InvalidDataException("Your Account is terminated or suspended");
            }

            if (existingAuth.Status != AuthStatusEnum.Verified)
            {
                throw new InvalidDataException("Your Account is not verified");
            }

            return await tokenService.GenerateTokensAsync(existingAuth);
        }

        public async Task<GetAuthResponseDTO> GoogleLoginAsync(GoogleLoginRequestDTO request)
        {
            var googleUser = await googleAuthService.VerifyGoogleToken(request.IdToken);
            var normalizedEmail = StringUtil.NormalizeEmail(googleUser.Email);

            var existingAuth = await _repository
                .GetByConditionAsync(u => u.ProviderUserId == googleUser.Subject || u.Email == normalizedEmail);

            if (existingAuth == null)
            {
                var password = StringUtil.GenerateRandomPassword();
                existingAuth = new AuthAccount
                {
                    Email = normalizedEmail,
                    Provider = ProviderEnum.Google,
                    ProviderUserId = googleUser.Subject,
                    Status = AuthStatusEnum.Verified,
                    Password = CryptoUtil.EncryptPassword(password),
                    Stage = AuthStageEnum.PatientProfilePendingWithoutPhone
                };

                await _repository.AddAsync(existingAuth);
                await _unitOfWork.SaveChangeAsync();
            }

            return await tokenService.GenerateTokensAsync(existingAuth);
        }
        #endregion

        #region Logout
        public async Task LogoutAsync(string accessToken, string? refreshToken)
        {
            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                await tokenService.RevokeTokenAsync(refreshToken);
            }

            await tokenService.BlacklistAccessTokenAsync(accessToken);
        }
        #endregion

        #region Get Me
        public async Task<GetUserProfileDTO> GetMeAsync()
        {
            var existingAuth = await _repository.GetByIdAsync(currentUserService.AuthId)
                ?? throw new DataNotFoundException("User not found");

            string? role = null;

            var isPendingStage = existingAuth.Stage == AuthStageEnum.PatientProfilePendingWithPhone
                || existingAuth.Stage == AuthStageEnum.PatientProfilePendingWithoutPhone;

            if (!isPendingStage)
            {
                var getUserRoleContract = await bus.RequestAsync<GetUserRoleByUserIdEvent, GetUserRoleByUserIdContract>(
                    new GetUserRoleByUserIdEvent { Id = existingAuth.UserId ?? 0 });
                role = getUserRoleContract.Role;
            }

            return new GetUserProfileDTO
            {
                AuthId = existingAuth.Id,
                UserId = existingAuth.UserId ?? 0,
                Role = role,
                Stage = existingAuth.Stage.ToString()
            };
        }
        #endregion

        #region Consumer Methods
        public override async Task<GetAuthDTO> UpdateAsync(int id, UpdateAuthDTO updateAuthDTO)
        {
            var existingAuth = await _repository.GetByConditionAsync(a => a.UserId == id)
                ?? throw new DataNotFoundException($"AuthAccount with UserId {id} not found");

            _mapper.Map(updateAuthDTO, existingAuth);

            var resultEntity = _repository.Update(existingAuth);
            await _unitOfWork.SaveChangeAsync();

            return await GetByIdAsync(resultEntity.Id);
        }

        public async Task UpdateAuthAccountWithUserIdAsync(UpdateAuthAccountWithUserIdDTO request)
        {
            var existingAuth = await _repository.GetByIdAsync(request.AuthId)
                ?? throw new DataNotFoundException(typeof(AuthAccount), request.AuthId);

            existingAuth.UserId = request.UserId;
            existingAuth.Stage = AuthStageEnum.Done;

            if (!string.IsNullOrWhiteSpace(request.Phone) && string.IsNullOrWhiteSpace(existingAuth.Phone))
            {
                existingAuth.Phone = request.Phone;
            }

            base._repository.Update(existingAuth);
            await _unitOfWork.SaveChangeAsync();
        }

        public override async Task DeleteAsync(int id)
        {
            var existingAuth = await _repository.GetByConditionAsync(a => a.UserId == id)
                ?? throw new DataNotFoundException(typeof(AuthAccount), id);

            _repository.Remove(existingAuth);
            await _unitOfWork.SaveChangeAsync();
        }

        public override async Task<List<GetAuthDTO>> GetAllByIdsAsync(List<int> ids)
        {
            var authAccounts = await _repository.GetAllAsync(a => ids.Contains(a.UserId ?? 0));
            return _mapper.Map<List<GetAuthDTO>>(authAccounts);
        }

        public override async Task<GetAuthDTO> GetByIdAsync(int id)
        {
            var authAccounts = await _repository.GetByConditionAsync(a => a.UserId == id);
            return _mapper.Map<GetAuthDTO>(authAccounts);
        }

        public async Task<List<int?>> GetUserIdsByFilterAsync(AuthFilterRequestDTO authFilterRequestDTO)
        {
            IQueryable<AuthAccount> query = _repository.GetAllQueryable();

            if (!string.IsNullOrEmpty(authFilterRequestDTO.Phone))
            {
                query = query.Where(x => x.Phone != null && x.Phone.Contains(authFilterRequestDTO.Phone));
            }

            if (!string.IsNullOrEmpty(authFilterRequestDTO.Email))
            {
                query = query.Where(x => x.Email != null && x.Email.Contains(authFilterRequestDTO.Email));
            }

            return await query.Select(x => x.UserId).ToListAsync();
        }

        public async Task TerminateAuthAccountsByUserIdsAsync(List<int> userIds)
        {
            if (userIds.Count == 0)
            {
                return;
            }

            var authAccounts = await _repository.GetAllAsync(a => a.UserId.HasValue && userIds.Contains(a.UserId.Value));
            if (authAccounts.Count == 0)
            {
                return;
            }

            foreach (var authAccount in authAccounts)
            {
                authAccount.Status = AuthStatusEnum.Terminated;
            }

            _repository.UpdateRange(authAccounts);
            await _unitOfWork.SaveChangeAsync();

            foreach (var authAccount in authAccounts)
            {
                await this.SyncAuthAccountAccessAsync(authAccount.Id, authAccount.Status);
            }
        }

        public async Task ChangeAuthStatusByUserIdAsync(int userId, string? status)
        {
            if (!Enum.TryParse<AuthStatusEnum>(status, out var newStatus))
            {
                throw new InvalidDataException($"Invalid status: {status}");
            }

            var existingAuth = await _repository.GetByConditionAsync(a => a.UserId == userId);

            if (existingAuth == null)
            {
                throw new DataNotFoundException($"AuthAccount with UserId {userId} not found");
            }

            if (existingAuth.Status == newStatus)
            {
                return;
            }

            if (newStatus == AuthStatusEnum.Verified
                && existingAuth.Status != AuthStatusEnum.Suspended)
            {
                throw new InvalidDataException("Cannot verify account unless it is suspended.");
            }

            existingAuth.Status = newStatus;
            _repository.Update(existingAuth);
            await _unitOfWork.SaveChangeAsync();

            await this.SyncAuthAccountAccessAsync(existingAuth.Id, newStatus);
        }

        private async Task<AuthAccount?> FindAuthByIdentifierAsync(string identifier)
        {
            if (identifier.Contains('@'))
            {
                var normalizedEmail = StringUtil.NormalizeEmail(identifier);
                return await _repository.GetByConditionAsync(u => u.Email == normalizedEmail);
            }

            return await _repository.GetByConditionAsync(u => u.Phone == identifier);
        }

        private async Task SyncAuthAccountAccessAsync(int authAccountId, AuthStatusEnum status)
        {
            if (status != AuthStatusEnum.Suspended
                && status != AuthStatusEnum.Terminated)
            {
                return;
            }

            await tokenService.RevokeAllTokensAsync(authAccountId);
            await tokenBlacklistService.BlacklistAuthAccountAsync(authAccountId);
        }
        #endregion
    }
}
