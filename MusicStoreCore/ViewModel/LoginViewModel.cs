using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreCore.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { set; get; }
        [Required, DataType(DataType.Password)]
        public string Password { set; get; }

        [Display(Name ="Remember Me")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }//grab the query string from url
    }
}
