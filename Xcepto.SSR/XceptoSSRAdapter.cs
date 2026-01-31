using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xcepto.Adapters;

namespace Xcepto.SSR;

public class XceptoSSRAdapter: XceptoAdapter
{
    private HttpClient _client;

    public XceptoSSRAdapter(HttpClient? httpClient = null)
    {
        if (httpClient is not null)
            _client = httpClient;
        else
        {
            var cookieContainer = new CookieContainer();

            var handler = new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                UseCookies = true
            };

            _client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(1)
            };
        }
    }
    public void Get(Uri url, Func<HttpResponseMessage, bool> responseValidator, bool retry = true) =>
        Get(url, response => Task.FromResult(responseValidator(response)), retry);

    public void Get(Uri url, Func<HttpResponseMessage,Task<bool>> responseValidator, bool retry = true)
    {
        AddStep(new XceptoGetSSRState("GetSSRState", 
            url,
            responseValidator,
            retry,
            _client
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
            retry,
            _client
        ));
    }
}