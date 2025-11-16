using Shared.Models;

namespace Infrastructure.Interfaces;

public interface ITokenService
{
    string GenerateToken(string username);
}