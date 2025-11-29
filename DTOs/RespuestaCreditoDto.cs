namespace RespuestaCredito.DTOs
{
    public class RespuestaCreditoDto
    {
        public string NumeroSolicitud { get; set; } = string.Empty;
        public string NombreCliente { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public decimal? MontoAprobado { get; set; }
        public decimal? TasaInteres { get; set; }
        public DateTime FechaRespuesta { get; set; }
        public string? Observaciones { get; set; }
        public List<string>? Condiciones { get; set; }
        public DateTime FechaProceso { get; set; }
    }
}
