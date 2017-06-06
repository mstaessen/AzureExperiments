using System.IO;
using System.Threading.Tasks;

namespace AzureExperiments.StorageQueues.Messaging
{
    public interface IMessageSerializer
    {
        Task SerializeAsync(object obj, Stream stream);

        Task<T> DeserializeAsync<T>(Stream stream);
    }
}