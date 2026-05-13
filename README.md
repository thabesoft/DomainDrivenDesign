# Domain Driven Design

包含了一些领域驱动常见概念的抽象

## 概念
- IEntity<out TKey>
- IAggregateRoot<out TKey> : IEntity<TKey>
- IRepository<TEntity, in TKey>
- IUnitOfWork