using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage ="Please Enter A Name")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Please Enter The First Address Line")]
        [Display(Name ="Line 1")]
        public string Line1 { get; set; }
        [Display(Name = "Line 2")]
        public string Line2 { get; set; }
       
        [Required(ErrorMessage ="Please Enter City Name")]
        public string City { get; set; }

        [Required(ErrorMessage ="Please Enter State Name")]
        public string State { get; set; }

        [Required(ErrorMessage ="Please Enter Country Name")]
        public string Country { get; set; }

        public bool IsGift { get; set; }

    }
}
