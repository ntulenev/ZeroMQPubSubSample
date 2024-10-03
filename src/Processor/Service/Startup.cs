using Microsoft.Extensions.Options;

using ZeroMQPubSubSample.Processor.Abstractions;
using ZeroMQPubSubSample.Processor.Logic;
using ZeroMQPubSubSample.Processor.Logic.Configuration;
using ZeroMQPubSubSample.Processor.Logic.Configuration.Validation;
using ZeroMQPubSubSample.Processor.Service.Services;

namespace ZeroMQPubSubSample.Processor.Service;

public class Startup(IConfiguration Configuration)
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

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
        app.UseHealthChecks("/hc");
    }
}
