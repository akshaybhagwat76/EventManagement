using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiidWeb.Models
{
    public class EOTicketReport
    {
        public int TicketID { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string IDNumber { get; set; }
        public string Email { get; set; }
        public string Cell { get; set; }
        public string TicketClassName { get; set; }
        public string TicketNumber { get; set; }
        public DateTime DatetimePurchased { get; set; }
        public string TicketStatus { get; set; }
        public DateTime TicketStartDate { get; set; }
        public DateTime TicketEndDate { get; set; }

    }

	public class EORemarketingReport
	{
		public int TicketID { get; set; }
		public string FirstName { get; set; }
		public string Surname { get; set; }
		public string IDNumber { get; set; }
		public string Email { get; set; }
		public string Cell { get; set; }
		public string TicketClassName { get; set; }
		public string TicketNumber { get; set; }
		public DateTime DatetimePurchased { get; set; }
		public string TicketStatus { get; set; }
		public DateTime TicketStartDate { get; set; }
		public DateTime TicketEndDate { get; set; }

	}
}