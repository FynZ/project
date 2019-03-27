using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleAuthApp.Settings
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string HmacSecretKey { get; set; }
        public int ExpiryDays { get; set; }
        public bool UseRsa { get; set; }
        public string RsaPrivateKeyXML { get; set; }
        public string RsaPublicKeyXML { get; set; }
    }
}
