using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class OrderedDish
    {
        public int Id { get; set; }
        public virtual Dish Dish { get; set; }
        public int Quantity { get; set; }
    }
}
