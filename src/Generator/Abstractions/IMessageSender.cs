using System.Threading;
using System.Threading.Tasks;

using ZeroMQPubSubSample.Common.Models;

namespace ZeroMQPubSubSample.Generator.Abstractions
{
    public interface IMessageSender
    {
        public Task SendMessageAsync(TargetedMessage message, CancellationToken ct);
    }
}
