using ThabeSoft.Ddd.Domain.Events;

namespace ThabeSoft.Ddd.Domain.Entities;

/// <summary>
/// 表示一个实体对象
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IAggregateRoot<out TKey> : IHasKey<TKey>
    where TKey : notnull
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
}
