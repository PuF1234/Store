using Microsoft.AspNetCore.Http;
using PhoneNumbers;
using Store.Messages;
using System.Net;
using System.Reflection;

namespace Store.Web.App
{
    public class OrderService
    {
        private readonly IBicycleRepository bicycleRepository;
        private readonly IOrderRepository orderRepository;
        private readonly INotificationService notificationService;
        private readonly IHttpContextAccessor httpContextAccessor;

        protected ISession Session => httpContextAccessor.HttpContext.Session;

        public OrderService(IBicycleRepository bicycleRepository,
                            IOrderRepository orderRepository,
                            INotificationService notificationService,
                            IHttpContextAccessor httpContextAccessor)
        {
            this.bicycleRepository = bicycleRepository;
            this.orderRepository = orderRepository;
            this.notificationService = notificationService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<(bool hasValue, OrderModel model)> TryGetModelAsync()
        {
            var (hasValue, order) = await TryGetOrderAsync();
            if (hasValue)
                return (true, await MapAsync(order));

            return (false, null);
        }

        internal async Task<(bool hasValue, Order order)> TryGetOrderAsync()    
        {
            if (Session.TryGetCart(out Cart cart))
            {
                var order = await orderRepository.GetByIdAsync(cart.OrderId);
                return (true, order);
            }

            return (false, null);
        }

        internal async Task<OrderModel> MapAsync(Order order)
        {
            var Bicycles = await GetBicyclesAsync(order);
            var items = from item in order.Items
                        join Bicycle in Bicycles on item.BicycleId equals Bicycle.ID
                        select new OrderItemModel
                        {
                            BicycleId = Bicycle.ID,
                            Title = Bicycle.Title,
                            Producer = Bicycle.Producer,
                            Price = item.Price,
                            Count = item.Count,
                        };

            return new OrderModel
            {
                Id = order.Id,
                Items = items.ToArray(),
                TotalCount = order.TotalCount,
                TotalPrice = order.TotalPrice,
                CellPhone = order.CellPhone,
                DeliveryDescription = order.Delivery?.Description,
                PaymentDescription = order.Payment?.Description
            };
        }

        internal async Task<IEnumerable<Bicycle>> GetBicyclesAsync(Order order)
        {
            var BicycleIds = order.Items.Select(item => item.BicycleId);

            return await bicycleRepository.GetAllByIdsAsync(BicycleIds);
        }

        public async Task<OrderModel> AddBicycleAsync(int bicycleId, int count)
        {
            if (count < 1)
                throw new InvalidOperationException("Too few Bicycles to add");

            var (hasValue, order) = await TryGetOrderAsync();

            if (!hasValue)
                order = await orderRepository.CreateAsync();

            await AddOrUpdateBicycleAsync(order, bicycleId, count); UpdateSession(order);

            return await MapAsync(order);
        }

        internal async Task AddOrUpdateBicycleAsync(Order order, int bicycleId, int count)
        {
            var bicycle = await bicycleRepository.GetByIdAsync(bicycleId);
            if (order.Items.TryGet(bicycleId, out OrderItem orderItem))
                orderItem.Count += count;
            else
                order.Items.Add(bicycle.ID, bicycle.Price, count);

            await orderRepository.UpdateAsync(order);
        }

        internal void UpdateSession(Order order)
        {
            var cart = new Cart(order.Id, order.TotalCount, order.TotalPrice);
            Session.Set(cart);
        }

        public async Task<OrderModel> UpdateBicycleAsync(int BicycleId, int count)
        {
            var order = await GetOrderAsync();
            order.Items.Get(BicycleId).Count = count;

            await orderRepository.UpdateAsync(order);
            UpdateSession(order);

            return await MapAsync(order);
        }

        public async Task<OrderModel> RemoveBicycleAsync(int BicycleId)
        {
            var order = await GetOrderAsync();
            order.Items.Remove(BicycleId);

            await orderRepository.UpdateAsync(order);
            UpdateSession(order);

            return await MapAsync(order);
        }

        public async Task<Order> GetOrderAsync()
        {
            var (hasValue, order) = await TryGetOrderAsync();

            if (hasValue)
                return order;

            throw new InvalidOperationException("Empty session.");
        }

        public async Task<OrderModel> SendConfirmationAsync(string cellPhone)
        {
            var order = await GetOrderAsync();
            var model = await MapAsync(order);

            if (TryFormatPhone(cellPhone, out string formattedPhone))
            {
                var confirmationCode = 1111; // todo: random.Next(1000, 10000) = 1000, 1001, ..., 9998, 9999
                model.CellPhone = formattedPhone;
                Session.SetInt32(formattedPhone, confirmationCode);
                await notificationService.SendConfirmationCodeAsync(formattedPhone, confirmationCode);
            }
            else
                model.Errors["cellPhone"] = "Номер телефона не соответствует формату +79876543210";

            return model;
        }

        private readonly PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();

        internal bool TryFormatPhone(string cellPhone, out string formattedPhone)
        {
            try
            {
                var phoneNumber = phoneNumberUtil.Parse(cellPhone, "ru");
                formattedPhone = phoneNumberUtil.Format(phoneNumber, PhoneNumberFormat.INTERNATIONAL);
                return true;
            }
            catch (NumberParseException)
            {
                formattedPhone = null;
                return false;
            }
        }

        public async Task<OrderModel> ConfirmCellPhoneAsync(string cellPhone, int confirmationCode)
        {
            int? storedCode = Session.GetInt32(cellPhone);
            var model = new OrderModel();

            if (storedCode == null)
            {
                model.Errors["cellPhone"] = "Something goes wrong. Try again.";
                return model;
            }

            if (storedCode != confirmationCode)
            {
                model.Errors["confirmationCode"] = "Wrong code. Check it and try again.";
                return model;
            }

            var order = await GetOrderAsync();
            order.CellPhone = cellPhone;
            await orderRepository.UpdateAsync(order);

            Session.Remove(cellPhone);

            return await MapAsync(order);
        }

        public async Task<OrderModel> SetDeliveryAsync(OrderDelivery delivery)
        {
            var order = await GetOrderAsync();
            order.Delivery = delivery;
            await orderRepository.UpdateAsync(order);

            return await MapAsync(order);
        }

        public async Task<OrderModel> SetPaymentAsync(OrderPayment payment)
        {
            var order = await GetOrderAsync();
            order.Payment = payment;
            await orderRepository.UpdateAsync(order);
            Session.RemoveCart();

            await notificationService.StartProcessAsync(order);

            return await MapAsync(order);
        }
    }
}