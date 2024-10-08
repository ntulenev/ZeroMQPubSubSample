﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using NetMQ;
using NetMQ.Sockets;

using Newtonsoft.Json;

using ZeroMQPubSubSample.Common.Serialization;
using ZeroMQPubSubSample.Generator.Abstractions;
using ZeroMQPubSubSample.Generator.Logic.Configuration;

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
    public MessageSender(
                         ILogger<MessageSender> logger,
                         IOptions<MessageSenderConfiguration> options)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(options);

        if (options.Value is null)
        {
            throw new ArgumentException("Options is not set.", nameof(options));
        }

        _logger = logger;
        _config = options.Value;

        _pubSocket = new PublisherSocket();
        _pubSocket.Options.SendHighWatermark = _config.SendHighWatermark;
        _pubSocket.Bind(_config.Address);

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
        _logger.LogDebug("Sending raw message {data} to {address} / {destination}.",
               payload, _config.Address, message.Destination);
        _pubSocket.SendMoreFrame(message.Destination).SendFrame(payload);
        _logger.LogDebug("Message {data} has been sent to {address} / {destination}.",
               payload, _config.Address, message.Destination);
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
        if (_isDisposed)
        {
            throw new ObjectDisposedException(GetType().FullName);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _pubSocket.Dispose();
        }
    }

    private readonly ILogger _logger;
    private readonly MessageSenderConfiguration _config;
    private readonly PublisherSocket _pubSocket;
    private bool _isDisposed;
}
