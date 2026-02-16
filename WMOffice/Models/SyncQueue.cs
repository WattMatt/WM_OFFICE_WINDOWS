using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace WMOffice.Models
{
    public class SyncItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public object Data { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class SyncQueue
    {
        private readonly ConcurrentQueue<SyncItem> _queue = new ConcurrentQueue<SyncItem>();
        private readonly int _maxRetries = 3;
        private readonly int _baseBackoffMs = 500;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        public void Enqueue(object data)
        {
            _queue.Enqueue(new SyncItem { Data = data });
            System.Diagnostics.Debug.WriteLine("Item enqueued.");
        }

        public async Task StartProcessingAsync()
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                if (_queue.TryDequeue(out var item))
                {
                    await ProcessItemWithRetryAsync(item);
                }
                else
                {
                    await Task.Delay(100); // Poll delay when empty
                }
            }
        }

        public void Stop()
        {
            _cts.Cancel();
        }

        private async Task ProcessItemWithRetryAsync(SyncItem item)
        {
            int attempts = 0;
            bool success = false;

            while (attempts <= _maxRetries && !success)
            {
                try
                {
                    attempts++;
                    // Simulate processing (Replace with actual API call)
                    System.Diagnostics.Debug.WriteLine($"Processing item {item.Id}, attempt {attempts}...");
                    
                    // Simulate random failure for demo
                    if (new Random().Next(0, 5) == 0) throw new Exception("Random network glitch");

                    success = true; // Mark as done
                    System.Diagnostics.Debug.WriteLine($"Item {item.Id} synced successfully.");
                }
                catch (Exception ex)
                {
                    if (attempts > _maxRetries)
                    {
                        System.Diagnostics.Debug.WriteLine($"Failed to sync item {item.Id} after {_maxRetries} attempts. Error: {ex.Message}");
                        // Handle dead letter queue here ideally
                    }
                    else
                    {
                        // Exponential Backoff
                        int delay = _baseBackoffMs * (int)Math.Pow(2, attempts - 1);
                        System.Diagnostics.Debug.WriteLine($"Retry {attempts} failed. Waiting {delay}ms...");
                        await Task.Delay(delay);
                    }
                }
            }
        }
    }
}
