namespace ThabeSoft.DomainDrivenDesign;


/// <summary>
/// 实体基类
/// </summary>
/// <typeparam name="TKey">主键类型</typeparam>
public abstract class Entity<TKey> : IEntity<TKey>
    where TKey : notnull
{
    /// <summary>
    /// 实体的唯一标识符
    /// </summary>
    public TKey Id { get; }


    protected Entity()
    {
        Id = default!;
    }

    protected Entity(TKey id)
    {
        Id = id;
    }


    /// <summary>
    /// 根据主键判断是否是一个对象
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;

        if (obj is not IEntity<TKey> other) return false;
        return Id.Equals(other.Id);
    }

    /// <summary>
    /// 主键 ToString
    /// </summary>
    public override string ToString()
    {
        return Id.ToString() ?? string.Empty;
    }

    /// <summary>
    /// 主键 HashCode
    /// </summary>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}