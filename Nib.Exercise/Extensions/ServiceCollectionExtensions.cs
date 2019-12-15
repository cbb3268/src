using Microsoft.Extensions.DependencyInjection;
using System;
using Newtonsoft.Json;
using Nib.Exercise.Interfaces;
using Polly;
using Nib.Exercise.Helpers;
using Microsoft.Extensions.Logging;

namespace Nib.Exercise.Extensions
{
    /// <summary>
    /// Wrapper for any DI service extensions required for this project
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Register the ILocations implementation dependencies
        /// </summary>
        /// <param name="services"></param>
        /// <param name="host"></param>
        public static void RegisterLocationsClient(this IServiceCollection services, string host)
        {
            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetRequiredService<ILogger<LocationsSource>>();
            logger.LogInformation("Seeded the database.");

            //Just incase we need to control serializer settings
            services.AddSingleton<JsonSerializerSettings>();

            //Adding some basic resilience to the locations resources
            int maxRetry = 2;
            services.AddHttpClient<ILocations, LocationsSource>(client =>
            {
                client.BaseAddress = new Uri(host);
            }).AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(
                // number of retries
                maxRetry,
                // standard backoff
                retryAttempt => TimeSpan.FromSeconds(2),
                // on retry
                (httpResponseMessage, timeSpan, retries, context) =>
                {
                    logger.LogWarning($"Retrying {retries}/{maxRetry} httpResponseMessage error : " + httpResponseMessage.Exception.Message);
                }));
        }


    }
}
