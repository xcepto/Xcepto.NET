using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Xcepto.Exceptions;
using Xcepto.Interfaces;
using Xcepto.Rest.Data;
using Xcepto.Rest.Internals;
using Xcepto.States;

namespace Xcepto.Rest.Builders;

public sealed class RestStateStateBuilder: RestStateStateBuilder<RestStateStateBuilder>
{
    internal RestStateStateBuilder(IStateMachineBuilder stateMachineBuilder, RestHttpMethod method, HttpClient client, PathString pathString) 
        : base(stateMachineBuilder, method, client, pathString)
    {
    }

    protected override XceptoState Build()
    {
        if (BaseUrl is null)
            throw new BuilderException("no Url defined");
        if (!Uri.TryCreate(BaseUrl, PathString + QueryString.Create(QueryArgs), out var uri))
            throw new ArgumentException("Url creation failed");
        return new XceptoPostRestState(Name, RequestBody, ResponseValidation, uri, Client, Method);
    }
}