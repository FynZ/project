using System.IO;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Monsters.Configuration.Security
{
    public class PublicKeyManager
    {
        public static TokenValidationParameters InitializeJwtParameters(string xmlPublicKeyPath)
        {
            // dirty but the information is needed before the DI can be used (reason the method is static in the first place)
            var issuerSigningKey = CreateSecurityKey(xmlPublicKeyPath);

            return new TokenValidationParameters
            {
                NameClaimType = "name",
                // No need to specify the key as the generated token use a key name which is implicitly converted by the framework
                //RoleClaimType = "roles",
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidIssuer = "FDO",
                RequireSignedTokens = true,
                IssuerSigningKey = issuerSigningKey
            };
        }

        private static RsaSecurityKey CreateSecurityKey(string xmlPublicKeyPath)
        {
            //using (RSA publicRsa = RSA.Create())
            //{
            //    var publicKeyXml = File.ReadAllText(xmlPublicKeyPath);
            //    publicRsa.FromXml(publicKeyXml);

            //    return new RsaSecurityKey(publicRsa);
            //}

            RSA publicRsa = RSA.Create();
            var publicKeyXml = File.ReadAllText(xmlPublicKeyPath);
            publicRsa.FromXml(publicKeyXml);

            return new RsaSecurityKey(publicRsa);
        }
    }
}
