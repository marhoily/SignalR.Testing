using Microsoft.AspNet.SignalR;
using Owin;

namespace SignalR.Tests.Common
{
    public class MemoryTestHost : TracingTestHost
    {
        private readonly MemoryHost _host;

        public MemoryTestHost(MemoryHost host)
            : base("")
        {
            _host = host;
        }

        public override string Url
        {
            get { return "http://memoryhost"; }
        }

        public override void Initialize(int? keepAlive = -1,
            int? connectionTimeout = 110,
            int? disconnectTimeout = 30,
            int? transportConnectTimeout = 5,
            int? maxIncomingWebSocketMessageSize = 64*1024,
            bool enableAutoRejoiningGroups = false)
        {
            base.Initialize(keepAlive,
                connectionTimeout,
                disconnectTimeout,
                transportConnectTimeout,
                maxIncomingWebSocketMessageSize,
                enableAutoRejoiningGroups);

            _host.Configure(app =>
            {
                app.MapSignalR(new HubConfiguration
                {
                    EnableDetailedErrors = true
                });
            });
        }

        public override void Dispose()
        {
            _host.Dispose();

            base.Dispose();
        }
    }
}