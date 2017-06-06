using System.Threading.Tasks;

namespace AzureExperiments.StorageQueues.Messaging
{
    public interface IReceiveEndpoint
    {
        bool IsStarted { get; }
        
        Task StartAsync();

        Task StopAsync();
    }
}