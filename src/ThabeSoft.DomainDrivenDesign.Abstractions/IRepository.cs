namespace ThabeSoft.DomainDrivenDesign;


/// <summary>
/// 仓储
/// </summary>
/// <typeparam name="TEntity">聚合根类型</typeparam>
/// <typeparam name="TKey">聚合根的主键类型</typeparam>
public interface IRepository<TEntity, in TKey>
    where TEntity : IAggregateRoot<TKey>
    where TKey : notnull
{
    /// <summary>
    /// 根据Id查询
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns>如果不存在则返回 null</returns>
    ValueTask<TEntity?> FindByIdAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 添加实体
    /// </summary>
    /// <param name="entity">实体实例</param>
    ValueTask AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// 删除实体
    /// </summary>
    /// <param name="id">主键</param>
    ValueTask RemoveAsync(TKey id, CancellationToken cancellationToken = default);


    /// <summary>
    /// 实体是否存在
    /// </summary>
    ValueTask<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取所有
    /// </summary>
    ValueTask<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    /// <summary>
    /// 分页获取
    /// </summary>
    ValueTask<IReadOnlyCollection<TEntity>> GetPagedAsync(int take, int skip = 0, CancellationToken cancellationToken = default);
    /// <summary>
    /// 跳过n个获取
    /// </summary>
    ValueTask<IReadOnlyCollection<TEntity>> GetAllAfterSkipAsync(int skip, CancellationToken cancellationToken = default);


    ValueTask<IReadOnlyCollection<TResult>> GetAllAsync<TResult>(CancellationToken cancellationToken = default) where TResult : TEntity;
    ValueTask<IReadOnlyCollection<TResult>> GetPagedAsync<TResult>(int take, int skip = 0, CancellationToken cancellationToken = default) where TResult : TEntity;
    ValueTask<IReadOnlyCollection<TResult>> GetAllAfterSkipAsync<TResult>(int skip, CancellationToken cancellationToken = default) where TResult : TEntity;
}