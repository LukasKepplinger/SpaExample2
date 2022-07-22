using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bestellservice4.Shared.Transfer
{
    public class DishCeDto : Dto
    {
        [Required(ErrorMessage = "Sie müssen den Namen angeben")]
        [MinLength(5, ErrorMessage = "Name ist zu kurz")]
        [MaxLength(50, ErrorMessage = "Name ist zu lang")]
        [DataType(DataType.Text)]
        [DisplayName("Name")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Sie müssen eine Beschreibung angeben")]
        [MinLength(10, ErrorMessage = "Beschreibung ist zu kurz")]
        [MaxLength(1000, ErrorMessage = "Name ist zu lang")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Beschreibung")]
        public string Description { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayName("Erstelldatum")]
        public DateTime Created { get; set; }

        [Required(ErrorMessage = "Sie müssen den Preis angeben")]
        [Range(1, 100, ErrorMessage = "Preis ist zu hoch/gerning")]
        [DataType(DataType.Currency)]
        [DisplayName("Preis")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Sie müssen ein Bild zuteilen")]
        [DataType(DataType.Upload)]
        [DisplayName("Bild")]
        public byte[] ImageData { get; set; }
    }
}

