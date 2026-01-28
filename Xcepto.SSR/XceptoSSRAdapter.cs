using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xcepto.Adapters;

namespace Xcepto.SSR;

public class XceptoSSRAdapter: XceptoAdapter
{
    public void GetRequest(Uri url, Func<HttpContent,Task<bool>> responseValidator)
    {
        AddStep(new XceptoGetSSRState("GetSSRState", 
            url,
            responseValidator 
        ));
    }
    
    public void PostRequest(Uri url, HttpContent request, Func<HttpContent,Task<bool>> responseValidator)
    {
        AddStep(new XceptoPostSSRState("PostSSRState", 
            request,
            url,
            responseValidator 
        ));
    }
}