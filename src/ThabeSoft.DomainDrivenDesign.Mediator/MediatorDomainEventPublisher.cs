using ThabeSoft.Mediator;

namespace ThabeSoft.DomainDrivenDesign.Mediator;


/// <summary>
/// 中介者领域事件发布器
/// </summary>
internal sealed class MediatorDomainEventPublisher(IPublisher publisher) : IDomainEventPublisher
{
    public ValueTask PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : IDomainEvent
    {
        return publisher.PublishAsync(@event, cancellationToken);
    }
}