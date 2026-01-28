using System;
using System.Net.Http;
using Xcepto.Adapters;

namespace Xcepto.SSR;

public class XceptoSSRAdapter: XceptoAdapter
{
    public void PostRequest(Uri url, HttpContent request, Predicate<HttpContent> responseValidator)
    {
        AddStep(new XceptoPostSSRState("PostSSRState", 
            request,
            url,
            responseValidator 
        ));
    }
}