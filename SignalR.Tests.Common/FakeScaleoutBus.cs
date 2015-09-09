using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Messaging;

namespace SignalR.Tests.Common
{
    public class FakeScaleoutBus : ScaleoutMessageBus
    {
        public FakeScaleoutBus(IDependencyResolver dr)
            : base(dr, new ScaleoutConfiguration())
        {
          
        }
    }
}