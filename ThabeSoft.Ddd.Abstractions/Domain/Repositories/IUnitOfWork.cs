namespace ThabeSoft.Ddd.Domain.Repositories;

/// <summary>
/// 工作单元契约：管理事务一致性与持久化提交。
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// 将所有变更持久化到数据库。
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>受影响的行数</returns>
    ValueTask<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 开启一个显式的异步事务。
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>事务操作对象</returns>
    ValueTask<IDomainTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// 领域事务契约：提供对数据库事务的细粒度控制。
/// </summary>
public interface IDomainTransaction : IAsyncDisposable, IDisposable
{
    /// <summary>
    /// 提交当前事务。
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    ValueTask CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 回滚当前事务。
    /// </summary>
    /// <param name="cancellationToken">取消令牌</param>
    ValueTask RollbackAsync(CancellationToken cancellationToken = default);
}