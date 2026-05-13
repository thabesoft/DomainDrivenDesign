namespace ThabeSoft.DomainDrivenDesign.Infrastructure;


/// <summary>
/// 可以清除领域事件的
/// </summary>
public interface IDomainEventClearable
{
    /// <summary>
    /// 清空领域事件
    /// </summary>
    void ClearDomainEvents();
}