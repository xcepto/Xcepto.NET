using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        
        protected Func<HttpClient> ClientProducer = () => new();
        protected Func<Uri> BaseUrl = () => new("http://localhost:8080");
        protected HttpMethodVerb MethodVerb = HttpMethodVerb.Get;
        protected Func<PathString> PathString = () => "/";
        protected readonly List<Func<KeyValuePair<string, string>>> QueryArgs = new();
        protected readonly List<HttpResponseAssertion> ResponseAssertions = new();

        protected HttpStateBuilderIdentity(IStateMachineBuilder stateMachineBuilder, IStateBuilderIdentity stateBuilderIdentity) : base(stateMachineBuilder, stateBuilderIdentity) { }
        protected HttpStateBuilderIdentity(IStateMachineBuilder stateMachineBuilder) : base(stateMachineBuilder) { }

        protected override string DefaultName
        {
            get
            {
                string url;
                try
                {
                    url = Url().ToString();
                }
                catch (Exception)
                {
                    url = "promised url";
                }
                return $"Http {MethodVerb} request to {url}";
            }
        }

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


        protected Func<Uri> Url
        {
            get
            {
                if (BaseUrl is null)
                    throw new BuilderException("no Url defined");
                return () =>
                {
                    if (!Uri.TryCreate(BaseUrl(), PathString() + QueryString.Create(QueryArgs.Select(x=> x())), out var uri))
                        throw new ArgumentException("Url creation failed");
                    return uri;
                };
            }
        } 
        
        public TBuilder WithCustomClient(HttpClient client)
        {
            ClientProducer = () => client;
            return (TBuilder)this;
        }
        
        public TBuilder WithCustomClient(Func<HttpClient> clientProducer)
        {
            ClientProducer = clientProducer;
            return (TBuilder)this;
        }

        public TBuilder WithCustomBaseUrl(Uri uri)
        {
            BaseUrl = () => uri;
            return (TBuilder)this;
        }
        
        public TBuilder WithCustomBaseUrl(Func<Uri> uri)
        {
            BaseUrl = uri;
            return (TBuilder)this;
        }
    
        public TBuilder AddQueryArgument(string key, string value)
        {
            QueryArgs.Add(() => new KeyValuePair<string, string>(key, value));
            return (TBuilder)this;
        }
        
        public TBuilder AddQueryArgument(Func<KeyValuePair<string, string>> argument)
        {
            QueryArgs.Add(argument);
            return (TBuilder)this;
        }
        
        public TBuilder WithHttpVerb(HttpMethodVerb verb)
        {
            MethodVerb = verb;
            return (TBuilder)this;
        }
        
        public TBuilder WithPathString(PathString pathString)
        {
            PathString = () => pathString;
            return (TBuilder)this;
        }
        
        public TBuilder WithPathString(Func<PathString> pathStringPromise)
        {
            PathString = pathStringPromise;
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
        
        public TBuilder AssertSuccess()
        {
            return AssertThatResponse(x => x.StatusCode, Is.GreaterThanOrEqualTo(HttpStatusCode.OK).And.LessThan(300));
        }
        
        public TBuilder AssertClientFailure()
        {
            return AssertThatResponse(x => x.StatusCode, Is.GreaterThanOrEqualTo(HttpStatusCode.BadRequest).And.LessThan(500));
        }
        
        public TBuilder AssertServerFailure()
        {
            return AssertThatResponse(x => x.StatusCode, Is.GreaterThanOrEqualTo(HttpStatusCode.InternalServerError).And.LessThan(600));
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