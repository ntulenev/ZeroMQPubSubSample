using System.Threading;
using System.Threading.Tasks;

namespace ZeroMQPubSubSample.Generator.Abstractions
{
    public interface IMemoryChannelProcessor
    {
        public Task ProcessAsync(CancellationToken ct);

    }
}
