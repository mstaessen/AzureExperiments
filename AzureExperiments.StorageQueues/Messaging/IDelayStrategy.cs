using System;

namespace AzureExperiments.StorageQueues.Messaging
{
    public interface IDelayStrategy
    {
        TimeSpan Next(bool hit);
    }
}