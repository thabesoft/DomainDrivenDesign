using ThabeSoft.Mediator;

namespace ThabeSoft.DomainDrivenDesign;


/// <summary>
/// 中介者领域事件发布器
/// </summary>
internal sealed class MediatorDomainEventPublisher(IPublisher publisher) : IDomainEventPublisher
{
    public ValueTask PublishAsync(IDomainEvent @event, CancellationToken cancellationToken = default)
    {
        if (@event is not INotification notification)
        {
            throw new InvalidOperationException($"""
事件 {@event.GetType()} 必须实现 {typeof(INotification)} 接口。
请确保在应用层或基础设施层实现该接口。
""");
        }

        return publisher.PublishUntypedAsync(notification, cancellationToken);
    }
}
