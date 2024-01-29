using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Presentation.Models;
using System.Linq;

namespace Store.Presentation.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBicycleRepos bicycleRepos;

        private readonly IOrderRepository orderRepository;

        public OrderController(IBicycleRepos bicycleRepos, 
                              IOrderRepository orderRepository)
        {
            this.bicycleRepos = bicycleRepos;
            this.orderRepository = orderRepository;
        }

        public IActionResult Index()
        {
            
            if (HttpContext.Session.TryGetCart(out Cart cart))
            {
                var order = orderRepository.GetById(cart.OrderId);
                OrderModel model = Map(order);

                return View(model);
            }

            return View("Empty");
        }

        private OrderModel Map(Order order)
        {
            var bicycleId = order.Items.Select(item => item.BicycleId);
            var bicycles = bicycleRepos.GetAllByIds(bicycleId);
            var itemModels = from item in order.Items
                             join bicycle in bicycles on item.BicycleId equals bicycle.ID
                             select new OrderItemModel
                             {
                                 BicycleId = bicycle.ID,
                                 Title = bicycle.Title,
                                 Producer = bicycle.Producer,
                                 Price = item.Price,
                                 Count = item.Count,
                             };

            return new OrderModel
            {
                Id = order.Id,
                Items = itemModels.ToArray(),
                TotalCount = order.TotalCount,
                TotalPrice = order.TotalPrice,

            };
        }

        public IActionResult AddItem(int id)
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

            var bicycle = bicycleRepos.GetByIds(id);
            order.AddItem(bicycle, 1);
            orderRepository.Update(order);

            cart.TotalCount = order.TotalCount;
            cart.TotalPrice = order.TotalPrice;
            HttpContext.Session.Set(cart);

            return RedirectToAction("Index", "Bicycle", new {id});
        }
    }
}
