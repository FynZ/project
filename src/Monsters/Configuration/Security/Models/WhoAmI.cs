using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;

namespace Monsters.Configuration.Security.Models
{
#pragma warning disable IDE1006 // Naming Styles
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class WhoAmI
    {
        public bool authenticated { get; set; }
        public JwtPayload payload { get; set; }
    }
#pragma warning restore IDE1006 // Naming Styles
}
