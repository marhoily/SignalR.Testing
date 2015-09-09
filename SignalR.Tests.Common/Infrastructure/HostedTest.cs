using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.AspNet.SignalR.Client;
using Xunit;

namespace SignalR.Tests.Common
{
    public static class HostedTest 
    {
        private static long _id;

        public static ITestHost CreateHost(TransportType transportType = TransportType.Auto)
        {
            var detailedTestName = GetTestName() + "." + transportType + "." + Interlocked.Increment(ref _id);

            var testHost = HostedTestFactory.CreateHost(transportType, detailedTestName);
            testHost.Initialize();

            return testHost;
        }

        public static HubConnection CreateHubConnection(ITestHost host, string path = null, bool useDefaultUrl = true)
        {
            var query = new Dictionary<string, string>();
            query["test"] = GetTestName() + "." + Interlocked.Increment(ref _id);
            SetHostData(host, query);
            var connection = new HubConnection(host.Url + path, query, useDefaultUrl);
            connection.TraceWriter = host.ClientTraceOutput ?? connection.TraceWriter;
            return connection;
        }


        public static string GetTestName()
        {
            var stackTrace = new StackTrace();
            return (from f in stackTrace.GetFrames()
                select f.GetMethod()
                into m
                let anyFactsAttributes = m.GetCustomAttributes(typeof (FactAttribute), true).Length > 0
                let anyTheories = m.GetCustomAttributes(typeof (TheoryAttribute), true).Length > 0
                where anyFactsAttributes || anyTheories
                select GetName(m)).First();
        }

        public static void SetHostData(ITestHost host, Dictionary<string, string> query)
        {
            foreach (var item in host.ExtraData)
            {
                query[item.Key] = item.Value;
            }
        }

        private static string GetName(MethodBase m)
        {
            return m.DeclaringType.FullName.Substring(m.DeclaringType.Namespace.Length).TrimStart('.', '+') + "." +
                   m.Name;
        }
    }
}