## 关于

提供领域驱动的抽象概念

## 如何使用

作为抽象层通常搭配具体实现使用，例如：

- `ThabeSoft.DomainDrivenDesign` 提供了聚合根基类
- `ThabeSoft.DomainDrivenDesign.EntityFrameworkCore` 基于EfCore的仓储和工作单元实现
- `ThabeSoft.DomainDrivenDesign.Mediator` 基于 ThabeSoft.Mediator 的领域事件分发器

## 主要类型

该库提供的主要类型有：

- `ThabeSoft.DomainDrivenDesign.IEntity<TId>`
- `ThabeSoft.DomainDrivenDesign.IAggregateRoot<TId>`
- `ThabeSoft.DomainDrivenDesign.IDomainEvent`
- `ThabeSoft.DomainDrivenDesign.IRepository`
- `ThabeSoft.DomainDrivenDesign.IUnitOfWork`
- `ThabeSoft.DomainDrivenDesign.IValueObject`