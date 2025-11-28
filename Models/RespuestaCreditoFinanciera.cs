using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RespuestaCredito.Models
{
    public class RespuestaCreditoFinanciera
    {
        [Key]
        public int Id { get; set; }

        public int IdSolicitud { get; set; }
        [ForeignKey("IdSolicitud")]
        public SolicitudCredito? Solicitud { get; set; }

        [Required, MaxLength(50)]
        public string Estado { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal? MontoAprobado { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? TasaInteres { get; set; }

        public DateTime FechaRespuesta { get; set; }
        public string? Observaciones { get; set; }
        public string? JsonCompleto { get; set; }
        public DateTime FechaProceso { get; set; } = DateTime.Now;
    }
}