namespace ThabeSoft.DomainDrivenDesign;


/// <summary>
/// 实体
/// </summary>
/// <typeparam name="TId">主键类型</typeparam>
public interface IEntity<out TId>
    where TId : notnull
{
    /// <summary>
    /// 主键
    /// </summary>
    TId Id { get; }
}