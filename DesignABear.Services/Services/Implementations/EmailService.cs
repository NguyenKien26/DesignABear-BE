using System.Net;
using System.Net.Mail;
using DesignABear.Services.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DesignABear.Services.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            try
            {
                var host = _configuration["Smtp:Host"];
                var port = int.Parse(_configuration["Smtp:Port"] ?? "587");
                var enableSsl = bool.Parse(_configuration["Smtp:EnableSsl"] ?? "true");
                var username = _configuration["Smtp:Username"];
                var password = _configuration["Smtp:Password"];

                if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    _logger.LogError("SMTP settings are missing in configuration.");
                    throw new InvalidOperationException("SMTP settings are not fully configured.");
                }

                using var client = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = enableSsl
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(username, "DesignABear"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml
                };

                mailMessage.To.Add(to);

                await client.SendMailAsync(mailMessage);
                _logger.LogInformation("Email successfully sent to {To} with subject {Subject}", to, subject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {To}", to);
                throw new Exception("Failed to send email.", ex);
            }
        }
    }
}
