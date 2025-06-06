using Project.Application.Utils;

namespace Project.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string email);

        Task<CustomResult<string>> GetEmailbyTokenClaims();
    }
}
