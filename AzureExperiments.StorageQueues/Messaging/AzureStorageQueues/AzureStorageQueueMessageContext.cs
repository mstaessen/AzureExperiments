using System.Collections.Generic;

namespace AzureExperiments.StorageQueues.Messaging.AzureStorageQueues {
    public class AzureStorageQueueMessageContext : IMessageContext
    {
        public string MessageId { get; set; }
        public IReadOnlyDictionary<string, string> MessageHeaders { get; set; }
    }
}