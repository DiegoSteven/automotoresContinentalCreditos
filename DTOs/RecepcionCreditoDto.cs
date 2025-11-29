using System.ComponentModel.DataAnnotations;

namespace RespuestaCredito.DTOs
{
    public class RecepcionCreditoDto
    {
        [Required]
        public string NumeroSolicitud { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(APROBADO|NEGADO|CONDICIONADO|REQUIERE_DOCUMENTOS|EN_PROCESO)$",
            ErrorMessage = "Estado no válido.")]
        public string Estado { get; set; } = string.Empty;

        public decimal? MontoAprobado { get; set; }
        public decimal? Tasa { get; set; }
        public string? Observacion { get; set; }

        // Para el estado REQUIERE_DOCUMENTOS
        public List<string>? ListaDocumentos { get; set; }

        // Para el estado CONDICIONADO
        public List<string>? Condiciones { get; set; }

        public DateTime FechaRespuesta { get; set; }
    }
}