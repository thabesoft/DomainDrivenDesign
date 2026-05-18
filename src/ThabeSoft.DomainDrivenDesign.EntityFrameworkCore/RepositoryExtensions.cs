using Microsoft.EntityFrameworkCore;

namespace ThabeSoft.DomainDrivenDesign;

public static class RepositoryExtensions
{
    extension<TEntity, TKey>(IRepository<TEntity, TKey> repository)
         where TEntity : IAggregateRoot<TKey>
        where TKey : notnull
    {
        public Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return repository
                .Query
                .AnyAsync(x => x.Id.Equals(id), cancellationToken);
        }

        public async Task<IReadOnlyCollection<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await repository.Query
                .OrderBy(x => x.Id)
                .ToArrayAsync(cancellationToken);
        }
        public async ValueTask<IReadOnlyCollection<TEntity>> GetPagedAsync(int take, int skip = 0, CancellationToken cancellationToken = default)
        {
            if (take <= 0) throw new ArgumentException("take 必须大于 0", nameof(take));
            if (skip < 0) throw new ArgumentException("skip 不能为负数", nameof(skip));

            return await repository.Query
                .OrderBy(x => x.Id)
                .Skip(skip)
                .Take(take)
                .ToArrayAsync(cancellationToken);
        }
        public async ValueTask<IReadOnlyCollection<TEntity>> GetAllAfterSkipAsync(int skip, CancellationToken cancellationToken = default)
        {
            if (skip < 0) throw new ArgumentException("skip 不能为负数", nameof(skip));

            return await repository.Query
                .OrderBy(x => x.Id)
                .Skip(skip)
                .ToListAsync(cancellationToken);
        }

        public async ValueTask<IReadOnlyCollection<TResult>> GetAllAsync<TResult>(CancellationToken cancellationToken = default) where TResult : TEntity
        {
            return await repository.Query
                .OfType<TResult>()
                .OrderBy(x => x.Id)
                .ToListAsync(cancellationToken);
        }
        public async ValueTask<IReadOnlyCollection<TResult>> GetPagedAsync<TResult>(int take, int skip = 0, CancellationToken cancellationToken = default) where TResult : TEntity
        {
            if (take <= 0) throw new ArgumentException("take 必须大于 0", nameof(take));
            if (skip < 0) throw new ArgumentException("skip 不能为负数", nameof(skip));

            return await repository.Query
                .OfType<TResult>()
                .OrderBy(x => x.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync(cancellationToken);
        }
        public async ValueTask<IReadOnlyCollection<TResult>> GetAllAfterSkipAsync<TResult>(int skip, CancellationToken cancellationToken = default) where TResult : TEntity
        {
            if (skip < 0) throw new ArgumentException("skip 不能为负数", nameof(skip));

            return await repository.Query
                .OfType<TResult>()
                .OrderBy(x => x.Id)
                .Skip(skip)
                .ToListAsync(cancellationToken);
        }
    }
}