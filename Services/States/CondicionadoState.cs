using System.Text.Json;
using RespuestaCredito.DTOs;
using RespuestaCredito.Interfaces;
using RespuestaCredito.Models;

namespace RespuestaCredito.Services.States
{
    public class CondicionadoState : IEstadoCreditoStrategy
    {
        public void Validar(RecepcionCreditoDto dto)
        {
            // Validar que se envíen condiciones específicas
            if (dto.Condiciones == null || !dto.Condiciones.Any())
                throw new ArgumentException("Debe especificar las condiciones para la aprobación del crédito.");

            // Validar que el monto y tasa estén presentes
            if (!dto.MontoAprobado.HasValue)
                throw new ArgumentException("El monto aprobado es obligatorio para crédito CONDICIONADO.");
        }

        public void Procesar(RespuestaCreditoFinanciera entidad, RecepcionCreditoDto dto)
        {
            entidad.MontoAprobado = dto.MontoAprobado;
            entidad.TasaInteres = dto.Tasa;

            // Guardar condiciones en formato JSON
            entidad.CondicionesJson = JsonSerializer.Serialize(dto.Condiciones);

            // Crear observación legible
            string condicionesTexto = string.Join(", ", dto.Condiciones!);
            entidad.Observaciones = $"APROBADO CON CONDICIONES: {condicionesTexto}";

            if (!string.IsNullOrWhiteSpace(dto.Observacion))
                entidad.Observaciones += $". {dto.Observacion}";
        }
    }
}