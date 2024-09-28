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

            services.AddSingleton(CreateGenerators);
            services.AddSingleton<IMessageMemoryChannel, MessageMemoryChannel>();
            services.AddSingleton<IMemoryChannelProcessor, MemoryChannelProcessor>();
            services.AddSingleton<IMessageSender, MessageSender>();

            services.Configure<MessageMemoryChannelConfiguration>(Configuration.GetSection(nameof(MessageMemoryChannelConfiguration)));
            services.Configure<MessageSenderConfiguration>(Configuration.GetSection(nameof(MessageSenderConfiguration)));

            services.AddSingleton<IValidateOptions<MessageMemoryChannelConfiguration>, MessageMemoryChannelConfigurationValidator>();
            services.AddSingleton<IValidateOptions<MessageSenderConfiguration>, MessageSenderConfigurationValidator>();

            services.AddSingleton<IValidateOptions<DataGeneratorConfiguration>, DataGeneratorConfigurationValidator>();

            services.AddHostedService<GenerationService>();
            services.AddHostedService<SenderService>();
        }

        private IEnumerable<IDataGenerator> CreateGenerators(IServiceProvider serviceProvider)
        {
            List<IDataGenerator> generators = [];
            IConfigurationSection generatorConfigs = Configuration.GetSection("Generators");

            var validator = serviceProvider.GetRequiredService<IValidateOptions<DataGeneratorConfiguration>>();

            foreach (IConfigurationSection generatorConfig in generatorConfigs.GetChildren())
            {
                var configData = generatorConfig.Get<DataGeneratorConfiguration>()!;
                var options = Options.Create(configData);

                // Crutch to use IValidateOptions in manual generation logic.
                var validationResult = validator.Validate(string.Empty, configData);
                if (validationResult.Failed)
                {
                    throw new OptionsValidationException
                        (string.Empty, options.GetType(), new[] { validationResult.FailureMessage });
                }

                generators.Add(new DataGenerator(serviceProvider.GetRequiredService<ILogger<DataGenerator>>(),
                                                 options,
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
