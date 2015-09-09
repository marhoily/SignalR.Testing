// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.md in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client.Transports;

namespace SignalR.Tests.Common
{
    public static class HostedTestFactory
    {
        public static ITestHost CreateHost()
        {
            var mh = new MemoryHost();
            var host = new MemoryTestHost(mh);
            host.TransportFactory = () => (IClientTransport) new AutoTransport(mh);
            host.Transport = host.TransportFactory();

         

            EventHandler<UnobservedTaskExceptionEventArgs> handler = (sender, args) =>
            {
                Trace.TraceError("Unobserved task exception: " + args.Exception.GetBaseException());

                args.SetObserved();
            };

            TaskScheduler.UnobservedTaskException += handler;
            host.Disposables.Add(new DisposableAction(() => { TaskScheduler.UnobservedTaskException -= handler; }));

            return host;
        }

    }
}