using Microsoft.Extensions.DependencyInjection;
using Xcepto;

namespace Endless.Tests.Adapters;

public class LongInitializationAdapter: XceptoAdapter
{
    public override void AssignBuilder(TransitionBuilder builder) { }

    protected override Task Initialize(IServiceProvider serviceProvider) => Task.Delay(TimeSpan.FromSeconds(10));

    protected override Task Cleanup(IServiceProvider serviceProvider) => Task.CompletedTask;

    protected override Task AddServices(IServiceCollection serviceCollection) => Task.CompletedTask;
}