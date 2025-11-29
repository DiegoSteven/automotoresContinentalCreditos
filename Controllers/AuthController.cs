using Microsoft.AspNetCore.Mvc;
using RespuestaCredito.DTOs;
using RespuestaCredito.Interfaces;

namespace RespuestaCredito.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _authService.LoginAsync(loginDto);

            if (response == null)
            {
                _logger.LogWarning("Intento de login fallido para usuario: {Username}", loginDto.Username);
                return Unauthorized(new { error = "Usuario o contraseña incorrectos" });
            }

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var response = await _authService.RegisterAsync(registerDto);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar usuario {Username}", registerDto.Username);
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }
    }
}

