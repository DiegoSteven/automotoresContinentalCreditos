using RespuestaCredito.DTOs;

namespace RespuestaCredito.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
        Task<LoginResponseDto?> RegisterAsync(RegisterDto registerDto);
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
    }
}

