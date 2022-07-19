using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bestellservice4.Services.Models
{
    public class Dish : Entity
    {
        public Dish()
        {
            Allergens = new HashSet<Allergen>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public double Price { get; set; }
        public byte[] ImageData { get; set; }
        public ICollection<Allergen> Allergens { get; set; }
    }
}
