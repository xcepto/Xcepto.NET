using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xcepto.Adapters;

namespace Xcepto.SSR;

public class XceptoSSRAdapter: XceptoAdapter
{
    public void Get(Uri url, Func<HttpContent,Task<bool>> responseValidator, bool retry = true)
    {
        AddStep(new XceptoGetSSRState("GetSSRState", 
            url,
            responseValidator,
            retry
        ));
    }
    
    public void Post(Uri url, HttpContent request, Func<HttpContent,Task<bool>> responseValidator, bool retry = false)
    {
        AddStep(new XceptoPostSSRState("PostSSRState", 
            request,
            url,
            responseValidator,
            retry
        ));
    }
}