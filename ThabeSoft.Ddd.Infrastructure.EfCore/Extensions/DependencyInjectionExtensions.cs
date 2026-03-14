using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;
using ThabeSoft.Ddd.Domain.Entities;
using ThabeSoft.Ddd.Domain.Events;
using ThabeSoft.Ddd.Domain.Repositories;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配

public static class DependencyInjectionExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// 添加 Ef-core 持久层
        /// </summary>
        /// <returns></returns>
        public IServiceCollection AddEfCorePersistence<TDbContext>(Action<EfCorePersistenceOptionsBuilder<TDbContext>> persistenceOptionsAction = null)
            where TDbContext : DbContext
        {
            // 配置
            var options = new EfCorePersistenceOptionsBuilder<TDbContext>(services);
            persistenceOptionsAction.Invoke(options);

            return services;
        }
    }
}

public class EfCorePersistenceOptionsBuilder<TDbContext> 
    where TDbContext : DbContext
{
    private readonly IServiceCollection services;

    public EfCorePersistenceOptionsBuilder(IServiceCollection services)
    {
        this.services = services;

        AddUnitOfWork();
        AddDomainEventDispatcher();
    }

    /// <summary>
    /// 配置数据库
    /// </summary>
    /// <param name="dbContextOptionsAction"></param>
    /// <returns></returns>
    public EfCorePersistenceOptionsBuilder<TDbContext> AddDbContext(Action<DbContextOptionsBuilder> dbContextOptionsAction)
    {
        services.AddDbContext<TDbContext>(dbContextOptionsAction);
        return this;
    }

    /// <summary>
    /// 扫描指定程序集中的所有领域事件处理器
    /// </summary>
    public EfCorePersistenceOptionsBuilder<TDbContext> AddDomainEventHandlers(params Assembly[] assemblies)
    {
        var handlerInterface = typeof(IDomainEventHandler<>);

        foreach (var assembly in assemblies)
        {
            var handlers = assembly.GetTypes()
                .Where(t => t is { IsClass: true, IsAbstract: false })
                .SelectMany(t => t.GetInterfaces(), (implementation, @interface) => new { implementation, @interface })
                .Where(x => x.@interface.IsGenericType && x.@interface.GetGenericTypeDefinition() == handlerInterface);

            foreach (var handler in handlers)
            {
                services.AddScoped(handler.@interface, handler.implementation);
            }
        }

        return this;
    }

    /// <summary>
    /// 自动扫描并注册所有聚合根的仓储实现。
    /// 优先级：1.用户手动注册 > 2.程序集中的自定义类(如 UserRepository) > 3.框架通用兜底(Repository<T,K>)
    /// </summary>
    /// <param name="assembly">扫描程序集</param>
    /// <returns>服务集合</returns>
    /// <exception cref="AmbiguousMatchException">当同一个接口发现多个自定义实现类时抛出</exception>
    public EfCorePersistenceOptionsBuilder<TDbContext> AddRepositoriesFromAssembly(Assembly assembly)
    {
        // 1. 查找所有有效的聚合根（实现了 IAggregateRoot<TKey> 的非抽象类）
        var aggregateInfos = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false })
            .Select(t => new
            {
                EntityType = t,
                // 获取该类实现的 IAggregateRoot<TKey> 接口
                AggregateInterface = t.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType &&
                                       i.GetGenericTypeDefinition() == typeof(IAggregateRoot<>))
            })
            .Where(x => x.AggregateInterface != null)
            .ToList();

        foreach (var aggregate in aggregateInfos)
        {
            // 获取主键类型 TKey
            var tKey = aggregate.AggregateInterface!.GetGenericArguments()[0];

            // 构造标准接口类型：IRepository<TEntity, TKey>
            var baseRepoInterface = typeof(IRepository<,>).MakeGenericType(aggregate.EntityType, tKey);

            // 【约定 1】寻找自定义特定接口：例如 IUserRepository 继承自 IRepository<User, Guid>
            var customInterface = aggregate.EntityType.GetInterfaces()
                .FirstOrDefault(i => baseRepoInterface.IsAssignableFrom(i) && i != baseRepoInterface);

            // 【约定 2】寻找程序集中的显式实现类（无论是否继承默认基类）
            var implementationTypes = assembly.GetTypes()
                .Where(t => t is { IsClass: true, IsAbstract: false } &&
                           baseRepoInterface.IsAssignableFrom(t) &&
                           !t.IsGenericType) // 排除泛型定义类本身
                .ToList();

            // 防御性编程：如果同一个实体发现了多个手写仓储，抛出异常避免歧义
            if (implementationTypes.Count > 1)
            {
                throw new AmbiguousMatchException(
                    $"Entity '{aggregate.EntityType.Name}' has multiple repository implementations: " +
                    $"{string.Join(", ", implementationTypes.Select(x => x.Name))}. Please resolve the ambiguity.");
            }

            // 确定最终使用的实现类型：手写类优先，否则使用通用泛型基类兜底
            var finalImplementation = implementationTypes.FirstOrDefault()
                ?? typeof(Repository<,,>).MakeGenericType(typeof(TDbContext), aggregate.EntityType, tKey);

            // 【注册策略】
            if (customInterface != null)
            {
                // 情况 A：存在自定义接口 (如 IUserRepository)
                // 1. 注册自定义接口到实现类
                services.TryAddScoped(customInterface, finalImplementation);

                // 2. 将基础接口转发给自定义接口，确保在同一个 Scope 内拿到的是同一个实例
                // 这样无论开发者注入 IUserRepository 还是 IRepository<User, Guid>，DbContext 都是一致的
                services.TryAddScoped(baseRepoInterface, sp => sp.GetRequiredService(customInterface));
            }
            else
            {
                // 情况 B：仅使用标准接口 (IRepository<TEntity, TKey>)
                services.TryAddScoped(baseRepoInterface, finalImplementation);
            }
        }

        return this;
    }



    /// <summary>
    /// 注册工作单元（Unit of Work）实现。
    /// </summary>
    /// <typeparam name="TDbContext">该工作单元管理的具体 DbContext 类型。</typeparam>
    /// <returns></returns>
    private EfCorePersistenceOptionsBuilder<TDbContext> AddUnitOfWork()
    {
        services.TryAddScoped<IUnitOfWork, UnitOfWork<TDbContext>>();
        return this;
    }
    /// <summary>
    /// 注册领域事件分发器
    /// </summary>
    private EfCorePersistenceOptionsBuilder<TDbContext> AddDomainEventDispatcher()
    {
        services.TryAddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        return this;
    }

}
