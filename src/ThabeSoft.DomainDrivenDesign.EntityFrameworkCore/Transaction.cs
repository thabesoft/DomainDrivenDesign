using Microsoft.EntityFrameworkCore.Storage;

namespace ThabeSoft.DomainDrivenDesign.EntityFrameworkCore;


/// <summary>
/// 包装了Efcore 的事务
/// </summary>
internal sealed class Transaction(IDbContextTransaction transaction) : ITransaction
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