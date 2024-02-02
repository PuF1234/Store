using Store.Contractors;
using Store.Web.Contractors;

namespace Store.PayPalPayment
{
    public class PayPalPaymentService : IPaymentService, IWebContractorService
    {
        public string UniqueCode => "PayPal";

        public string Title => "Pay by credit card";

        public string GetUri => "/PayPal/";

        public Form CreateForm(Order order)
        {
            return new Form(UniqueCode, order.Id, 1, false, new Field[0]) ;
        }

        public OrderPayment GetPayment(Form form)
        {
            return new OrderPayment(UniqueCode, "Pay by credit card", new Dictionary<string, string>());
        }

        public Form MoveNextForm(int orderId, int step, IReadOnlyDictionary<string, string> values)
        {
            return new Form(UniqueCode, orderId, 2, true, new Field[0]);
        }
    }
}
