using System;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;
using NUnit.Framework.Constraints;
using Xcepto.Exceptions;
using Xcepto.Interfaces;
using Xcepto.Internal.Http.Builders;
using Xcepto.Internal.Http.Data;
using Xcepto.Rest.Data;
using Xcepto.Rest.Internals;
using Xcepto.States;

namespace Xcepto.Rest.Builders;

public sealed class RestStateStateBuilder: HttpStateBuilder<RestStateStateBuilder>
{
    private RequestBody? _requestBody;
    private ISerializer? _serializer;


    internal RestStateStateBuilder(IStateMachineBuilder stateMachineBuilder) : base(stateMachineBuilder) { }

    protected override string DefaultName => $"REST {MethodVerb} request state to {PathString}";
    
    public RestStateStateBuilder WithRequestBody<TRequestBody>(TRequestBody requestBody)
    where TRequestBody: notnull
    {
        _requestBody = new RequestBody(typeof(TRequestBody), requestBody,
            o =>
            {
                if (_serializer is null)
                    throw new SerializationException("No serializer defined"); 
                return _serializer.Serialize((TRequestBody)o);
            });
        return this;
    }
    
    public RestStateStateBuilder WithRequestBody<TRequestBody>(TRequestBody requestBody, 
        Func<TRequestBody, string> customSerialization)
        where TRequestBody: notnull
    {
        _requestBody = new RequestBody(typeof(TRequestBody), requestBody, 
            o => customSerialization((TRequestBody)o));
        return this;
    }
    
    public RestStateStateBuilder WithSerializer(ISerializer serializer)
    {
        _serializer = serializer;
        return this;
    }
    
    protected override XceptoState Build()
    {
        return new XceptoRestState(Name, _requestBody, Url, Client, MethodVerb, Retry, ResponseAssertions);
    }

    public RestStateStateBuilder AssertThatDeserializedResponse<TResponse>(
        Func<TResponse, object> selector, IResolveConstraint constraint) =>
        AssertThatResponse(async responseMessage =>
        {
            if(_serializer is null)
                throw new SerializationException("No serializer defined");
            var content = await responseMessage.Content.ReadAsStringAsync();
            var deserialized = _serializer.Deserialize<TResponse>(content);
            return selector(deserialized);
        }, constraint);
    
    public RestStateStateBuilder AssertThatDeserializedResponse<TResponse>(IResolveConstraint constraint) 
        where TResponse: notnull =>
        AssertThatDeserializedResponse<TResponse>(x => x, constraint);
}