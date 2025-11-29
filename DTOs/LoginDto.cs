using System.ComponentModel.DataAnnotations;

namespace RespuestaCredito.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "El username es obligatorio")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "El password es obligatorio")]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterDto
    {
        [Required(ErrorMessage = "El username es obligatorio")]
        [MinLength(4, ErrorMessage = "El username debe tener al menos 4 caracteres")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "El password es obligatorio")]
        [MinLength(6, ErrorMessage = "El password debe tener al menos 6 caracteres")]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public DateTime Expiracion { get; set; }
    }
}

