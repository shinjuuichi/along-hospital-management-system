namespace SharedLibrary.Commons
{
    public class AppConfiguration
    {
        public AppInfo AppInfo { get; set; } = null!;
        public ApiUrlsConfig ApiUrlsConfig { get; set; } = null!;
        public UrlsConfig UrlsConfig { get; set; } = null!;
        public DatabaseConfig DatabaseConfig { get; set; } = null!;
        public DatabaseMongoConfig DatabaseMongoConfig { get; set; } = null!;
        public JwtConfig JwtConfig { get; set; } = null!;
        public EmailConfig EmailConfig { get; set; } = null!;
        public RedisConfig RedisConfig { get; set; } = null!;
        public GoogleConfig GoogleConfig { get; set; } = null!;
        public RabbitMQConfig RabbitMQConfig { get; set; } = null!;
        public R2Config R2Config { get; set; } = null!;
        public PayOSConfig PayOSConfig { get; set; } = null!;
        public VnPayConfig VnPayConfig { get; set; } = null!;
        public TwilioConfig TwilioConfig { get; set; } = null!;
        public TextBeeConfig TextBeeConfig { get; set; } = null!;
        public WebRtcConfig WebRtcConfig { get; set; } = null!;
        public RefreshTokenConfig RefreshTokenConfig { get; set; } = null!;
        public ExchangeRateConfig ExchangeRateConfig { get; set; } = null!;
        public SePayConfig SePayConfig { get; set; } = null!;
        public QdrantConfig QdrantConfig { get; set; } = null!;
    }

    #region AppInfo
    public class AppInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
    }
    #endregion

    #region ApiUrlsConfig
    public class ApiUrlsConfig
    {
        public string UploadUrl { get; set; } = string.Empty;
        public string MachineLearningUrl { get; set; } = string.Empty;
    }
    #endregion

    #region UrlsConfig
    public class UrlsConfig
    {
        public string FrontendUrl { get; set; } = string.Empty;
        public string BackendUrl { get; set; } = string.Empty;
    }
    #endregion

    #region DatabaseConfig
    public class DatabaseConfig
    {
        public string ConnectionString { get; set; } = string.Empty;
    }
    #endregion

    #region DatabaseMongoConfig
    public class DatabaseMongoConfig
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
    }
    #endregion

    #region JwtConfig
    public class JwtConfig
    {
        public string SecretKey { get; set; } = string.Empty;
        public int ExpireTimeInMinutes { get; set; } = 30;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
    }
    #endregion

    #region RefreshTokenConfig
    public class RefreshTokenConfig
    {
        public string Domain { get; set; } = string.Empty;
        public bool Secure { get; set; } = true;
        public int ExpirationDays { get; set; } = 7;
    }
    #endregion

    #region EmailConfig
    public class EmailConfig
    {
        public string ApiKey { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
    }
    #endregion

    #region RedisConfig
    public class RedisConfig
    {
        public string Host { get; set; } = string.Empty;
        public string InstanceName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    #endregion

    #region GoogleConfig
    public class GoogleConfig
    {
        public string ClientId { get; set; } = string.Empty;
        public string GeminiAPIKey { get; set; } = string.Empty;
    }
    #endregion

    #region RabbitMQConfig
    public class RabbitMQConfig
    {
        public string Host { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    #endregion

    #region R2Config
    public class R2Config
    {
        public string AccountId { get; set; } = string.Empty;
        public string AccessKeyId { get; set; } = string.Empty;
        public string SecretAccessKey { get; set; } = string.Empty;
        public string Bucket { get; set; } = string.Empty;
        public string PublicBaseUrl { get; set; } = string.Empty;

    }
    #endregion

    #region PayOSConfig
    public class PayOSConfig
    {
        public string ClientId { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string ChecksumKey { get; set; } = string.Empty;
    }
    #endregion

    #region VnPayConfig
    public class VnPayConfig
    {
        public string TmnCode { get; set; } = string.Empty;
        public string HashSecret { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty;
    }
    #endregion

    #region TwilioConfig
    public class TwilioConfig
    {
        public string AccountSid { get; set; } = string.Empty;
        public string AuthToken { get; set; } = string.Empty;
        public string MessageSid { get; set; } = string.Empty;
    }
    #endregion

    #region TextBeeConfig
    public class TextBeeConfig
    {
        public string ApiKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
        public string DeviceId { get; set; } = string.Empty;
    }
    #endregion

    #region WebRtcConfig
    public class WebRtcConfig
    {
        public List<IceServerConfig> IceServers { get; set; } = [];
    }

    public class IceServerConfig
    {
        public string Urls { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Credential { get; set; } = string.Empty;
    }
    #endregion

    #region ExchangeRateConfig
    public class ExchangeRateConfig
    {
        public string ApiKey { get; set; } = string.Empty;
    }
    #endregion

    #region SePayConfig
    public class SePayConfig
    {
        public string MerchantId { get; set; } = string.Empty;

        public string SecretKey { get; set; } = string.Empty;

        public string BaseUrl { get; set; } = string.Empty;

        public string BankName { get; set; } = string.Empty;

        public string VA { get; set; } = string.Empty;

        public string ApiKeyValidator { get; set; } = string.Empty;
    }
    #endregion

    #region QdrantConfig
    public class QdrantConfig
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 6334;
        public string CollectionName { get; set; } = string.Empty;
        public int VectorSize { get; set; } = 768;
    }
    #endregion
}