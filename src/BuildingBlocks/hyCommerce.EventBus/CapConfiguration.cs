using DotNetCore.CAP;
using DotNetCore.CAP.Messages;
using hyCommerce.EventBus.Event;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace hyCommerce.EventBus;

public static class CapConfiguration
{
    public static IServiceCollection AddCap<TContext>(this IServiceCollection services, IConfiguration configuration, Action<RabbitMQOptions> configures = null)
        where TContext : DbContext
    {
        var settings = new CapSettings();

        configuration.Bind("CapSettings", settings);

        services.AddCap(options =>
        {
            options.UseEntityFramework<TContext>();

            if (settings.Provider.Equals("RabbitMq"))
            {
                options.UseRabbitMQ(configure =>
                {
                    configure.HostName = settings.Host;
                    configure.UserName = settings.UserName;
                    configure.Password = settings.Password;
                    configures?.Invoke(configure);
                });
            }
            else
            {
                options.UseAzureServiceBus(settings.Host);
            }

            options.UseDashboard();

            options.FailedRetryCount = 5;

            options.FailedThresholdCallback = failed =>
            {
                var logger = failed.ServiceProvider.GetService<ILogger>();

                logger?.LogError("A message of type {MessageType} failed after executing {FailedRetryCount} several times, requiring manual troubleshooting. Message name: {MessageName}", failed.MessageType, options.FailedRetryCount, failed.Message.GetName());
            };
        });

        services.Scan(s =>
            s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c => c.AssignableTo(typeof(IIntegrationEventHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        return services;
    }
}