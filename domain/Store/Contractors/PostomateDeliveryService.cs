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

        public string UniqueCode => "Postomate";

        public string Title => "Nova Poshta";

        public Form CreateForm(Order order)
        {
            if(order == null)
                throw new ArgumentNullException(nameof(order));

            return new Form(UniqueCode, order.Id, 1, false, new[]
            {
                new SelectionField("City", "city", "1", cities),
            });
        }

        public Form MoveNext(int orderId, int step, IReadOnlyDictionary<string, string> values)
        {
            if (step == 1)
            {
                if (values["city"] == "1")
                {
                    return new Form(UniqueCode, orderId, 2, false, new Field[]
                    {
                        new HiddenField("City", "city", "1"),
                        new SelectionField("Postomate", "postomate", "1", postomates["1"]),
                    });
                }
                else if (values["city"] == "2")
                {
                    return new Form(UniqueCode, orderId, 2, false, new Field[]
                    {
                        new HiddenField("City", "city", "2"),
                        new SelectionField("Postomate", "postomate", "4", postomates["2"]),
                    });
                }
                else
                    throw new InvalidOperationException("Invalid postomate city.");
            }
            else if (step == 2)
            {
                return new Form(UniqueCode, orderId, 3, true, new Field[]
                {
                    new HiddenField("City", "city", values["city"]),
                    new HiddenField("Postomate", "postomate", values["postomate"]),
                });
            }
            else
                throw new InvalidOperationException("Invalid postomate step.");
        }
    }    
}
