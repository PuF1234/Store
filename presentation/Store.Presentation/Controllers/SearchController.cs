﻿using Microsoft.AspNetCore.Mvc;
using Store.Web.App;

namespace Store.Presentation.Controllers
{
    public class SearchController : Controller
    {
        private readonly BicycleService bicycleService;
        public SearchController(BicycleService bicycleService)
        {
            this.bicycleService = bicycleService;
        }

        public IActionResult Index(string query)
        {
            var bicycles = bicycleService.GetAllByQuery(query);

            return View("Index",bicycles);
        }
    }
}
