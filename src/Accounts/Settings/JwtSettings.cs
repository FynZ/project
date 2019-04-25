namespace Accounts.Settings
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string HmacSecretKey { get; set; }
        public int ExpiryDays { get; set; }
        public bool UseRsa { get; set; }
        public string RsaPrivateKeyXml { get; set; }
    }
}
