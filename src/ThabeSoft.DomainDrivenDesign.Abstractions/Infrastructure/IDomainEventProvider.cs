using ThabeSoft.DomainDrivenDesign.Domain;

namespace ThabeSoft.DomainDrivenDesign.Infrastructure;


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