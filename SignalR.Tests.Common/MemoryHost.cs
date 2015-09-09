using System;
using System.Net.Http;
using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.Owin.Testing;

namespace SignalR.Tests.Common
{
    public class MemoryHost : DefaultHttpClient
    {
        private readonly TestServer _host;
     
        public MemoryHost(TestServer host)
        {
            _host = host;
            Initialize(null);
        }

        protected override HttpMessageHandler CreateHandler()
        {
            return new MemoryHostHttpHandler(_host.Handler);
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