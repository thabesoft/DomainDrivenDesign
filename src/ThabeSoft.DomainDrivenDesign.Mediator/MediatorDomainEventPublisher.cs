using ThabeSoft.Mediator;

namespace ThabeSoft.DomainDrivenDesign.Mediator;


/// <summary>
/// 中介者领域事件发布器
/// </summary>
internal sealed class MediatorDomainEventPublisher(IPublisher publisher) : IDomainEventPublisher
{
    public ValueTask PublishAsync(IDomainEvent @event, CancellationToken cancellationToken = default)
    {
        return publisher.PublishUntypedAsync(@event, cancellationToken);
    }
}