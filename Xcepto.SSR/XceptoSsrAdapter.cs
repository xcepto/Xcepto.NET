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

    private SsrStateBuilderIdentity Inject(SsrStateBuilderIdentity builderIdentity, Func<PathString> pathString, HttpMethodVerb httpMethodVerb)
    {
        if (_baseUrl is not null)
            builderIdentity.WithCustomBaseUrl(_baseUrl);
        
        builderIdentity.WithCustomClient(_client);
        builderIdentity.WithPathString(pathString);
        builderIdentity.WithHttpVerb(httpMethodVerb);

        return builderIdentity;
    }

    public SsrStateBuilderIdentity Request(PathString pathString, HttpMethodVerb verb)
    {
        return Inject(new SsrStateBuilderIdentity(Builder), () => pathString, verb);
    }
    
    public SsrStateBuilderIdentity Request(Func<PathString> pathString, HttpMethodVerb verb)
    {
        return Inject(new SsrStateBuilderIdentity(Builder), pathString, verb);
    }
    
    public SsrStateBuilderIdentity Get(PathString pathString)
    {
        return Inject(new SsrStateBuilderIdentity(Builder), () => pathString, HttpMethodVerb.Get);
    }
    
    public SsrStateBuilderIdentity Post(PathString pathString)
    {
        return Inject(new SsrStateBuilderIdentity(Builder), () => pathString, HttpMethodVerb.Post);
    }
    
    public SsrStateBuilderIdentity Get(Func<PathString> pathString)
    {
        return Inject(new SsrStateBuilderIdentity(Builder), pathString, HttpMethodVerb.Get);
    }
    
    public SsrStateBuilderIdentity Post(Func<PathString> pathString)
    {
        return Inject(new SsrStateBuilderIdentity(Builder), pathString, HttpMethodVerb.Post);
    }
}