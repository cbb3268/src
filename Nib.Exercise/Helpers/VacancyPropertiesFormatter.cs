using Microsoft.Extensions.Logging;
using Nib.Exercise.Configurations;
using Nib.Exercise.Interfaces;
using Nib.Exercise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nib.Exercise.Helpers
{
    public class VacancyPropertiesFormatter : IPropertiesFormatter<Vacancy>
    {
        #region MEMBERS

        private ConfigurationExercise _configurationExercise;

        private ILogger<VacancyPropertiesFormatter> _logger;

        #endregion

        #region CONSTRUCTORS

        public VacancyPropertiesFormatter(ILogger<VacancyPropertiesFormatter> logger
                ,ConfigurationExercise configurationExercise)
        {
            _configurationExercise = configurationExercise;
            _logger = logger;
        }

        #endregion

        public void Format(Vacancy resource)
        {
            //Would usually unit test this but just an exercise already taking longer than expected

            if (resource.Description?.Length > _configurationExercise.MaxVacancyDescriptionLength-1)
            {
                _logger.LogDebug($"Triming Description for ShortDescription property as is longer than {_configurationExercise.MaxVacancyDescriptionLength}");
                resource.ShortDescription = resource.Description.Substring(0, _configurationExercise.MaxVacancyDescriptionLength - 3) + "...";                
            }
            else
            {
                resource.ShortDescription = resource.Description;
            }
        }
    }
}
