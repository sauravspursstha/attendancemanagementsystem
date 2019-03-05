using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AMS.Models.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string password { get; set; }


    }
}