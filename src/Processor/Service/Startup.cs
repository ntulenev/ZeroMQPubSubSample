using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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

            //TODO Add IMessageProcessor 
            //TODO Add IMessageReceiver

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
