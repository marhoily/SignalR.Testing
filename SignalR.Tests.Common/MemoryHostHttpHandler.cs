using System.Net.Http;

namespace SignalR.Tests.Common
{
    public class MemoryHostHttpHandler : DelegatingHandler
    {
        public MemoryHostHttpHandler(HttpMessageHandler handler)
        {
            InnerHandler = handler;
        }
    }
}