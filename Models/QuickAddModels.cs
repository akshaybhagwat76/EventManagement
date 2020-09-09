using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiidWeb.Models
{
    public class QuickAddModels
    {





    }

    public class QuickAddTicketViewModel
    {
        public IEnumerable EventList { get; set; }
        public IEnumerable TicketClassList { get; set; }

       
        public string UserName { get; set; }
        public int Quantity { get; set; }
        public int TicketClassID { get; set; }
        public int EventID { get; set; }
    }


    public class QuickAddEndUser
    {

        public int ID { get; set; }
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string IDNumber { get; set; }
        public string Cell { get; set; }
        public string Email { get; set; }
     
      
        public bool Newsletter { get; set; }
        public bool Male { get; set; }
        public bool Female { get; set; }
        public bool TermsAndConditions { get; set; }

        public bool IsBandActive { get; set; }


        public QuickAddEndUser()
        {
        }

        public QuickAddEndUser(EndUser user)
        {
            this.ID = user.ID;
            this.Surname = user.Surname;
            this.Firstname = user.Firstname;
            this.IDNumber = user.IDNumber;
            this.Cell = user.Cell;
            this.Email = user.Email;
          
        }
    }
}


