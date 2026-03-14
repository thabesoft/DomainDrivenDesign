using Microsoft.Extensions.DependencyInjection;
using Test.Mocks;
using ThabeSoft.Ddd.Domain.Repositories;

namespace Test.Cases;

[TestClass]
public class TransactionTests : TestBase
{
    [TestMethod]
    public async Task Rollback_Should_Not_Save_Data()
    {
        // Arrange
        var repo = ServiceProvider.GetRequiredService<IRepository<TestUser, TestUserId>>();
        var uow = ServiceProvider.GetRequiredService<IUnitOfWork>();
        var userId = new TestUserId(Guid.NewGuid());

        // Act
        using (var transaction = await uow.BeginTransactionAsync())
        {
            await repo.AddAsync(new TestUser(new TestUserId(Guid.NewGuid()), "Ghost"), TestContext.CancellationToken);
            await uow.SaveChangesAsync(TestContext.CancellationToken);

            // 显式回滚，不调用 Commit
            await transaction.RollbackAsync(TestContext.CancellationToken);
        }

        // Assert
        var found = await repo.FindByIdAsync(userId, TestContext.CancellationToken);
        Assert.IsNull(found, "回滚后，数据库中不应存在该实体。");
    }

    public TestContext TestContext { get; set; }
}