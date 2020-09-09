using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiidWeb.Models
{
    public class PromoCodeViewModel
    {
        public decimal DiscountPercentage { get; set; }
        public string First8 { get; set; }
    }
}