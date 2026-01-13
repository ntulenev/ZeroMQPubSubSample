using System.Diagnostics;
using System.Runtime.CompilerServices;

using ZeroMQPubSubSample.Processor.Abstractions;
using ZeroMQPubSubSample.Processor.Logic.Configuration;
using ZeroMQPubSubSample.Common.Serialization;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using NetMQ.Sockets;
using NetMQ;

using Newtonsoft.Json;

using Domain = ZeroMQPubSubSample.Common.Models;

namespace ZeroMQPubSubSample.Processor.Logic;

/// <summary>
/// ZeroMQ message receiver.
/// </summary>
public sealed class MessageReceiver : IMessageReceiver
{
    /// <summary>
    /// Creates <see cref="MessageReceiver"/>.
    /// </summary>
    public MessageReceiver(ILogger<MessageReceiver> logger, IOptions<MessageReceiverConfiguration> config)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(config);

        if (config.Value is null)
        {
            throw new ArgumentException("Configuration is not set.", nameof(config));
        }

        _logger = logger;
        _config = config.Value;

        _logger.LogDebug("MessageReceiver created.");
    }

    private SubscriberSocket CreateSocket()
    {
        var subSocket = new SubscriberSocket();
        subSocket.Options.ReceiveHighWatermark = _config.ReceiveHighWatermark;
        subSocket.Connect(_config.Address);
        subSocket.Subscribe(_config.Topic);
        return subSocket;
    }


    private static (string topic, string payload) ReceiveData(SubscriberSocket subSocket)
    {
        var topic = subSocket.ReceiveFrameString();
        var data = subSocket.ReceiveFrameString();
        return (topic, data);
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<Domain.Message> ReceiveAsync([EnumeratorCancellation] CancellationToken ct)
    {
        using var subSocket = CreateSocket();

        var stopperTcs = new TaskCompletionSource<Domain.Message>(TaskCreationOptions.RunContinuationsAsynchronously);
        _ = ct.Register(stopperTcs.SetCanceled);

        while (true)
        {
            ct.ThrowIfCancellationRequested();

            yield return await Task.WhenAny(Task.Run(() =>
            {
                try
                {
                    var (topic, data) = ReceiveData(subSocket);

                    _logger.LogDebug("Gets raw data '{Data}' for topic {Topic}.", data, topic);

                    return Deserialize(data);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error on receive message.");
                    throw;
                }

            }, ct), stopperTcs.Task).Unwrap().ConfigureAwait(false);
        }
    }

    private static Domain.Message Deserialize(string message)
    {
        //TODO Move to separate dependency
        var transport = JsonConvert.DeserializeObject<Message>(message);

        Debug.Assert(transport is not null);

        return transport.FromTransport();
    }

    private readonly ILogger _logger;
    private readonly MessageReceiverConfiguration _config;
}
