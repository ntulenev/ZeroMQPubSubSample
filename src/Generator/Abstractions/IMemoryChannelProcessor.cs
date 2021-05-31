using System.Threading;
using System.Threading.Tasks;

namespace ZeroMQPubSubSample.Generator.Abstractions
{
    /// <summary>
    /// Worker that reads data from channel and does processing.
    /// </summary>
    public interface IMemoryChannelProcessor
    {
        /// <summary>
        /// Process data.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        public Task ProcessAsync(CancellationToken ct);

    }
}
