using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Contractors
{
    public class PostomateDeliveryService : IDeliveryService
    {

        private static IReadOnlyDictionary<string, string> cities = new Dictionary<string, string>
        {
            {"1", "Kyiv" },
            {"2", "Ivano-Frankivsk" },
        };

        private static IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> postomates = new Dictionary<string, IReadOnlyDictionary<string, string>>
        {
            {
                "1",
                new Dictionary<string, string>
                {
                    {"1", "Zaliznychny vokzal" },
                    {"2", "Avto-stancija" },
                    {"3", "Misky park" },
                }

            },
            {
                "2",
                new Dictionary<string, string>
                {
                    {"4", "Miske ozero" },
                    {"5", "Avto-stancija №2" },
                    {"6", "Ratusha" },
                }

            },
        };

        public string Name => "Postomate";

        public string Title => "Nova Poshta";

        public Form FirstForm(Order order)
        {
            return Form.CreateFirst(Name)
                       .AddParameter("orderId", order.Id.ToString())
                       .AddField(new SelectionField("City", "city", "1", cities));
        }        

        public Form NextForm(int step, IReadOnlyDictionary<string, string> values)
        {
            if (step == 1)
            {
                if (values["city"] == "1")
                {
                    return Form.CreateNext(Name, 2, values)
                               .AddField(new SelectionField("Postomate", "postomate", "1", postomates["1"]));
                }
                else if (values["city"] == "2")
                {
                    return Form.CreateNext(Name, 2, values)
                               .AddField(new SelectionField("Posomate", "postomate", "4", postomates["2"]));
                }
                else
                    throw new InvalidOperationException("Invalid postomate city.");
            }
            else if (step == 2)
            {
                return Form.CreateLast(Name, 3, values);
            }
            else
                throw new InvalidOperationException("Invalid postomate step.");
        }

        public OrderDelivery GetDelivery(Form form)
        {
            if (form.ServiceName != Name || !form.IsFinal)
                throw new InvalidOperationException("Invalid form");

            var cityId = form.Parameters["city"];
            var cityName = cities[cityId];
            var postomateId = form.Parameters["postomate"];
            var postomateName = postomates[cityId][postomateId];

            var parameters = new Dictionary<string, string>
            {
                {nameof(cityId), cityId },
                {nameof(cityName), cityName },
                {nameof(postomateId), postomateId },
                {nameof(postomateName), postomateName },
            };

            var description = $"City: {cityName}\nPostomate: {postomateName}";

            return new OrderDelivery(Name, description, 10m, parameters);   
        }       
    }    
}
