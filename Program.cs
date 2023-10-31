using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using micm_etpl_job.Interfaces;
using micm_etpl_job.Processors;
using micm_etpl_job.Service;
//using micm-etpl - job.Interfaces;
namespace micm_etpl_job;
public class Program
{
    private static ILogger<Program> _logger;

    static async Task Main(string[] args)
    {
        try
        {
            using var host = CreateHostBuilder(args).Build();

            _logger = host.Services.GetRequiredService<ILogger<Program>>();

            var applicationService = host.Services.GetService<ApplicationService>();
            await applicationService.Run(args);
        }

        catch (Exception ex)
        {
            _logger?.LogCritical(ex, "An unhandled exception occurred while starting the application.");
            throw;
        }
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args).ConfigureServices((_, services) =>
        {
            services.AddLogging();
            services.AddSingleton<IMessageQueueHandler, SqsMessageHandler>();
            services.AddSingleton<IMessageProcessor, MessageProcessor>();
            services.AddSingleton<IMessageProcessingLogger, MessageProcessingLogger>();
            services.AddSingleton<ApplicationService>();
        });
}