using Microsoft.AspNetCore.Mvc;
using Store.Contractors;
using Store.Messages;
using Store.Presentation.Models;
using Store.Web.App;
using Store.Web.Contractors;
using System.Net;
using System.Text.RegularExpressions;

namespace Store.Presentation.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderService orderService;
        private readonly IEnumerable<IDeliveryService> deliveryServices;
        private readonly IEnumerable<IWebContractorService> webContractorServices;
        private readonly IEnumerable<IPaymentService> paymentServices;

        public OrderController(OrderService orderService,
                              IEnumerable<IDeliveryService> deliveryServices,    
                              IEnumerable<IPaymentService> paymentServices,
                              IEnumerable<IWebContractorService> webContractorServices)
        {
            this.orderService = orderService;
            this.deliveryServices = deliveryServices;
            this.paymentServices = paymentServices;
            this.webContractorServices = webContractorServices;
        }

        [HttpGet]
        public IActionResult Index()
        {
            
            if (orderService.TryGetModel(out OrderModel model))                        
                return View(model);            

            return View("Empty");
        }
        

        [HttpPost]
        public IActionResult AddItem(int bicycleId, int count = 1)
        {
            orderService.AddBicycle(bicycleId, count);

            return RedirectToAction("Index", "Bicycle", new { id = bicycleId });


        }

        [HttpPost]
        public IActionResult UpdateItem(int bicycleId, int count)
        {
            var model = orderService.UpdateBicycle(bicycleId, count);

            return View("Index", model);
        }
        
        [HttpPost]
        public IActionResult RemoveItem(int bicycleId)
        {
            var model = orderService.RemoveBicycle(bicycleId);

            return View("Index", model);
        }        

        [HttpPost]
        public IActionResult SendConfirmation(int id,  string cellPhone)
        {
            var model = orderService.SendConfirmation(cellPhone);

            return View("Confirmation", model);

        }

        [HttpPost]
        public IActionResult ConfirmCellPhone(string cellPhone, int confirmationCode)
        {
            var model = orderService.ConfirmCellPhone(cellPhone, confirmationCode);

            if (model.Errors.Count > 0)
                return View("Confirmation", model);

            var deliveryMethods = deliveryServices.ToDictionary(service => service.Name,
                                                                           service => service.Title);

            return View("DeliveryMethod", deliveryMethods);
        }        

        [HttpPost]
        public IActionResult StartDelivery(string serviceName)
        {
            var deliveryService = deliveryServices.Single(service => service.Name == serviceName);
            var order = orderService.GetOrder();
            var form = deliveryService.FirstForm(order);

            var webContractorService = webContractorServices.SingleOrDefault(service => service.Name == serviceName);

            if (webContractorService == null)
                return View("DeliveryStep", form);

            var returnUri = GetReturnUri(nameof(NextDelivery));
            var redirectUri = webContractorService.StartSession(form.Parameters, returnUri);

            return Redirect(redirectUri.ToString());
        }

        private Uri GetReturnUri(string action)
        {
            var builder = new UriBuilder(Request.Scheme, Request.Host.Host)
            {
                Path = Url.Action(action),
                Query = null,
            };

            if(Request.Host.Port != null)
                builder.Port = Request.Host.Port.Value;

            return builder.Uri;
        }

        [HttpPost]
        public IActionResult NextDelivery(string serviceName, int step, Dictionary<string, string> values) 
        {
            var deliveryService = deliveryServices.Single(service => service.Name == serviceName);

            var form = deliveryService.NextForm(step, values);

            if (!form.IsFinal)            
                return View("DeliveryStep", form);

            var delivery = deliveryService.GetDelivery(form);
            orderService.SetDelivery(delivery);

            var paymentMethods = paymentServices.ToDictionary(service => service.Name,
                                                              service => service.Title);

            return View("PaymentMethod", paymentMethods);
        }

        [HttpPost]
        public IActionResult StartPayment(string serviceName)
        {
            var paymentService = paymentServices.Single(service => service.Name == serviceName);
            var order = orderService.GetOrder();
            var form = paymentService.FirstForm(order);

            var webContractorService = webContractorServices.SingleOrDefault(service => service.Name == serviceName);

            if(webContractorService == null)            
                return View("PaymentStep", form);

            var returnUri = GetReturnUri(nameof(NextPayment));
            var redirectUri = webContractorService.StartSession(form.Parameters, returnUri);

            return Redirect(redirectUri.ToString());
        }

        [HttpPost]
        public IActionResult NextPayment(string serviceName, int step, Dictionary<string, string> values)
        {
            var paymentService = paymentServices.Single(service => service.Name == serviceName);

            var form = paymentService.NextForm(step, values);

            if (!form.IsFinal)
                return View("PaymentStep", form);

            var payment = paymentService.GetPayment(form);
            var model = orderService.SetPayment(payment);

            return View("Finish", model);                          
        }
    } 
}
