using Microsoft.Extensions.Options;
using NetMQ;
using NetMQ.Sockets;
using ZeroMQPubSubSample.Common.Models;
using ZeroMQPubSubSample.Generator.Abstractions;
using ZeroMQPubSubSample.Generator.Logic.Configuration;

namespace ZeroMQPubSubSample.Generator.Logic;

public sealed class DestinationSender : IDestinationSender
{
    public DestinationSender(
        IOptions<DestinationSenderConfiguration> configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        _pubSocket = new PublisherSocket();
        _pubSocket.Options.SendHighWatermark = configuration.Value.SendHighWatermark;
        _pubSocket.Bind(configuration.Value.Address);
    }

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
