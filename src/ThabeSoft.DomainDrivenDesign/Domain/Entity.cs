namespace ThabeSoft.DomainDrivenDesign.Domain;


/// <summary>
/// 实体基类
/// </summary>
/// <typeparam name="TKey">主键类型</typeparam>
public abstract class Entity<TKey>(TKey id) : IEntity<TKey>
    where TKey : notnull
{
    /// <summary>
    /// 实体的唯一标识符
    /// </summary>
    public TKey Id { get; } = id;


    protected Entity() : this(default!)
    {

    }


    /// <summary>
    /// 根据主键判断是否是一个对象
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
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
    /// <returns></returns>
    public override string ToString()
    {
        return Id.ToString() ?? string.Empty;
    }

    /// <summary>
    /// 主键 HashCode
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}