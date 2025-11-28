using System.ComponentModel.DataAnnotations;

namespace RespuestaCredito.Models
{
    public class NotificacionAsesor
    {
        [Key]
        public int Id { get; set; }
        public int IdAsesor { get; set; }
        public int IdSolicitud { get; set; }
        [MaxLength(500)]
        public string Mensaje { get; set; } = string.Empty;
        public DateTime FechaNotificacion { get; set; } = DateTime.Now;
        public bool Leido { get; set; }
    }
}