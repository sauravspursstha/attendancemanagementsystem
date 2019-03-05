using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AMS.Models.ViewModel
{
    public class DisplayUserViewModel
    {
        [Key]
        public int userid { get; set; }

        [Display(Name="First Name")]
        public string firstname { get; set; }

        [Display(Name = "Second Name")]
        public string secondname { get; set; }
        [Display(Name = "Email Address")]
        public string email { get; set; }

        [Display(Name = "Address")]
        public string address { get; set; }

        [Display(Name = "Phone")]
        public string phone { get; set; }

        [Display(Name ="Role")]
        public string usertype { get; set; }
    }
}