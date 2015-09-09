using System;
using Microsoft.AspNet.SignalR.Client.Transports;

namespace SignalR.Tests.Common
{
    public interface ITestHost : IDisposable
    {
        string Url { get; }
        IClientTransport Transport { get; }
    }
}