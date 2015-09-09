using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Messaging;

namespace SignalR.Tests.Common
{
    public class FakeScaleoutBus : ScaleoutMessageBus
    {
        private ulong _id;
        private readonly int _streams;

        public FakeScaleoutBus(IDependencyResolver resolver)
            : this(resolver, 1)
        {
        }

        public FakeScaleoutBus(IDependencyResolver dr, int streams)
            : base(dr, new ScaleoutConfiguration())
        {
            _streams = streams;

            for (var i = 0; i < _streams; i++)
            {
                Open(i);
            }
        }

        protected override int StreamCount
        {
            get { return _streams; }
        }

        protected override Task Send(int streamIndex, IList<Message> messages)
        {
            var message = new ScaleoutMessage(messages);

            OnReceived(streamIndex, _id, message);

            _id++;

            return TaskAsyncHelper.Empty;
        }
    }
}