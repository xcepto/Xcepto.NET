using Microsoft.Extensions.DependencyInjection;
using Samples.Shopping.Tests.Providers;
using Xcepto;
using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Interfaces;
using Xcepto.Internal;
using Xcepto.RabbitMQ;
using Xcepto.Scenarios;

namespace Samples.Shopping.Tests.Scenarios;

public class ChristmasGiftsSyncScenario: XceptoScenario
{
    protected override ScenarioSetup Setup(ScenarioSetupBuilder builder) => builder
        .AddServices(services => services
            .AddSingleton<ILoggingProvider, LoggingProvider>()
            .AddSingleton<XceptoRabbitMqRepository>()
        )
        .Build();

}