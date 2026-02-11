using System;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework.Constraints;

namespace Xcepto.Internal.Http.Data
{
    public class HttpResponseAssertion
    {
        public Func<HttpResponseMessage, Task<object>> Selector { get; }
        public IResolveConstraint ResolveConstraint { get; }

        public HttpResponseAssertion(Func<HttpResponseMessage, Task<object>> selector, IResolveConstraint resolveConstraint)
        {
            Selector = selector;
            ResolveConstraint = resolveConstraint;
        }
    }
}