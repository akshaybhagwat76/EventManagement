using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiidWeb.Models
{
    public class TicketSeatTuple
    {

        public int TicketClassID { get; set; }
        //public int Qty { get; set; }//always 1
        public int RowID { get; set; }//rows exist
        public int SeatNumber { get; set; }//
        public int TicketClassSeatID { get; set; }//seats are created on reservation
    }
}