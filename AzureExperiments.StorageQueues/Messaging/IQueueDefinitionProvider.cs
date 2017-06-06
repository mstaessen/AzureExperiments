using System.Collections.Generic;

namespace AzureExperiments.StorageQueues.Messaging
{
    public interface IQueueDefinitionProvider
    {
        IEnumerable<IQueueDefinition> Provide();
    }
}