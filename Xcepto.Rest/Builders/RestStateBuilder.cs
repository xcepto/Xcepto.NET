using System;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;
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
    private ResponseValidation? _responseValidation;
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
    
    public RestStateStateBuilder WithResponseValidation<TResponseBody>(Predicate<TResponseBody> predicate)
    {
        _responseValidation = new ResponseValidation(typeof(TResponseBody),
            o => predicate((TResponseBody)o),
            s =>
            {
                if (_serializer is null)
                    throw new SerializationException("No serializer defined");
                var deserialized = _serializer.Deserialize<TResponseBody>(s);
                if (deserialized is null)
                    throw new SerializationException($"Could not deserialize string to {typeof(TResponseBody).FullName} properly: {s}");
                return deserialized;
            });
        return this;
    }
    
    public RestStateStateBuilder WithResponseValidation<TResponseBody>(Predicate<TResponseBody> predicate, 
        Func<string, TResponseBody> customDeserialization)
    {
        _responseValidation = new ResponseValidation(typeof(TResponseBody),
            o => predicate((TResponseBody)o),
            s =>
            {
                var deserialized = customDeserialization(s);
                if (deserialized is null)
                    throw new SerializationException($"Could not deserialize string to {typeof(TResponseBody).FullName} properly: {s}");
                return deserialized;
            });
        return this;
    }
    
    public RestStateStateBuilder WithSerializer(ISerializer serializer)
    {
        _serializer = serializer;
        return this;
    }
    
    protected override XceptoState Build()
    {
        return new XceptoPostRestState(Name, _requestBody, _responseValidation, Url, Client, MethodVerb);
    }
}