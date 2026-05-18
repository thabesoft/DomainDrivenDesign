## 关于

基于 EfCore 的 领域模型持久化

## 如何使用

```C#
services.AddEfCorePersistence<TDbContext>(x =>
{
    // 添加默认仓储
    x.AddRepository<TEntity, TId>();

    // 添加自定义仓储
    x.AddRepository<TRepositoryService, TRepositoryImplementation, TEntity, TId>();
});
```