using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Transports;
using Microsoft.Owin.Testing;
using Owin;
using SignalR.Tests.Common;
using Xunit;

namespace SignalR.FunctionalTests
{
    public class HubProxyFacts
    {
        [Fact]
        public async Task EndToEndTest()
        {
            using (var host = TestServer.Create(app => { app.MapSignalR(new HubConfiguration()); }))
            using (var hubConnection = new HubConnection("http://any.valid.url"))
            {
                var proxy = hubConnection.CreateHubProxy("ChatHub");
                proxy.On("addMessage", data =>
                {
                    Assert.Equal("hello", data);
                });

                await hubConnection.Start(
                    new AutoTransport(new MemoryHost(host.Handler)));

                await proxy.Invoke("Send", "hello");
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