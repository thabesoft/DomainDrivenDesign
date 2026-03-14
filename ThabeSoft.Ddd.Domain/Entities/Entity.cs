namespace ThabeSoft.Ddd.Domain.Entities;

/// <summary>
/// 实体基类：定义了领域模型中具有唯一身份标识的对象。
/// 两个实体即使属性完全相同，只要标识符不同，就被视为不同的对象。
/// </summary>
/// <typeparam name="TKey">主键类型（如 Guid, long 或强类型 Record ID）</typeparam>
public abstract class Entity<TKey>(TKey id) : EqualityObject, IEntity<TKey>
    where TKey : notnull
{
    /// <summary>
    /// 实体的唯一标识符
    /// </summary>
    public TKey Id { get; init; } = id;


    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Id;
    }
}