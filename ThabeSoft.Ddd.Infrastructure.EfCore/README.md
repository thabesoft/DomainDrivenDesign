# ThabeSoft.Ddd.Infrastructure.EfCore

基于 EF Core 的基础设施实现层。它提供了自动化的仓储注册、工作单元管理以及高性能的领域事件分发。

## 核心组件

- **UnitOfWork<TDbContext>**: 深度集成 EF Core ChangeTracker，实现持久化与事件分发的原子操作。
- **DomainEventDispatcher**: 基于运行时类型缓存的高性能事件分发器。
- **EfCorePersistenceOptionsBuilder**: 提供 Fluent API 风格的配置扩展。

## 快速配置

利用最新的 C# 扩展语法（extension）在宿主工程中一键注册：

```csharp
services.AddEfCorePersistence<AppDbContext>(options =>
{
    // 配置数据库
    options.AddDbContext(db => db.UseSqlServer(connectionString));

    // 自动扫描程序集中的聚合根并注册仓储
    // 优先级：手动注册 > 自定义实现类 > 通用泛型实现
    options.AddRepositoriesFromAssembly(typeof(Order).Assembly);

    // 自动注册所有领域事件处理器
    options.AddDomainEventHandlers(typeof(OrderCreatedHandler).Assembly);
});