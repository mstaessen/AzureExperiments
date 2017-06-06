using System.Threading.Tasks;

namespace AzureExperiments.StorageQueues.Messaging.AzureStorageQueues
{
    public class AzureStorageQueueDefinition : IQueueDefinition
    {
        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
        public string PoisonQueueName { get; set; }

        public async Task<IQueueListener> CreateAsync(IQueueListenerFactory listenerFactory, HandlerDelegate handlerDelegate, IExceptionHandler exceptionHandler)
        {
            return await listenerFactory.CreateAzureStorageQueueListener(this, handlerDelegate, exceptionHandler);
        }
    }
}