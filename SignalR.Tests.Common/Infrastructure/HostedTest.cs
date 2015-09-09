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
    }
}