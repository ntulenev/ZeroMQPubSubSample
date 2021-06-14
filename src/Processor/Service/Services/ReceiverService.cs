using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using ZeroMQPubSubSample.Processor.Abstractions;

namespace ZeroMQPubSubSample.Processor.Service.Services
{
    /// <summary>
    /// Hosted service that receives messages.
    /// </summary>
    public class ReceiverService : IHostedService
    {
        public ReceiverService(ILogger<ReceiverService> logger,
                            IHostApplicationLifetime hostApplicationLifetime,
                            IMessageReceiver receiver,
                            IMessageProcessor processor
                                   )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _hostApplicationLifetime = hostApplicationLifetime ?? throw new ArgumentNullException(nameof(hostApplicationLifetime));
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
            _receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));

            _logger.LogInformation("ReceiverService created.");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _stopper = Task.Run(async () =>
                {
                    await foreach (var message in _receiver.ReceiveAsync(_hostApplicationLifetime.ApplicationStopping)
                                                          .ConfigureAwait(false))
                    {
                        _logger.LogInformation("Process message {message}.", message);
                        await _processor.ProcessAsync(message, _hostApplicationLifetime.ApplicationStopping);
                    }
                });
                _stopper.ContinueWith(x => _hostApplicationLifetime.StopApplication(), TaskContinuationOptions.OnlyOnFaulted);
            }
            catch (Exception ex)
            {
                _logger?.LogCritical(ex, "Error starting SenderService.");
                _hostApplicationLifetime.StopApplication();
            }

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping service.");
            try
            {
                await _stopper.ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                //Skip
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error on stopping service.");
            }
        }

        private readonly ILogger<ReceiverService> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IMessageReceiver _receiver;
        private readonly IMessageProcessor _processor;
        private Task _stopper = default!;
    }
}
