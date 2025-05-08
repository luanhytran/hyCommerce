using DotNetCore.CAP;

namespace hyCommerce.EventBus.Event;

public interface IIntegrationEventHandler<TIntegrationEvent> : ICapSubscribe
    where TIntegrationEvent : IntegrationEvent
{
    Task HandleAsync(TIntegrationEvent @event);
}