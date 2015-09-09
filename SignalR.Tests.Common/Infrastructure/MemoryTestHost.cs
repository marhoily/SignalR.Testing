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

        public MemoryTestHost()
        {
            _host = new MemoryHost();

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
            Transport = new AutoTransport(_host);
        }

        public IClientTransport Transport { get; set; }

        public string Url
        {
            get { return "http://memoryhost"; }
        }

        public void Dispose()
        {
            _host.Dispose();

        }
    }
}