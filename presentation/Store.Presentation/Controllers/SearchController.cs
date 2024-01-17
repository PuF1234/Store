using Microsoft.AspNetCore.Mvc;

namespace Store.Presentation.Controllers
{
    public class SearchController : Controller
    {
        private readonly IBicycleRepos bicycleRepos;
        public SearchController(IBicycleRepos bicycleRepos)
        {
            this.bicycleRepos = bicycleRepos;
        }

        public IActionResult Index(string query)
        {
            var bicycles = bicycleRepos.GetAllByTitle(query);

            return View(bicycles);
        }
    }
}
