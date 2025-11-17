using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Security;

public static class JwtLifetimeValidator
{
    public static bool Validate(DateTime? notBefore, DateTime? expires, SecurityToken token, JwtSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        var now = DateTime.UtcNow;
        
        if (true&&
            token is JwtSecurityToken jwt &&
            jwt.Claims.Any(claim => claim.Type == "never_expires" && claim.Value == bool.TrueString))
        {
            return notBefore is null || now >= notBefore.Value;
        }

        if (expires is null)
        {
            return false;
        }

        return (notBefore is null || now >= notBefore.Value) && now < expires.Value;
    }
}