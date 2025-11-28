using System.ComponentModel.DataAnnotations;

namespace RespuestaCredito.Models
{
    public class SolicitudCredito
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(20)]
        public string NumeroSolicitud { get; set; } = string.Empty;
        public int IdAsesor { get; set; }
        public string NombreCliente { get; set; } = string.Empty;

        // Relación
        public ICollection<RespuestaCreditoFinanciera> Respuestas { get; set; }
    }
}