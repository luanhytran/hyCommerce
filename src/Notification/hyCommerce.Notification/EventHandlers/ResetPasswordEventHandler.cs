using DotNetCore.CAP;
using hyCommerce.Common.Event;
using hyCommerce.EventBus.Event;
using hyCommerce.Notification.Providers;
using Newtonsoft.Json;

namespace hyCommerce.Notification.EventHandlers;

public class ResetPasswordEventHandler(IEmailSender emailSender, ILogger<ResetPasswordEventHandler> logger) : IIntegrationEventHandler<ResetPasswordEvent>
{
    [CapSubscribe(nameof(ResetPasswordEvent))]
    public async Task HandleAsync(ResetPasswordEvent @event)
    {
        logger.LogInformation($"Message Received: {JsonConvert.SerializeObject(@event)}");
        
        var emailRequest = new EmailRequest<object>
        {
            To = @event.Email,
            Subject = "Reset your password",
            Body = $"Please reset your password by clicking this link: <a href={@event.ReturnUrl}>Reset Password</a>",
            IsHtml = true,
        };

        await  emailSender.SendEmailAsync(emailRequest);
    }
}