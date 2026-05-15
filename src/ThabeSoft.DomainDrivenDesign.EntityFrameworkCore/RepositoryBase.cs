using Microsoft.EntityFrameworkCore;

namespace ThabeSoft.DomainDrivenDesign.EntityFrameworkCore;


/// <summary>
/// Ef-core 仓储基类
/// </summary>
public abstract class RepositoryBase<TDbContext, TEntity, TKey>(TDbContext dbContext) : IRepository<TEntity, TKey>
    where TDbContext : DbContext
    where TEntity : class, IAggregateRoot<TKey>
    where TKey : notnull, IEquatable<TKey>
{
    public virtual async ValueTask<TEntity?> FindByIdAsync(TKey key, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<TEntity>()
            .FindAsync([key], cancellationToken);
    }
    public virtual async ValueTask AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await dbContext
            .AddAsync(entity, cancellationToken);
    }
    public virtual async ValueTask RemoveAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.FindAsync<TEntity>([id], cancellationToken);
        if (entity is null) return;

        dbContext.Remove(entity);
    }
    public async virtual ValueTask<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<TEntity>()
            .AnyAsync(x => x.Id.Equals(id), cancellationToken);
    }


    public async virtual ValueTask<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<TEntity>()
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
    }
    public async virtual ValueTask<IReadOnlyCollection<TEntity>> GetPagedAsync(int take, int skip = 0, CancellationToken cancellationToken = default)
    {
        if (take <= 0) throw new ArgumentException("take 必须大于 0", nameof(take));
        if (skip < 0) throw new ArgumentException("skip 不能为负数", nameof(skip));

        return await dbContext.Set<TEntity>()
            .OrderBy(x => x.Id)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }
    public async virtual ValueTask<IReadOnlyCollection<TEntity>> GetAllAfterSkipAsync(int skip, CancellationToken cancellationToken = default)
    {
        if (skip < 0) throw new ArgumentException("skip 不能为负数", nameof(skip));

        return await dbContext.Set<TEntity>()
            .OrderBy(x => x.Id)
            .Skip(skip)
            .ToListAsync(cancellationToken);
    }

    public async virtual ValueTask<IReadOnlyCollection<TResult>> GetAllAsync<TResult>(CancellationToken cancellationToken = default) where TResult : TEntity
    {
        return await dbContext.Set<TEntity>()
            .OfType<TResult>()
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);
    }
    public async virtual ValueTask<IReadOnlyCollection<TResult>> GetPagedAsync<TResult>(int take, int skip = 0, CancellationToken cancellationToken = default) where TResult : TEntity
    {
        if (take <= 0) throw new ArgumentException("take 必须大于 0", nameof(take));
        if (skip < 0) throw new ArgumentException("skip 不能为负数", nameof(skip));

        return await dbContext.Set<TEntity>()
            .OfType<TResult>()
            .OrderBy(x => x.Id)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }
    public async virtual ValueTask<IReadOnlyCollection<TResult>> GetAllAfterSkipAsync<TResult>(int skip, CancellationToken cancellationToken = default) where TResult : TEntity
    {
        if (skip < 0) throw new ArgumentException("skip 不能为负数", nameof(skip));

        return await dbContext.Set<TEntity>()
            .OfType<TResult>()
            .OrderBy(x => x.Id)
            .Skip(skip)
            .ToListAsync(cancellationToken);
    }
}