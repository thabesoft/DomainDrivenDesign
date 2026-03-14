namespace ThabeSoft.Ddd.Domain.Events;

/// <summary>
/// 领域事件提供者
/// </summary>
public interface IDomainEventProvider
{
    /// <summary>
    /// 领域事件集合
    /// </summary>
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    /// <summary>
    /// 清除所有领域事件
    /// </summary>
    void ClearDomainEvents();
}