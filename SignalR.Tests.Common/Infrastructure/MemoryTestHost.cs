using System;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client.Transports;
using Microsoft.AspNet.SignalR.Messaging;
using Owin;

namespace SignalR.Tests.Common
{
    public class MemoryTestHost : ITestHost
    {
        private readonly MemoryHost _host;

        public MemoryTestHost(MemoryHost host)
        {
            Disposables = new List<IDisposable>();
            _host = host;

            var resolver = new DefaultDependencyResolver();

            var bus = new FakeScaleoutBus(resolver);
            resolver.Register(typeof (IMessageBus), () => bus);

            _host.Configure(app =>
            {
                app.MapSignalR(new HubConfiguration
                {
                    EnableDetailedErrors = true
                });
            });
        }

        public IList<IDisposable> Disposables { get; private set; }
        public IClientTransport Transport { get; set; }

        public string Url
        {
            get { return "http://memoryhost"; }
        }

        public void Dispose()
        {
            _host.Dispose();

            foreach (var d in Disposables)
            {
                d.Dispose();
            }
        }
    }
}