using System;
using System.Net;
using System.Net.Http;
using Xcepto.Builder;
using Xcepto.Interfaces;

namespace Xcepto.SSR.Builders;

public class SsrAdapterBuilder: AbstractAdapterBuilder<SsrAdapterBuilder, XceptoSsrAdapter>
{
    private HttpClient? _httpClient;
    private Uri? _baseUrl;

    public SsrAdapterBuilder(IStateMachineBuilder stateMachineBuilder) : base(stateMachineBuilder)
    {
    }
    
    public SsrAdapterBuilder WithHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        return this;
    }
    
    public SsrAdapterBuilder WithBaseUrl(Uri baseUrl)
    {
        _baseUrl = baseUrl;
        return this;
    }
    
    public override XceptoSsrAdapter Build()
    {
        if (_httpClient is null)
        {
            var cookieContainer = new CookieContainer();

            var handler = new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                UseCookies = true
            };

            _httpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
        }
        var xceptoSsrAdapter = new XceptoSsrAdapter(_httpClient, _baseUrl);
        StateMachineBuilder.RegisterAdapter(xceptoSsrAdapter);
        return xceptoSsrAdapter;
    }
}