using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using NetMQ;
using NetMQ.Sockets;

using Newtonsoft.Json;

using ZeroMQPubSubSample.Common.Serialization;
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
    /// <param name="logger">Logger.</param>
    /// <param name="options">Config params.</param>
    public MessageSender(IDestinationSender sender,
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

        using var _ = _logger.BeginScope("Sending message {message}", message);

        var sendTask = Task.Run(() => SendMessage(message), ct);

        TaskCompletionSource cancelTaskSource = new(TaskCreationOptions.RunContinuationsAsynchronously);
        ct.Register(cancelTaskSource.SetResult);

        await Task.WhenAny(cancelTaskSource.Task, sendTask).Unwrap().ConfigureAwait(false);
    }

    private void SendMessage(Domain.TargetedMessage message)
    {
        _logger.LogDebug("Serializing message");
        var payload = Serialize(message);
        _logger.LogDebug("Sending raw message {data} to {destination}.",
               payload, message.Destination);
        _sender.Send(message.Destination, payload);
        _logger.LogDebug("Message {data} has been sent to {destination}.",
               payload, message.Destination);
    }

    private static string Serialize(Domain.Message message)
    {
        //TODO Move to separate Serializer dependency
        var transport = message.ToTransport();
        return JsonConvert.SerializeObject(transport);
    }


    /// <inheritdoc/>
    public void Dispose()
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

    protected void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_isDisposed,this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _sender.Dispose();
        }
    }

    private readonly ILogger _logger;
    private readonly IDestinationSender _sender;
    private bool _isDisposed;
}
