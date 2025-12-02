using Microsoft.Extensions.DependencyInjection;
using Xcepto;
using Xcepto.Adapters;

namespace Timeout.Tests.Adapters;

public class LongCleanupAdapter: XceptoAdapter
{
    protected override Task Initialize(IServiceProvider serviceProvider) => Task.CompletedTask;

    protected override Task Cleanup(IServiceProvider serviceProvider) => Task.Delay(TimeSpan.FromSeconds(10));
}