using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AMS.Models
{
    public class Class1
    {
        public int logid { get; set; }

        [Display(Name = "Login Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm tt}")]
        public DateTime logintime { get; set; }

        [Display(Name = "Logout Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm tt}")]
        public DateTime? logouttime { get; set; }

        [Display(Name = "Checked Dated")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> checkeddate { get; set; }

        public Nullable<int> userid { get; set; }

        public virtual User User { get; set; }
    }
}