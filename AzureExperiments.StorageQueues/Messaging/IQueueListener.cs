using System.Threading.Tasks;

namespace AzureExperiments.StorageQueues.Messaging
{
    public interface IQueueListener
    {
        Task StartAsync();

        Task StopAsync();
    }
}