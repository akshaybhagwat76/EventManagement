using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiidWeb.Models
{
    public class TicketClassSeatRangeViewModel
    {


        public TicketClassSeatRangeViewModel()
        {
            //this.TicketClassSeats = new HashSet<TicketClassSeat>();
        }
        public TicketClassSeatRange TicketClassSeatRange { get; set; }
        public int ID { get; set; }
        public Nullable<int> TicketClassID { get; set; }
        public string RowNumber { get; set; }
        public Nullable<int> FromSeatNumber { get; set; }
        public Nullable<int> ToSeatNumber { get; set; }

        //public virtual TicketClass TicketClass { get; set; }
        //public virtual ICollection<TicketClassSeat> TicketClassSeats { get; set; }

        public SelectList SeatNumberList { get; set; }

        private void Setup(TicketClassSeatRange model)
        {
            this.ID = model.ID;
            this.TicketClassID = model.TicketClassID;
            this.RowNumber = model.RowNumber;
            this.FromSeatNumber = model.FromSeatNumber;
            this.ToSeatNumber = model.ToSeatNumber;

        }

        public TicketClassSeatRangeViewModel(int id)
        {
            List<SelectListItem> availableseats = new List<SelectListItem>();

            using (var db = new MiidEntities())
            {
                this.TicketClassSeatRange = db.TicketClassSeatRanges.Find(id);
                Setup(this.TicketClassSeatRange);
                List<TicketClassSeat> takenseats = db.TicketClassSeats.Where(x => x.TicketClassSeatRangeID == id).ToList();

                int xy = 0;
                int seatrange = this.ToSeatNumber ?? 0 - this.FromSeatNumber ?? 0;
                while (xy <= seatrange)
                {
                    if (!takenseats.Any(x => x.SeatNumber == xy))//If this seat number is not in the taken seats Then add it to dropdown
                    {
                        availableseats.Add(new SelectListItem { Text = xy.ToString(), Value = xy.ToString(), Selected = true });
                    }
                    xy++;
                }
            }

            this.SeatNumberList = new SelectList(availableseats, "Value", "Text");

            // this.SelectedTicketQuantity = selectedTicketCount;

        }
    }
}