using ThabeSoft.DomainDrivenDesign.Infrastructure;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配

public static class DependencyInjectionExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// 添加 Mediator 领域事件发布器
        /// </summary>
        public IServiceCollection AddMediatorDomainEventPublisher()
        {
            services.AddScoped<IDomainEventPublisher, MediatorDomainEventPublisher>();
            return services;
        }
    }
}