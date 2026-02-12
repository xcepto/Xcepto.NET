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

public abstract class RestStateBuilder<TBuilder>: HttpStateBuilder<TBuilder>
where TBuilder: RestStateBuilder<TBuilder>
{
    protected RequestBody? RequestBody;
    protected ISerializer? Serializer;

    internal RestStateBuilder(IStateMachineBuilder stateMachineBuilder) : base(stateMachineBuilder) { }

    protected override string DefaultName => $"REST {MethodVerb} request state to {PathString}";
    
    public TBuilder WithRequestBody<TRequestBody>(TRequestBody requestBody)
    where TRequestBody: notnull
    {
        RequestBody = new RequestBody(typeof(TRequestBody), requestBody,
            o =>
            {
                if (Serializer is null)
                    throw new SerializationException("No serializer defined"); 
                return Serializer.Serialize((TRequestBody)o);
            });
        return (TBuilder)this;
    }
    
    public TBuilder WithRequestBody<TRequestBody>(TRequestBody requestBody, 
        Func<TRequestBody, string> customSerialization)
        where TRequestBody: notnull
    {
        RequestBody = new RequestBody(typeof(TRequestBody), requestBody, 
            o => customSerialization((TRequestBody)o));
        return (TBuilder)this;
    }
    
    public TBuilder WithRequestBody(Type requestType, object requestBody, Func<object, string> customSerialization)
    {
        RequestBody = new RequestBody(requestType, requestBody, customSerialization);
        return (TBuilder)this;
    }
    
    public TBuilder WithSerializer(ISerializer serializer)
    {
        Serializer = serializer;
        return (TBuilder)this;
    }

    public DeserializedResponseRestStateBuilder<TResponse> WithResponseType<TResponse>()
    where TResponse: notnull
    {
        var builder = new DeserializedResponseRestStateBuilder<TResponse>(StateMachineBuilder)
            .WithRetry(Retry)
            .WithCustomName(Name)
            .WithCustomClient(Client)
            .WithHttpVerb(MethodVerb)
            .WithPathString(PathString)
            .WithCustomBaseUrl(Url);
        
        if (Serializer is not null)
            builder.WithSerializer(Serializer);
        if (RequestBody is not null)
            builder.WithRequestBody(RequestBody.RequestType, RequestBody.RequestObject, RequestBody.SerializationMethod);
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