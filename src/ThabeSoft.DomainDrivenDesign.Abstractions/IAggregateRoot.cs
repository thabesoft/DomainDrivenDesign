namespace ThabeSoft.DomainDrivenDesign;


/// <summary>
/// 聚合根
/// </summary>
/// <typeparam name="TId">强类型主键</typeparam>
public interface IAggregateRoot<out TId> : IEntity<TId>
    where TId : notnull;


/// <summary>
/// 聚合根基类
/// </summary>
/// <typeparam name="TKey">主键类型</typeparam>
public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>, IDomainEventProvider, IDomainEventClearable
    where TKey : notnull
{
    protected AggregateRoot()
    {
    }

    protected AggregateRoot(TKey id) : base(id)
    {
    }


    // 领域事件
    private readonly List<IDomainEvent> _domainEvents = [];
    // 领域事件
    IReadOnlyCollection<IDomainEvent> IDomainEventProvider.DomainEvents => _domainEvents.AsReadOnly();


    /// <summary>
    /// 添加领域事件
    /// </summary>
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    void IDomainEventClearable.ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}