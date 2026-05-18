using Microsoft.EntityFrameworkCore;

namespace ThabeSoft.DomainDrivenDesign;


/// <summary>
/// 包装了efcore的工作单元
/// </summary>
/// <typeparam name="TDbContext">关联的具体数据库上下文类型。</typeparam>
internal sealed class UnitOfWork<TDbContext>(TDbContext dbContext, IDomainEventPublisher publisher) : IUnitOfWork
    where TDbContext : DbContext
{
    public async ValueTask<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        return new Transaction(transaction);
    }

    public async ValueTask SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // 收集事件
        var domainEvents = ExtractAndClearDomainEvents();
        // 保存修改
        await dbContext.SaveChangesAsync(cancellationToken);
        // 发布事件
        await PublishDomainEvent(domainEvents, cancellationToken);
    }


    // 提取领域事件后清除
    private List<IDomainEvent> ExtractAndClearDomainEvents()
    {
        // 获取有领域事件的对象
        var eventProviders = dbContext.ChangeTracker
            .Entries<IDomainEventProvider>()
            .Select(x => x.Entity)
            .ToList();

        // 获取事件
        var events = eventProviders
            .SelectMany(x => x.DomainEvents)
            .OfType<IDomainEvent>()
            .ToList();

        if (events.Count == 0) return [];

        // 清空事件
        foreach (var i in eventProviders.OfType<IDomainEventClearable>()) i.ClearDomainEvents();
        return events;
    }

    // 发布领域事件
    private async ValueTask PublishDomainEvent(IReadOnlyCollection<IDomainEvent> domainEvents, CancellationToken cancellationToken)
    {
        foreach (var domainEvent in domainEvents)
        {
            await publisher.PublishAsync(domainEvent, cancellationToken);
        }
    }
}
