using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using VillustraTask.Api.Interfaces;

namespace VillustraTask.Api.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromEmail;

        public EmailService(IConfiguration configuration)
        {
            var host = configuration["Smtp:Host"];
            var port = int.Parse(configuration["Smtp:Port"]);
            var username = configuration["Smtp:Username"];
            var password = configuration["Smtp:Password"];
            _fromEmail = configuration["Smtp:FromEmail"];

            _smtpClient = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = bool.Parse(configuration["Smtp:EnableSsl"] ?? "true")
            };
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using var mailMessage = new MailMessage(_fromEmail, toEmail, subject, body)
            {
                IsBodyHtml = true
            };
            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
