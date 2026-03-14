using Test.Mocks;

namespace Test.Cases;

[TestClass]
public class EntityTests : TestBase
{
    [TestMethod]
    public void SameId_Entities_ShouldBeEqual()
    {
        // Arrange
        var id = new TestUserId(Guid.NewGuid());
        var user1 = new TestUser(id, "CSharp");
        var user2 = new TestUser(id, "DoNet");

        // Assert - 使用 MSTest 的 Assert
        Assert.AreEqual(user1, user2, "只要 ID 相同，聚合根应该被视为相等。");
        Assert.IsTrue(user1 == user2);
    }
}
