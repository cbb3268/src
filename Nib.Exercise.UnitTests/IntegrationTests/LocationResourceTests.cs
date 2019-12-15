using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Nib.Exercise.Configurations;
using Xunit;
using Nib.Exercise.Extensions;
using Nib.Exercise.Interfaces;
using Microsoft.Extensions.Logging;
using Nib.Exercise.Helpers;
using System.Net.Http;

namespace Nib.Excercise.UnitTests.IntegrationTests
{
    public class LocationResourceTests
    {

        [Fact(DisplayName = "CanGetLocationResources")]
        public void CanGetLocationResources()
        {
            var tester = ServiceProvider.GetRequiredService<ILocations>();
            var restResult = tester.GetLocations().Result;
            Assert.True(restResult.Count>0, "No locations returned");
        }
     

        #region TEST DI SETUP

        public ServiceProvider ServiceProvider { get; }
        public ServiceCollection ServiceCollection { get; } = new ServiceCollection();


        public LocationResourceTests()
        {
            (ConfigurationExercise configurationExercise, IConfigurationRoot rootConfig) = ReadActionsConfig();
            ServiceCollection.AddLogging();
            ServiceCollection.AddSingleton(configurationExercise);
            ServiceCollection.RegisterLocationsClient(configurationExercise.LocationsApiUrl);
            ServiceProvider= ServiceCollection.BuildServiceProvider();
        }


        

        private static (ConfigurationExercise configurationExercise, IConfigurationRoot rootConfig)  ReadActionsConfig()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile($@"Assets\appsettings.Testing.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();

            return (configuration.GetSection("application").Get<ConfigurationExercise>(), configuration);
        }

        #endregion
    }
}
