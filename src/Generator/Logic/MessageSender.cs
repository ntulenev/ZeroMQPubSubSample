﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using NetMQ;
using NetMQ.Sockets;

using Newtonsoft.Json;

using ZeroMQPubSubSample.Common.Serialization;
using ZeroMQPubSubSample.Generator.Abstractions;
using ZeroMQPubSubSample.Generator.Logic.Configuration;

using Domain = ZeroMQPubSubSample.Common.Models;

namespace ZeroMQPubSubSample.Generator.Logic
{
    /// <summary>
    ///Sends data to other systems.
    /// </summary>
    public class MessageSender : IMessageSender, IDisposable
    {
        public MessageSender(
                             ILogger<MessageSender> logger,
                             IOptions<MessageSenderConfiguration> options)
        {
            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.Value is null)
            {
                throw new ArgumentException("Options is not set.", nameof(options));
            }

            _logger = logger;
            _config = options.Value;

            _pubSocket = new PublisherSocket();
            _pubSocket.Options.SendHighWatermark = _config.SendHighWatermark;
            _pubSocket.Bind(_config.Address);

            _logger.LogInformation("MessageSender created.");
        }

        /// <inheritdoc/>
        public async Task SendMessageAsync(Domain.TargetedMessage message, CancellationToken ct)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            ThrowIfDisposed();

            _logger.LogDebug("Seralizing message {message}", message);

            var data = Serialize(message);

            _logger.LogDebug("Sending raw message {data} to {address} / {destination}", data, _config.Address, message.Destination);

            var sendTask = Task.Run(() => _pubSocket.SendMoreFrame(message.Destination).SendFrame(data), ct);

            TaskCompletionSource cancelTaskSource = new(TaskCreationOptions.RunContinuationsAsynchronously);

            ct.Register(cancelTaskSource.SetResult);

            await Task.WhenAny(cancelTaskSource.Task, sendTask).Unwrap().ConfigureAwait(false);
        }

        private static string Serialize(Domain.Message message)
        {
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

            _logger.LogInformation("Instance disposed.");

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

        private readonly ILogger<MessageSender> _logger;
        private readonly MessageSenderConfiguration _config;
        private readonly PublisherSocket _pubSocket;
        private bool _isDisposed;
    }
}
