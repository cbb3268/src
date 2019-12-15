using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nib.Exercise.Configurations
{
    public class ConfigurationExercise
    {
        /// <summary>
        /// Controls how many chars are displayed for the vacancy desciprtion
        /// </summary>
        public int MaxVacancyDescriptionLength { get; set; }

        /// <summary>
        /// The path the Locations Api
        /// </summary>
        public string LocationsApiUrl { get; set; }
    }
}
