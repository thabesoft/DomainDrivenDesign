using Microsoft.Extensions.DependencyInjection;
using Test.Mocks;
using ThabeSoft.Ddd.Domain.Repositories;

namespace Test.Cases;

[TestClass]
public class DomainEventTests : TestBase
{
    [TestMethod]
    public async Task SaveChanges_Should_Trigger_DomainEventHandler()
    {
        // Arrange
        var repo = ServiceProvider.GetRequiredService<IRepository<TestUser, TestUserId>>();
        var uow = ServiceProvider.GetRequiredService<IUnitOfWork>();

        var userId = new TestUserId(Guid.NewGuid());
        var user = new TestUser(userId, "CSharp");

        // Act
        await repo.AddAsync(user, TestContext.CancellationToken);
        await uow.SaveChangesAsync(TestContext.CancellationToken);

        var tracker = ServiceProvider.GetRequiredService<TestMessageTracker>();
        // Assert
        Assert.IsTrue(tracker.WasCalled, "领域事件处理器应该被执行");
        Assert.AreEqual(userId, tracker.LastUserId, "处理器接收到的 ID 应该一致");
    }

    public TestContext TestContext { get; set; }
}