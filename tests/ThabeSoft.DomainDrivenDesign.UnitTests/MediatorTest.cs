using Microsoft.Extensions.DependencyInjection;
using Moq;
using ThabeSoft.Mediator;

namespace ThabeSoft.DomainDrivenDesign.UnitTests;


[TestClass]
public class MediatorTest
{
    [TestMethod(DisplayName = "领域事件中介者分发器")]
    public async Task PublishAsync_ShouldCallMediatorHandler()
    {
        // Arrange
        var @event = new Mock<IMediatorDomainEvent>();

        var event_handler = new Mock<INotificationHandler<IMediatorDomainEvent>>();
        event_handler
            .Setup(x => x.HandleAsync(@event.Object, It.IsAny<CancellationToken>()))
            .Returns(ValueTask.CompletedTask)
            .Verifiable();

        ServiceCollection descriptors = new();
        descriptors.AddMediator(ServiceLifetime.Scoped);
        descriptors.AddDomainEventPublisher();
        descriptors.AddScoped(_ => event_handler.Object);

        var services = descriptors.BuildServiceProvider();
        var publisher = services.GetRequiredService<IDomainEventPublisher>();

        // Act
        await publisher.PublishAsync(@event.Object, default);

        // Assert
        event_handler.Verify(x => x.HandleAsync(@event.Object, It.IsAny<CancellationToken>()), Times.Once);
    }
}