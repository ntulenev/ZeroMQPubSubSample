using Microsoft.Extensions.Options;

using ZeroMQPubSubSample.Processor.Abstractions;
using ZeroMQPubSubSample.Processor.Logic;
using ZeroMQPubSubSample.Processor.Logic.Configuration;
using ZeroMQPubSubSample.Processor.Logic.Configuration.Validation;
using ZeroMQPubSubSample.Processor.Service.Services;

namespace ZeroMQPubSubSample.Processor.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();

            services.AddSingleton<IMessageProcessor, FakeMessageProcessor>();
            services.AddSingleton<IMessageReceiver, MessageReceiver>();

            services.Configure<MessageReceiverConfiguration>(Configuration.GetSection(nameof(MessageReceiverConfiguration)));
            services.AddSingleton<IValidateOptions<MessageReceiverConfiguration>, MessageReceiverConfigurationValidator>();

            services.AddHostedService<ReceiverService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseHealthChecks("/hc");
        }
    }
}
