using System.Runtime.ExceptionServices;

namespace AzureExperiments.StorageQueues.Messaging
{
    public interface IExceptionHandler
    {
        void HandleException(ExceptionDispatchInfo capture);
    }
}