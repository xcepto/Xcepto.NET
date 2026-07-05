using Xcepto.Adapters;

namespace Samples.ExceptionDetail.Tests.Adapters;

public class FailingConstructionAdapter: XceptoAdapter
{
    public FailingConstructionAdapter()
    {
        throw new Exception();
    }
}