using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Nib.Exercise.Interfaces;
using Nib.Exercise.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nib.Exercise.Helpers
{
    public class VacanciesSource : IVancancies
    {
        #region PROPERTIES

        private readonly ILogger<VacanciesSource> _logger;

        private readonly IWebHostEnvironment _webHostEnvironment;

        private VacancyListViewModel _vacancyListViewModel;

        private IPropertiesFormatter<Vacancy> _propertiesFormatter;

        private object lockObject = new object();

        #endregion

        #region CONSTRUCTORS

        public VacanciesSource(ILogger<VacanciesSource> logger
            , IPropertiesFormatter<Vacancy> propertiesFormatter
            , IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _propertiesFormatter = propertiesFormatter;
        }

        #endregion

        public Task<VacancyListViewModel> GetVacancyListViewModel(int locationId=0)
        {
            VacancyListViewModel returnValue;
            string fileName = $@"{_webHostEnvironment.WebRootPath}\Content\Careers\vacancies.json";

            try
            {
                //Basic thread saftey. Wouldn't usually do this as should sit in DB.
                //Probably either Postgres documentstore or DynamoDb as suits document or key value store. 
                lock (lockObject)
                {
                    if (_vacancyListViewModel == null)
                    {
                        
                        _logger.LogTrace($"Vacancies filename : {fileName}");
                        _logger.LogDebug($"Loading vacancies from repo for locationid {locationId}");
                        using (StreamReader sr = new StreamReader(fileName))
                        {
                            _vacancyListViewModel = JsonConvert.DeserializeObject<VacancyListViewModel>(sr.ReadToEnd());
                            returnValue = _vacancyListViewModel;
                        }
                    }
                    else
                    {
                        if (locationId > 0)
                        {
                            //Specific locations
                            var matchingVacancies = (from n in _vacancyListViewModel.Vacancies
                                                     where n.LocationId == locationId
                                                     select n).ToList();

                            returnValue = new VacancyListViewModel();
                            returnValue.Vacancies = matchingVacancies;
                            _logger.LogDebug($"Filtered {returnValue.Vacancies.Count} matching vacancies");
                        }
                        else //All Locations
                        {
                            returnValue = _vacancyListViewModel;
                            _logger.LogDebug($"Showing all vacancies {returnValue.Vacancies.Count}");
                        }
                    }
                }
            }
            catch (Exception fEx) when (fEx is FileNotFoundException
                                       || fEx is DirectoryNotFoundException)
            {
                fEx.Data.Add("GetVacancyListViewModel", $"Can't find vacancyFileName : {fileName}");
                throw;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.GetType().Name);

                ex.Data.Add("GetVacancyListViewModel", $"Error either reading vacancy data from vacancyFileName : {fileName} or filter locationId : {locationId}");
                throw;
            }

            ApplyFormatting(returnValue);

            return Task.FromResult(returnValue);
        }

        private void ApplyFormatting(VacancyListViewModel vacancyListViewModel)
        {
            foreach (var vacancy in vacancyListViewModel.Vacancies)
            {
                _propertiesFormatter.Format(vacancy);
            }
        }


    }
}
