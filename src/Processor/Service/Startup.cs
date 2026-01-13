using Microsoft.Extensions.Options;

using ZeroMQPubSubSample.Processor.Abstractions;
using ZeroMQPubSubSample.Processor.Logic;
using ZeroMQPubSubSample.Processor.Logic.Configuration;
using ZeroMQPubSubSample.Processor.Logic.Configuration.Validation;
using ZeroMQPubSubSample.Processor.Service.Services;

namespace ZeroMQPubSubSample.Processor.Service;

internal sealed class Startup(IConfiguration Configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHealthChecks();

        services.AddSingleton<IMessageProcessor, FakeMessageProcessor>();
        services.AddSingleton<IMessageReceiver, MessageReceiver>();
        services.AddSingleton<IReceiveHandler, ReceiveHandler>();

        services.Configure<MessageReceiverConfiguration>(Configuration.GetSection(nameof(MessageReceiverConfiguration)));
        services.AddSingleton<IValidateOptions<MessageReceiverConfiguration>, MessageReceiverConfigurationValidator>();

        services.AddHostedService<ReceiverService>();
    }

    /// <summary>
    /// Configures the application's request pipeline with routing and health check endpoints.
    /// </summary>
    /// <remarks>This method adds routing middleware and maps a health check endpoint at '/hc'. It should be
    /// called during application startup to ensure proper middleware configuration.</remarks>
    /// <param name="app">The application builder used to configure the HTTP request pipeline.</param>
#pragma warning disable CA1822 // Mark members as static
    public void Configure(IApplicationBuilder app)
#pragma warning restore CA1822 // Mark members as static
    {
        app.UseRouting();
        app.UseHealthChecks("/hc");
    }
}
