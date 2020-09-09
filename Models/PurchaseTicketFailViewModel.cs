using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiidWeb.Models
{
    public class PurchaseTicketResultViewModel
    {
        public int EndUserID { get; set; }
        public bool HasErrors { get; set; }
        public List<ConfirmationError> ErrorList { get; set; }

        public string UniquePaymentID { get; set; }

        public int EventID { get; set; }
        public List<TicketViewModel> reservedTickets { get; set; }

        public List<TicketViewModel> purchasedTickets { get; set; }

        public int TotalTicketCost { get; set; }
    }

    public class ConfirmationError
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }

        public int ID { get; set; }
    
    }
}