using System.Runtime.ExceptionServices;
using Microsoft.Extensions.Logging;

namespace AzureExperiments.StorageQueues.Messaging {
    internal class ExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ExceptionHandler> logger;

        public ExceptionHandler(ILogger<ExceptionHandler> logger)
        {
            this.logger = logger;
        }
        
        public void HandleException(ExceptionDispatchInfo capture)
        {
            logger.LogError(capture.SourceException, capture.SourceException.Message);
        }
    }
}