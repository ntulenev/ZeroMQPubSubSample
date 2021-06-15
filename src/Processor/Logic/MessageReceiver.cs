using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

using Transport = ZeroMQPubSubSample.Common.Serialization;
using Domain = ZeroMQPubSubSample.Common.Models;

namespace ZeroMQPubSubSample.Processor.Logic
{
    /// <summary>
    /// ZeroMQ message receiver
    /// </summary>
    public class MessageReceiver : IMessageReceiver
    {
        /// <summary>
        /// Creates <see cref="MessageReceiver"/>
        /// </summary>
        public MessageReceiver(ILogger<MessageReceiver> logger, IOptions<MessageReceiverConfiguration> config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (config.Value is null)
            {
                throw new ArgumentException("Configuration is not set", nameof(config));
            }

            _config = config.Value;
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<Domain.Message> ReceiveAsync([EnumeratorCancellation] CancellationToken ct)
        {
            using var subSocket = new SubscriberSocket();

            subSocket.Options.ReceiveHighWatermark = _config.ReceiveHighWatermark;

            subSocket.Connect(_config.Address);
            subSocket.Subscribe(_config.Topic);

            var stopperTcs = new TaskCompletionSource<Domain.Message>(TaskCreationOptions.RunContinuationsAsynchronously);
            ct.Register(() => stopperTcs.SetCanceled());

            while (!ct.IsCancellationRequested)
            {
                yield return await Task.WhenAny(Task.Run(() =>
                {
                    _ = subSocket.ReceiveFrameString();
                    var data = subSocket.ReceiveFrameString();
                    return Deserialize(data);
                }, ct), stopperTcs.Task).Unwrap();
            }
        }

        private static Domain.Message Deserialize(string message)
        {
            var transport = JsonConvert.DeserializeObject<Transport.Message>(message);

            Debug.Assert(transport is not null);

            return transport.FromTransport();
        }

        private readonly ILogger<MessageReceiver> _logger;
        private readonly MessageReceiverConfiguration _config;
    }
}
