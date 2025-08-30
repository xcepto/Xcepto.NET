using Microsoft.Extensions.DependencyInjection;
using Samples.Shopping.Tests.Providers;
using Xcepto;
using Xcepto.Interfaces;

namespace Samples.Shopping.Tests.Scenarios;

public class ChristmasGiftsScenario: Scenario
{
    public override IServiceCollection Setup()
    {
        LoggingProvider loggingProvider = new LoggingProvider();
        
        ServiceCollection services = new ServiceCollection();
        services.AddSingleton<ILoggingProvider, LoggingProvider>(x => loggingProvider);
        return services;
    }

    public override Task Initialize(IServiceProvider serviceProvider) => Task.CompletedTask;

    public override Task Cleanup(IServiceProvider serviceProvider) => Task.CompletedTask;
}