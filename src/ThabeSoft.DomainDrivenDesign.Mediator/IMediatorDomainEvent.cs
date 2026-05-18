using ThabeSoft.Mediator;

namespace ThabeSoft.DomainDrivenDesign;

/// <summary>
/// 领域事件
/// </summary>
public interface IMediatorDomainEvent : IDomainEvent, INotification;