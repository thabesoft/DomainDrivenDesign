# ThabeSoft.Ddd.Abstractions

这是 ThabeSoft DDD 框架的核心契约层。它不依赖任何第三方库，定义了领域驱动设计中最基本的接口和语意，确保业务逻辑与具体技术实现完全解耦。

## 核心组件

- **IAggregateRoot<TKey>**: 聚合根标记接口，所有仓储操作的入口。
- **IDomainEvent**: 领域事件标记接口，用于定义业务发生的重要时刻。
- **IDomainEventHandler<TEvent>**: 领域事件处理器接口，定义了如何响应特定事件。
- **IRepository<TEntity, TKey>**: 仓储模式标准接口。
- **IUnitOfWork**: 工作单元接口，管理事务原子性并驱动事件分发。

## 使用场景

在定义纯粹的领域模型接口（Domain Model Interfaces）或 API 契约时引用此包。