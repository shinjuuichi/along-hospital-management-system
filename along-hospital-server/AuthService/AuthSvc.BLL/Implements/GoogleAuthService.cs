using AuthSvc.BLL.Interfaces;
using Google.Apis.Auth;
using SharedLibrary.Commons;

namespace AuthSvc.BLL.Implements
{
    public class GoogleAuthService(AppConfiguration _configuration) : IGoogleAuthService
    {
        public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string idToken)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = [_configuration.GoogleConfig.ClientId]
            };

            return await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
        }
    }
}
