using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace BookManager.Models
{
    public class ResetPasswordModel
    {
        [Display(Name = "New Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "New Password is required!")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum 6 characters needed!")]
        public string NewPassword { get; set; }
        [Display(Name = "Confirm Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Confirm Password is required!")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Confirm password and New Password doesn't match! Try again...")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string ResetCode { get; set; }

    }
}