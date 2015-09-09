// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.md in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client.Transports;

namespace SignalR.Tests.Common
{
    public static class HostedTestFactory
    {
        public static ITestHost CreateHost()
        {
            var memoryHost = new MemoryHost();

            return new MemoryTestHost(memoryHost)
            {
                Transport = new AutoTransport(memoryHost)
            };
        }

    }
}