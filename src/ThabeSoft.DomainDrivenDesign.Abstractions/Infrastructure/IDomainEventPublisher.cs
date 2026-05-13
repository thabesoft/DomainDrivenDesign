using ThabeSoft.DomainDrivenDesign.Domain;

namespace ThabeSoft.DomainDrivenDesign.Infrastructure;


/// <summary>
/// 领域事件发布器
/// </summary>
public interface IDomainEventPublisher
{
    /// <summary>
    /// 发布事件
    /// </summary>
    ValueTask PublishAsync(IDomainEvent @event, CancellationToken cancellationToken = default);
}