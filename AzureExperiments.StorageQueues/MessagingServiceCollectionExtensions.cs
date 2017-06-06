using System;
using System.Linq;
using System.Reflection;
using AzureExperiments.StorageQueues.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AzureExperiments.StorageQueues
{
    public static class MessagingServiceCollectionExtensions
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration,
            Action<MessagingOptions> options)
        {
            var messagingOptions = new MessagingOptions();
            options?.Invoke(messagingOptions);

            var providersSection = configuration.GetSection("Inbound:Providers");
            var providers = providersSection.GetChildren()
                .Select(section => messagingOptions.GetProviderFactory(section["Type"])(section))
                .ToList();

            services.AddSingleton<IReceiveEndpoint, ReceiveEndpoint>();
            services.AddTransient<IExceptionHandler, ExceptionHandler>();

            services.AddSingleton(serviceProvider => { return providers.SelectMany(x => x.Provide()).ToArray(); });

            return services;
        }

        public static IServiceCollection AddHandlers(this IServiceCollection services, Assembly assembly)
        {
            var handlerInterface = typeof(IHandler<>);
            foreach (var type in assembly.GetTypes()) {
                foreach (var handlerType in type.GetInterfaces()
                    .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == handlerInterface)) {
                    services.AddTransient(handlerType, type);
                }
            }
            return services;
        }
    }
}