using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bestellservice4.Services.Models
{
    public class Allergen : Entity
    {
        public int Id { get; set; }
        public char Letter { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int DishId { get; set; }
        public Dish Dish { get; set; }
    }
}
