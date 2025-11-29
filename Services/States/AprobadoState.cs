using RespuestaCredito.DTOs;
using RespuestaCredito.Interfaces;
using RespuestaCredito.Models;

namespace RespuestaCredito.Services.States
{
    public class AprobadoState : IEstadoCreditoStrategy
    {
        public void Validar(RecepcionCreditoDto dto)
        {
            if (dto.MontoAprobado == null || dto.MontoAprobado <= 0)
                throw new ArgumentException("El monto aprobado es obligatorio y debe ser positivo.");

            if (dto.Tasa == null)
                throw new ArgumentException("La tasa de interés es obligatoria.");
        }

        public void Procesar(RespuestaCreditoFinanciera entidad, RecepcionCreditoDto dto)
        {
            entidad.MontoAprobado = dto.MontoAprobado;
            entidad.TasaInteres = dto.Tasa;
            entidad.Observaciones = dto.Observacion ?? "Crédito Aprobado.";
        }
    }
}