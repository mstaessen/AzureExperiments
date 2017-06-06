using System.Collections.Generic;

namespace AzureExperiments.StorageQueues.Messaging
{
    public interface IMessageContext
    {
        string MessageId { get; }

        IReadOnlyDictionary<string, string> MessageHeaders { get; }
    }
}