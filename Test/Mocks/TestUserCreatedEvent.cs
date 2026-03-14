using ThabeSoft.Ddd.Domain.Events;

namespace Test.Mocks;

// 1. 定义事件
public record TestUserCreatedEvent(TestUserId UserId) : IDomainEvent;


// 定义处理器（这里用静态属性方便测试断言）
public class TestUserCreatedHandler(TestMessageTracker testMessageTracker) : IDomainEventHandler<TestUserCreatedEvent>
{
    public Task HandleAsync(TestUserCreatedEvent domainEvent, CancellationToken ct)
    {
        testMessageTracker.WasCalled = true;
        testMessageTracker.LastUserId = domainEvent.UserId;
        return Task.CompletedTask;
    }
}

public class TestMessageTracker
{
    public TestUserId? LastUserId { get; set; }
    public bool WasCalled { get; set; }
}