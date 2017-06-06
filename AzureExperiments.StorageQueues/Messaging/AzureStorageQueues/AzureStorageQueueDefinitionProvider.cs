using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace AzureExperiments.StorageQueues.Messaging.AzureStorageQueues
{
    public class AzureStorageQueueDefinitionProvider : IQueueDefinitionProvider
    {
        private readonly IConfiguration configuration;

        public AzureStorageQueueDefinitionProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IEnumerable<IQueueDefinition> Provide()
        {
            var connectionString = configuration["ConnectionString"] ??
                                   throw new ArgumentException("ConnectionString must not be empty.");
            var poisonQueueSuffix = configuration["PoisonQueueSuffix"] ?? "-poison";
            foreach (var definition in configuration.GetSection("Queues").GetChildren()) {
                yield return new AzureStorageQueueDefinition {
                    ConnectionString = connectionString,
                    QueueName = definition["QueueName"] ?? throw new ArgumentException("QueueName cannot be empty."),
                    PoisonQueueName = definition["PoisonQueueName"] ?? $"{definition["QueueName"]}{poisonQueueSuffix}"
                };
            }
        }
    }
}