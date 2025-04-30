using SendGrid;
using SendGrid.Helpers.Mail;

namespace hyCommerce.Notification.Providers.SendGrid
{
    public class SendGridSender(ISendGridClient sendGridClient,
        SendGridOptions sendGridOptions, ILogger<SendGridSender> logger) : IEmailSender
    {
        private readonly ISendGridClient _sendGridClient = sendGridClient;
        private readonly SendGridOptions _sendGridOptions = sendGridOptions;
        private readonly ILogger<SendGridSender> _logger = logger;

        public async Task SendEmailAsync<T>(EmailRequest<T> request)
        {
            var msg = new SendGridMessage();
            var emails = new string[] { request.To }.Select(x => new EmailAddress(x)).ToList();
            msg.AddTos(emails);
            msg.SetTemplateId(request.TemplateId);
            msg.SetTemplateData(request.TemplateData);
            msg.SetFrom(_sendGridOptions.SenderEmail, _sendGridOptions.SenderName);

            var response = await _sendGridClient.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
                _logger.LogError(await response.Body.ReadAsStringAsync());
        }
    }
}
