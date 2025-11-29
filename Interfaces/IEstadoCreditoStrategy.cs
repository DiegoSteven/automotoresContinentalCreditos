using RespuestaCredito.DTOs;
using RespuestaCredito.Models;

namespace RespuestaCredito.Interfaces
{
    public interface IEstadoCreditoStrategy
    {
        // Método para validar reglas de negocio específicas (ej: Monto > 0)
        void Validar(RecepcionCreditoDto dto);

        // Método para preparar la entidad final
        void Procesar(RespuestaCreditoFinanciera entidad, RecepcionCreditoDto dto);
    }
}