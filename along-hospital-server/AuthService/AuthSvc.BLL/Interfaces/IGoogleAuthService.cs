using Google.Apis.Auth;

namespace AuthSvc.BLL.Interfaces
{
    public interface IGoogleAuthService
    {
        Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string idToken);
    }
}
