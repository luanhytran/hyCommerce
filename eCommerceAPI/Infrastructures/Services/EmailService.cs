using eCommerceAPI.Core.Contracts.Services;
using eCommerceAPI.Core.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace eCommerceAPI.Infrastructures.Services;

public class EmailService(IOptions<EmailSettings> emailSettings) : IEmailService
{
    private readonly EmailSettings _emailSettings = emailSettings.Value;

    public void SendEmail(string to, string subject, string body, bool isHtml = false)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(_emailSettings.SenderEmail));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart(isHtml ? "html" : "plain")
        {
            Text = body
        };

        using var smtpClient = new SmtpClient();
        
        try
        {
            smtpClient.Connect(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
            smtpClient.Authenticate(_emailSettings.SenderEmail, _emailSettings.Password);
            smtpClient.Send(message);
            smtpClient.Disconnect(true);

            Console.WriteLine("Email sent successfully.");
        }
        catch(Exception ex)
        {
            Console.WriteLine("Error sending email: " + ex.Message);
        }
    }
}