using ThabeSoft.DomainDrivenDesign.Domain;
using ThabeSoft.Mediator;

namespace ThabeSoft.DomainDrivenDesign.Infrastructure;


/// <summary>
/// 中介者领域事件发布器
/// </summary>
internal sealed class MediatorDomainEventPublisher(IPublisher publisher) : IDomainEventPublisher
{
    public ValueTask PublishAsync(IDomainEvent @event, CancellationToken cancellationToken = default)
    {
        if (@event is not IMediatorDomainEvent mediatorDomainEvent) return default;
        return publisher.PublishAsync(mediatorDomainEvent, cancellationToken);
    }
}