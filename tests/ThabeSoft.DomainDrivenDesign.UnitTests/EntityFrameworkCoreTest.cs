using Microsoft.EntityFrameworkCore;

namespace ThabeSoft.DomainDrivenDesign.UnitTests;

[TestClass]
public class EntityFrameworkCoreTest
{
    private DbContextOptions<TestDbContext> _options = null!;
    private TestDbContext _dbContext = null!;
    private TestRepository _repository = null!;

    public TestContext TestContext { get; set; }

    [TestInitialize]
    public void Setup()
    {
        _options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new TestDbContext(_options);
        _repository = new TestRepository(_dbContext);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _dbContext.Dispose();
    }



    [TestMethod]
    public async Task AddAsync_ShouldAddEntity()
    {
        // Arrange
        var entity = new TestEntity(Guid.CreateVersion7());

        // Act
        await _repository.AddAsync(entity, TestContext.CancellationToken);
        await _dbContext.SaveChangesAsync(TestContext.CancellationToken);

        // Assert
        var saved = await _dbContext.TestEntities.FindAsync([entity.Id], cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(saved);
        Assert.AreEqual(entity.Id, saved.Id);
    }

    [TestMethod]
    public async Task FindByIdAsync_ShouldReturnEntity()
    {
        // Arrange
        var entity = new TestEntity(Guid.CreateVersion7());
        await _dbContext.TestEntities.AddAsync(entity, TestContext.CancellationToken);
        await _dbContext.SaveChangesAsync(TestContext.CancellationToken);

        // Act
        var found = await _repository.FindByIdAsync(entity.Id, TestContext.CancellationToken);

        // Assert
        Assert.IsNotNull(found);
        Assert.AreEqual(entity.Id, found.Id);
    }

    [TestMethod]
    public async Task FindByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        // Act
        var found = await _repository.FindByIdAsync(Guid.NewGuid(), TestContext.CancellationToken);

        // Assert
        Assert.IsNull(found);
    }

    [TestMethod]
    public async Task RemoveAsync_ShouldRemoveEntity()
    {
        // Arrange
        var entity = new TestEntity(Guid.CreateVersion7());
        await _dbContext.TestEntities.AddAsync(entity, TestContext.CancellationToken);
        await _dbContext.SaveChangesAsync(TestContext.CancellationToken);

        // Act
        await _repository.RemoveByIdAsync(entity.Id, TestContext.CancellationToken);
        await _dbContext.SaveChangesAsync(TestContext.CancellationToken);

        // Assert
        var deleted = await _dbContext.TestEntities.FindAsync([entity.Id], cancellationToken: TestContext.CancellationToken);
        Assert.IsNull(deleted);
    }

    [TestMethod]
    public async Task RemoveAsync_ShouldDoNothing_WhenNotExists()
    {
        // Act（不应抛出异常）
        await _repository.RemoveByIdAsync(Guid.NewGuid(), TestContext.CancellationToken);
        await _dbContext.SaveChangesAsync(TestContext.CancellationToken);

        // Assert
        int count = await _dbContext.TestEntities.CountAsync(TestContext.CancellationToken);
        Assert.AreEqual(0, count);
    }


    #region -- 测试数据定义 --

    public sealed class TestEntity(Guid id) : IAggregateRoot<Guid>
    {
        public Guid Id { get; } = id;
    }
    public class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
    {
        public DbSet<TestEntity> TestEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestEntity>().HasKey(x => x.Id);
        }
    }
    public class TestRepository(TestDbContext dbContext) : Repository<TestDbContext, TestEntity, Guid>(dbContext)
    {
    }

    #endregion
}