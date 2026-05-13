namespace ThabeSoft.DomainDrivenDesign;


/// <summary>
/// 聚合根
/// </summary>
/// <typeparam name="TKey">强类型主键</typeparam>
public interface IAggregateRoot<out TKey> : IEntity<TKey>
    where TKey : notnull;