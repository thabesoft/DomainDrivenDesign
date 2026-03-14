namespace ThabeSoft.Ddd.Domain.Events;

/// <summary>
/// 领域事件发布器接口
/// </summary>
public interface IDomainEventDispatcher
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent;
}