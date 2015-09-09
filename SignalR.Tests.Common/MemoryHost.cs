using System;
using System.Net.Http;
using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.Owin.Testing;

namespace SignalR.Tests.Common
{
    public class MemoryHost : DefaultHttpClient, IDisposable
    {
        private TestServer _host;

        public void Dispose()
        {
            _host.Dispose();
        }

        public void Configure(TestServer testServer)
        {
            _host = testServer;
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