using I2M.KenticoContactRemover.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace I2M.KenticoContactRemover
{
    public class Program
    {
        public static void Main()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            var services = new ServiceCollection();

            var startup = new Startup(configuration);

            startup.ConfigureServices(services);

            using var serviceProvider = services.BuildServiceProvider();

            var contactRemoverClient = serviceProvider.GetService<IContactRemoverClient>();

            contactRemoverClient?.Process();
        }
    }
}
