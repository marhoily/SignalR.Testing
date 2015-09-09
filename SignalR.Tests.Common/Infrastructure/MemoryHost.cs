using System;
using System.Net.Http;
using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.Owin.Testing;
using Owin;

namespace Microsoft.AspNet.SignalR.Hosting.Memory
{
    public class MemoryHost : DefaultHttpClient, IDisposable
    {
        private TestServer _host;

        public void Dispose()
        {
            _host.Dispose();
        }

        public void Configure(Action<IAppBuilder> startup)
        {
            _host = TestServer.Create(startup);
            Initialize(null);
        }

        protected override HttpMessageHandler CreateHandler()
        {
            return new MemoryHostHttpHandler(_host.Handler);
        }
    }
}