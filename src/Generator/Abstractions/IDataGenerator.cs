using System.Threading;
using System.Threading.Tasks;

namespace ZeroMQPubSubSample.Generator.Abstractions
{
    /// <summary>
    /// Generates test messages.
    /// </summary>
    public interface IDataGenerator
    {
        /// <summary>
        /// Start test messages generation.
        /// </summary>
        public Task GenerateDataAsync(CancellationToken ct);
    }
}
