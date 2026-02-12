using System;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;
using NUnit.Framework.Constraints;
using Xcepto.Data;
using Xcepto.Exceptions;
using Xcepto.Interfaces;
using Xcepto.Internal.Http.Builders;
using Xcepto.Internal.Http.Data;
using Xcepto.Rest.Data;
using Xcepto.Rest.Internals;
using Xcepto.States;

namespace Xcepto.Rest.Builders;

public abstract class RestStateBuilderIdentity<TBuilder>: HttpStateBuilderIdentity<TBuilder>
where TBuilder: RestStateBuilderIdentity<TBuilder>
{
    protected RequestBody? RequestBody;
    protected ISerializer? Serializer;

    internal RestStateBuilderIdentity(IStateMachineBuilder stateMachineBuilder) : base(stateMachineBuilder) { }
    internal RestStateBuilderIdentity(IStateMachineBuilder stateMachineBuilder, IStateBuilderIdentity stateBuilderIdentity) : base(stateMachineBuilder, stateBuilderIdentity) { }

    protected override string DefaultName => $"REST {MethodVerb} request state to {PathString}";
    
    public TBuilder WithRequestBody<TRequestBody>(Func<TRequestBody> requestBodyProducer)
    where TRequestBody: notnull
    {
        return WithRequestBody<TRequestBody>(requestBodyProducer, o =>
        {
            if (Serializer is null)
                throw new SerializationException("No serializer defined"); 
            return Serializer.Serialize((TRequestBody)o);
        });
    }
    
    public TBuilder WithRequestBody<TRequestBody>(Func<TRequestBody> requestBodyProducer, 
        Func<TRequestBody, string> customSerialization)
        where TRequestBody: notnull
    {
        RequestBody = new RequestBody(typeof(TRequestBody), () => requestBodyProducer(), 
            o => customSerialization((TRequestBody)o));
        return (TBuilder)this;
    }
    
    public TBuilder WithSerializer(ISerializer serializer)
    {
        Serializer = serializer;
        return (TBuilder)this;
    }

    public DeserializedResponseRestStateBuilderIdentity<TResponse> WithResponseType<TResponse>()
    where TResponse: notnull
    {
        var builder = new DeserializedResponseRestStateBuilderIdentity<TResponse>(StateMachineBuilder, this)
            .WithRetry(Retry)
            .WithCustomName(Name)
            .WithCustomClient(Client)
            .WithHttpVerb(MethodVerb)
            .WithPathString(PathString)
            .WithCustomBaseUrl(Url);
        
        if (Serializer is not null)
            builder.WithSerializer(Serializer);
        if (RequestBody is not null)
            builder.InjectRequestBody(RequestBody);
        foreach (var pair in QueryArgs)
        {
            builder.AddQueryArgument(pair.Key, pair.Value);
        }
        foreach (var assertion in ResponseAssertions)
        {
            builder.AssertThatResponse(assertion.Selector, assertion.ResolveConstraint);
        }

        return builder;
    }
}