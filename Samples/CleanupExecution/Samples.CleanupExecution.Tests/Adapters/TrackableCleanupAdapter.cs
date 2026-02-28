using Xcepto.Adapters;

namespace Samples.CleanupExecution.Tests.Adapters;

public abstract class TrackableCleanupAdapter: XceptoAdapter
{
    public bool CleanedUp { get; private set; } = false;
    protected override Task Cleanup(IServiceProvider serviceProvider)
    {
        CleanedUp = true;
        return Task.CompletedTask;
    }
}