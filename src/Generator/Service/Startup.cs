using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using ZeroMQPubSubSample.Generator.Abstractions;
using ZeroMQPubSubSample.Generator.Logic;
using ZeroMQPubSubSample.Generator.Logic.Configuration;
using ZeroMQPubSubSample.Generator.Logic.Configuration.Validation;
using ZeroMQPubSubSample.Generator.Service.Services;

namespace ZeroMQPubSubSample.Generator.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();

            services.AddSingleton(RegisterGenerators);
            services.AddSingleton<IMessageMemoryChannel, MessageMemoryChannel>();
            services.AddSingleton<IMemoryChannelProcessor, MemoryChannelProcessor>();
            services.AddSingleton<IMessageSender, MessageSender>();

            services.Configure<MessageMemoryChannelConfiguration>(Configuration.GetSection(nameof(MessageMemoryChannelConfiguration)));
            services.Configure<MessageSenderConfiguration>(Configuration.GetSection(nameof(MessageSenderConfiguration)));

            services.AddSingleton<IValidateOptions<MessageMemoryChannelConfiguration>, MessageMemoryChannelConfigurationValidator>();
            services.AddSingleton<IValidateOptions<MessageSenderConfiguration>, MessageSenderConfigurationValidator>();
            
            // TODO Need To use in manual options creaton logic.
            //services.AddSingleton<IValidateOptions<DataGeneratorConfiguration>, DataGeneratorConfigurationValidator>();

            services.AddHostedService<GenerationService>();
            services.AddHostedService<SenderService>();
        }

        private IEnumerable<IDataGenerator> RegisterGenerators(IServiceProvider serviceProvider)
        {
            List<IDataGenerator> generators = new();
            IConfigurationSection generatorConfigs = Configuration.GetSection("Generators");
            foreach (IConfigurationSection generatorConfig in generatorConfigs.GetChildren())
            {

                generators.Add(new DataGenerator(serviceProvider.GetRequiredService<ILogger<DataGenerator>>(),
                                                 Options.Create(generatorConfig.Get<DataGeneratorConfiguration>()),
                                                 serviceProvider.GetRequiredService<IMessageMemoryChannel>()));
            }

            return generators;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseHealthChecks("/hc");
        }
    }
}
