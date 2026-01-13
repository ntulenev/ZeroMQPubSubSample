using Microsoft.Extensions.Options;

using ZeroMQPubSubSample.Common.Models;
using ZeroMQPubSubSample.Generator.Abstractions;
using ZeroMQPubSubSample.Generator.Logic;
using ZeroMQPubSubSample.Generator.Logic.Configuration;
using ZeroMQPubSubSample.Generator.Logic.Configuration.Validation;
using ZeroMQPubSubSample.Generator.Service.Services;

namespace ZeroMQPubSubSample.Generator.Service;

internal sealed class Startup(IConfiguration Configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHealthChecks();

        services.AddSingleton(CreateGenerators);
        services.AddSingleton<IMessageMemoryChannel, MessageMemoryChannel>();
        services.AddSingleton<IMemoryChannelProcessor, MemoryChannelProcessor>();
        services.AddSingleton<IMessageSender, MessageSender>();
        services.AddSingleton<IDestinationSender<Message>, DestinationSender>();
        services.AddSingleton<ISerializer<Message, string>, Serializer>();

        services.Configure<MessageMemoryChannelConfiguration>(Configuration.GetSection(nameof(MessageMemoryChannelConfiguration)));
        services.Configure<DestinationSenderConfiguration>(Configuration.GetSection(nameof(DestinationSenderConfiguration)));

        services.AddSingleton<IValidateOptions<MessageMemoryChannelConfiguration>, MessageMemoryChannelConfigurationValidator>();
        services.AddSingleton<IValidateOptions<DestinationSenderConfiguration>, DestinationSenderConfigurationValidator>();

        services.AddSingleton<IValidateOptions<DataGeneratorConfiguration>, DataGeneratorConfigurationValidator>();

        services.AddHostedService<GenerationService>();
        services.AddHostedService<SenderService>();
    }

    private IEnumerable<IDataGenerator> CreateGenerators(IServiceProvider serviceProvider)
    {
        List<IDataGenerator> generators = [];
        var generatorConfigs = Configuration.GetSection("Generators");

        var validator = serviceProvider.GetRequiredService<IValidateOptions<DataGeneratorConfiguration>>();

        foreach (var generatorConfig in generatorConfigs.GetChildren())
        {
            var configData = generatorConfig.Get<DataGeneratorConfiguration>()!;
            var options = Options.Create(configData);

            // Crutch to use IValidateOptions in manual generation logic.
            var validationResult = validator.Validate(string.Empty, configData);
            if (validationResult.Failed)
            {
                throw new OptionsValidationException
                    (string.Empty, options.GetType(), [validationResult.FailureMessage]);
            }

            generators.Add(new DataGenerator(serviceProvider.GetRequiredService<ILogger<DataGenerator>>(),
                                             options,
                                             serviceProvider.GetRequiredService<IMessageMemoryChannel>()));
        }

        return generators;
    }

    /// <summary>
    /// Configures the application's request pipeline and health check endpoint.
    /// </summary>
    /// <remarks>This method sets up routing and enables a health check endpoint at '/hc'. It should be called
    /// during application startup to ensure proper middleware configuration.</remarks>
    /// <param name="app">The application builder used to configure the HTTP request pipeline.</param>
#pragma warning disable CA1822 // Mark members as static
    public void Configure(IApplicationBuilder app)
#pragma warning restore CA1822 // Mark members as static
    {
        app.UseRouting();
        app.UseHealthChecks("/hc");
    }
}
