using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client.Transports;
using Microsoft.Owin.Testing;
using Owin;

namespace SignalR.Tests.Common
{
    public class MemoryTestHost : IDisposable
    {
        private readonly TestServer _host;

        public MemoryTestHost()
        {
            _host = TestServer.Create(
                app => { app.MapSignalR(new HubConfiguration()); });
            var host = new MemoryHost(_host);
            Transport = new AutoTransport(host);
        }

        public IClientTransport Transport { get; private set; }

        public void Dispose()
        {
            _host.Dispose();
        }
    }
}