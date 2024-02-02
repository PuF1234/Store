using Microsoft.AspNetCore.Mvc;

namespace Store.PayPalPayment.Areas.PayPal.Controllers
{
    [Area("PayPal")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // /PayPal/Home/Callback

        public IActionResult Callback()
        {
            return View();
        }
    }
}
