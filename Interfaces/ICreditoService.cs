using RespuestaCredito.DTOs;
using RespuestaCredito.Models;

namespace RespuestaCredito.Interfaces
{
    public interface ICreditoService
    {
        // Procesa la respuesta de la financiera (POST)
        Task ProcesarRespuestaAsync(RecepcionCreditoDto dto);

        // Consulta el historial de una solicitud (GET)
        Task<RespuestaCreditoDto?> ConsultarRespuestaAsync(string numeroSolicitud);
    }
}