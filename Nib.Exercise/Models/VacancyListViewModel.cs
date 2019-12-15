using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nib.Exercise.Models
{
    /// <summary>
    /// A collection of vacancies
    /// </summary>
    public class VacancyListViewModel
    {
        public List<Vacancy> Vacancies { get; set; } = new List<Vacancy>();
    }
}
