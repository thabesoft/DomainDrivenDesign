using Microsoft.EntityFrameworkCore;

namespace ThabeSoft.DomainDrivenDesign.EntityFrameworkCore;


/// <summary>
/// Ef-core 仓储基类
/// </summary>
public class Repository<TDbContext, TEntity, TId>(TDbContext dbContext) : IRepository<TEntity, TId>
    where TDbContext : DbContext
    where TEntity : class, IAggregateRoot<TId>
    where TId : notnull
{
    private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

    public IQueryable<TEntity> Query => _dbSet;


    public ValueTask<TEntity?> FindByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        return _dbSet
            .FindAsync([id], cancellationToken);
    }
    public async ValueTask RemoveByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        var tracked = _dbSet.Local.FirstOrDefault(e => e.Id.Equals(id));

        // 本地有跟踪，直接标记删除
        if (tracked is not null)
        {
            _dbSet.Remove(tracked);
            return;
        }

        // 查询删除
        var finded = await _dbSet.FindAsync([id], cancellationToken);
        if (finded is not null) _dbSet.Remove(finded);
    }

    public async ValueTask AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await _dbSet
            .AddAsync(entity, cancellationToken);
    }

    public async ValueTask AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entities);

        await _dbSet
           .AddRangeAsync(entities, cancellationToken);
    }
}