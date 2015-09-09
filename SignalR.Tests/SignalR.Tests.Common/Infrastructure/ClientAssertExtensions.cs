using System;
using Microsoft.AspNet.SignalR.Client;
using Xunit;

namespace Microsoft.AspNet.SignalR.Tests.Common
{
    public static class ClientAssertExtensions
    {
        private static readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(10);

        public static void InvokeWithTimeout(this IHubProxy proxy, string method, params object[] args)
        {
            InvokeWithTimeout(proxy, _defaultTimeout, method, args);
        }

        public static void InvokeWithTimeout(this IHubProxy proxy, TimeSpan timeout, string method, params object[] args)
        {
            var task = proxy.Invoke(method, args);

            Assert.True(task.Wait(timeout), "Failed to get response from " + method);
        }
    }
}