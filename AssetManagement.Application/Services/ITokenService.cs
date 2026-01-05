using System.Security.Claims;

namespace AssetManagement.Application.Services;

public interface ITokenService
{
    string GenerateAccessToken(string userId, string username, string email, IList<string> permissions);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
