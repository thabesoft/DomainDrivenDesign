namespace ThabeSoft.DomainDrivenDesign;


/// <summary>
/// 领域事件发布器
/// </summary>
public interface IDomainEventPublisher
{
    /// <summary>
    /// 发布事件
    /// </summary>
    ValueTask PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : IDomainEvent;
}