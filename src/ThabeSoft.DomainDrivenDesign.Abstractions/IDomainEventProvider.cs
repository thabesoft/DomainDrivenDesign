namespace ThabeSoft.DomainDrivenDesign;


/// <summary>
/// 领域事件提供者
/// </summary>
public interface IDomainEventProvider
{
    /// <summary>
    /// 领域事件
    /// </summary>
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
}