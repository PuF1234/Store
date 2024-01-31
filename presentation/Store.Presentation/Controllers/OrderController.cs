using Microsoft.AspNetCore.Mvc;
using Store.Messages;
using Store.Presentation.Models;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Store.Presentation.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBicycleRepos bicycleRepos;

        private readonly IOrderRepository orderRepository;

        private readonly INotificationService notificationService;

        public OrderController(IBicycleRepos bicycleRepos, 
                              IOrderRepository orderRepository,
                              INotificationService notificationService)
        {
            this.bicycleRepos = bicycleRepos;
            this.orderRepository = orderRepository;
            this.notificationService = notificationService;
        }

        [HttpGet]
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

    
        [HttpPost]             
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

        [HttpPost]
        public IActionResult RemoveItem(int bicycleId)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();

            order.RemoveItem(bicycleId);

            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Order");
        }

        /*[HttpPost]
        public IActionResult ClearCart()
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();            

            return RedirectToAction("Index", "Home");
        }*/

        [HttpPost]
        public IActionResult SendConfirmationCode(int id,  string cellPhone)
        {
            var order = orderRepository.GetById(id);
            var model = Map(order); 

            if(!IsValidCellphone(cellPhone))
            {
                model.Errors["cellPhone"] = "Your number does not match proper format +12345678901";
                return View("Index", model);
            }

            int code = 1111;

            HttpContext.Session.SetInt32(cellPhone, code);
            notificationService.SendConfirmationCode(cellPhone, code);

            return View("Confirmation",
                        new ConfirmationModel
                        {
                            OrderId = id,
                            Cellphone = cellPhone,

                        });

        }

        private bool IsValidCellphone(string cellPhone)
        {
            if(cellPhone == null)
                return false;

            cellPhone = cellPhone.Replace(" ", "")
                                 .Replace("-", "");

            return Regex.IsMatch(cellPhone, @"^\+?\d{11}$");
        }

        [HttpPost]
        public IActionResult StartDelivery(int id, string cellPhone, int code)
        {
            int? storedCode = HttpContext.Session.GetInt32(cellPhone);
            if (storedCode == null)
            {
                return View("Confirmation",
                            new ConfirmationModel
                            {
                                OrderId = id,
                                Cellphone = cellPhone,
                                Errors = new Dictionary<string, string>
                                {
                                    {"code", "Empty code, try again to send" }
                                },
                            }); ;
            }

            if (storedCode != code)
            {
                return View("Confirmation",
                            new ConfirmationModel
                            {
                                OrderId = id,
                                Cellphone = cellPhone,
                                Errors = new Dictionary<string, string>
                                {
                                    {"code", "Is diffrent than sended" }
                                },
                            }); ;
            }

            return View();
        }
    } 
}
