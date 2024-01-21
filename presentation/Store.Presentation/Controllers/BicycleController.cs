using Microsoft.AspNetCore.Mvc;

namespace Store.Presentation.Controllers
{
    public class BicycleController : Controller
    {
        private readonly IBicycleRepos bicycleRepos;

        public BicycleController(IBicycleRepos bicycleRepos)
        {
            this.bicycleRepos = bicycleRepos;
        }
        public IActionResult Index(int id)
        {
            Bicycle bicycle = bicycleRepos.GetById(id);

            return View(bicycle);
        }
    }
}
