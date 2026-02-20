using Xcepto.Adapters;

namespace Samples.ExceptionDetail.Tests.Adapters;

public class FailingCleanupAdapter: XceptoAdapter
{
    protected override Task Cleanup(IServiceProvider serviceProvider)
    {
        throw new Exception();
    }
}