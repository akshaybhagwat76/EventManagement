using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiidWeb.Models
{
    public class VendorEventViewModel
    {
        public Vendor Vendor { get; set; }

        public Event Event { get; set; }

        public List<POSTransaction> POSTransactions { get; set; }
    }
}