using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
                //Get all vancancies to filter the locations
                var repoTask = _vancancies.GetVacancyListViewModel();
                repoTask.Wait();
                var allVacancies = repoTask.Result;
                var allLocationsWithVacancies = (from n in allVacancies.Vacancies
                                                 select n.LocationId).Distinct().ToList();
                var task = CacheLocationListViewModel(allLocationsWithVacancies);
                task.Wait();


                //Now filter locations based on prevous selections or not
                var filteredVacancies = allVacancies;
                if (HttpContext.Session.GetInt32("LocationId").HasValue)
                {
                    //filter based on prevous selection, user must be coming back from another page    
                    var matchingVacancies = (from n in allVacancies.Vacancies
                                             where n.LocationId== HttpContext.Session.GetInt32("LocationId").Value
                                             select n).ToList();

                    //Must copy as result is persisted in singleton so has to hold all vacancies
                    filteredVacancies = new VacancyListViewModel();
                    filteredVacancies.Vacancies = matchingVacancies;
                }


                return View(filteredVacancies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unknown exception trying to create Careers view ");
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        /// <summary>
        /// Returns PartialView for filtered vacancies
        /// </summary>
        /// <param name="locationId">Filter Id</param>
        /// <returns></returns>
        public IActionResult ApplyFilter(int locationId)
        {
            HttpContext.Session.SetInt32("LocationId", locationId);
            var repoTask = _vancancies.GetVacancyListViewModel(locationId);
            repoTask.Wait();
            var data = repoTask.Result;
            return PartialView("_Careers", data);
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