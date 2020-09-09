using MiidWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiidWeb.Models
{
    public class MyEventsViewModel
    {

        public int EndUserID { get; set; }
        public List<MyEventItem> Events { get; set; }

        

       
        public MyEventsViewModel(int EndUserID, DateTime AnyDate, int PlusMonths)
        {

            this.EndUserID = EndUserID;
                        
            this.Events = new List<MyEventItem>();
            
            
                        

            //Middle
            this.Events.AddRange(EndUserRepository.GetMyEventsOnly(EndUserID, AnyDate, PlusMonths));

          
        
        }

    
    }
    public class MyEventItem
    {
        public int EventID { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string EventName { get; set; }
        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        public int TicketCount { get; set; }
        public string ImageURL { get; set; }
        public string ImageAltText { get; set; }


        //1	2015-06-01	1	Michael Buble Kirstenbosch	2015-06-01 18:00:00.000	1	electro.jpg	electro

    }

}