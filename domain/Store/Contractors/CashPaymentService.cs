using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Contractors
{
    public class CashPaymentService : IPaymentService
    {
        public string Name => "Cash";

        public string Title => "Pay with cash";

        public Form FirstForm(Order order)
        {
            return Form.CreateFirst(Name)
                       .AddParameter("orderId", order.Id.ToString());
        }


        public OrderPayment GetPayment(Form form)
        {
            if (form.UniqueCode != Name || !form.IsFinal)
                throw new InvalidOperationException("Invalid payment form");

            return new OrderPayment(Name, "Payment with cash", new Dictionary<string, string>());
        }

        public Form NextForm(int orderId, int step, IReadOnlyDictionary<string, string> values)
        {
            if (step != 1)
                throw new InvalidOperationException("Invalid cash payment step.");

            return Form.CreateLast(Name, step + 1, values);
        }
    }
}
