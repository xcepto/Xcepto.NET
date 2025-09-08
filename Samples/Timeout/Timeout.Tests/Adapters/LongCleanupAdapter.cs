using Microsoft.Extensions.DependencyInjection;
using Xcepto;

namespace Timeout.Tests.Adapters;

public class LongCleanupAdapter: XceptoAdapter
{
    protected override Task Initialize(IServiceProvider serviceProvider) => Task.CompletedTask;

    protected override Task Cleanup(IServiceProvider serviceProvider) => Task.Delay(TimeSpan.FromSeconds(10));

    protected override Task AddServices(IServiceCollection serviceCollection) => Task.CompletedTask;
}