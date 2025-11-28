using System.ComponentModel.DataAnnotations;

namespace RespuestaCredito.DTOs
{
    public class RecepcionCreditoDto
    {
        [Required]
        public string NumeroSolicitud { get; set; } = string.Empty;

        [Required]
        // Validamos que solo entren estos textos exactos
        [RegularExpression("^(APROBADO|NEGADO|CONDICIONADO|REQUIERE_DOCUMENTOS|EN_PROCESO)$",
            ErrorMessage = "Estado no válido.")]
        public string Estado { get; set; } = string.Empty;

        public decimal? MontoAprobado { get; set; }
        public decimal? Tasa { get; set; }
        public string? Observacion { get; set; }
        public DateTime FechaRespuesta { get; set; }
    }
}