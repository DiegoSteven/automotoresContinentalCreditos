using RespuestaCredito.Interfaces;
using RespuestaCredito.Services.States;

namespace RespuestaCredito.Services
{
    public class CreditoStateFactory
    {
        // Método para obtener la estrategia según el estado
        // Ahora recibe parámetros adicionales para estados que los requieran
        public IEstadoCreditoStrategy GetStrategy(string estado, int tiempoEsperaMinutos = 30)
        {
            return estado.ToUpper() switch
            {
                "APROBADO" => new AprobadoState(),
                "NEGADO" => new NegadoState(),
                "CONDICIONADO" => new CondicionadoState(),
                "REQUIERE_DOCUMENTOS" => new RequiereDocumentosState(),
                "EN_PROCESO" => new EnProcesoState(tiempoEsperaMinutos),
                _ => throw new ArgumentException($"El estado '{estado}' no está configurado en el sistema.")
            };
        }
    }
}