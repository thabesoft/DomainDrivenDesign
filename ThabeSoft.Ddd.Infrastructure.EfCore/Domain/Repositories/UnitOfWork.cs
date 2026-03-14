using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ThabeSoft.Ddd.Domain.Events;

namespace ThabeSoft.Ddd.Domain.Repositories;

/// <summary>
/// 工作单元的具体实现。
/// 通过包装 DbContext，为领域层提供统一的持久化和事务控制入口。
/// </summary>
/// <typeparam name="TDbContext">关联的具体数据库上下文类型。</typeparam>
internal class UnitOfWork<TDbContext>(TDbContext dbContext, IDomainEventDispatcher domainEventDispatcher) : IUnitOfWork
    where TDbContext : DbContext
{
    /// <summary>
    /// 显式开启一个异步数据库事务。
    /// 用于需要跨多个 SaveChangesAsync 操作或确保多表操作原子性的场景。
    /// </summary>
    public async ValueTask<IDomainTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        // 开启 EF Core 原生事务
        var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        // 将原生事务包装为领域事务对象返回，实现与 EF 的解耦
        return new DomainTransaction(transaction);
    }

    /// <summary>
    /// 将内存中所有受跟踪的实体更改持久化到数据库。
    /// </summary>
    /// <returns>影响的行数。</returns>
    public async ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // 获取所有待发布的事件
        var domainEvents = ExtractAndClearDomainEvents();

        // 持久化数据
        var result = await dbContext.SaveChangesAsync(cancellationToken);

        // 发布事件 (在数据保存成功后)
        foreach (var domainEvent in domainEvents)
        {
            await domainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        }

        return result;
    }


    private List<IDomainEvent> ExtractAndClearDomainEvents()
    {
        // 筛选出所有实现了 IDomainEventProvider 的实体（无论它是状态是什么）
        var eventProviders = dbContext.ChangeTracker
            .Entries<IDomainEventProvider>()
            .Select(x => x.Entity)
            .ToList();

        var events = eventProviders
            .SelectMany(x => x.DomainEvents)
            .ToList();

        // 立即清除实体内部的事件缓存，防止重复发布
        eventProviders.ForEach(x => x.ClearDomainEvents());

        return events;
    }
}

/// <summary>
/// 领域事务包装器。
/// 封装了底层 IDbContextTransaction，使其符合领域层的接口契约。
/// </summary>
internal class DomainTransaction(IDbContextTransaction transaction) : IDomainTransaction
{
    /// <summary>
    /// 提交当前事务，使所有数据库更改永久生效。
    /// </summary>
    public async ValueTask CommitAsync(CancellationToken cancellationToken = default)
    {
        await transaction.CommitAsync(cancellationToken);
    }

    /// <summary>
    /// 回滚当前事务，撤销该事务内执行的所有数据库操作。
    /// </summary>
    public async ValueTask RollbackAsync(CancellationToken cancellationToken = default)
    {
        await transaction.RollbackAsync(cancellationToken);
    }

    /// <summary>
    /// 释放非托管资源（同步）。
    /// </summary>
    public void Dispose()
    {
        transaction.Dispose();
    }

    /// <summary>
    /// 异步释放非托管资源。
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await transaction.DisposeAsync();
    }
}