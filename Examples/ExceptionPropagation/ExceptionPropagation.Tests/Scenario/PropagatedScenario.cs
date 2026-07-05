using ExceptionPropagation.Tests.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Xcepto;
using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Interfaces;
using Xcepto.Internal;
using Xcepto.Provider;
using Xcepto.Scenarios;

namespace ExceptionPropagation.Tests.Scenario;

public class PropagatedScenario: XceptoScenario
{
    protected override ScenarioSetup Setup(ScenarioSetupBuilder builder) => builder
        .AddServices(services => services
            .AddSingleton<ILoggingProvider, XceptoBasicLoggingProvider>()
        )
        .Build();

    protected override ScenarioInitialization Initialize(ScenarioInitializationBuilder builder) {
        var tcs = new TaskCompletionSource();
        builder.FireAndForget(_ => tcs.Task);
        tcs.SetException(new PropagatedException());
        return builder.Build();
    }
}