using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public virtual List<OrderedDish> Orders { get; set; }
        public decimal Price { get; set; }
        public bool Realized { get; set; }

    }
}
