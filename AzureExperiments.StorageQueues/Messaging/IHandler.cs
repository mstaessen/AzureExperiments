using System.Threading.Tasks;

namespace AzureExperiments.StorageQueues.Messaging
{
    public interface IHandler<in TMessage>
    {
        Task HandleAsync(TMessage message, IMessageContext context);
    }
}