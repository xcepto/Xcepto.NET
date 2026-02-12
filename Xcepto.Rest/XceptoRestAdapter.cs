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

        private RestStateBuilderIdentity Inject(RestStateBuilderIdentity builderIdentity, HttpMethodVerb verb, PathString pathString)
        {
            if(_baseUrl is not null)
                builderIdentity.WithCustomBaseUrl(_baseUrl);
            if(_serializer is not null)
                builderIdentity.WithSerializer(_serializer);
            builderIdentity.WithCustomClient(_client);
            builderIdentity.WithHttpVerb(verb);
            builderIdentity.WithPathString(pathString);
            return builderIdentity;
        }
        
        public RestStateBuilderIdentity Get(PathString pathString)
        {
            return Inject(new RestStateBuilderIdentity(Builder), HttpMethodVerb.Get, pathString);
        }
        
        public RestStateBuilderIdentity Post(PathString pathString)
        {
            return Inject(new RestStateBuilderIdentity(Builder), HttpMethodVerb.Post, pathString);
        }
        
        public RestStateBuilderIdentity Patch(PathString pathString)
        {
            return Inject(new RestStateBuilderIdentity(Builder), HttpMethodVerb.Patch, pathString);
        }
        
        public RestStateBuilderIdentity Put(PathString pathString)
        {
            return Inject(new RestStateBuilderIdentity(Builder), HttpMethodVerb.Put, pathString);
        }
        
        public RestStateBuilderIdentity Delete(PathString pathString)
        {
            return Inject(new RestStateBuilderIdentity(Builder), HttpMethodVerb.Delete, pathString);
        }
    }
}