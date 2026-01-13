namespace ZeroMQPubSubSample.Generator.Abstractions;

/// <summary>
/// Represents a worker that continuously reads data from an in-memory channel and processes it.
/// </summary>
public interface IMemoryChannelProcessor
{
    /// <summary>
    /// Asynchronously processes data read from the in-memory channel until cancellation is requested.
    /// </summary>
    /// <param name="ct">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous processing operation.</returns>
    Task ProcessAsync(CancellationToken ct);
}

