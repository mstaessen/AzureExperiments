using System;
using System.Threading.Tasks;
using AzureExperiments.StorageQueues.Messaging.AzureStorageQueues;
using Microsoft.WindowsAzure.Storage;

namespace AzureExperiments.StorageQueues.Messaging
{
    public static class QueueListenerFactoryExtensions
    {
        public static async Task<AzureStorageQueueListener> CreateAzureStorageQueueListener(this IQueueListenerFactory listenerFactory, AzureStorageQueueDefinition definition, HandlerDelegate handlerDelegate, IExceptionHandler exceptionHandler)
        {
            var storageAccount = CloudStorageAccount.Parse(definition.ConnectionString);
            var queueClient = storageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference(definition.QueueName);
            var poisonQueue = queueClient.GetQueueReference(definition.PoisonQueueName);
            await queue.CreateIfNotExistsAsync();
            await poisonQueue.CreateIfNotExistsAsync();
            
            
            
            return new AzureStorageQueueListener(queue, poisonQueue,
                new ExponentialBackoffDelayStrategy(TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(10), 2),
                handlerDelegate, exceptionHandler);
        }
    }
}