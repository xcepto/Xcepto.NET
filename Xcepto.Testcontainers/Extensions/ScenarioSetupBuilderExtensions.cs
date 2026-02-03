using Microsoft.Extensions.DependencyInjection;
using Xcepto.Builder;
using Xcepto.Testcontainers.Interfaces;
using Xcepto.Testcontainers.Internal;

namespace Xcepto.Testcontainers.Extensions;

public static class ScenarioSetupBuilderExtensions
{
    public static ScenarioSetupBuilder AddTestcontainersSupport(this ScenarioSetupBuilder builder)
    {
        builder.AddServices(x => x
                .AddSingleton<ITestContainerSupport, TestContainerOutputProvider>()
            )
            .RegisterDisposable<ITestContainerSupport>();
        return builder;
    }
}