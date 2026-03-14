using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace ThabeSoft.Ddd.Domain.Events;


/// <summary>
/// 领域事件分发器
/// </summary>
/// <param name="services"></param>
internal class DomainEventDispatcher(IServiceProvider services) : IDomainEventDispatcher
{
    // 缓存：事件类型 -> 发布函数委托
    private static readonly ConcurrentDictionary<Type, Func<IServiceProvider, IDomainEvent, CancellationToken, Task>> _publishers = new();

    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent
    {
        if (@event == null) return Task.CompletedTask;

        var eventType = @event.GetType();

        // 获取或创建针对该事件类型的发布委托
        var publisher = _publishers.GetOrAdd(eventType, CreatePublisherDelegate);

        return publisher(services, @event, cancellationToken);
    }

    private static Func<IServiceProvider, IDomainEvent, CancellationToken, Task> CreatePublisherDelegate(Type eventType)
    {
        // 构造 IDomainEventHandler<TActual>
        var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);
        var methodName = nameof(IDomainEventHandler<>.HandleAsync);

        // 返回一个闭包委托，它在运行时处理所有逻辑
        return async (provider, ev, ct) =>
        {
            var handlers = provider.GetServices(handlerType);
            var tasks = new List<Task>();

            foreach (var handler in handlers)
            {
                if (handler == null) continue;

                // 只有第一次需要 GetMethod，后续可以通过缓存该 MethodInfo 进一步优化
                // 但由于 handlerType 本身是动态的，直接使用 dynamic 或是编译后的表达式会更快
                // 这里我们采用最平衡的方案：
                var method = handlerType.GetMethod(methodName);
                if (method != null)
                {
                    tasks.Add((Task)method.Invoke(handler, [ev, ct])!);
                }
            }

            await Task.WhenAll(tasks);
        };
    }
}