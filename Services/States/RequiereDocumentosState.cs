using RespuestaCredito.DTOs;
using RespuestaCredito.Interfaces;
using RespuestaCredito.Models;

namespace RespuestaCredito.Services.States
{
    public class RequiereDocumentosState : IEstadoCreditoStrategy
    {
        public void Validar(RecepcionCreditoDto dto)
        {
            if (dto.ListaDocumentos == null || !dto.ListaDocumentos.Any())
                throw new ArgumentException("Debe enviar la lista de documentos faltantes.");
        }

        public void Procesar(RespuestaCreditoFinanciera entidad, RecepcionCreditoDto dto)
        {
            // Convertimos la lista ["Cédula", "Recibo Luz"] en un string "Cédula, Recibo Luz"
            string documentosFaltantes = string.Join(", ", dto.ListaDocumentos!);

            entidad.Observaciones = $"DOCUMENTACIÓN PENDIENTE: {documentosFaltantes}. {dto.Observacion}";

        }
    }
}