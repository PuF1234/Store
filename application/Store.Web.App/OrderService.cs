using Microsoft.AspNetCore.Http;
using PhoneNumbers;
using Store.Messages;

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

        public bool TryGetModel(out OrderModel model)
        {
            if (TryGetOrder(out Order order))
            {
                model = Map(order);
                return true;
            }

            model = null;
            return false;
        }

        internal bool TryGetOrder(out Order order)
        {
            if (Session.TryGetCart(out Cart cart))
            {
                order = orderRepository.GetById(cart.OrderId);
                return true;
            }

            order = null;
            return false;
        }

        internal OrderModel Map(Order order)
        {
            var Bicycles = GetBicycles(order);
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

        internal IEnumerable<Bicycle> GetBicycles(Order order)
        {
            var BicycleIds = order.Items.Select(item => item.BicycleId);

            return bicycleRepository.GetAllByIds(BicycleIds);
        }

        public OrderModel AddBicycle(int BicycleId, int count)
        {
            if (count < 1)
                throw new InvalidOperationException("Too few Bicycles to add");

            if (!TryGetOrder(out Order order))
                order = orderRepository.Create();

            AddOrUpdateBicycle(order, BicycleId, count);
            UpdateSession(order);

            return Map(order);
        }

        internal void AddOrUpdateBicycle(Order order, int BicycleId, int count)
        {
            var Bicycle = bicycleRepository.GetByIds(BicycleId);
            if (order.Items.TryGet(BicycleId, out OrderItem orderItem))
                orderItem.Count += count;
            else
                order.Items.Add(Bicycle.ID, Bicycle.Price, count);

            orderRepository.Update(order);
        }

        internal void UpdateSession(Order order)
        {
            var cart = new Cart(order.Id, order.TotalCount, order.TotalPrice);
            Session.Set(cart);
        }

        public OrderModel UpdateBicycle(int BicycleId, int count)
        {
            var order = GetOrder();
            order.Items.Get(BicycleId).Count = count;

            orderRepository.Update(order);
            UpdateSession(order);

            return Map(order);
        }

        public OrderModel RemoveBicycle(int BicycleId)
        {
            var order = GetOrder();
            order.Items.Remove(BicycleId);

            orderRepository.Update(order);
            UpdateSession(order);

            return Map(order);
        }

        public Order GetOrder()
        {
            if (TryGetOrder(out Order order))
                return order;

            throw new InvalidOperationException("Empty session.");
        }

        public OrderModel SendConfirmation(string cellPhone)
        {
            var order = GetOrder();
            var model = Map(order);

            if (TryFormatPhone(cellPhone, out string formattedPhone))
            {
                var confirmationCode = 1111; // todo: random.Next(1000, 10000) = 1000, 1001, ..., 9998, 9999
                model.CellPhone = formattedPhone;
                Session.SetInt32(formattedPhone, confirmationCode);
                notificationService.SendConfirmationCode(formattedPhone, confirmationCode);
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

        public OrderModel ConfirmCellPhone(string cellPhone, int confirmationCode)
        {
            int? storedCode = Session.GetInt32(cellPhone);
            var model = new OrderModel();

            if (storedCode == null)
            {
                model.Errors["cellPhone"] = "Что-то случилось. Попробуйте получить код ещё раз.";
                return model;
            }

            if (storedCode != confirmationCode)
            {
                model.Errors["confirmationCode"] = "Неверный код. Проверьте и попробуйте ещё раз.";
                return model;
            }

            var order = GetOrder();
            order.CellPhone = cellPhone;
            orderRepository.Update(order);

            Session.Remove(cellPhone);

            return Map(order);
        }

        public OrderModel SetDelivery(OrderDelivery delivery)
        {
            var order = GetOrder();
            order.Delivery = delivery;
            orderRepository.Update(order);

            return Map(order);
        }

        public OrderModel SetPayment(OrderPayment payment)
        {
            var order = GetOrder();
            order.Payment = payment;
            orderRepository.Update(order);
            Session.RemoveCart();

            notificationService.StartProcess(order);

            return Map(order);
        }
    }
}