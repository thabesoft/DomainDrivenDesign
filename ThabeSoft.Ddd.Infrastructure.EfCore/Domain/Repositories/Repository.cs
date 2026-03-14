using Microsoft.EntityFrameworkCore;
using ThabeSoft.Ddd.Domain.Entities;

namespace ThabeSoft.Ddd.Domain.Repositories;

public class Repository<TDbContext, TEntity, TKey>(TDbContext dbContext) : IRepository<TEntity, TKey>
    where TDbContext : DbContext
    where TEntity : class, IAggregateRoot<TKey>
    where TKey : notnull
{
    public virtual IQueryable<TEntity> Nodes => dbContext.Set<TEntity>().AsQueryable();

    public virtual async ValueTask AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await dbContext.AddAsync(entity, cancellationToken);
    }

    public virtual async ValueTask<TEntity?> FindByIdAsync(TKey key, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<TEntity>().FindAsync([key], cancellationToken);
    }

    public virtual async ValueTask RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        dbContext.Remove(entity);
    }

    public virtual async ValueTask UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        dbContext.Update(entity);
    }
}