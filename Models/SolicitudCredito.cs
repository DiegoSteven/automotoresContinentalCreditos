using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RespuestaCredito.Models
{
    public class SolicitudCredito
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string NumeroSolicitud { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string NombreCliente { get; set; } = string.Empty;

        // Datos del Vehículo
        [MaxLength(50)]
        public string? MarcaVehiculo { get; set; }

        [MaxLength(100)]
        public string? ModeloVehiculo { get; set; }

        public int? AnioVehiculo { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? PrecioVehiculo { get; set; }

        [MaxLength(20)]
        public string? TipoVehiculo { get; set; } // Nuevo o Usado

        // Foreign Keys
        public int IdAsesor { get; set; }
        public int IdFinanciera { get; set; }

        // Navegación
        [ForeignKey("IdAsesor")]
        public Asesor? Asesor { get; set; }

        [ForeignKey("IdFinanciera")]
        public Financiera? Financiera { get; set; }

        // Relación con respuestas
        public ICollection<RespuestaCreditoFinanciera> Respuestas { get; set; } = new List<RespuestaCreditoFinanciera>();
    }
}