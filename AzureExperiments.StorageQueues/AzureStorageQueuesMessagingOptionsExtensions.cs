using AzureExperiments.StorageQueues.Controllers;
using AzureExperiments.StorageQueues.Messaging.AzureStorageQueues;

namespace AzureExperiments.StorageQueues
{
    public static class AzureStorageQueuesMessagingOptionsExtensions
    {
        public static MessagingOptions AddAzureStorageQueues(this MessagingOptions options)
        {
            options.RegisterProviderFactory("AzureStorageQueues",
                configuration => new AzureStorageQueueDefinitionProvider(configuration));
            return options;
        }
    }
}