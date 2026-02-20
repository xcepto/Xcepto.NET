using Xcepto.Adapters;

namespace Samples.ExceptionDetail.Tests.Adapters;

public class FailingInitAdapter: XceptoAdapter
{
    protected override Task Initialize(IServiceProvider serviceProvider)
    {
        throw new Exception();
    }
}