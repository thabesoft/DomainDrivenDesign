using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Test.Mocks;

namespace Test;

public abstract class TestBase : IDisposable
{
    protected readonly IServiceProvider ServiceProvider;
    protected readonly TestDbContext DbContext;

    protected TestBase()
    {
        var services = new ServiceCollection();

        // 配置持久层业务
        services.AddEfCorePersistence<TestDbContext>(x =>
        {
            x.AddDbContext(opts =>
            {
                opts.UseInMemoryDatabase(Guid.NewGuid().ToString());
                opts.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            x.AddDomainEventHandlers(typeof(TestUser).Assembly);
            x.AddRepositoriesFromAssembly(typeof(TestUser).Assembly);
        });
        services.AddScoped<TestMessageTracker>();

        ServiceProvider = services.BuildServiceProvider();
        DbContext = ServiceProvider.GetRequiredService<TestDbContext>();
        DbContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();

        GC.SuppressFinalize(this);
    }
}