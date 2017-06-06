using System.Threading.Tasks;

namespace AzureExperiments.StorageQueues.Messaging
{
    public interface IQueueDefinition
    {
        Task<IQueueListener> CreateAsync(IQueueListenerFactory listenerFactory, 
            HandlerDelegate handlerDelegate,
            IExceptionHandler exceptionHandler);
    }
}