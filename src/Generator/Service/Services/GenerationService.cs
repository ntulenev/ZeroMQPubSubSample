﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using ZeroMQPubSubSample.Generator.Abstractions;

namespace ZeroMQPubSubSample.Generator.Service.Services
{
    /// <summary>
    /// Hosted service that generates test data messages
    /// </summary>
    public class GenerationService : IHostedService
    {
        public GenerationService(ILogger<GenerationService> logger,
                             IHostApplicationLifetime hostApplicationLifetime,
                             IEnumerable<IDataGenerator> generators
                                    )
        {
            _logger = logger;
            _hostApplicationLifetime = hostApplicationLifetime ?? throw new ArgumentNullException(nameof(hostApplicationLifetime));
            _generators = generators ?? throw new ArgumentNullException(nameof(generators));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _stopper = Task.WhenAll(_generators
                            .Select(x =>
                                        Task.Run(async () => await x.GenerateDataAsync(_hostApplicationLifetime.ApplicationStopping)
                                                                    .ConfigureAwait(false)
                                                )
                                   )
                            );
            }
            catch (Exception ex)
            {
                _logger?.LogCritical(ex, "Error starting GenerationService.");
                _hostApplicationLifetime.StopApplication();
            }

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken) => await _stopper.ConfigureAwait(false);

        private readonly ILogger<GenerationService> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IEnumerable<IDataGenerator> _generators;
        private Task _stopper = default!;
    }
}
