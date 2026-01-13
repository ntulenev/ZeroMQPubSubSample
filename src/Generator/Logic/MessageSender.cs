using Microsoft.Extensions.Logging;

using ZeroMQPubSubSample.Generator.Abstractions;

using Domain = ZeroMQPubSubSample.Common.Models;

namespace ZeroMQPubSubSample.Generator.Logic;

/// <summary>
///Sends data to other systems.
/// </summary>
public class MessageSender : IMessageSender, IDisposable
{
    /// <summary>
    /// Creates <see cref="MessageSender"/>.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="logger">Logger.</param>
    public MessageSender(IDestinationSender<Domain.Message> sender,
                         ILogger<MessageSender> logger)
    {
        ArgumentNullException.ThrowIfNull(sender);
        ArgumentNullException.ThrowIfNull(logger);

        _logger = logger;
        _sender = sender;

        _logger.LogDebug("MessageSender created.");
    }

    /// <inheritdoc/>
    public async Task SendMessageAsync(Domain.TargetedMessage message, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(message);

        ThrowIfDisposed();

        using var _ = _logger.BeginScope("Sending message {Message}", message);

        var sendTask = Task.Run(() => SendMessage(message), ct);

        TaskCompletionSource cancelTaskSource = new(TaskCreationOptions.RunContinuationsAsynchronously);
        var __ = ct.Register(cancelTaskSource.SetResult);

        await Task.WhenAny(cancelTaskSource.Task, sendTask).Unwrap().ConfigureAwait(false);
    }

    private void SendMessage(Domain.TargetedMessage message) => _sender.Send(message.Destination, message);

    /// <inheritdoc/>
#pragma warning disable CA1063 // Implement IDisposable Correctly
    public void Dispose()
#pragma warning restore CA1063 // Implement IDisposable Correctly
    {
        if (_isDisposed)
        {
            return;
        }

        Dispose(true);
        _isDisposed = true;

        _logger.LogDebug("Instance disposed.");

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Throws an ObjectDisposedException if the current instance has been disposed.
    /// </summary>
    /// <remarks>Call this method at the beginning of operations that require the object to be in a valid,
    /// undisposed state. This helps prevent access to resources that have already been released.</remarks>
    protected void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(_isDisposed, this);


    /// <summary>
    /// Releases the unmanaged resources used by the object and optionally releases the managed resources.
    /// </summary>
    /// <remarks>This method is called by public Dispose methods and can be overridden to release additional
    /// resources. When disposing is true, both managed and unmanaged resources can be disposed; when false, only
    /// unmanaged resources should be released.</remarks>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _sender.Dispose();
        }
    }

    private readonly ILogger _logger;
    private readonly IDestinationSender<Domain.Message> _sender;
    private bool _isDisposed;
}
