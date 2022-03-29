using I2M.KenticoContactRemover.Interfaces;
using I2M.KenticoContactRemover.Models;
using I2M.KenticoContactRemover.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace I2M.KenticoContactRemover.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureContactRemoverClient(this IServiceCollection services)
        {
            services.AddSingleton<IContactRemoverClient, ContactRemoverClient>();
        }

        public static void ConfigureContactService(this IServiceCollection services)
        {
            services.AddSingleton<IContactService, ContactService>();
        }

        public static void ConfigureLogger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(x =>
            {
                x.ClearProviders();
                x.SetMinimumLevel(LogLevel.Trace);
                //x.AddConfiguration(configuration.GetSection("Logging"));
                x.AddNLog(configuration);
            });
        }

        public static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ClientOptions>(configuration.GetSection(nameof(ClientOptions)));
        }
    }
}
