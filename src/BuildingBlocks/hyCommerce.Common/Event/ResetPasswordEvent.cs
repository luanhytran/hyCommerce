using hyCommerce.EventBus.Event;

namespace hyCommerce.Common.Event;

public class ResetPasswordEvent : IntegrationEvent
{
    public string Email { get; set; }

    public string UserDisplayName { get; set; }

    public string ReturnUrl { get; set; }
}