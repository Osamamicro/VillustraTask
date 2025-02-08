using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VillustraTask.Api.Interfaces;

namespace VillustraTask.Api.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        private readonly string _fromEmail;
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly bool _enableSsl;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            _smtpHost = _configuration["Smtp:Host"] ?? throw new ArgumentNullException("Smtp:Host is required.");
            _smtpPort = int.TryParse(_configuration["Smtp:Port"], out int port) ? port : throw new ArgumentException("Smtp:Port must be a valid number.");
            _smtpUsername = _configuration["Smtp:Username"] ?? throw new ArgumentNullException("Smtp:Username is required.");
            _smtpPassword = _configuration["Smtp:Password"] ?? throw new ArgumentNullException("Smtp:Password is required.");
            _fromEmail = _configuration["Smtp:FromEmail"] ?? throw new ArgumentNullException("Smtp:FromEmail is required.");
            _enableSsl = bool.TryParse(_configuration["Smtp:EnableSsl"], out bool ssl) && ssl;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                using var smtpClient = new SmtpClient(_smtpHost, _smtpPort)
                {
                    Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                    EnableSsl = _enableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false
                };

                using var mailMessage = new MailMessage(_fromEmail, toEmail, subject, body)
                {
                    IsBodyHtml = true
                };

                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation($"Email sent successfully to {toEmail}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send email to {toEmail}. Error: {ex.Message}");
                throw;
            }
        }
    }
}
