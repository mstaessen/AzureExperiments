using System.Threading.Tasks;

namespace AzureExperiments.StorageQueues.Messaging
{
    public delegate Task HandlerDelegate(object o, IMessageContext context);
}