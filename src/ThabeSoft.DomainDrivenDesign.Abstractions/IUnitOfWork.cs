namespace ThabeSoft.DomainDrivenDesign;


/// <summary>
/// 工作单元契约：管理事务一致性与持久化提交
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// 保存所有更改
    /// </summary>
    ValueTask SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 开启事务
    /// </summary>
    ValueTask<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// 事务
/// </summary>
public interface ITransaction : IAsyncDisposable, IDisposable
{
    /// <summary>
    /// 提交
    /// </summary>
    ValueTask CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 回滚
    /// </summary>
    ValueTask RollbackAsync(CancellationToken cancellationToken = default);
}