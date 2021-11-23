using ZeroMQPubSubSample.Generator.Service;

var builder = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
var app = builder.Build(); 
app.Run();