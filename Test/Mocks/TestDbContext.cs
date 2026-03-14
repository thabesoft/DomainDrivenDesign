using Microsoft.EntityFrameworkCore;

namespace Test.Mocks;

public class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
{
    public DbSet<TestUser> TestEntities { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 配置 TestUser 实体
        modelBuilder.Entity<TestUser>(builder =>
        {
            // 设置主键
            builder.HasKey(x => x.Id);

            // 核心：配置值转换器
            // 告诉 EF：存入数据库用 Guid，读取到内存用 TestUserId
            builder.Property(x => x.Id)
                .HasConversion(
                    id => id.Value,              // 转换为数据库类型 (Guid)
                    value => new TestUserId(value) // 转换回领域类型 (TestUserId)
                );
        });
    }
}

