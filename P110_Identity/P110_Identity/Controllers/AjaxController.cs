using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using P110_Identity.DAL;

namespace P110_Identity.Controllers
{
    public class AjaxController : Controller
    {
        private readonly AppDbContext _context;

        public AjaxController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult LoadCitiesByCountryId(int countryId)
        {
            return PartialView("_SelectCitiesPartialView", _context.Cities.Where(c => c.CountryId == countryId));
        }
    }
}