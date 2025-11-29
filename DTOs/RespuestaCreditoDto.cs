using System.Text.Json.Serialization;

namespace RespuestaCredito.DTOs
{
    public class RespuestaCreditoDto
    {
        public string NumeroSolicitud { get; set; } = string.Empty;
        public string NombreCliente { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? MontoAprobado { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? TasaInteres { get; set; }
        
        public DateTime FechaRespuesta { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Observaciones { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? Condiciones { get; set; }
        
        public DateTime FechaProceso { get; set; }
    }
}
