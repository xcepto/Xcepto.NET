using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Xcepto.Adapters;
using Xcepto.Internal.Http.Data;
using Xcepto.SSR.Builders;

namespace Xcepto.SSR;

public class XceptoSsrAdapter: XceptoAdapter
{
    private HttpClient _client;
    private Uri? _baseUrl;

    internal XceptoSsrAdapter(HttpClient httpClient, Uri? baseUrl)
    {
        _baseUrl = baseUrl;
        _client = httpClient;
    }

    private SsrStateBuilder Inject(SsrStateBuilder builder, PathString pathString, HttpMethodVerb httpMethodVerb)
    {
        if (_baseUrl is not null)
            builder.WithCustomBaseUrl(_baseUrl);
        
        builder.WithCustomClient(_client);
        builder.WithPathString(pathString);
        builder.WithHttpVerb(httpMethodVerb);

        return builder;
    }

    public SsrStateBuilder Get(PathString pathString)
    {
        return Inject(new SsrStateBuilder(Builder), pathString, HttpMethodVerb.Get);
    }
    
    public SsrStateBuilder Post(PathString pathString)
    {
        return Inject(new SsrStateBuilder(Builder), pathString, HttpMethodVerb.Post);
    }
}