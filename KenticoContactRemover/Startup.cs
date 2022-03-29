using I2M.KenticoContactRemover.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace I2M.KenticoContactRemover
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(ServiceCollection services)
        {
            services.ConfigureContactRemoverClient();
            services.ConfigureContactService();
            services.ConfigureOptions(_configuration);
            services.ConfigureLogger(_configuration);
        }
    }
}
