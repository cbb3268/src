using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Nib.Exercise.Configurations;
using Xunit;
using Nib.Exercise.Extensions;
using Nib.Exercise.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;
using System;

namespace Nib.Exercise.UnitTests.IntegrationTests
{
    public class VacancyResourceTests
    {

        [Fact(DisplayName = "CanGetVacancyResources")]
        public void CanGetVacancyResources()
        {
            
            var tester = ServiceProvider.GetRequiredService<IVancancies>();
            var restResult = tester.GetVacancyListViewModel(locationId:0).Result;
            Assert.True(restResult.Vacancies.Count > 0, "No locations returned");
        }




        #region TEST DI SETUP

        public ServiceProvider ServiceProvider { get; }
        public ServiceCollection ServiceCollection { get; } = new ServiceCollection();


        public VacancyResourceTests()
        {
            (ConfigurationExercise configurationExercise, IConfigurationRoot rootConfig) = ReadActionsConfig();

            var mockIWebHostEnvironment = new Mock<IWebHostEnvironment>();
            string webRootFolder = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"wwwroot\";
            mockIWebHostEnvironment.SetupGet(p => p.WebRootPath).Returns(webRootFolder);
            ServiceCollection.AddLogging();
            ServiceCollection.AddSingleton(configurationExercise);
            ServiceCollection.AddSingleton(mockIWebHostEnvironment.Object);
            ServiceCollection.RegisterVacanciesSource();
            ServiceProvider = ServiceCollection.BuildServiceProvider();
        }




        private static (ConfigurationExercise configurationExercise, IConfigurationRoot rootConfig) ReadActionsConfig()
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
