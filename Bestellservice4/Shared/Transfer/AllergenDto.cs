
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bestellservice4.Shared.Transfer
{
    public class AllergenDto : Dto
    {
        public int Id { get; set; }
        public char Letter { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DishDto Dish { get; set; }
    }
}
