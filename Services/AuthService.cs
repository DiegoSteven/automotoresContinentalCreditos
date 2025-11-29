using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using RespuestaCredito.Data;
using RespuestaCredito.DTOs;
using RespuestaCredito.Interfaces;

namespace RespuestaCredito.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(AppDbContext context, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
        {
            _logger.LogInformation("Intento de login para usuario: {Username}", loginDto.Username);

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username && u.Activo);

            if (usuario == null)
            {
                _logger.LogWarning("Usuario no encontrado o inactivo: {Username}", loginDto.Username);
                return null;
            }

            if (!VerifyPassword(loginDto.Password, usuario.PasswordHash))
            {
                _logger.LogWarning("Password incorrecto para usuario: {Username}", loginDto.Username);
                return null;
            }

            var token = GenerateJwtToken(usuario);
            var expiracion = DateTime.UtcNow.AddHours(8);

            _logger.LogInformation("Login exitoso para usuario: {Username}", usuario.Username);

            return new LoginResponseDto
            {
                Token = token,
                Username = usuario.Username,
                Expiracion = expiracion
            };
        }

        public async Task<LoginResponseDto?> RegisterAsync(RegisterDto registerDto)
        {
            _logger.LogInformation("Intento de registro para usuario: {Username}", registerDto.Username);

            var existeUsuario = await _context.Usuarios
                .AnyAsync(u => u.Username == registerDto.Username);

            if (existeUsuario)
            {
                _logger.LogWarning("El username {Username} ya está registrado", registerDto.Username);
                throw new ArgumentException($"El username '{registerDto.Username}' ya está en uso.");
            }

            var nuevoUsuario = new Models.Usuario
            {
                Username = registerDto.Username,
                PasswordHash = HashPassword(registerDto.Password),
                Activo = true,
                FechaCreacion = DateTime.Now
            };

            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Usuario {Username} registrado exitosamente", nuevoUsuario.Username);

            var token = GenerateJwtToken(nuevoUsuario);
            var expiracion = DateTime.UtcNow.AddHours(8);

            return new LoginResponseDto
            {
                Token = token,
                Username = nuevoUsuario.Username,
                Expiracion = expiracion
            };
        }

        private string GenerateJwtToken(Models.Usuario usuario)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Username),
                new Claim(ClaimTypes.Role, "Administrador")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == passwordHash;
        }
    }
}
