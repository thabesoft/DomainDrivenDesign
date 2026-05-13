using ThabeSoft.Mediator;

namespace ThabeSoft.DomainDrivenDesign.Domain;


/// <summary>
/// Mediator 领域事件包装器
/// </summary>
public interface IMediatorDomainEvent : IDomainEvent, INotification;