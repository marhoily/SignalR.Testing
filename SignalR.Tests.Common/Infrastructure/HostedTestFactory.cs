// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.md in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.AspNet.SignalR.Client.Transports;

namespace SignalR.Tests.Common
{
    public static class HostedTestFactory
    {
        public static ITestHost CreateHost(TransportType transportType, string testName, string url = null)
        {
            var logBasePath = Path.Combine(Directory.GetCurrentDirectory(), "..");
            TraceListener traceListener = EnableTracing(testName, logBasePath);

            var mh = new MemoryHost();
            var host = new MemoryTestHost(mh, Path.Combine(logBasePath, testName));
            host.TransportFactory = () => CreateTransport(transportType, mh);
            host.Transport = host.TransportFactory();

            //    var writer = CreateClientTraceWriter(testName);
            //  host.ClientTraceOutput = writer;

            host.Disposables.Add(new DisposableAction(() =>
            {
                traceListener.Close();
                Trace.Listeners.Remove(traceListener);
            }));

            EventHandler<UnobservedTaskExceptionEventArgs> handler = (sender, args) =>
            {
                Trace.TraceError("Unobserved task exception: " + args.Exception.GetBaseException());

                args.SetObserved();
            };

            TaskScheduler.UnobservedTaskException += handler;
            host.Disposables.Add(new DisposableAction(() => { TaskScheduler.UnobservedTaskException -= handler; }));

            return host;
        }

        public static IClientTransport CreateTransport(TransportType transportType, IHttpClient client)
        {
            switch (transportType)
            {
                case TransportType.Websockets:
                    return new WebSocketTransport(client);
                case TransportType.ServerSentEvents:
                    return new ServerSentEventsTransport(client);
                case TransportType.ForeverFrame:
                    break;
                case TransportType.LongPolling:
                    return new LongPollingTransport(client);
                default:
                    return new AutoTransport(client);
            }

            throw new NotSupportedException("Transport not supported");
        }

        public static TextWriterTraceListener EnableTracing(string testName, string logBasePath)
        {
            var testTracePath = Path.Combine(logBasePath, testName + ".test.trace.log");
            var traceListener = new TextWriterTraceListener(testTracePath);
            Trace.Listeners.Add(traceListener);
            Trace.AutoFlush = true;
            return traceListener;
        }
    }
}