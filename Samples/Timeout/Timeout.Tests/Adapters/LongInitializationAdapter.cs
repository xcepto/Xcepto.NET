using Microsoft.Extensions.DependencyInjection;
using Xcepto;
using Xcepto.Adapters;

namespace Timeout.Tests.Adapters;

public class LongInitializationAdapter: XceptoAdapter
{
    protected override Task Initialize(IServiceProvider serviceProvider) => Task.Delay(TimeSpan.FromSeconds(10));

    protected override Task Cleanup(IServiceProvider serviceProvider) => Task.CompletedTask;
}