using Application.Interfaces;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Services;

/// <summary>
/// Background service that reads and processes audit queue items.
/// </summary>
public class AuditQueueReader(ILocalQueueService<int> queue) : BackgroundService
{
    /// <summary>
    /// Executes the audit queue reader background task.
    /// </summary>
    /// <param name="stoppingToken">Cancellation token for stopping the service.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await queue.WaitToReadAsync(stoppingToken))
        {
            var item = await queue.DequeueAsync(stoppingToken);
            Console.WriteLine("Processed audit item {0}", item);
        }
    }
}