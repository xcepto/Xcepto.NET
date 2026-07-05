using System.Net;
using System.Text.RegularExpressions;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.DependencyInjection;
using Samples.SSR.GUI.Requests;
using Samples.SSR.GUI.Tests.Scenarios;
using Testcontainers.PostgreSql;
using Xcepto;
using Xcepto.Config;
using Xcepto.Interfaces;
using Xcepto.Internal.Http.Data;
using Xcepto.SSR;
using Xcepto.SSR.Extensions;

namespace Samples.SSR.GUI.Tests.Xcepto;

[TestFixture]
public class PromisedPathTests
{
    private static string ParsePath(string html)
    {
        var regex = new Regex(@"Path:\s*([^<]+)");
        var match = regex.Match(html);

        if (!match.Success)
            throw new Exception($"Path not found in response: {html}"); 
        return match.Groups[1].Value;
    }

    readonly SsrGuiScenario _scenario = new();
    readonly TimeoutConfig _timeoutConfig = new(
        TimeSpan.FromSeconds(120), 
        TimeSpan.FromSeconds(10) 
    );
    
    [Test]
    public async Task PromisedPath_GET_works()
    {
        await XceptoTest.Given(_scenario, _timeoutConfig, builder =>
        {
            var ssr = builder.SsrAdapterBuilder()
                .WithBaseUrl(new Uri($"http://localhost:{_scenario.GuiPort}"))
                .Build();

            var promise = ssr.Get("/path/get/get")
                .AssertSuccess()
                .PromiseResponse();

            ssr.Get(() => ParsePath(promise.Resolve()))
                .AssertSuccess();
        });
    }
    
    [Test]
    public async Task PromisedPath_POST_works()
    {
        await XceptoTest.Given(_scenario, _timeoutConfig, builder =>
        {
            var ssr = builder.SsrAdapterBuilder()
                .WithBaseUrl(new Uri($"http://localhost:{_scenario.GuiPort}"))
                .Build();

            var promise = ssr.Get("/path/get/post")
                .AssertSuccess()
                .PromiseResponse();

            ssr.Post(() => ParsePath(promise.Resolve()))
                .AssertSuccess();
        });
    }
    
    [Test]
    public async Task PromisedPath_PATCH_works()
    {
        await XceptoTest.Given(_scenario, _timeoutConfig, builder =>
        {
            var ssr = builder.SsrAdapterBuilder()
                .WithBaseUrl(new Uri($"http://localhost:{_scenario.GuiPort}"))
                .Build();

            var promise = ssr.Get("/path/get/patch")
                .AssertSuccess()
                .PromiseResponse();

            ssr.Request(() => ParsePath(promise.Resolve()), HttpMethodVerb.Patch)
                .AssertSuccess();
        });
    }
    
    [Test]
    public async Task PromisedPath_PUT_works()
    {
        await XceptoTest.Given(_scenario, _timeoutConfig, builder =>
        {
            var ssr = builder.SsrAdapterBuilder()
                .WithBaseUrl(new Uri($"http://localhost:{_scenario.GuiPort}"))
                .Build();

            var promise = ssr.Get("/path/get/put")
                .AssertSuccess()
                .PromiseResponse();

            ssr.Request(() => ParsePath(promise.Resolve()), HttpMethodVerb.Put)
                .AssertSuccess();
        });
    }
    
    [Test]
    public async Task PromisedPath_DELETE_works()
    {
        await XceptoTest.Given(_scenario, _timeoutConfig, builder =>
        {
            var ssr = builder.SsrAdapterBuilder()
                .WithBaseUrl(new Uri($"http://localhost:{_scenario.GuiPort}"))
                .Build();

            var promise = ssr.Get("/path/get/delete")
                .AssertSuccess()
                .PromiseResponse();

            ssr.Request(() => ParsePath(promise.Resolve()), HttpMethodVerb.Delete)
                .AssertSuccess();
        });
    }
}