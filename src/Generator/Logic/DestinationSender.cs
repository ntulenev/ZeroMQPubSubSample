using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using NetMQ;
using NetMQ.Sockets;

using ZeroMQPubSubSample.Common.Models;
using ZeroMQPubSubSample.Generator.Abstractions;
using ZeroMQPubSubSample.Generator.Logic.Configuration;

namespace ZeroMQPubSubSample.Generator.Logic;

/// <summary>
/// Implementation for <see cref="IDestinationSender{T}"/> that sends messages to a specified destination
/// using a publisher socket.
/// </summary>
public sealed class DestinationSender : IDestinationSender<Message>
{

    /// <summary>
    /// Initializes a new instance of the DestinationSender class with the specified configuration, logger, and message
    /// serializer.
    /// </summary>
    /// <param name="configuration">The configuration options for the destination sender,
    /// including address and socket settings. Cannot be null.</param>
    /// <param name="logger">The logger used to record operational and error information. Cannot be null.</param>
    /// <param name="serializer">The serializer used to convert Message objects to string representations for transmission.
    /// Cannot be null.</param>
    public DestinationSender(
        IOptions<DestinationSenderConfiguration> configuration,
        ILogger<DestinationSender> logger,
        ISerializer<Message, string> serializer)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(serializer);

        _pubSocket = new PublisherSocket();
        _pubSocket.Options.SendHighWatermark = configuration.Value.SendHighWatermark;
        _pubSocket.Bind(configuration.Value.Address);
        _serializer = serializer;
        _logger = logger;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="destination"/> or <paramref name="message"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if the sender is disposed before the message is sent.
    /// </exception>
    public void Send(Destination destination, Message message)
    {
        ArgumentNullException.ThrowIfNull(destination);
        ArgumentNullException.ThrowIfNull(message);

        ThrowIfDisposed();

        var payload = _serializer.Serialize(message);

        _logger.LogDebug("Sending raw message {Data} to {Destination}.", payload, destination);


        _pubSocket.SendMoreFrame(destination.Route)
                  .SendFrame(payload);

        _logger.LogDebug("Message {Data} has been sent to {Destination}.", payload, destination);
    }

    private void ThrowIfDisposed() => ObjectDisposedException.ThrowIf(_isDisposed, this);

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            _pubSocket.Dispose();
        }
    }

    /// <summary>
    /// Disposes of the resources used by this instance of <see cref="DestinationSender"/>.
    /// </summary>
    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        Dispose(true);
        _isDisposed = true;

        GC.SuppressFinalize(this);
    }

    private readonly ILogger _logger;
    private readonly PublisherSocket _pubSocket;
    private readonly ISerializer<Message, string> _serializer;
    private bool _isDisposed;
}
