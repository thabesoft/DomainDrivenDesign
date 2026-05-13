using ThabeSoft.DomainDrivenDesign.Infrastructure;

namespace ThabeSoft.DomainDrivenDesign.Domain;


/// <summary>
/// 聚合根基类
/// </summary>
/// <typeparam name="TKey">主键类型</typeparam>
/// <param name="id">聚合根的唯一标识</param>
public abstract class AggregateRoot<TKey>(TKey id) : Entity<TKey>(id), IAggregateRoot<TKey>, IDomainEventProvider, IDomainEventClearable
    where TKey : notnull
{
    protected AggregateRoot() : this(default!)
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