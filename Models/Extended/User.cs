using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookManager.Models
{
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
        public string ConfirmPassword { get; set; }
    }

    public class UserMetadata
    {
        [Display(Name ="First Name")]
        [Required(AllowEmptyStrings =false, ErrorMessage ="First name is required!")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name is required!")]
        public string LastName { get; set; }
        [Display(Name = "Email Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email Address is required!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required!")]
        [DataType(DataType.Password)]
        [MinLength(6,ErrorMessage ="Minimum 6 characters needed!")]
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Confirm Password is required!")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Confirm password and Password doesn't match! Try again...")]
        public string ConfirmPassword { get; set; }
    }
}