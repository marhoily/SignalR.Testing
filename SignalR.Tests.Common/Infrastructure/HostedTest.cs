using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.AspNet.SignalR.Client.Transports;
using Microsoft.AspNet.SignalR.Messaging;
using Xunit;

namespace SignalR.Tests.Common
{
    public static class HostedTest 
    {
        private static long _id;

        public static ITestHost CreateHost()
        {
            return CreateHost(TransportType.Auto);
        }

        public static ITestHost CreateHost(TransportType transportType)
        {
            var detailedTestName = GetTestName() + "." + transportType + "." + Interlocked.Increment(ref _id);

            var testHost = HostedTestFactory.CreateHost(transportType, detailedTestName);
            testHost.Initialize();

            return testHost;
        }

        public static void UseMessageBus(IDependencyResolver resolver)
        {
            IMessageBus bus = new FakeScaleoutBus(resolver);
            resolver.Register(typeof (IMessageBus), () => bus);
        }

        public static void SetReconnectDelay(IClientTransport transport, TimeSpan delay)
        {
            // SUPER ugly, alternative is adding an overload to the create host function, adding a member to the
            // IClientTransport object or using Reflection.  Adding a member to IClientTransport isn't horrible
            // but we want to avoid making a breaking change... Therefore this is the least of the evils.
            if (transport is ServerSentEventsTransport)
            {
                (transport as ServerSentEventsTransport).ReconnectDelay = delay;
            }
            else if (transport is LongPollingTransport)
            {
                (transport as LongPollingTransport).ReconnectDelay = delay;
            }
            else if (transport is WebSocketTransport)
            {
                (transport as WebSocketTransport).ReconnectDelay = delay;
            }
        }

        public static TextWriterTraceListener EnableTracing()
        {
            var testName = GetTestName() + "." + Interlocked.Increment(ref _id);
            var logBasePath = Path.Combine(Directory.GetCurrentDirectory(), "..");
            return HostedTestFactory.EnableTracing(testName, logBasePath);
        }

        public static HubConnection CreateHubConnection(string url)
        {
            var testName = GetTestName() + "." + Interlocked.Increment(ref _id);
            var query = new Dictionary<string, string>();
            query["test"] = testName;
            var connection = new HubConnection(url, query);
            connection.TraceWriter = HostedTestFactory.CreateClientTraceWriter(testName);
            return connection;
        }

        public static Connection CreateConnection(string url)
        {
            var testName = GetTestName() + "." + Interlocked.Increment(ref _id);
            var query = new Dictionary<string, string>();
            query["test"] = testName;
            var connection = new Connection(url, query);
            connection.TraceWriter = HostedTestFactory.CreateClientTraceWriter(testName);
            return connection;
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

        public static HubConnection CreateAuthHubConnection(ITestHost host, string user, string password)
        {
            var path = "/cookieauth/signalr";
            var useDefaultUrl = false;
            var query = new Dictionary<string, string>();
            query["test"] = GetTestName() + "." + Interlocked.Increment(ref _id);
            SetHostData(host, query);

            var handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            using (var httpClient = new HttpClient(handler))
            {
                var content = string.Format("UserName={0}&Password={1}", user, password);
                var response =
                    httpClient.PostAsync(host.Url + "/cookieauth/Account/Login",
                        new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded")).Result;
            }

            var connection = new HubConnection(host.Url + path, query, useDefaultUrl);
            connection.TraceWriter = host.ClientTraceOutput ?? connection.TraceWriter;
            connection.CookieContainer = handler.CookieContainer;
            return connection;
        }

        public static Connection CreateConnection(ITestHost host, string path)
        {
            var query = new Dictionary<string, string>();
            query["test"] = GetTestName() + "." + Interlocked.Increment(ref _id);
            SetHostData(host, query);
            var connection = new Connection(host.Url + path, query);
            connection.TraceWriter = host.ClientTraceOutput ?? connection.TraceWriter;
            return connection;
        }

        public static Connection CreateAuthConnection(ITestHost host, string path, string user, string password)
        {
            var query = new Dictionary<string, string>();
            query["test"] = GetTestName() + "." + Interlocked.Increment(ref _id);
            SetHostData(host, query);

            var handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            using (var httpClient = new HttpClient(handler))
            {
                var content = string.Format("UserName={0}&Password={1}", user, password);
                var response =
                    httpClient.PostAsync(host.Url + "/cookieauth/Account/Login",
                        new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded")).Result;
            }

            var connection = new Connection(host.Url + "/cookieauth" + path, query);
            connection.TraceWriter = host.ClientTraceOutput ?? connection.TraceWriter;
            connection.CookieContainer = handler.CookieContainer;

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

        public static IClientTransport CreateTransport(TransportType transportType)
        {
            return CreateTransport(transportType, new DefaultHttpClient());
        }

        public static IClientTransport CreateTransport(TransportType transportType, IHttpClient client)
        {
            return HostedTestFactory.CreateTransport(transportType, client);
        }

    }
}