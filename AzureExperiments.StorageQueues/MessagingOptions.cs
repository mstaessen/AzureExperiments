using System;
using System.Collections.Generic;
using AzureExperiments.StorageQueues.Controllers;
using AzureExperiments.StorageQueues.Messaging;
using Microsoft.Extensions.Configuration;

namespace AzureExperiments.StorageQueues {
    public class MessagingOptions
    {
        private readonly IDictionary<string, Func<IConfiguration, IQueueDefinitionProvider>> providerFactories =
            new Dictionary<string, Func<IConfiguration, IQueueDefinitionProvider>>();

        public void RegisterProviderFactory(string key, Func<IConfiguration, IQueueDefinitionProvider> factory)
        {
            if (providerFactories.ContainsKey(key)) {
                throw new ArgumentException($"There is already a provider factory for the key '{key}'.", nameof(key));
            }
            providerFactories[key] = factory;
        }

        public Func<IConfiguration, IQueueDefinitionProvider> GetProviderFactory(string key)
        {
            if (!providerFactories.ContainsKey(key)) {
                throw new ArgumentException($"There is no provider factory for the key '{key}'.", nameof(key));
            }
            return providerFactories[key];
        }
    }
}