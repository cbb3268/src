using Microsoft.Extensions.Configuration;

namespace Nib.Exercise.Helpers
{
    /// <summary>
    /// Used to load the conifguration (settings) into in memory object that can be used for DI
    /// </summary>
    public static class ConfigurationLoader
    {
        public static (T config, IConfigurationRoot rootConfig) LoadConfigurations<T>(string environment)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appSettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            return (configuration.GetSection("application").Get<T>(), configuration);
        }
    }
}
