using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client.Transports;
using Microsoft.Owin.Testing;
using Owin;

namespace SignalR.Tests.Common
{
    public class MemoryTestHost : IDisposable
    {
        private readonly MemoryHost _host;

        public MemoryTestHost()
        {
            _host = new MemoryHost();

            Action<IAppBuilder> startup = app =>
            {
                app.MapSignalR(new HubConfiguration());
            };
            _host.Configure(TestServer.Create(startup));
            _host.Initialize(null);
            Transport = new AutoTransport(_host);
        }

        public IClientTransport Transport { get; private set; }

        public void Dispose()
        {
            _host.Dispose();
        }
    }
}