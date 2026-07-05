using System.Collections.Concurrent;

namespace Compartments.Tests.Repositories;

public class TaskRepository
{
    private ConcurrentQueue<Guid> _queue = new();
    public void Enqueue(Guid task)
    {
        _queue.Enqueue(task);
    }

    public bool TryDequeue(out Guid task)
    {
        return _queue.TryDequeue(out task);
    }
}