using System.ComponentModel.DataAnnotations;

namespace RespuestaCredito.Models
{
    public class Financiera
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Codigo { get; set; }

        // Parámetro para el estado EN_PROCESO
        public int TiempoEsperaMinutos { get; set; } = 30;

        public bool Activa { get; set; } = true;

        // Relación
        public ICollection<SolicitudCredito> Solicitudes { get; set; } = new List<SolicitudCredito>();
    }
}
