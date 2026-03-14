# Domain Driven Design Framework

这是一个轻量级、第三方依赖的领域驱动设计 (DDD) 基础框架。
它旨在简化 EF Core 的配置，并提供全自动的仓储模式与领域事件处理机制。

## 核心特性

- Fluent Configuration: 使用最新的 C# 扩展方法语法，一键式装载持久化层。

- Auto-Repositories: 自动扫描聚合根并注册仓储，支持特定仓储接口的自动转发。

- Decoupled Domain Events: 内置高性能领域事件分发器，支持多处理器并行执行。

- Unit of Work: 深度集成 EF Core ChangeTracker，确保领域事件与数据库更改的原子性。

- Zero Bloat: 核心库仅依赖于 .NET Standard / Core 基础库，不强制绑定任何第三方库。

## 安装与配置

在你的 Program.cs 或 Startup 中，通过 AddEfCorePersistence 扩展方法快速集成：

```C#
builder.Services.AddEfCorePersistence<MyDbContext>(options =>
{
    // 1. 配置数据库（如使用内存数据库进行测试）
    options.AddDbContext(opts => opts.UseInMemoryDatabase("MyDb"));

    // 2. 自动扫描并注册所有聚合根的仓储实现
    options.AddRepositoriesFromAssembly(typeof(User).Assembly);

    // 3. 自动扫描并注册所有领域事件处理器
    options.AddDomainEventHandlers(typeof(UserCreatedHandler).Assembly);
});
```

## 核心组件使用

### 1. 聚合根与领域事件

实体只需继承 AggregateRoot<TKey>，并在构造函数或方法中添加事件：

```C#
public class User : AggregateRoot<UserId>
{
    public string Name { get; private set; }

    public User(UserId id, string name) : base(id)
    {
        Name = name;
        // 添加领域事件
        AddDomainEvent(new UserCreatedEvent(this.Id));
    }
}
```

### 2.定义领域事件处理器

实现 IDomainEventHandler<TEvent> 接口即可，框架会自动完成依赖注入：

```C#
public class WelcomeEmailHandler : IDomainEventHandler<UserCreatedEvent>
{
    public async Task HandleAsync(UserCreatedEvent ev, CancellationToken ct)
    {
        // 业务逻辑：发送欢迎邮件
        await _emailService.SendAsync(ev.UserId);
    }
}
```

### 3.使用仓储与工作单元

在服务层中注入接口，通过 UnitOfWork 提交变更并触发事件：

```C#
public class UserService(IRepository<User, UserId> repository, IUnitOfWork uow)
{
    public async Task CreateUserAsync(string name)
    {
        var user = new User(new UserId(Guid.NewGuid()), name);
        await repository.AddAsync(user);
        
        // 保存时：1. 持久化到 DB  2. 自动分发领域事件
        await uow.SaveChangesAsync();
    }
}
```

## 约定与优先级

框架在自动注册仓储时遵循以下优先级：

1. 用户手动注册: 如果你在 DI 容器中手动注册了仓储，框架将跳过自动注册。

2. 自定义特定类: 如 UserRepository 实现类会被优先匹配给 IUserRepository。

3. 通用基类: 若无特定实现，框架会使用默认的 Repository<T, K> 进行兜底。

## 贡献指南

在使用过程中，请确保实体类实现了 IAggregateRoot<TKey> 接口，否则自动仓储扫描器将无法识别。