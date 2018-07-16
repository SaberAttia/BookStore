using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Entities
{
    public class ShippingDetails  //134
    {
        [Required(ErrorMessage ="Please enter a name")]
        public string Name { set; get; }
        [Required(ErrorMessage = "Please enter the first address line")]
        [Display(Name="Address Line 1")]
        public string Line1 { set; get; }
        [Display(Name = "Address Line 2")]
        public string Line2 { set; get; }
        [Required(ErrorMessage = "Please enter the city name")]
        public string City { set; get; }
        public string State { set; get; }
        [Required(ErrorMessage = "Please enter the country name")]
        public string Country { set; get; }
        public bool GiftWrap { set; get; }
    }
}
