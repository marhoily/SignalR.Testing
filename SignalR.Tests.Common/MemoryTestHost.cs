using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client.Transports;
using Owin;

namespace SignalR.Tests.Common
{
    public class MemoryTestHost : IDisposable
    {
        private readonly MemoryHost _host;

        public MemoryTestHost()
        {
            _host = new MemoryHost();

            _host.Configure(app =>
            {
                app.MapSignalR(new HubConfiguration());
            });
            Transport = new AutoTransport(_host);
        }

        public IClientTransport Transport { get; private set; }

        public void Dispose()
        {
            _host.Dispose();
        }
    }
}