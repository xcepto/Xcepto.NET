using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Xcepto.Interfaces;
using Xcepto.Internal.Http.Builders;
using Xcepto.States;

namespace Xcepto.SSR.Builders;

public class SsrStateBuilder: HttpStateBuilder<SsrStateBuilder>
{
    private FormUrlEncodedContent? _formContent;

    public SsrStateBuilder(IStateMachineBuilder stateMachineBuilder) : base(stateMachineBuilder)
    {
    }

    protected override string DefaultName => $"SSR {MethodVerb} state";

    public SsrStateBuilder WithFormContent(FormUrlEncodedContent formContent)
    {
        _formContent = formContent;
        return this;
    }
    
    protected override XceptoState Build()
    {
        return new XceptoSsrState(Name, Url, _formContent, ResponseAssertions, Retry, Client, MethodVerb);
    }
}