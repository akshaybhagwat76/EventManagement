using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MiidWeb
{

    [MetadataType(typeof(EventOrganiserModelAttribute))]
    public partial class EventOrganiser
    {
        // leave it empty.
    }

    public class EventOrganiserModelAttribute
    {
        // Your attribs will come here.
        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "Please enter Company Name.")]
        public string CompanyName { get; set; }

        [Display(Name = "Contact Name")]
        [Required(ErrorMessage = "Please enter Contact Name.")]
        public string ContactName { get; set; }

        [Display(Name = "Telephone")]
        [Required(ErrorMessage = "Please enter Telephone.")]
        public string Telephone { get; set; }


           

     


    }

}