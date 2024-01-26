using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Store.Presentation.Models;

namespace Store.Presentation.Controllers
{
    public class CartController : Controller
    {
        private readonly IBicycleRepos bicycleRepos;

        private readonly IOrderRepository orderRepository;

        public CartController(IBicycleRepos bicycleRepos, 
                              IOrderRepository orderRepository)
        {
            this.bicycleRepos = bicycleRepos;
            this.orderRepository = orderRepository;
        }

        public IActionResult Add(int id)
        {          

            Order order;
            Cart cart;

            if (HttpContext.Session.TryGetCart(out cart))
            {
                order = orderRepository.GetById(cart.OrderId);
            }

            else
            {
                order = orderRepository.Create();
                cart = new Cart(order.Id);
            }

            var bicycle = bicycleRepos.GetById(id);
            order.AddItem(bicycle, 1);
            orderRepository.Update(order);

            cart.TotalCount = order.TotalCount;
            cart.TotalPrice = order.TotalPrice;
            HttpContext.Session.Set(cart);

            return RedirectToAction("Index", "Bicycle", new {id});
        }
    }
}
