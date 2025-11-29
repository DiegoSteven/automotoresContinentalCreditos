using RespuestaCredito.DTOs;
using RespuestaCredito.Interfaces;
using RespuestaCredito.Models;

namespace RespuestaCredito.Services.States
{
    public class CondicionadoState : IEstadoCreditoStrategy
    {
        public void Validar(RecepcionCreditoDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Observacion))
                throw new ArgumentException("Debe especificar la condición para la aprobación.");

        }

        public void Procesar(RespuestaCreditoFinanciera entidad, RecepcionCreditoDto dto)
        {
            entidad.MontoAprobado = dto.MontoAprobado;
            entidad.TasaInteres = dto.Tasa;
            entidad.Observaciones = $"APROBADO CON CONDICIÓN: {dto.Observacion}";
        }
    }
}