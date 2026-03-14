using ThabeSoft.Ddd.Domain.Entities;

namespace ThabeSoft.Ddd.Domain;

/// <summary>
/// 相等性基础对象：提供基于组件序列的逻辑相等性比较实现。
/// 该类是 <see cref="Entity{TKey}"/> 与 <see cref="ValueObject"/> 的共同基类，
/// 统一了领域模型中“如何判断两个对象相等”的技术实现。
/// </summary>
public abstract class EqualityObject : IEqualityComponents
{
    /// <summary>
    /// 显式实现接口方法，将相等性组件的访问权限收敛。
    /// 外部基础设施（如仓储或转换器）可以通过 <see cref="IEqualityComponents"/> 接口访问。
    /// </summary>
    /// <returns>用于比较的属性组件序列</returns>
    IEnumerable<object?> IEqualityComponents.GetEqualityComponents()
    {
        return GetEqualityComponents();
    }

    /// <summary>
    /// 在子类中重写此方法，以定义哪些属性参与相等性比较。
    /// 建议按照属性的重要性顺序返回组件，以优化比较性能。
    /// </summary>
    /// <returns>参与相等性比较的对象序列</returns>
    protected abstract IEnumerable<object?> GetEqualityComponents();

    /// <summary>
    /// 确定指定的对象是否等于当前对象。
    /// 逻辑：1. 类型必须完全一致；2. <see cref="GetEqualityComponents"/> 返回的序列必须完全匹配。
    /// </summary>
    /// <param name="obj">要比较的对象</param>
    /// <returns>如果逻辑相等则返回 true；否则返回 false</returns>
    public override bool Equals(object? obj)
    {
        // 1. 空值检查
        if (obj is null) return false;

        // 2. 引用检查
        if (ReferenceEquals(this, obj)) return true;

        // 3. 严格类型检查：防止不同类型的领域对象因为相同的标识组件而判定为相等
        if (obj.GetType() != GetType()) return false;

        // 4. 组件序列检查
        var other = (IEqualityComponents)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <summary>
    /// 获取对象的哈希值。
    /// 该实现使用 <see cref="HashCode"/> 结构体高效地合并所有标识组件的哈希值。
    /// </summary>
    /// <returns>当前对象的哈希码</returns>
    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var item in GetEqualityComponents())
        {
            // HashCode.Add 能够安全处理 null 值
            hash.Add(item);
        }
        return hash.ToHashCode();
    }

    /// <summary>
    /// 重载等于操作符。
    /// </summary>
    public static bool operator ==(EqualityObject? left, EqualityObject? right) => Equals(left, right);

    /// <summary>
    /// 重载不等于操作符。
    /// </summary>
    public static bool operator !=(EqualityObject? left, EqualityObject? right) => !Equals(left, right);
}