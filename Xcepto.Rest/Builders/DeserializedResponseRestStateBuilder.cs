using System;
using System.Runtime.Serialization;
using NUnit.Framework.Constraints;
using Xcepto.Data;
using Xcepto.Exceptions;
using Xcepto.Interfaces;
using Xcepto.Rest.Internals;
using Xcepto.States;

namespace Xcepto.Rest.Builders;

public class DeserializedResponseRestStateBuilder<TResponse>: RestStateBuilder<DeserializedResponseRestStateBuilder<TResponse>>
where TResponse: notnull
{
    private Promise<TResponse>? _promisedResponse;

    public DeserializedResponseRestStateBuilder(IStateMachineBuilder stateMachineBuilder) : base(stateMachineBuilder)
    {
    }

    public DeserializedResponseRestStateBuilder<TResponse> AssertThatResponse(
        Func<TResponse, object> selector, IResolveConstraint constraint) =>
        AssertThatResponse(async responseMessage =>
        {
            if(Serializer is null)
                throw new SerializationException("No serializer defined");
            var content = await responseMessage.Content.ReadAsStringAsync();
            var deserialized = Serializer.Deserialize<TResponse>(content);
            return selector(deserialized);
        }, constraint);
    
    public DeserializedResponseRestStateBuilder<TResponse> AssertThatResponse(IResolveConstraint constraint) =>
        AssertThatResponse(x => x, constraint);


    public Promise<TResponse> PromiseResponse()
    {
        _promisedResponse = new Promise<TResponse>();
        return _promisedResponse;
    }

    protected override XceptoState Build()
    {
        return new XceptoRestState(Name, RequestBody, Url, Client, MethodVerb, Retry, ResponseAssertions, async response =>
        {
            if(_promisedResponse is null)
                return;
            if (Serializer is null)
                throw new PromiseException("No serializer defined");
            var content = await response.Content.ReadAsStringAsync();
            var responseObject = Serializer.Deserialize<TResponse>(content);
            _promisedResponse.Settle(responseObject);
        });
    }
}