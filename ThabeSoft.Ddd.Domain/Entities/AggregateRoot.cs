using ThabeSoft.Ddd.Domain.Events;

namespace ThabeSoft.Ddd.Domain.Entities;

/// <summary>
/// 聚合根基类：聚合根是领域驱动设计（DDD）中的核心概念。
/// 它是外部对象访问聚合内实体的唯一入口，负责维护其内部实体的一致性。
/// 聚合根通常也是领域事件的产生者。
/// </summary>
/// <typeparam name="TKey">主键类型</typeparam>
public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>, IDomainEventProvider
    where TKey : notnull
{
    // 用于存储该聚合生命周期内产生的领域事件
    private readonly List<IDomainEvent> _domainEvents = [];

    /// <summary>
    /// 获取当前聚合根产生的所有领域事件
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// 初始化聚合根的新实例。
    /// </summary>
    /// <param name="id">聚合根的唯一标识</param>
    protected AggregateRoot(TKey id) : base(id)
    {
        Id = id;
    }

    /// <summary>
    /// 添加领域事件。
    /// 此方法通常在聚合根的业务方法内部调用，表示某个重要的业务动作已经发生。
    /// </summary>
    /// <param name="domainEvent">领域事件实例</param>
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// 清空所有的领域事件。
    /// 此方法通常由基础设施层（如 Repository 或 Unit of Work）在成功完成事务持久化后调用，
    /// 以防止事件被重复发布。
    /// </summary>
    void IDomainEventProvider.ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
