# Xcepto.NET — Agent Guide

C# / .NET implementation of the Xcepto system testing framework. Targets net9.0.

## Solution structure

```
Xcepto/               — Core state machine, scenarios, adapters base
Xcepto.Rest/          — REST adapter (typed responses, bearer tokens, promises)
Xcepto.SSR/           — SSR adapter (cookie jar, form content, HTML assertions)
Xcepto.NewtonsoftJson/ — ISerializer implementation via Newtonsoft.Json
Xcepto.Internal.Http/ — Shared HTTP state builder base (HttpStateBuilderIdentity<T>)
Xcepto.Testcontainers/ — Testcontainers integration for scenario setup
Examples/             — Runnable end-to-end sample projects + docs examples
Examples/Docs/        — Documentation example project (compile-only, not published)
```

## Docs example project

`Examples/Docs/Xcepto.Docs.Examples/` contains all code snippets shown on xcepto.org, as compilable NUnit tests marked `[Ignore("docs-example")]`.

**Purpose:** Snippets shown in the docs must compile against the real framework. Before changing a docs snippet, change it here first and verify:
```
dotnet build Examples/Docs/Xcepto.Docs.Examples/Xcepto.Docs.Examples.csproj
```
Then copy the verified code into `xcepto-docs/src/pages/index.tsx` or the relevant `.md` file.

## Key API facts

### REST adapter builder chain

```csharp
// builder is IStateMachineBuilder / TransitionBuilder
var rest = builder.RestAdapterBuilder()   // extension in Xcepto.Rest.Extensions
    .WithBaseUrl(new Uri("http://..."))
    .WithSerializer(new NewtonsoftSerializer())
    .Build();

// Plain step (status assertions only)
rest.Post("/path").WithRequestBody(() => new Req()).AssertSuccess();

// Typed response step — WithResponseType<T> returns DeserializedResponseRestStateBuilderIdentity<T>
rest.Get("/path")
    .WithResponseType<MyResponse>()
    .AssertThatResponse(r => r.SomeField, Is.EqualTo(expected));  // selector + NUnit constraint

// Promise
Promise<T> p = rest.Post("/path")
    .WithResponseType<T>()
    .AssertThatResponse(...)
    .PromiseResponse();
// Consume lazily:
rest.Get("/other").WithBearerTokenClient(() => p.Resolve().Token);
```

### SSR adapter

```csharp
var ssr = builder.SsrAdapterBuilder()   // extension in Xcepto.SSR.Extensions
    .WithBaseUrl(new Uri("http://..."))
    .Build();  // Creates cookie-aware HttpClient automatically

ssr.Post("/auth/login")
    .WithFormContent(new LoginRequest(u, p).ToForm())
    .AssertSuccess();

ssr.Get("/dashboard")
    .AssertThatResponseContentString(Does.Contain(username));  // IResolveConstraint
```

### HTTP verb behavior

- POST / PATCH → executes once, immediate failure on assertion error
- GET / PUT / DELETE → retried until assertions pass or timeout expires

### TimeoutConfig

```csharp
new TimeoutConfig(total: TimeSpan.FromSeconds(30), test: TimeSpan.FromSeconds(25))
// or
TimeoutConfig.FromSeconds(30)  // convenience factory
```

## Test project dependencies

NUnit 4.x + NUnit.Analyzers + NUnit3TestAdapter + Microsoft.NET.Test.Sdk.

Use `[TestFixture]` on the class, `[Test]` on methods, `[Ignore("reason")]` for compile-only examples.
