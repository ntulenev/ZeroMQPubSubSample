using Microsoft.Extensions.Options;
using NetMQ;
using NetMQ.Sockets;
using ZeroMQPubSubSample.Common.Models;
using ZeroMQPubSubSample.Generator.Abstractions;
using ZeroMQPubSubSample.Generator.Logic.Configuration;

namespace ZeroMQPubSubSample.Generator.Logic;

/// <summary>
/// Implementation for <see cref="IDestinationSender"/> that sends messages to a specified destination
/// using a publisher socket.
/// </summary>
public sealed class DestinationSender : IDestinationSender
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DestinationSender"/> class.
    /// </summary>
    /// <param name="configuration">
    /// The configuration settings for the <see cref="DestinationSender"/>, 
    /// provided as an <see cref="IOptions{TOptions}"/> of <see cref="DestinationSenderConfiguration"/>.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="configuration"/> is <c>null</c>.
    /// </exception>
    public DestinationSender(
        IOptions<DestinationSenderConfiguration> configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        _pubSocket = new PublisherSocket();
        _pubSocket.Options.SendHighWatermark = configuration.Value.SendHighWatermark;
        _pubSocket.Bind(configuration.Value.Address);
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="destination"/> or <paramref name="message"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// Thrown if the sender is disposed before the message is sent.
    /// </exception>
    public void Send(Destination destination, string message)
    {
        ArgumentNullException.ThrowIfNull(destination);
        ArgumentNullException.ThrowIfNull(message);

        ThrowIfDisposed();

        _pubSocket.SendMoreFrame(destination.Route).SendFrame(message);
    }

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);
    }

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

    private readonly PublisherSocket _pubSocket;
    private bool _isDisposed;
}
