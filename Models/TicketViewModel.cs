using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiidWeb.Models
{

    public class TicketLiteViewModel
    {
        public int ID { get; set; }
        public string TicketNumber { get; set; }
        public int EndUserID { get; set; }
        public int StatusID { get; set; }
        public int TicketPurchasePrice { get; set; }
        public int TicketClassID { get; set; }
        public Nullable<System.DateTime> DatetimePurchased { get; set; }
        public Nullable<System.DateTime> DatetimeReserved { get; set; }
        public Nullable<System.DateTime> DatetimeRedeemed { get; set; }
        public string Hash { get; set; }
        public Nullable<System.DateTime> DateTimeUpdated { get; set; }
        public string UniquePaymentID { get; set; }


        public TicketLiteViewModel() { }
        public TicketLiteViewModel(Ticket t) {

            this.ID = t.ID;
            this.TicketNumber = t.TicketNumber;
            this.EndUserID = t.EndUserID ?? 0;
            this.StatusID = t.StatusID ?? 0;
            this.TicketPurchasePrice = (int)t.TicketPurchasePrice;
            this.TicketClassID = t.TicketClassID ?? 0;
            this.DatetimePurchased = t.DatetimePurchased;
            this.Hash = t.Hash;
            this.UniquePaymentID = t.UniquePaymentID;
            


        }
    }


    public class TicketViewModel
    {
        public Ticket Ticket { get; set; }

        public string Status { get; set; }
        public TicketClass TicketClass { get; set; }
        public Event Event { get; set; }

        public TicketViewModel()
        { }

        public TicketViewModel(int TicketID)
        {

            var db = new MiidEntities();

            this.Ticket = db.Tickets.Find(TicketID);

            this.TicketClass = db.TicketClasses.Find(this.Ticket.TicketClassID);

            this.Status = db.Status.Find(this.Ticket.StatusID).Code;

            this.Event = db.Events.Find(this.TicketClass.EventID);
        }



    }
}