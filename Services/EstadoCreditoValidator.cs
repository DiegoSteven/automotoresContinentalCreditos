namespace RespuestaCredito.Services
{
    public static class EstadoCreditoValidator
    {
        private static readonly Dictionary<string, List<string>> TransicionesPermitidas = new()
        {
            { "NUEVA", new List<string> { "EN_PROCESO", "REQUIERE_DOCUMENTOS", "APROBADO", "CONDICIONADO", "NEGADO" } },
            { "EN_PROCESO", new List<string> { "REQUIERE_DOCUMENTOS", "CONDICIONADO", "APROBADO", "NEGADO" } },
            { "REQUIERE_DOCUMENTOS", new List<string> { "EN_PROCESO", "CONDICIONADO", "NEGADO" } },
            { "CONDICIONADO", new List<string> { "APROBADO", "NEGADO" } },
            { "APROBADO", new List<string>() },
            { "NEGADO", new List<string>() }
        };

        public static bool EsTransicionValida(string? estadoActual, string nuevoEstado)
        {
            var estadoOrigen = estadoActual ?? "NUEVA";

            if (!TransicionesPermitidas.ContainsKey(estadoOrigen))
            {
                return false;
            }

            return TransicionesPermitidas[estadoOrigen].Contains(nuevoEstado);
        }

        public static string ObtenerMensajeError(string? estadoActual, string nuevoEstado)
        {
            var estadoOrigen = estadoActual ?? "NUEVA";

            return estadoOrigen switch
            {
                "APROBADO" => $"La solicitud ya fue APROBADA. No se pueden procesar nuevas respuestas. Estado actual: {estadoActual}, Estado intentado: {nuevoEstado}",
                "NEGADO" => $"La solicitud fue NEGADA. Para solicitar un nuevo crédito, debe generarse una nueva solicitud. Estado actual: {estadoActual}, Estado intentado: {nuevoEstado}",
                "CONDICIONADO" => $"Las solicitudes CONDICIONADAS solo pueden pasar a APROBADO (si cumple condiciones) o NEGADO (si no cumple). Estado actual: {estadoActual}, Estado intentado: {nuevoEstado}",
                "REQUIERE_DOCUMENTOS" => $"Las solicitudes que requieren documentos deben pasar primero a EN_PROCESO o pueden ser NEGADAS. No pueden aprobarse directamente sin evaluación. Estado actual: {estadoActual}, Estado intentado: {nuevoEstado}",
                _ => $"Transición de estado no válida: {estadoOrigen} → {nuevoEstado}. Revise el flujo de estados permitido."
            };
        }

        public static bool EsEstadoFinal(string estado)
        {
            return estado == "APROBADO" || estado == "NEGADO";
        }
    }
}
