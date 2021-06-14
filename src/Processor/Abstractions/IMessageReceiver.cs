using System.Collections.Generic;
using System.Threading;

using ZeroMQPubSubSample.Common.Models;

namespace ZeroMQPubSubSample.Processor.Abstractions
{
    public interface IMessageReceiver
    {
        public IAsyncEnumerable<Message> ReceiveAsync(CancellationToken ct);
    }
}
