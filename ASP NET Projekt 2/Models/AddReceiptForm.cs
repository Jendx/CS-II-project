using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_NET_Projekt_2.Models
{
    public class AddReceiptForm
    {
        [Required]
        [Display(Name = "Product code")]
        public string PC { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Price")]
        public string Price { get; set; }

        [Required]
        [Display(Name = "Weight")]
        public string Weight { get; set; }

        [Required]
        [Display(Name = "Count")]
        public string Count { get; set; }



        //"id": 245,
        //"Nazev": "Donut",
        //"Cena": 15,
        //"vaha": 0,
        //"pocet": 1
    }
}
