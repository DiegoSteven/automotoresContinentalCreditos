using RespuestaCredito.DTOs;
using RespuestaCredito.Interfaces;
using RespuestaCredito.Models;

namespace RespuestaCredito.Services.States
{
    public class EnProcesoState : IEstadoCreditoStrategy
    {
        public void Validar(RecepcionCreditoDto dto)
        {
            // No hay validación estricta, la financiera solo nos dice "espere"
        }

        public void Procesar(RespuestaCreditoFinanciera entidad, RecepcionCreditoDto dto)
        {
            // Simulamos leer un parámetro de configuración (ej: 30 minutos)
            int tiempoEsperaMinutos = 30;
            entidad.Observaciones = $"EN ANÁLISIS: Consultar nuevamente en {tiempoEsperaMinutos} minutos.";
        }
    }
}