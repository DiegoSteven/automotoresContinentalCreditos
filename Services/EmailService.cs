using System.Net;
using System.Net.Mail;
using RespuestaCredito.Interfaces;

namespace RespuestaCredito.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task EnviarNotificacionAsync(string destinatario, string asunto, string mensaje)
        {
            try
            {
                var smtpHost = _configuration["Email:SmtpHost"];
                var smtpPort = int.Parse(_configuration["Email:SmtpPort"]!);
                var smtpUser = _configuration["Email:SmtpUser"];
                var smtpPass = _configuration["Email:SmtpPassword"];
                var fromEmail = _configuration["Email:FromEmail"];
                var fromName = _configuration["Email:FromName"];

                using var client = new SmtpClient(smtpHost, smtpPort)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential(smtpUser, smtpPass)
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail!, fromName),
                    Subject = asunto,
                    Body = mensaje,
                    IsBodyHtml = false
                };

                mailMessage.To.Add(destinatario);

                await client.SendMailAsync(mailMessage);

                _logger.LogInformation("Email enviado exitosamente a {Destinatario}", destinatario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar email a {Destinatario}", destinatario);
            }
        }
    }
}
