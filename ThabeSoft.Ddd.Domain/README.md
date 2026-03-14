# ThabeSoft.Ddd.Domain

领域驱动设计实现层。提供了聚合根、实体等基础实现，并内置了领域事件的收集机制。

## 核心组件

- **AggregateRoot<TKey>**: 实现了领域事件存储的基类。
- **Entity<TKey>**: 实体基类。
- **IDomainEventProvider**: 内部契约，允许基础设施层提取实体的领域事件。

## 快速入门

让你的聚合根继承 `AggregateRoot`：

```csharp
public class Order : AggregateRoot<OrderId>
{
    public Order(OrderId id) : base(id)
    {
        // 记录领域事件，这些事件会在 SaveChangesAsync 时自动触发
        AddDomainEvent(new OrderCreatedEvent(this.Id));
    }
}