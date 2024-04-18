namespace Howabout.Services
{
	public class ConfigurationsService
	{
        private readonly IConfiguration _configuration;

        public ConfigurationsService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
    }
}
