namespace ThabeSoft.Ddd.Domain.Entities;

/// <summary>
/// 表示一个实体对象
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IEntity<out TKey> : IHasKey<TKey>
    where TKey : notnull
{

}
