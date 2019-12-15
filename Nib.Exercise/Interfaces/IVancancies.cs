using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nib.Exercise.Models;

namespace Nib.Exercise.Interfaces
{
    public interface IVancancies
    {
        Task<VacancyListViewModel> GetVacancyListViewModel(int locationId);
    }
}
