using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Accounts.Configuration.Security.Models;
using Accounts.Models;
using Accounts.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebApi.Shared.Configuration.Security;

namespace Accounts.Configuration.Security
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
                var signingCredentials = CreateRsaCredentials(_settings.RsaPrivateKeyXml);

                _jwtHeader = new JwtHeader(signingCredentials);
            }
            else
            {
                var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.HmacSecretKey));
                var signingCredentials = new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256);

                _jwtHeader = new JwtHeader(signingCredentials);
            }
        }

        private static SigningCredentials CreateRsaCredentials(string privateKeyRelativePath)
        {
            //using (RSA privateRsa = RSA.Create())
            //{
            //    var privateKeyXml = File.ReadAllText(privateKeyRelativePath);
            //    privateRsa.FromXml(privateKeyXml);
            //    var privateKey = new RsaSecurityKey(privateRsa);

            //    return new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256);
            //}

            RSA privateRsa = RSA.Create();
            var privateKeyXml = File.ReadAllText(privateKeyRelativePath);
            privateRsa.FromXml(privateKeyXml);
            var privateKey = new RsaSecurityKey(privateRsa);

            return new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256);
        }

    public Jwt Create(User user)
    {
        var nowUtc = DateTime.UtcNow;
        var expires = nowUtc.AddDays(_settings.ExpiryDays);
        var centuryBegin = new DateTime(1970, 1, 1);
        var exp = (long)(new TimeSpan(expires.Ticks - centuryBegin.Ticks).TotalSeconds);
        var now = (long)(new TimeSpan(nowUtc.Ticks - centuryBegin.Ticks).TotalSeconds);

        var jwtToken = new JwtToken
        {
            id = user.Id,
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

        var jwt = new JwtSecurityToken(_jwtHeader, jwtToken);
        var token = _jwtSecurityTokenHandler.WriteToken(jwt);

        return new Jwt
        {
            Token = token,
            Expires = exp
        };
    }
}
}
