using System;

namespace AzureExperiments.StorageQueues.Messaging
{
    public class ExponentialBackoffDelayStrategy : IDelayStrategy
    {
        public TimeSpan MinDelay { get; }
        public TimeSpan MaxDelay { get; }
        public int BackoffModifier { get; }

        private TimeSpan delay;

        public ExponentialBackoffDelayStrategy(TimeSpan minDelay, TimeSpan maxDelay, int backoffModifier)
        {
            if (minDelay >= maxDelay) {
                throw new ArgumentException($"{nameof(maxDelay)} neeeds to be greater than {nameof(minDelay)}.");
            }
            MinDelay = minDelay;
            MaxDelay = maxDelay;
            BackoffModifier = backoffModifier;
            Reset();
        }

        private TimeSpan Reset()
        {
            delay = MinDelay;
            return delay;
        }

        private TimeSpan Backoff()
        {
            var newDelay = delay * 2;
            delay = newDelay < MaxDelay
                ? newDelay
                : MaxDelay;
            return delay;
        }

        public TimeSpan Next(bool hit)
        {
            return hit
                ? Reset()
                : Backoff();
        }
    }
}