using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ThabeSoft.DomainDrivenDesign.EntityFrameworkCore.DependencyInjection;


/// <summary>
/// EfCore 持久化配置
/// </summary>
public class EfCorePersistenceOptions<TDbContext>(IServiceCollection services)
    where TDbContext : DbContext
{
    /// <summary>
    /// 添加仓储
    /// </summary>
    public void AddRepository<TEntity, TId>()
        where TEntity : class, IAggregateRoot<TId>
        where TId : notnull
    {
        services.AddRepository<TDbContext, TEntity, TId>();
    }

    /// <summary>
    /// 添加自定义实现仓储
    /// </summary>
    /// <typeparam name="TRepositoryImplementation">仓储实现</typeparam>
    public void AddRepository<TRepositoryImplementation, TEntity, TId>()
       where TRepositoryImplementation : class, IRepository<TEntity, TId>
       where TEntity : class, IAggregateRoot<TId>
       where TId : notnull
    {
        services.AddRepository<TDbContext, TRepositoryImplementation, TEntity, TId>();
    }

    /// <summary>
    /// 添加自定义实现仓储
    /// </summary>
    /// <typeparam name="TRepositoryService">仓储类型</typeparam>
    /// <typeparam name="TRepositoryImplementation">仓储实现</typeparam>
    public void AddRepository<TRepositoryService, TRepositoryImplementation, TEntity, TId>()
       where TRepositoryService : class, IRepository<TEntity, TId>
       where TRepositoryImplementation : class, TRepositoryService
       where TEntity : class, IAggregateRoot<TId>
       where TId : notnull
    {
        services.AddRepository<TDbContext, TRepositoryService, TRepositoryImplementation, TEntity, TId>();
    }
}