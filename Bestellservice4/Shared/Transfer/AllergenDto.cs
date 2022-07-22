
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bestellservice4.Shared.Transfer
{
    public class AllergenDto : Dto
    {
        public int Id { get; set; }

        [Required]
        public char Letter { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(1000)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        public int DishId { get; set; }
    }
}
