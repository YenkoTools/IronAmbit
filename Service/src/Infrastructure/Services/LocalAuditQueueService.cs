using System.Threading.Channels;
using Application.Interfaces;

namespace Infrastructure.Services;

public class LocalAuditQueueService<T> : ILocalQueueService<T>
{
    private readonly Channel<T> _channel = Channel.CreateUnbounded<T>(new UnboundedChannelOptions());

    public ValueTask<T> DequeueAsync(CancellationToken cancellationToken = default)
    {
        return _channel.Reader.ReadAsync(cancellationToken);
    }

    public bool Enqueue(T item)
    {
        Console.WriteLine("Enqueuing item: {0}", item);
        return _channel.Writer.TryWrite(item);
    }

    public ValueTask<bool> WaitToReadAsync(CancellationToken cancellationToken = default)
    {
        return _channel.Reader.WaitToReadAsync();
    }
}