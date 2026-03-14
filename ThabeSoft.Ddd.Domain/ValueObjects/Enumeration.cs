namespace ThabeSoft.Ddd.Domain.ValueObjects;


/// <summary>
/// 强类型枚举基类：提供比原生 enum 更丰富的面向对象特性。
/// 建议用于定义如订单状态、用户角色等受限且具有业务逻辑的状态集合。
/// </summary>
/// <typeparam name="TId">标识符类型，通常为 <see cref="int"/> 或 <see cref="string"/>。</typeparam>
public class Enumeration<TId>(TId id, string name) : IEnumeration<TId>
    where TId : notnull, IComparable
{
    /// <summary>
    /// 获取枚举的唯一标识符（键）。
    /// </summary>
    public TId Id { get; } = id;

    /// <summary>
    /// 获取枚举的显示名称。
    /// </summary>
    public string Name { get; } = name;


    /// <summary>
    /// 确定指定的对象是否等于当前枚举项。
    /// 规则：类型必须完全一致，且 <see cref="Id"/> 必须相等。
    /// </summary>
    public override bool Equals(object? obj)
    {
        // 1. 空值检查
        if (obj is null) return false;

        // 2. 引用检查（同一实例直接返回 true）
        if (ReferenceEquals(this, obj)) return true;

        // 3. 严格类型检查：防止不同类型的枚举类（如 OrderStatus 和 UserStatus）
        // 因为拥有相同的 Id 值而被判定为相等。
        if (obj.GetType() != GetType()) return false;

        // 4. 基于标识符的相等性检查
        var other = (IEnumeration<TId>)obj;
        return Id.Equals(other.Id);
    }

    /// <summary>
    /// 返回该枚举项的哈希值。
    /// 结合了类型信息和 Id 的哈希值，以降低哈希冲突的概率。
    /// </summary>
    public override int GetHashCode()
    {
        // 建议结合 GetType()，因为不同枚举类的相同 Id 不应产生相同的 HashCode
        return HashCode.Combine(GetType(), Id);
    }

    /// <summary>
    /// 返回枚举项的显示名称。
    /// </summary>
    public override string ToString() => Name;

    /// <summary>
    /// 实现等于操作符。
    /// </summary>
    public static bool operator ==(Enumeration<TId>? left, Enumeration<TId>? right)
        => Equals(left, right);

    /// <summary>
    /// 实现不等于操作符。
    /// </summary>
    public static bool operator !=(Enumeration<TId>? left, Enumeration<TId>? right)
        => !Equals(left, right);
}