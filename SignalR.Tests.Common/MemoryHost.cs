using System;
using System.Net.Http;
using Microsoft.AspNet.SignalR.Client.Http;

namespace SignalR.Tests.Common
{
    public class MemoryHost : DefaultHttpClient
    {
        private readonly HttpMessageHandler _httpMessageHandler;

        public MemoryHost(HttpMessageHandler httpMessageHandler)
        {
            _httpMessageHandler = httpMessageHandler;
            Initialize(null);
        }

        protected override HttpMessageHandler CreateHandler()
        {
            return new MemoryHostHttpHandler(_httpMessageHandler);
        }

        private class MemoryHostHttpHandler : DelegatingHandler
        {
            public MemoryHostHttpHandler(HttpMessageHandler handler)
            {
                InnerHandler = handler;
            }
        }
    }
}