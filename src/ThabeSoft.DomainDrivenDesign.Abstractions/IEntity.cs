namespace ThabeSoft.DomainDrivenDesign;


/// <summary>
/// 实体
/// </summary>
/// <typeparam name="TKey">主键类型</typeparam>
public interface IEntity<out TKey>
    where TKey : notnull
{
    /// <summary>
    /// 主键
    /// </summary>
    TKey Id { get; }
}