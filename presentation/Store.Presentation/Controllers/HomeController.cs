using Microsoft.AspNetCore.Mvc;
using Store.Presentation.Models;
using System.Diagnostics;

namespace Store.Presentation.Controllers
{
    public class HomeController : Controller
    {


        public IActionResult Index()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
