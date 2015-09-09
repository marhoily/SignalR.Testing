using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SignalR.Tests.Common
{
    public class MemoryHostHttpHandler : DelegatingHandler
    {
        public MemoryHostHttpHandler(HttpMessageHandler handler)
        {
            InnerHandler = handler;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            try
            {
                return await base.SendAsync(request, cancellationToken);
            }
            catch (ObjectDisposedException)
            {
                return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            }
        }
    }
}