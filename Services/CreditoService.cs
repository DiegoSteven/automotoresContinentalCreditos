using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using RespuestaCredito.Data;
using RespuestaCredito.DTOs;
using RespuestaCredito.Interfaces;
using RespuestaCredito.Models;

namespace RespuestaCredito.Services
{
    public class CreditoService : ICreditoService
    {
        private readonly AppDbContext _context;
        private readonly CreditoStateFactory _stateFactory;
        private readonly ILogger<CreditoService> _logger;
        private readonly IEmailService _emailService;

        public CreditoService(
            AppDbContext context, 
            CreditoStateFactory stateFactory, 
            ILogger<CreditoService> logger,
            IEmailService emailService)
        {
            _context = context;
            _stateFactory = stateFactory;
            _logger = logger;
            _emailService = emailService;
        }

        public async Task ProcesarRespuestaAsync(RecepcionCreditoDto dto)
        {
            _logger.LogInformation("Iniciando procesamiento de respuesta para solicitud {NumeroSolicitud} con estado {Estado}", 
                dto.NumeroSolicitud, dto.Estado);

            try
            {
                var solicitud = await _context.Solicitudes
                    .Include(s => s.Financiera)
                    .Include(s => s.Asesor)
                    .FirstOrDefaultAsync(s => s.NumeroSolicitud == dto.NumeroSolicitud);

                if (solicitud == null)
                {
                    _logger.LogWarning("Solicitud {NumeroSolicitud} no encontrada", dto.NumeroSolicitud);
                    throw new KeyNotFoundException($"La solicitud {dto.NumeroSolicitud} no existe en el sistema.");
                }

                var strategy = _stateFactory.GetStrategy(dto.Estado, solicitud.Financiera?.TiempoEsperaMinutos ?? 30);
                strategy.Validar(dto);

                var nuevaRespuesta = new RespuestaCreditoFinanciera
                {
                    IdSolicitud = solicitud.Id,
                    Estado = dto.Estado,
                    FechaRespuesta = dto.FechaRespuesta,
                    JsonCompleto = JsonSerializer.Serialize(dto),
                    FechaProceso = DateTime.Now
                };

                strategy.Procesar(nuevaRespuesta, dto);

                var mensaje = $"La financiera {solicitud.Financiera?.Nombre ?? "externa"} respondió {dto.Estado} para la solicitud {dto.NumeroSolicitud}.";
                
                var notificacion = new NotificacionAsesor
                {
                    IdAsesor = solicitud.IdAsesor,
                    IdSolicitud = solicitud.Id,
                    Mensaje = mensaje,
                    Leido = false,
                    FechaNotificacion = DateTime.Now
                };

                _context.RespuestasFinanciera.Add(nuevaRespuesta);
                _context.Notificaciones.Add(notificacion);
                await _context.SaveChangesAsync();

                await _emailService.EnviarNotificacionAsync(
                    solicitud.Asesor!.Email,
                    $"Respuesta de Crédito - {dto.NumeroSolicitud}",
                    mensaje
                );

                _logger.LogInformation("Respuesta procesada exitosamente para solicitud {NumeroSolicitud}. Asesor {IdAsesor} notificado.", 
                    dto.NumeroSolicitud, solicitud.IdAsesor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar respuesta para solicitud {NumeroSolicitud}", dto.NumeroSolicitud);
                throw;
            }
        }

        public async Task<RespuestaCreditoDto?> ConsultarRespuestaAsync(string numeroSolicitud)
        {
            _logger.LogInformation("Consultando respuesta para solicitud {NumeroSolicitud}", numeroSolicitud);

            var respuesta = await _context.RespuestasFinanciera
                .Include(r => r.Solicitud)
                .Where(r => r.Solicitud!.NumeroSolicitud == numeroSolicitud)
                .OrderByDescending(r => r.FechaRespuesta)
                .FirstOrDefaultAsync();

            if (respuesta == null)
                return null;

            var dto = new RespuestaCreditoDto
            {
                NumeroSolicitud = respuesta.Solicitud!.NumeroSolicitud,
                NombreCliente = respuesta.Solicitud.NombreCliente,
                Estado = respuesta.Estado,
                MontoAprobado = respuesta.MontoAprobado,
                TasaInteres = respuesta.TasaInteres,
                FechaRespuesta = respuesta.FechaRespuesta,
                Observaciones = respuesta.Observaciones,
                FechaProceso = respuesta.FechaProceso
            };

            if (!string.IsNullOrEmpty(respuesta.CondicionesJson))
            {
                dto.Condiciones = JsonSerializer.Deserialize<List<string>>(respuesta.CondicionesJson);
            }

            return dto;
        }
    }
}