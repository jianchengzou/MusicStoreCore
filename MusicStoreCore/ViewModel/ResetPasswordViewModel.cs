using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreCore.ViewModel
{
    public class ResetPasswordViewModel
    {
        [Required, StringLength(36)]
        public string Id { get; set; }
        [Required]
        public string ResetToken { get; set; }
        [Required, MaxLength(256)]
        public string UserName { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { set; get; }
        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
