//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MiidWeb
{
    using System;
    using System.Collections.Generic;
    
    public partial class TicketClassSeat
    {
        public int ID { get; set; }
        public Nullable<int> TicketClassSeatRangeID { get; set; }
        public Nullable<int> SeatNumber { get; set; }
        public Nullable<int> TicketID { get; set; }
        public Nullable<System.DateTime> DateTimeReserved { get; set; }
    
        public virtual Ticket Ticket { get; set; }
        public virtual TicketClassSeatRange TicketClassSeatRange { get; set; }
    }
}
