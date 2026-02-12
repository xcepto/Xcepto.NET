using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Xcepto.Adapters;
using Xcepto.Interfaces;
using Xcepto.Internal.Http.Data;
using Xcepto.Rest.Builders;
using Xcepto.Rest.Data;
using Xcepto.States;

namespace Xcepto.Rest
{
    public class XceptoRestAdapter: XceptoAdapter
    {
        private HttpClient _client;
        private Uri? _baseUrl;
        private ISerializer? _serializer;

        internal XceptoRestAdapter(HttpClient client, Uri? baseUrl, ISerializer? serializer)
        {
            _serializer = serializer;
            _baseUrl = baseUrl;
            _client = client;
        }

        private RestStateBuilder Inject(RestStateBuilder builder, HttpMethodVerb verb, PathString pathString)
        {
            if(_baseUrl is not null)
                builder.WithCustomBaseUrl(_baseUrl);
            if(_serializer is not null)
                builder.WithSerializer(_serializer);
            builder.WithCustomClient(_client);
            builder.WithHttpVerb(verb);
            builder.WithPathString(pathString);
            return builder;
        }
        
        public RestStateBuilder Get(PathString pathString)
        {
            return Inject(new RestStateBuilder(Builder), HttpMethodVerb.Get, pathString);
        }
        
        public RestStateBuilder Post(PathString pathString)
        {
            return Inject(new RestStateBuilder(Builder), HttpMethodVerb.Post, pathString);
        }
        
        public RestStateBuilder Patch(PathString pathString)
        {
            return Inject(new RestStateBuilder(Builder), HttpMethodVerb.Patch, pathString);
        }
        
        public RestStateBuilder Put(PathString pathString)
        {
            return Inject(new RestStateBuilder(Builder), HttpMethodVerb.Put, pathString);
        }
        
        public RestStateBuilder Delete(PathString pathString)
        {
            return Inject(new RestStateBuilder(Builder), HttpMethodVerb.Delete, pathString);
        }
    }
}