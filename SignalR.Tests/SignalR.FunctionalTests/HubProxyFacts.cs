using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Tests.Common;
using Microsoft.AspNet.SignalR.Tests.Common.Infrastructure;
using Xunit;

namespace Microsoft.AspNet.SignalR.Tests
{
    public class HubProxyFacts : HostedTest
    {
        [Theory]
        [InlineData(TransportType.ServerSentEvents)]
        public async Task EndToEndTest(TransportType transportType)
        {
            using (var host = CreateHost(transportType))
            {
                host.Initialize();

                var hubConnection = CreateHubConnection(host);
                var proxy = hubConnection.CreateHubProxy("ChatHub");

                using (hubConnection)
                {
                    var wh = new ManualResetEvent(false);

                    proxy.On("addMessage", data =>
                    {
                        Assert.Equal("hello", data);
                        wh.Set();
                    });

                    await hubConnection.Start(host.Transport);

                    proxy.InvokeWithTimeout("Send", "hello");

                    Assert.True(wh.WaitOne(TimeSpan.FromSeconds(10)));
                }
            }
        }


        public class ChatHub : Hub
        {
            public Task Send(string message)
            {
                return Clients.All.addMessage(message);
            }
        }
    }
}