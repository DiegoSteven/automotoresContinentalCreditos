using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RespuestaCredito.DTOs;
using RespuestaCredito.Interfaces;

namespace RespuestaCredito.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requiere autenticación JWT
    public class CreditosController : ControllerBase
    {
        private readonly ICreditoService _creditoService;

        public CreditosController(ICreditoService creditoService)
        {
            _creditoService = creditoService;
        }

        [HttpPost("respuesta")]
        public async Task<IActionResult> RecibirRespuesta([FromBody] RecepcionCreditoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _creditoService.ProcesarRespuestaAsync(dto);
                return Ok(new { mensaje = "Respuesta procesada correctamente y asesor notificado." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Error interno del servidor", detalle = ex.Message });
            }
        }

        [HttpGet("respuesta/{numeroSolicitud}")]
        public async Task<IActionResult> ConsultarRespuesta(string numeroSolicitud)
        {
            var respuesta = await _creditoService.ConsultarRespuestaAsync(numeroSolicitud);

            if (respuesta == null)
                return NotFound(new { mensaje = "No hay respuestas registradas para esta solicitud." });

            return Ok(respuesta);
        }
    }
}