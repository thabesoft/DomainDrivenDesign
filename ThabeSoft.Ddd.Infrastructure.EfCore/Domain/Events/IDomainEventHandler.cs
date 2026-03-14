namespace ThabeSoft.Ddd.Domain.Events;

/// <summary>
/// 领域事件处理器接口
/// </summary>
public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken);
}