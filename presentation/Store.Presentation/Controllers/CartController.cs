using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Store.Presentation.Models;

namespace Store.Presentation.Controllers
{
    public class CartController : Controller
    {
        private readonly IBicycleRepos bicycleRepos;

        public CartController(IBicycleRepos bicycleRepos)
        {
            this.bicycleRepos = bicycleRepos;
        }

        public IActionResult Add(int id)
        {
            var bicycle = bicycleRepos.GetById(id);
            Cart cart;

            if (!HttpContext.Session.TryGetCart(out cart))
                cart = new Cart();
            
            if (cart.Items.ContainsKey(id))           
                cart.Items[id]++;
              
            else            
                cart.Items[id] = 1;

            cart.Amount += bicycle.Price;

            HttpContext.Session.Set(cart);

            return RedirectToAction("Index", "Bicycle", new {id});
        }
    }
}
