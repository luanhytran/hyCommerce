using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace hyCommerce.Notification.Providers.Smtp
{
    public class SmtpSender(IOptions<SmtpOptions> smtpOptions, ILogger<SmtpSender> logger) : IEmailSender
    {
        private readonly SmtpOptions _smtpOptions = smtpOptions.Value;
        private readonly ILogger<SmtpSender> _logger = logger;

        public async Task SendEmailAsync<T>(EmailRequest<T> request)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse(_smtpOptions.SenderEmail));
                message.To.Add(MailboxAddress.Parse(request.To));
                message.Subject = request.Subject;
                message.Body = new TextPart(request.IsHtml ? "html" : "plain")
                {
                    Text = request.Body
                };

                using var smtpClient = new SmtpClient();

                await smtpClient.ConnectAsync(_smtpOptions.SenderSmtpClient, 587, SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(_smtpOptions.SenderEmail, _smtpOptions.SenderPassword);
                await smtpClient.SendAsync(message);
                await smtpClient.DisconnectAsync(true);

                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
