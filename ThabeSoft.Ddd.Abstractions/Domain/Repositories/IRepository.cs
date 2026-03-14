using ThabeSoft.Ddd.Domain.Entities;

namespace ThabeSoft.Ddd.Domain.Repositories;


/// <summary>
/// 仓储标记接口
/// </summary>
public interface IRepository;


/// <summary>
/// 聚合根仓储契约
/// </summary>
/// <typeparam name="TEntity">聚合根类型</typeparam>
/// <typeparam name="TKey">聚合根的主键类型</typeparam>
public interface IRepository<TEntity, in TKey> : IRepository
    where TEntity : IAggregateRoot<TKey>
    where TKey : notnull
{
    /// <summary>
    /// 获取用于查询的 <see cref="IQueryable{TEntity}"/>。
    /// 允许在应用层或领域服务中构建灵活的查询逻辑。
    /// </summary>
    IQueryable<TEntity> Nodes { get; }

    /// <summary>
    /// 根据唯一标识符异步获取聚合根。
    /// </summary>
    /// <param name="id">主键 ID</param>
    /// <returns>聚合根实例，若不存在则返回 null</returns>
    ValueTask<TEntity?> FindByIdAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 将新的聚合根实例添加到仓储中。
    /// </summary>
    /// <param name="entity">聚合根实例</param>
    ValueTask AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// 移除指定的聚合根实例。
    /// </summary>
    /// <param name="entity">要删除的实体</param>
    ValueTask RemoveAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// 更新指定的聚合根实例
    /// </summary>
    /// <param name="entity">要更新的实体</param>
    ValueTask UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
}