using Microsoft.AspNet.SignalR.Client;

namespace SignalR.Tests.Common
{
    public static class HostedTest 
    {
        public static ITestHost CreateHost()
        {
            var testHost = HostedTestFactory.CreateHost();
            testHost.Initialize();
            return testHost;
        }

        public static HubConnection CreateHubConnection(ITestHost host, string path = null, bool useDefaultUrl = true)
        {
            var connection = new HubConnection(host.Url + path, useDefaultUrl);
            connection.TraceWriter = host.ClientTraceOutput ?? connection.TraceWriter;
            return connection;
        }
    }
}