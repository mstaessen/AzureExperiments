using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace AzureExperiments.StorageQueues.Messaging.AzureStorageQueues
{
    public class AzureStorageQueueListener : IQueueListener
    {
        private readonly CloudQueue queue;
        private readonly CloudQueue poisonQueue;
        private readonly IDelayStrategy delayStrategy;
        private readonly HandlerDelegate messageHandlerDelegate;
        private readonly IExceptionHandler exceptionHandler;

        private Task run;
        private CancellationTokenSource cts;

        private bool Started => run != null;

        public AzureStorageQueueListener(CloudQueue queue, CloudQueue poisonQueue, IDelayStrategy delayStrategy, HandlerDelegate messageHandlerDelegate, IExceptionHandler exceptionHandler)
        {
            this.queue = queue;
            this.poisonQueue = poisonQueue;
            this.delayStrategy = delayStrategy;
            this.messageHandlerDelegate = messageHandlerDelegate;
            this.exceptionHandler = exceptionHandler;
        }

        private async Task RunAsync(HandlerDelegate handlerDelegate, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested) {
                CloudQueueMessage message = null;
                try {
                    message = await queue.GetMessageAsync();
                    var context = new AzureStorageQueueMessageContext {
                        MessageId = message.Id,
                        MessageHeaders = new Dictionary<string, string>()
                    };
                    await handlerDelegate(message.AsString, context);
                    await queue.DeleteMessageAsync(message);
                } catch (StorageException exception) {
                    if (exception.IsNotFoundQueueNotFound() ||
                        exception.IsConflictQueueBeingDeletedOrDisabled() ||
                        exception.IsServerSideError()) {
                        await queue.DeleteMessageAsync(message);
                    } else {
                        throw;
                    }
                } catch (Exception e) {
                    exceptionHandler.HandleException(ExceptionDispatchInfo.Capture(e));
                }
                await Task.Delay(delayStrategy.Next(message != null), cancellationToken);
            }
        }

        public Task StartAsync()
        {
            if (!Started) {
                cts = new CancellationTokenSource();
                run = RunAsync(messageHandlerDelegate, cts.Token);
            }
            return Task.CompletedTask;
        }

        public Task StopAsync()
        {
            if (Started && !cts.IsCancellationRequested) {
                cts.Cancel();
            }
            return run;
        }
    }
}