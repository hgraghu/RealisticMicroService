namespace UserService.Common
{
    public class AuthSettingOptions
    {
        public static string AuthOption = "AuthOptions";
        public string SecretCode { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string IssuerDomain { get; set; } = string.Empty;
        public int JwtExpiryMinutes { get; set; }
        public string Connection { get; set; } = string.Empty;
    }
}
