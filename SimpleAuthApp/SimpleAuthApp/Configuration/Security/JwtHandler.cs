using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SimpleAuthApp.Configuration.Security.Models;
using SimpleAuthApp.Extensions;
using SimpleAuthApp.Models;
using SimpleAuthApp.Settings;

namespace SimpleAuthApp.Configuration.Security
{
    public class JwtHandler : IJwtHandler
    {
        // Settings from the AppSettings file
        private readonly JwtSettings _settings;

        // object used to write the token
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        // Represent the Jwt Header (alg + typ)
        private readonly JwtHeader _jwtHeader;

        public JwtHandler(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            if (_settings.UseRsa)
            {
                var signingCredentials = CreateRsaCredentials(_settings.RsaPrivateKeyXML);

                _jwtHeader = new JwtHeader(signingCredentials);
            }
            else
            {
                var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.HmacSecretKey));
                var signingCredentials = new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256);

                _jwtHeader = new JwtHeader(signingCredentials);
            }
        }

        private static RsaSecurityKey CreateSecurityKey(string publicKeyRelativePath)
        {
            using (RSA publicRsa = RSA.Create())
            {
                var publicKeyXml = File.ReadAllText(publicKeyRelativePath);
                publicRsa.FromXml(publicKeyXml);

                return new RsaSecurityKey(publicRsa);
            }
        }

        private static SigningCredentials CreateRsaCredentials(string privateKeyRelativePath)
        {
            using (RSA privateRsa = RSA.Create())
            {
                var privateKeyXml = File.ReadAllText(privateKeyRelativePath);
                privateRsa.FromXml(privateKeyXml);
                var privateKey = new RsaSecurityKey(privateRsa);

                return new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256);
            }
        }

        public static TokenValidationParameters InitializeJwtParameters()
        {
            // dirty but the information is needed before the DI can be used (reason the method is static in the first place)
            var issuerSigningKey = CreateSecurityKey("Ressources/public-key.xml");

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

        public JWT Create(User user)
        {
            var nowUtc = DateTime.UtcNow;
            var expires = nowUtc.AddDays(_settings.ExpiryDays);
            var centuryBegin = new DateTime(1970, 1, 1);
            var exp = (long)(new TimeSpan(expires.Ticks - centuryBegin.Ticks).TotalSeconds);
            var now = (long)(new TimeSpan(nowUtc.Ticks - centuryBegin.Ticks).TotalSeconds);

            var jwtToken = new JwtToken
            {
                name = user.Username,
                iss = _settings.Issuer ?? String.Empty,
                sub = user.Id,
                aud = _settings.Audience ?? String.Empty,
                exp = exp,
                nbf = now,
                iat = now,
                jti = Guid.NewGuid().ToString("N"),
                roles = user.Roles.Select(x => x.Name)
            };

            //var payload = new JwtPayload(JsonConvert.SerializeObject(jwtToken));

            //var payload = new JwtPayload
            //{
            //    {"unique_name", user.Username},
            //    {"name", user.Username},
            //    {"iss", _settings.Issuer ?? String.Empty},
            //    {"sub", user.Id},
            //    {"aud", _settings.Audience ?? String.Empty},
            //    {"exp", exp},
            //    {"nbf", now},
            //    {"iat", now},
            //    {"jti", Guid.NewGuid().ToString("N")}
            //};
            //payload.AddClaims(user.Roles.Select(x => new Claim("roles", x.Name)));

            var jwt = new JwtSecurityToken(_jwtHeader, jwtToken);
            var token = _jwtSecurityTokenHandler.WriteToken(jwt);

            return new JWT
            {
                Token = token,
                Expires = exp
            };
        }
    }
}
