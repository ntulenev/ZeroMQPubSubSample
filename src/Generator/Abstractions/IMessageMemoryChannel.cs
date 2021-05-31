using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ZeroMQPubSubSample.Common.Models;

namespace ZeroMQPubSubSample.Generator.Abstractions
{
    /// <summary>
    /// In-memory messages queue
    /// </summary>
    public interface IMessageMemoryChannel
    {
        /// <summary>
        /// Adds new item in queue.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="ct">Cancellation token.</param>
        public ValueTask WriteAsync(TargetedMessage message, CancellationToken ct);

        /// <summary>
        /// Read all data from channel as async iterator.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        public IAsyncEnumerable<TargetedMessage> ReadAllAsync(CancellationToken cancellationToken = default);
    }
}
