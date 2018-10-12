using System.Collections.Generic;

namespace Models
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public virtual List<Dish> Dishes { get; set; }
    }
}