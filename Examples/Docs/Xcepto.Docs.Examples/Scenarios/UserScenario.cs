using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Xcepto.Builder;
using Xcepto.Data;
using Xcepto.Scenarios;

namespace Xcepto.Docs.Examples;

public class UserScenario : XceptoScenario
{
    private IContainer? _api;
    public Uri BaseUri { get; private set; } = new Uri("http://localhost:8080");
    public Uri GuiAddress { get; private set; } = new Uri("http://localhost:8080");

    protected override ScenarioSetup Setup(ScenarioSetupBuilder builder) => builder.Build();

    protected override ScenarioInitialization Initialize(ScenarioInitializationBuilder builder) => builder
        .Do(async () =>
        {
            _api = new ContainerBuilder(await DocsApiImageSingleton.GetImageOnce())
                .WithPortBinding(8080, true)
                .WithWaitStrategy(Wait.ForUnixContainer()
                    .UntilHttpRequestIsSucceeded(r => r.ForPath("/api/ping").ForPort(8080)))
                .Build();
            await _api.StartAsync();
            var address = new Uri($"http://localhost:{_api.GetMappedPublicPort(8080)}");
            BaseUri = address;
            GuiAddress = address;
        })
        .Build();

    protected override ScenarioCleanup Cleanup(ScenarioCleanupBuilder builder) => builder
        .Do(async () =>
        {
            if (_api != null)
                await _api.StopAsync();
        })
        .Build();
}
