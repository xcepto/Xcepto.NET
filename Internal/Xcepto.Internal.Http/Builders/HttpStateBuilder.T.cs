using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Xcepto.Builder;
using Xcepto.Exceptions;
using Xcepto.Interfaces;
using Xcepto.Internal.Http.Data;

namespace Xcepto.Internal.Http.Builders
{
    public abstract class HttpStateBuilderIdentity<TBuilder> : AbstractStateBuilderIdentity<TBuilder>
    where TBuilder: HttpStateBuilderIdentity<TBuilder>
    {
        
        protected HttpClient Client = new();
        protected Uri BaseUrl = new("http://localhost:8080");
        protected HttpMethodVerb MethodVerb = HttpMethodVerb.Get;
        protected PathString PathString = "/";
        protected readonly List<KeyValuePair<string, string>> QueryArgs = new();
        protected readonly List<HttpResponseAssertion> ResponseAssertions = new();

        protected HttpStateBuilderIdentity(IStateMachineBuilder stateMachineBuilder, IStateBuilderIdentity stateBuilderIdentity) : base(stateMachineBuilder, stateBuilderIdentity) { }
        protected HttpStateBuilderIdentity(IStateMachineBuilder stateMachineBuilder) : base(stateMachineBuilder) { }
        
        /// <summary>
        /// Based on Http Verb idempotency
        /// </summary>
        protected override bool DefaultRetry
        {
            get
            {
                return MethodVerb switch
                {
                    HttpMethodVerb.Get => true,
                    HttpMethodVerb.Post => false,
                    HttpMethodVerb.Patch => false,
                    HttpMethodVerb.Put => true,
                    HttpMethodVerb.Delete => true,
                    _ => false
                };
            }
        }


        protected Uri Url
        {
            get
            {
                if (BaseUrl is null)
                    throw new BuilderException("no Url defined");
                if (!Uri.TryCreate(BaseUrl, PathString + QueryString.Create(QueryArgs), out var uri))
                    throw new ArgumentException("Url creation failed");
                return uri;
            }
        } 
        
        public TBuilder WithCustomClient(HttpClient client)
        {
            Client = client;
            return (TBuilder)this;
        }

        public TBuilder WithCustomBaseUrl(Uri uri)
        {
            BaseUrl = uri;
            return (TBuilder)this;
        }
    
        public TBuilder AddQueryArgument(string key, string value)
        {
            QueryArgs.Add(new KeyValuePair<string, string>(key, value));
            return (TBuilder)this;
        }
        
        public TBuilder WithHttpVerb(HttpMethodVerb verb)
        {
            MethodVerb = verb;
            return (TBuilder)this;
        }
        
        public TBuilder WithPathString(PathString pathString)
        {
            PathString = pathString;
            return (TBuilder)this;
        }
        
        public TBuilder AssertThatResponse(Func<HttpResponseMessage, Task<object>> actual, IResolveConstraint constraint)
        {
            ResponseAssertions.Add(new HttpResponseAssertion(actual, constraint));
            return (TBuilder)this;
        }
        public TBuilder AssertThatResponse(Func<HttpResponseMessage, object> actual, IResolveConstraint constraint)
        {
            return AssertThatResponse(x => Task.FromResult(actual(x)), constraint);
        }
    
        public TBuilder AssertThatResponseStatus(IResolveConstraint constraint)
        {
            return AssertThatResponse(x => x.StatusCode, constraint);
        }
    
        public TBuilder AssertThatResponseContentString(IResolveConstraint constraint)
        {
            var responseAssertion = new HttpResponseAssertion(
                async x => await x.Content.ReadAsStringAsync(), 
                constraint
            );
            ResponseAssertions.Add(responseAssertion);
            return (TBuilder)this;
        }

    }
}