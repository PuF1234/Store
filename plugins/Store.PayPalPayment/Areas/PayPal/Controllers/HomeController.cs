using Microsoft.AspNetCore.Mvc;
using Store.PayPalPayment.Areas.PayPal.Models;

namespace Store.PayPalPayment.Areas.PayPal.Controllers
{
    [Area("PayPal")]
    public class HomeController : Controller
    {
        public IActionResult Index(int orderId, string returnUni)
        {
            var model = new ExampleModel
            {
                OrderId = orderId,
                ReturnUri = returnUni
            };

            return View(model);
        }       

        public IActionResult Callback(int orderId, string returnUni)
        {
            var model = new ExampleModel
            {
                OrderId = orderId,
                ReturnUri = returnUni,
            };

            return View(model);
        }
    }
}
