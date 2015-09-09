using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using SignalR.Tests.Common;
using Xunit;

namespace SignalR.FunctionalTests
{
    public class HubProxyFacts 
    {
        [Fact]
        public async Task EndToEndTest()
        {
            using (var host = HostedTestFactory.CreateHost())
            using (var hubConnection = new HubConnection(host.Url, true))
            {
                var proxy = hubConnection.CreateHubProxy("ChatHub");
                proxy.On("addMessage", data => { Assert.Equal("hello", data); });

                await hubConnection.Start(host.Transport);

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