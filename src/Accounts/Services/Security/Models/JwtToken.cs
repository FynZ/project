using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Accounts.Services.Security.Models
{
#pragma warning disable IDE1006 // Naming Styles
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class JwtToken
    {
        public int id { get; set; }
        public string name { get; set; }
        public string iss { get; set; }
        public int sub { get; set; }
        public string aud { get; set; }
        public long exp { get; set; }
        public long nbf { get; set; }
        public long iat { get; set; }
        public string jti { get; set; }
        public IEnumerable<string> roles { get; set; }

        public JwtToken()
        {
            roles = new List<string>();
        }

        public static implicit operator JwtPayload(JwtToken token)
        {
            var payload = new JwtPayload
            {
                {nameof(id), token.id },
                {nameof(name), token.name},
                {nameof(iss), token.iss},
                {nameof(sub), token.sub},
                {nameof(aud), token.aud},
                {nameof(exp), token.exp},
                {nameof(nbf), token.nbf},
                {nameof(iat), token.iat},
                {nameof(jti), token.jti},
            };

            payload.AddClaims(token.roles.Select(x => new Claim("roles", x)));

            return payload;
        }
    }
#pragma warning restore IDE1006 // Naming Styles
}
