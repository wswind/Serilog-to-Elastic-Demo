using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace SerilogDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateBootstrapLogger();

            try
            {
                // const string outputTemplate = "<{ThreadId}> {Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}";

                var builder = WebApplication.CreateBuilder(args);

                //var esSinkOption = new ElasticsearchSinkOptions(new Uri("http://192.168.196.110:9200"))
                //{
                //    IndexFormat = "custom-index-{0:yyyy.MM}"
                //};


                // Add services to the container.
                //builder.Services.AddSerilog((services, lc) => lc
                //    .MinimumLevel.Information()
                //    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
                //    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
                //    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
                //    .ReadFrom.Configuration(builder.Configuration)
                //    .ReadFrom.Services(services)
                //    .Enrich.FromLogContext()
                //    .Enrich.WithThreadId()
                //    .Enrich.WithMachineName()
                //    .WriteTo.Console(outputTemplate: outputTemplate)
                //    .WriteTo.File("Logs/.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7, outputTemplate: outputTemplate)
                //    .WriteTo.Elasticsearch(esSinkOption)
                //);

                builder.Services.AddSerilog(
                    (services, lc) => { 
                        IConfiguration configuration = services.GetRequiredService<IConfiguration>();
                        lc.ReadFrom.Configuration(configuration)
                          .ReadFrom.Services(services);
                    }
                );

                builder.Services.AddControllers();

                var app = builder.Build();

                app.UseSerilogRequestLogging();

                // Configure the HTTP request pipeline.

                app.UseAuthorization();

                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
