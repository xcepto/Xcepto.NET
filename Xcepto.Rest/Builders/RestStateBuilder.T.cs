using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;
using Xcepto.Builder;
using Xcepto.Interfaces;
using Xcepto.Rest.Data;

namespace Xcepto.Rest.Builders;

public abstract class RestStateStateBuilder<TBuilder>: AbstractStateBuilder<TBuilder>
where TBuilder : RestStateStateBuilder<TBuilder>
{
    protected readonly RestHttpMethod Method;
    protected readonly PathString PathString;
    protected readonly List<KeyValuePair<string, string>> QueryArgs = new();
    protected HttpClient Client;
    protected Uri? BaseUrl;
    protected RequestBody? RequestBody;
    protected ResponseValidation? ResponseValidation;
    protected ISerializer? Serializer;


    internal RestStateStateBuilder(IStateMachineBuilder stateMachineBuilder, RestHttpMethod method, HttpClient client, PathString pathString) 
        : base(stateMachineBuilder)
    {
        Method = method;
        Client = client;
        PathString = pathString;
    }

    protected override string DefaultName => $"REST {Method} request state to {PathString}";

    public TBuilder WithCustomClient(HttpClient client)
    {
        Client = client;
        return (TBuilder)this;
    }
    
    internal TBuilder InjectBaseUrl(Uri? uri)
    {
        BaseUrl = uri;
        return (TBuilder)this;
    }
    public TBuilder WithCustomBaseUrl(Uri uri)
    {
        BaseUrl = uri;
        return (TBuilder)this;
    }
    
    public TBuilder AddQueryArgument(string key, string value)
    {
        QueryArgs.Add(new KeyValuePair<string, string>(key, value));
        return (TBuilder)this;
    }
    
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
    
    public TBuilder WithResponseValidation<TResponseBody>(Predicate<TResponseBody> predicate)
    {
        ResponseValidation = new ResponseValidation(typeof(TResponseBody),
            o => predicate((TResponseBody)o),
            s =>
            {
                if (Serializer is null)
                    throw new SerializationException("No serializer defined");
                var deserialized = Serializer.Deserialize<TResponseBody>(s);
                if (deserialized is null)
                    throw new SerializationException($"Could not deserialize string to {typeof(TResponseBody).FullName} properly: {s}");
                return deserialized;
            });
        return (TBuilder)this;
    }
    
    public TBuilder WithResponseValidation<TResponseBody>(Predicate<TResponseBody> predicate, 
        Func<string, TResponseBody> customDeserialization)
    {
        ResponseValidation = new ResponseValidation(typeof(TResponseBody),
            o => predicate((TResponseBody)o),
            s =>
            {
                var deserialized = customDeserialization(s);
                if (deserialized is null)
                    throw new SerializationException($"Could not deserialize string to {typeof(TResponseBody).FullName} properly: {s}");
                return deserialized;
            });
        return (TBuilder)this;
    }
    
    public TBuilder WithSerializer(ISerializer serializer)
    {
        Serializer = serializer;
        return (TBuilder)this;
    }
    
    public TBuilder InjectSerializer(ISerializer? serializer)
    {
        Serializer = serializer;
        return (TBuilder)this;
    }
}