using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Nib.Exercise.Controllers
{
    public class VacancyController : Controller
    {
        public IActionResult Vacancy(string vacancyDescription)
        {
            ViewData["VacancyDescription"] = vacancyDescription;
            return View();
        }
    }
}
