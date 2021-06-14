using System.Threading;
using System.Threading.Tasks;

using ZeroMQPubSubSample.Common.Models;

namespace ZeroMQPubSubSample.Processor.Abstractions
{
    public interface IMessageProcessor
    {
        public Task ProcessAsync(Message message, CancellationToken ct);
    }
}
