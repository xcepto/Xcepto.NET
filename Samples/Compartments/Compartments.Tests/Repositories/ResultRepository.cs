using System.Collections.Concurrent;

namespace Compartments.Tests.Repositories;

public class ResultRepository
{
    private readonly ConcurrentDictionary<Guid, bool> _completedGuids = new();

    public bool TryGet(Guid guid) =>
        _completedGuids.ContainsKey(guid);

    public void Add(Guid guid) =>
        _completedGuids.TryAdd(guid, true);
}
