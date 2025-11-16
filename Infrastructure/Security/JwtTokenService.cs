using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Security;

public class JwtTokenService : ITokenService
{
    private readonly JwtSettings settings;
    private readonly SigningCredentials signingCredentials;

    public JwtTokenService(IOptions<JwtSettings> options)
    {
        settings = options.Value ?? throw new ArgumentNullException(nameof(options));

        if (settings.Expires <= TimeSpan.Zero)
        {
            throw new InvalidOperationException("JWT expiry must be a positive TimeSpan value.");
        }

        var secret = JwtSecretProvider.ResolveSecretFromEnvironment(settings.SecretKey);
        signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            SecurityAlgorithms.HmacSha256);
    }

    public string GenerateToken(string username)
    {
        var claims = new List<Claim>
        {
            new Claim("username", username)
        };
        var jwtToken = new JwtSecurityToken(
            expires: DateTime.Now.Add(settings.Expires),
            claims: claims,
            signingCredentials: signingCredentials);
        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}