namespace ZeroMQPubSubSample.Processor.Abstractions;

/// <summary>
/// Defines a handler for receiving and processing tasks.
/// </summary>
public interface IReceiveHandler
{
    /// <summary>
    /// Handles a task asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task HandleAsync(CancellationToken cancellationToken);
}
