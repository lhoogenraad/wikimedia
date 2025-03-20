using Models.Entities;

namespace Application.Services.Auth
{
    public interface IAuthService
    {
        string GenerateToken(User user);
        bool ValidateToken(string token);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}

