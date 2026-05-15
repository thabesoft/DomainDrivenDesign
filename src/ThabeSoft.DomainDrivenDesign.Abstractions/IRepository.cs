namespace ThabeSoft.DomainDrivenDesign;


/// <summary>
/// 仓储
/// </summary>
/// <typeparam name="TEntity">聚合根类型</typeparam>
/// <typeparam name="TId">聚合根的主键类型</typeparam>
public interface IRepository<TEntity, in TId>
    where TEntity : IAggregateRoot<TId>
    where TId : notnull
{
    /// <summary>
    /// 查询
    /// </summary>
    IQueryable<TEntity> Query { get; }

    /// <summary>
    /// 根据Id查询
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns>如果不存在则返回 null</returns>
    ValueTask<TEntity?> FindByIdAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 根据Id删除实体
    /// </summary>
    /// <param name="id">主键</param>
    ValueTask RemoveByIdAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 添加实体
    /// </summary>
    /// <param name="entity">实体实例</param>
    ValueTask AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// 添加一些实体
    /// </summary>
    ValueTask AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
}