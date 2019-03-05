using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AMS.Models.ViewModel
{
    public class RegisterUserViewModel
    {
        [Key]
        public int userid { get; set; }
        
        [Display(Name ="First Name")]
        [DataType(DataType.Text)]
        
        public string firstname { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Second Name")]
        public string secondname { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [Display(Name = "Address")]
        [DataType(DataType.Text)]
        public string address { get; set; }

        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        public string phone { get; set; }

        [Display(Name = "Role")]
        public TypeofUser usertype { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Compare("password")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string confirmpassword { get; set; }
    }

   public enum TypeofUser {
        Admin, 
        User
    }

}