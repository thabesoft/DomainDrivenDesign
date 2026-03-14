using Microsoft.Extensions.DependencyInjection;
using Test.Mocks;
using ThabeSoft.Ddd.Domain.Repositories;

namespace Test.Cases;

[TestClass]
public class RepositoryTests : TestBase
{
    public TestContext TestContext { get; set; }


    [TestMethod]
    public async Task AddAsync_Should_PersistToDatabase()
    {
        // 1. Arrange
        var repo = ServiceProvider.GetRequiredService<IRepository<TestUser, TestUserId>>();
        var uow = ServiceProvider.GetRequiredService<IUnitOfWork>();

        var id = new TestUserId(Guid.NewGuid());
        var entity = new TestUser(id, "MSTest User");

        // 2. Act
        await repo.AddAsync(entity, TestContext.CancellationToken);
        await uow.SaveChangesAsync(TestContext.CancellationToken);

        // 3. Assert
        var found = await repo.FindByIdAsync(entity.Id, TestContext.CancellationToken);
        Assert.IsNotNull(found);
        Assert.AreEqual("MSTest User", found.Name);
    }
}