using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xcepto.Adapters;

namespace Xcepto.SSR;

public class XceptoSSRAdapter: XceptoAdapter
{
    public void Get(Uri url, Func<HttpResponseMessage, bool> responseValidator, bool retry = true) =>
        Get(url, response => Task.FromResult(responseValidator(response)), retry);

    public void Get(Uri url, Func<HttpResponseMessage,Task<bool>> responseValidator, bool retry = true)
    {
        AddStep(new XceptoGetSSRState("GetSSRState", 
            url,
            responseValidator,
            retry
        ));
    }
    
    public void Post(Uri url, HttpContent request, Func<HttpResponseMessage,bool> responseValidator, bool retry = false) =>
        Post(url, request, response => Task.FromResult(responseValidator(response)), retry);

    public void PostAssertions(Uri url, HttpContent request, Func<HttpResponseMessage, Task<Action[]>> responseValidator, bool retry = false) =>
        Post(url, request, async response =>
        {
            var assertions = await responseValidator(response);
            foreach (var assertion in assertions)
            {
                try
                {
                    assertion();
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }, retry);

    
    public void Post(Uri url, HttpContent request, Func<HttpResponseMessage,Task<bool>> responseValidator, bool retry = false)
    {
        AddStep(new XceptoPostSSRState("PostSSRState", 
            request,
            url,
            responseValidator,
            retry
        ));
    }
}