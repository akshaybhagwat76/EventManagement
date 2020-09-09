using FluentValidation.Attributes;
using MiidWeb.Rules;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiidWeb.Models
{
    [Validator(typeof(MyViewModelValidator))]
    public class MyViewModel
    {
        [Display(Name = "Start date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
                                                      ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
                                                      ApplyFormatInEditMode = true)]
        public DateTime DateToCompareAgainst { get; set; }
    }
}