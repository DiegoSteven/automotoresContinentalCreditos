namespace RespuestaCredito.Interfaces
{
    public interface IEmailService
    {
        Task EnviarNotificacionAsync(string destinatario, string asunto, string mensaje);
    }
}
