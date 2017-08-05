using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreCore.ViewModel
{
    public class AddressAndPaymentViewModel
    {
        [Display(Name ="First Name")]
        [Required]
        public string FirstName { set; get; }
        [Display(Name ="Last Name")]
        [Required]
        public string LastName { set; get; }
        [Required]
        public string Address { set; get; }
        [Required]
        public string City { set; get; }
        [Required]
        public string State { set; get; }
        [Display(Name = "Postal Code")]
        [Required]
        public string PostalCode { set; get; }
        [Required]
        public string Country { set; get; }
        [Required]
        public string Phone { set; get; }
        [Display(Name ="Email Address")]
        [Required]
        public string EmailAddress { set; get; }
        [ScaffoldColumn(false)]
        [Display(Name ="Promo Code")]
        [Required]
        public string PromoCode { set; get; }
    }
}
