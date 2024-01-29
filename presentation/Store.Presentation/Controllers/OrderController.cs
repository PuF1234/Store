using Microsoft.AspNetCore.Mvc;
using Store.Presentation.Models;

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

        public IActionResult AddItem(int bicycleId, int count = 1)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();

            var bicycle = bicycleRepos.GetByIds(bicycleId);

            order.AddOrUpdateItem(bicycle, count);

            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Bicycle", new { id = bicycleId });


        }

        [HttpPost]
        public IActionResult UpdateItem(int bicycleId, int count)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();

            order.GetItem(bicycleId).Count = count;

            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Order");
        }

        [HttpPost]
        private void SaveOrderAndCart(Order order, Cart cart)
        {
            orderRepository.Update(order);
            cart.TotalCount = order.TotalCount;
            cart.TotalPrice = order.TotalPrice;
            HttpContext.Session.Set(cart);
        }

        private (Order order, Cart cart) GetOrCreateOrderAndCart()
        {
            Order order;

            if (HttpContext.Session.TryGetCart(out Cart cart))
            {
                order = orderRepository.GetById(cart.OrderId);
            }

            else
            {
                order = orderRepository.Create();
                cart = new Cart(order.Id);
            }
            return (order, cart);
        }       

        public IActionResult RemoveItem(int bicycleId)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();

            order.RemoveItem(bicycleId);

            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Order");
        }
    } 
}
