namespace Samples.CleanupExecution.Tests.Adapters;

public class FailingInitAdapter: TrackableCleanupAdapter
{
    protected override Task Initialize(IServiceProvider serviceProvider)
    {
        throw new Exception();
    }
}