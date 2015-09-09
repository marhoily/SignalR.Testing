using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNet.SignalR.Client.Transports;

namespace SignalR.Tests.Common
{
    public interface ITestHost : IDisposable
    {
        string Url { get; }

        IClientTransport Transport { get; set; }

        TextWriter ClientTraceOutput { get; set; }

        IDictionary<string, string> ExtraData { get; }

        void Initialize(int? keepAlive = -1,
            int? connectionTimeout = 110,
            int? disconnectTimeout = 30,
            int? transportConnectTimeout = 5,
            int? maxIncomingWebSocketMessageSize = 64*1024, // Default 64 KB
            bool enableAutoRejoiningGroups = false);

    }
}