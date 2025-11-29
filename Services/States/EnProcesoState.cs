using RespuestaCredito.DTOs;
using RespuestaCredito.Interfaces;
using RespuestaCredito.Models;

namespace RespuestaCredito.Services.States
{
    public class EnProcesoState : IEstadoCreditoStrategy
    {
        private readonly int _tiempoEsperaMinutos;

        public EnProcesoState(int tiempoEsperaMinutos)
        {
            _tiempoEsperaMinutos = tiempoEsperaMinutos;
        }

        public void Validar(RecepcionCreditoDto dto)
        {
        }

        public void Procesar(RespuestaCreditoFinanciera entidad, RecepcionCreditoDto dto)
        {
            // Usar el parámetro de la financiera
            entidad.Observaciones = $"EN ANÁLISIS: Consultar nuevamente en {_tiempoEsperaMinutos} minutos.";
        }
    }
}