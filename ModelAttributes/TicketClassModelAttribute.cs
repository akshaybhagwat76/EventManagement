using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiidWeb.ModelAttributes
{

    [MetadataType(typeof(TicketClassModelAttribute))]
    public partial class TicketClass
    {
        // leave it empty.
    }

    public class TicketClassModelAttribute
    {
        [Display(Name = "Quantity")]
        [Required(ErrorMessage = "Please enter Quantity.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only numbers allowed")]
        public string Quantity { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "Please enter Price.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Only numbers allowed")]
        public string Price { get; set; }

    }
}