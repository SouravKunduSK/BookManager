using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace BookManager.Models
{
    public class UserLogin
    {
        [Display(Name = "Email Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email Address is required!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name ="Remember Me")]
        public bool RememberMe { get; set; }
    }
}