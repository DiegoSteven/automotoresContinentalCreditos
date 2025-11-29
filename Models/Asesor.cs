using System.ComponentModel.DataAnnotations;

namespace RespuestaCredito.Models
{
    public class Asesor
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Telefono { get; set; }

        public bool Activo { get; set; } = true;

        // Relación
        public ICollection<SolicitudCredito> Solicitudes { get; set; } = new List<SolicitudCredito>();
    }
}
