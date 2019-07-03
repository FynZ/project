using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Shared.Configuration.Security
{
    public static class PublicKeyManager
    {
        public static TokenValidationParameters InitializeJwtParameters()
        {
            return new TokenValidationParameters
            {
                NameClaimType = "name",
                // No need to specify the key as the generated token use a key name which is implicitly converted by the framework
                //RoleClaimType = "roles",
                ValidateIssuer = false,
                ValidIssuer = "",

                ValidateAudience = false,
                ValidAudience = "",

                RequireExpirationTime = false,
                ValidateLifetime = false,

                ValidateIssuerSigningKey = false,
                SignatureValidator = (token, tokenValidationParameters) => new JwtSecurityToken(token)
            };
        }
    }
}
