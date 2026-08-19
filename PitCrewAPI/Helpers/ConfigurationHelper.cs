namespace PitCrewAPI.Helpers
{
    using Microsoft.Extensions.Configuration;

    public static class ConfigurationHelper
    {
        public static IConfiguration Configuration;

        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}

