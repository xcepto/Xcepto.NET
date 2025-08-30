using Microsoft.Extensions.DependencyInjection;
using Xcepto;

namespace Timeout.Tests.Adapters;

public class LongAddServicesAdapter: XceptoAdapter
{
    public override void AssignBuilder(TransitionBuilder builder) { }

    protected override Task Initialize(IServiceProvider serviceProvider) => Task.CompletedTask;

    protected override Task Cleanup(IServiceProvider serviceProvider) => Task.CompletedTask;

    protected override Task AddServices(IServiceCollection serviceCollection) => Task.Delay(TimeSpan.FromSeconds(10));
}