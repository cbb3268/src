using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Nib.Exercise.Interfaces;
using Nib.Exercise.Models;

namespace Nib.Exercise.Controllers
{

    public class CareersController : Controller
    {
        #region MEMBERS

        private readonly ILogger<CareersController> _logger;

        private readonly ILocations _locations;

        private readonly IVancancies _vancancies;

        #endregion

        #region CONSTRUCTORS

        public CareersController(ILogger<CareersController> logger
            , ILocations locations
            , IVancancies vancancies)
        {
            _logger = logger;
            _locations = locations;
            _vancancies = vancancies;
        }

        #endregion


        public IActionResult Careers()
        {
            try
            {
                var repoTask = _vancancies.GetVacancyListViewModel(0);
                repoTask.Wait();
                var allVacancies = repoTask.Result;
                var allLocationsWithVacancies = (from n in allVacancies.Vacancies
                                                 select n.LocationId).Distinct().ToList();

                var task = CacheLocationListViewModel(allLocationsWithVacancies);
                task.Wait();

                return View(allVacancies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unknown exception trying to create Careers view ");
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region HELPERS

        /// <summary>
        /// Adds the location list data to the ViewData bag
        /// </summary>
        /// <returns>The locations</returns>
        private async Task CacheLocationListViewModel(List<int> locationIdFilterList)
        {
            var locations = await _locations.GetLocationListViewModel();
            var locationList = (from n in locations
                                where locationIdFilterList.Contains(n.Id)
                                select new SelectListItem
                                {
                                    Text = n.Name,
                                    Value = n.Id.ToString()
                                }).ToList();

            _logger.LogDebug($"locations retrieved {locationList.Count}");
            ViewData["LocationList"] = locationList;
        }

        #endregion
    }
}