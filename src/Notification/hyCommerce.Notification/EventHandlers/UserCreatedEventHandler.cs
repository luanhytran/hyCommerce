using DotNetCore.CAP;
using hyCommerce.Common.Event;
using hyCommerce.EventBus.Event;
using hyCommerce.Notification.Providers;
using Newtonsoft.Json;

namespace hyCommerce.Notification.EventHandlers;

public class UserCreatedEventHandler(IEmailSender emailSender, ILogger<UserCreatedEventHandler> logger) : IIntegrationEventHandler<UserCreatedEvent>
{
    [CapSubscribe(nameof(UserCreatedEvent))]
    public async Task HandleAsync(UserCreatedEvent @event)
    {
        logger.LogInformation($"Message Received: {JsonConvert.SerializeObject(@event)}");
        
        var emailRequest = new EmailRequest<object>
        {
            To = @event.Email,
            Subject = "Confirm your account",
            Body = $"Please confirm your account by clicking this link: <a href='{@event.ReturnUrl}'>Confirm Email</a>",
            IsHtml = true,
        };

       await  emailSender.SendEmailAsync(emailRequest);
    }
}