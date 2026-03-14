namespace ThabeSoft.Ddd.Domain.ValueObjects;

/// <summary>
/// 标识一个枚举
/// </summary>
public interface IEnumeration<TId>
    where TId : notnull, IComparable
{
    TId Id { get; }

    string Name { get; }
}