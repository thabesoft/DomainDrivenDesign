namespace ThabeSoft.Ddd;

/// <summary>
/// 定义一个能够提供用于相等性比较的组件集合的对象。
/// 通常用于值对象（Value Object）或需要基于成员值进行比较的场景。
/// </summary>
public interface IEqualityComponents
{
    /// <summary>
    /// 获取构成该对象相等性逻辑的所有成员组件。
    /// </summary>
    /// <returns>对象成员的可枚举集合</returns>
    IEnumerable<object?> GetEqualityComponents();
}