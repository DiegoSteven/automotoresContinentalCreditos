using RespuestaCredito.DTOs;
using RespuestaCredito.Interfaces;
using RespuestaCredito.Models;

namespace RespuestaCredito.Services.States
{
    public class NegadoState : IEstadoCreditoStrategy
    {
        public void Validar(RecepcionCreditoDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Observacion))
                throw new ArgumentException("Es obligatorio indicar el motivo del rechazo en la observación.");
        }

        public void Procesar(RespuestaCreditoFinanciera entidad, RecepcionCreditoDto dto)
        {
            entidad.MontoAprobado = 0;
            entidad.Observaciones = $"RECHAZADO: {dto.Observacion}";
        }
    }
}