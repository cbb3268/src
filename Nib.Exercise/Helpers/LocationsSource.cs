using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nib.Exercise.Interfaces;
using Nib.Exercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Nib.Exercise.Helpers
{
    public class LocationsSource : ILocations
    {
        #region PROPERTIES

        private readonly ILogger<LocationsSource> _logger;

        protected readonly HttpClient _httpClient;

        protected JsonSerializerSettings JsonSerializerSettings { get; set; }

        #endregion

        #region CONSTRUCTORS

        public LocationsSource(ILogger<LocationsSource> logger
           , HttpClient httpClient
           , JsonSerializerSettings jsonSerializerSettings)
        {
            _logger = logger;
            _httpClient = httpClient;
            JsonSerializerSettings = jsonSerializerSettings;
        }

        #endregion

        public async Task<List<Location>> GetLocationListViewModel(CancellationToken cancellationToken = new CancellationToken())
        {
            List<Location> returnValue = null;

            try
            {
                var uri = new Uri(_httpClient.BaseAddress.ToString());
                _logger.LogDebug($"Getting locations via API URL : {uri.OriginalString}");
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = uri
                };

                var response = await _httpClient.SendAsync(request, cancellationToken);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    _logger.LogInformation($"Failed to get locations ReasonPhrase : {response.ReasonPhrase}");

                returnValue = JsonConvert.DeserializeObject<List<Location>>(responseContent, JsonSerializerSettings);
            }
            catch (Exception ex)
            {
                ex.Data.Add("GetLocationsErrorUrl", _httpClient.BaseAddress.ToString());
                throw;
            }

            return returnValue;
        }
    }
}
