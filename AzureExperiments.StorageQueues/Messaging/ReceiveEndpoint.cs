using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AzureExperiments.StorageQueues.Messaging
{
    public class ReceiveEndpoint : IReceiveEndpoint
    {
        private readonly IQueueDefinition[] queueDefinitions;
        private readonly IServiceProvider serviceProvider;
        private readonly IExceptionHandler exceptionHandler;

        private readonly IList<IQueueListener> listeners = new List<IQueueListener>();

        public ReceiveEndpoint(IQueueDefinition[] queueDefinitions, IExceptionHandler exceptionHandler, IServiceProvider serviceProvider)
        {
            this.queueDefinitions = queueDefinitions;
            this.exceptionHandler = exceptionHandler;
            this.serviceProvider = serviceProvider;
        }

        public bool IsStarted => listeners.Any();

        public async Task StartAsync()
        {
            var factory = new QueueListenerFactory();
            foreach (var definition in queueDefinitions) {
                var listener = await definition.CreateAsync(factory, Handle, exceptionHandler);
                listeners.Add(listener);
                await listener.StartAsync();
            }
        }

        public async Task StopAsync()
        {
            while (listeners.Any()) {
                var listener = listeners.First();
                await listener.StopAsync();
                listeners.Remove(listener);
            }
        }

        private async Task Handle(object o, IMessageContext context)
        {
            var messageType = o.GetType();
            var handlerType = typeof(IHandler<>).MakeGenericType(messageType);
            var handler = serviceProvider.GetService(handlerType);
            if (handler == null) {
                throw new ArgumentOutOfRangeException($"No handler available for message type '{messageType.Name}'.");
            }

            await (Task) handlerType.InvokeMember("HandleAsync", BindingFlags.InvokeMethod, null, handler, new [] {o, context});
        }
    }
}