using System;
using Microsoft.WindowsAzure.Storage;

namespace AzureExperiments.StorageQueues.Messaging
{
    public static class StorageExceptionExtensions
    {
        public static bool IsNotFoundQueueNotFound(this StorageException exception)
        {
            if (exception == null) {
                throw new ArgumentNullException(nameof(exception));
            }

            var result = exception.RequestInformation;

            if (result == null) {
                return false;
            }

            if (result.HttpStatusCode != 404) {
                return false;
            }

            var extendedInformation = result.ExtendedErrorInformation;

            if (extendedInformation == null) {
                return false;
            }

            return extendedInformation.ErrorCode == "QueueNotFound";
        }

        public static bool IsConflictQueueBeingDeletedOrDisabled(this StorageException exception)
        {
            if (exception == null) {
                throw new ArgumentNullException(nameof(exception));
            }

            var result = exception.RequestInformation;

            if (result == null) {
                return false;
            }

            if (result.HttpStatusCode != 409) {
                return false;
            }

            var extendedInformation = result.ExtendedErrorInformation;

            if (extendedInformation == null) {
                return false;
            }

            return extendedInformation.ErrorCode == "QueueBeingDeleted";
        }

        public static bool IsServerSideError(this StorageException exception)
        {
            if (exception == null) {
                throw new ArgumentNullException(nameof(exception));
            }

            var result = exception.RequestInformation;

            if (result == null) {
                return false;
            }

            int statusCode = result.HttpStatusCode;
            return statusCode >= 500 && statusCode < 600;
        }
    }
}