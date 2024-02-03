using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data
{
    public class BicycleDto
    {
        public int ID { get; set; }

        public string Title { get; set;  }

        public string Serial_number { get; set; }

        public string Producer { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}
