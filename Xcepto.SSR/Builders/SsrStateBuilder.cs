using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Xcepto.Data;
using Xcepto.Interfaces;
using Xcepto.Internal.Http.Builders;
using Xcepto.States;

namespace Xcepto.SSR.Builders;

public class SsrStateBuilder: HttpStateBuilder<SsrStateBuilder>
{
    private Func<HttpContent>? _formContent;
    private Promise<string>? _promise;

    public SsrStateBuilder(IStateMachineBuilder stateMachineBuilder) : base(stateMachineBuilder)
    {
    }

    protected override string DefaultName => $"SSR {MethodVerb} state";

    public SsrStateBuilder WithFormContent(FormUrlEncodedContent formContent)
    {
        _formContent = () => formContent;
        return this;
    }
    
    public SsrStateBuilder WithFormContent(Func<FormUrlEncodedContent> formContentProducer)
    {
        _formContent = formContentProducer;
        return this;
    }

    public Promise<string> PromiseResponse()
    {
        _promise = new Promise<string>();
        return _promise;
    }
    
    protected override XceptoState Build()
    {
        return new XceptoSsrState(Name, Url, _formContent, ResponseAssertions, Retry, Client, MethodVerb, async response =>
        {
            if(_promise is null)
                return;
            var content = await response.Content.ReadAsStringAsync();
            _promise.Settle(content);
        });
    }
}