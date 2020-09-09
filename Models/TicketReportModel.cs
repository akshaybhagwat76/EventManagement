using System;

namespace MiidWeb.Models
{
  public class TicketReportModel
  {
    public string Firstname { get; set; }
        public string Surname { get; set; }
        public string IDNumber { get; set; }
        
        public string TicketClass { get; set; }
        public string EventName { get; set; }
    public DateTime StartDateTime { get; set; }
        public DateTime TicketClassStartDate { get; set; }
        public string StreetAddress { get; set; }
    public string Suburb { get; set; }
    public string UserName { get; set; }
    public string TicketId { get; set; }
    public string ImageURL { get; set; }
    public byte[] Image { get; set; }
        public string        QrCode { get; set; }
        public string RowNumber { get; set; }
        public string SeatNumber { get; set; }
    }
}