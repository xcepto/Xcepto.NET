using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Xcepto.Adapters;
using Xcepto.Interfaces;
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
        
        public RestStateStateBuilder Get(PathString pathString)
        {
            return new RestStateStateBuilder(Builder, RestHttpMethod.Get, _client, pathString)
                .InjectBaseUrl(_baseUrl)
                .InjectSerializer(_serializer);
        }
        
        public RestStateStateBuilder Post(PathString pathString)
        {
            return new RestStateStateBuilder(Builder, RestHttpMethod.Post, _client, pathString)
                .InjectBaseUrl(_baseUrl)
                .InjectSerializer(_serializer);
        }
        
        public RestStateStateBuilder Patch(PathString pathString)
        {
            return new RestStateStateBuilder(Builder, RestHttpMethod.Patch, _client, pathString)
                .InjectBaseUrl(_baseUrl)
                .InjectSerializer(_serializer);
        }
        
        public RestStateStateBuilder Put(PathString pathString)
        {
            return new RestStateStateBuilder(Builder, RestHttpMethod.Put, _client, pathString)
                .InjectBaseUrl(_baseUrl)
                .InjectSerializer(_serializer);
        }
        
        public RestStateStateBuilder Delete(PathString pathString)
        {
            return new RestStateStateBuilder(Builder, RestHttpMethod.Delete, _client, pathString)
                .InjectBaseUrl(_baseUrl)
                .InjectSerializer(_serializer);
        }
    }
}