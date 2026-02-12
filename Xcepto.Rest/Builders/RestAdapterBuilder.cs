using System;
using System.Net.Http;
using Xcepto.Builder;
using Xcepto.Interfaces;

namespace Xcepto.Rest.Builders;

public class RestAdapterBuilder: AbstractAdapterBuilder<RestAdapterBuilder, XceptoRestAdapter>
{
    private HttpClient _client = new();
    private Uri? _baseUrl = null;
    private ISerializer? _serializer;

    public RestAdapterBuilder(IStateMachineBuilder stateMachineBuilder): base(stateMachineBuilder)
    {
    }
    public override XceptoRestAdapter Build()
    {
        var xceptoRestAdapter = new XceptoRestAdapter(_client, _baseUrl, _serializer);
        StateMachineBuilder.RegisterAdapter(xceptoRestAdapter);
        return xceptoRestAdapter;
    }

    public RestAdapterBuilder WithHttpClient(HttpClient httpClient)
    {
        _client = httpClient;
        return this;
    }

    public RestAdapterBuilder WithBaseUrl(Uri baseUrl)
    {
        _baseUrl = baseUrl;
        return this;
    }
    
    public RestAdapterBuilder WithSerializer(ISerializer serializer)
    {
        _serializer = serializer;
        return this;
    }
}