using MiidWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiidWeb.Models
{

    public class EOEventSummaryViewModel
    { 
    
           public int EndUserID { get; set; }
        public List<EOEventSummaryItem> EventSummaries { get; set; }




        public EOEventSummaryViewModel(int EventOrganiserID)
        {

            this.EndUserID = EventOrganiserID;

            this.EventSummaries = new List<EOEventSummaryItem>();
            
            //Middle
            this.EventSummaries.AddRange(EventOrganiserRepository.GetMyEventSummaries(EventOrganiserID));

        
        }


    }

    public class EOEventSummaryItem
    {

        public string EventName { get; set; }
        public int EventID { get; set; }
        public string TicketClassName { get; set; }
        public DateTime? StartDateTime { get; set; }
        public int DaysTillEvent { get; set; }
        public int TicketCount { get; set; }
        public decimal TicketValue { get; set; }
        

        
    }
}