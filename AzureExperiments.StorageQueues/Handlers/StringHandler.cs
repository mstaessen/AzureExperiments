using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AzureExperiments.StorageQueues.Controllers;
using AzureExperiments.StorageQueues.Messaging;
using Microsoft.Extensions.Configuration;

namespace AzureExperiments.StorageQueues.Handlers
{
    public class StringHandler : IHandler<string>
    {
        public Task HandleAsync(string message, IMessageContext context)
        {
            Console.WriteLine(message);
            return Task.CompletedTask;
        }
    }
}