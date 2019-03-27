using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimpleAuthApp.Models;

namespace SimpleAuthApp
{
    public interface IJwtHandler
    {
        JWT Create(User user);
    }

    public class JwtHandler : IJwtHandler
    {
        // Settings from the AppSettings file
        private readonly JwtSettings _settings;

        // object used to write the token
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly SecurityKey _issuerSigningKey;
        private readonly SigningCredentials _signingCredentials;
        private readonly JwtHeader _jwtHeader;

        public JwtHandler(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            if (_settings.UseRsa)
            {
                using (RSA publicRsa = RSA.Create())
                {
                    var publicKeyXml = File.ReadAllText(_settings.RsaPublicKeyXML);
                    publicRsa.FromXml(publicKeyXml);
                    _issuerSigningKey = new RsaSecurityKey(publicRsa);
                }

                using (RSA privateRsa = RSA.Create())
                {
                    var privateKeyXml = File.ReadAllText(_settings.RsaPrivateKeyXML);
                    privateRsa.FromXml(privateKeyXml);
                    var privateKey = new RsaSecurityKey(privateRsa);
                    _signingCredentials = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256);
                }
            }
            else
            {
                _issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.HmacSecretKey));
                _signingCredentials = new SigningCredentials(_issuerSigningKey, SecurityAlgorithms.HmacSha256);
            }

            _jwtHeader = new JwtHeader(_signingCredentials);
        }

        public static TokenValidationParameters InitializeJwtParameters()
        {
            using (RSA publicRsa = RSA.Create())
            {
                var publicKeyXml = File.ReadAllText("Ressources/public-key.xml");
                publicRsa.FromXml(publicKeyXml);
                var publicKey = new RsaSecurityKey(publicRsa);

                return new TokenValidationParameters
                {

                    //NameClaimType = "name",
                    // No need to specify the key as the generated token use a key name which is implicitly converted by the framework
                    //RoleClaimType = "roles",
                    ValidateAudience = false,
                    //ValidateLifetime = false,
                    ValidIssuer = "FDO",
                    RequireSignedTokens = true,
                    IssuerSigningKey = publicKey,
                };
            }
        }

        public JWT Create(User user)
        {
            var nowUtc = DateTime.UtcNow;
            var expires = nowUtc.AddDays(_settings.ExpiryDays);
            var centuryBegin = new DateTime(1970, 1, 1);
            var exp = (long)(new TimeSpan(expires.Ticks - centuryBegin.Ticks).TotalSeconds);
            var now = (long)(new TimeSpan(nowUtc.Ticks - centuryBegin.Ticks).TotalSeconds);

            var payload = new JwtPayload
            {
                {"unique_name", user.Username},
                {"name", user.Username},
                {"iss", _settings.Issuer ?? String.Empty},
                {"sub", user.Id},
                {"aud", _settings.Audience ?? String.Empty},
                {"exp", exp},
                {"nbf", now},
                {"iat", now},
                {"jti", Guid.NewGuid().ToString("N")}
            };
            payload.AddClaims(user.Roles.Select(x => new Claim("caca", x.Name)));
            //payload.AddClaims(user.Roles.Select(x => new Claim(ClaimTypes.Role, x.Name)));

            var jwt = new JwtSecurityToken(_jwtHeader, payload);
            var token = _jwtSecurityTokenHandler.WriteToken(jwt);

            return new JWT
            {
                Token = token,
                Expires = exp
            };
        }
    }

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

    public class JWT
    {
        public string Token { get; set; }
        public long Expires { get; set; }
    }

    public static class RSAExtension
    {
        public static void FromXml(this RSA rsa, string xmlString)
        {
            var parameters = new RSAParameters();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
            {
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "Modulus": parameters.Modulus = Convert.FromBase64String(node.InnerText); break;
                        case "Exponent": parameters.Exponent = Convert.FromBase64String(node.InnerText); break;
                        case "P": parameters.P = Convert.FromBase64String(node.InnerText); break;
                        case "Q": parameters.Q = Convert.FromBase64String(node.InnerText); break;
                        case "DP": parameters.DP = Convert.FromBase64String(node.InnerText); break;
                        case "DQ": parameters.DQ = Convert.FromBase64String(node.InnerText); break;
                        case "InverseQ": parameters.InverseQ = Convert.FromBase64String(node.InnerText); break;
                        case "D": parameters.D = Convert.FromBase64String(node.InnerText); break;
                    }
                }
            }
            else
            {
                throw new Exception("Invalid XML RSA key.");
            }

            rsa.ImportParameters(parameters);
        }
    }
}
