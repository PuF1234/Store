using Microsoft.AspNetCore.Mvc;
using Store.Web.App;

namespace Store.Presentation.Controllers
{
    public class BicycleController : Controller
    {
        private readonly BicycleService bicycleService;

        public BicycleController(BicycleService bicycleService)
        {
            this.bicycleService = bicycleService;
        }
        public IActionResult Index(int id)
        {
            var model = bicycleService.GetById(id);

            return View(model);
        }
    }
}
