using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client.Transports;
using Microsoft.AspNet.SignalR.Configuration;
using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.AspNet.SignalR.Tracing;
using Owin;

namespace SignalR.Tests.Common
{
    public class MemoryTestHost : ITestHost
    {
        private static readonly string[] TraceSources =
        {
            "SignalR.Transports.WebSocketTransport",
            "SignalR.Transports.ServerSentEventsTransport",
            "SignalR.Transports.ForeverFrameTransport",
            "SignalR.Transports.LongPollingTransport",
            "SignalR.Transports.TransportHeartBeat"
        };

        private readonly MemoryHost _host;
        private readonly TextWriterTraceListener _listener;
        private ITraceManager _traceManager;

        public MemoryTestHost(MemoryHost host)

        {
            _listener = new TextWriterTraceListener(".transports.log");
            Disposables = new List<IDisposable>();
            _host = host;
        }

        public IList<IDisposable> Disposables { get; private set; }
        public Func<IClientTransport> TransportFactory { get; set; }
        private IDependencyResolver Resolver { get; set; }
        public IClientTransport Transport { get; set; }

        public string Url
        {
            get { return "http://memoryhost"; }
        }

        public void Initialize(int? keepAlive = -1,
            int? connectionTimeout = 110,
            int? disconnectTimeout = 30,
            int? transportConnectTimeout = 5,
            int? maxIncomingWebSocketMessageSize = 64*1024,
            bool enableAutoRejoiningGroups = false)
        {
            Resolver = Resolver ?? new DefaultDependencyResolver();

            _traceManager = Resolver.Resolve<ITraceManager>();
            _traceManager.Switch.Level = SourceLevels.Verbose;

            foreach (var sourceName in TraceSources)
            {
                var source = _traceManager[sourceName];
                source.Listeners.Add(_listener);
            }

            var configuration = Resolver.Resolve<IConfigurationManager>();

            if (connectionTimeout != null)
            {
                configuration.ConnectionTimeout = TimeSpan.FromSeconds(connectionTimeout.Value);
            }

            if (disconnectTimeout != null)
            {
                configuration.DisconnectTimeout = TimeSpan.FromSeconds(disconnectTimeout.Value);
            }

            if (transportConnectTimeout != null)
            {
                configuration.TransportConnectTimeout = TimeSpan.FromSeconds(transportConnectTimeout.Value);
            }

            configuration.MaxIncomingWebSocketMessageSize = maxIncomingWebSocketMessageSize;

            var bus = new FakeScaleoutBus(Resolver);
            Resolver.Register(typeof (IMessageBus), () => bus);

            _host.Configure(app =>
            {
                app.MapSignalR(new HubConfiguration
                {
                    EnableDetailedErrors = true
                });
            });
        }

        public void Dispose()
        {
            _host.Dispose();

            _listener.Flush();

            foreach (var sourceName in TraceSources)
            {
                _traceManager[sourceName].Listeners.Remove(_listener);
            }

            _listener.Dispose();

            foreach (var d in Disposables)
            {
                d.Dispose();
            }
        }
    }
}