using Microsoft.Extensions.DependencyInjection;
using Samples.Shopping.Tests.Providers;
using Xcepto;
using Xcepto.Interfaces;
using Xcepto.RabbitMQ;

namespace Samples.Shopping.Tests.Scenarios;

public class ChristmasGiftsScenario: XceptoScenario
{
    protected override Task<IServiceCollection> Setup()
    {
        LoggingProvider loggingProvider = new LoggingProvider();
        
        ServiceCollection services = new ServiceCollection();
        services.AddSingleton<ILoggingProvider, LoggingProvider>(x => loggingProvider);
        services.AddSingleton<XceptoRabbitMqRepository>();
        return Task.FromResult<IServiceCollection>(services);
    }

    protected override Task Initialize(IServiceProvider serviceProvider) => Task.CompletedTask;

    protected override Task Cleanup(IServiceProvider serviceProvider) => Task.CompletedTask;
}