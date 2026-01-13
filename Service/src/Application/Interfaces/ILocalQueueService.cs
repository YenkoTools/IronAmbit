namespace Application.Interfaces;

public interface ILocalQueueService<T>
{
    public ValueTask<T> DequeueAsync(CancellationToken cancellationToken = default);
    public bool Enqueue(T item);
    public ValueTask<bool> WaitToReadAsync(CancellationToken cancellationToken = default);
    
}