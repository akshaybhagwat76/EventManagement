using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MiidWeb.Models
{
    public class EventOrganiserViewModel
    {
        public EventOrganiser EventOrganiser { get; set; }

        public List<EventViewModel> MyEvents { get; set; }

		

	


		public bool ResetPasswordTrue { get; set; }
        public EventOrganiserViewModel()
        { 
        }

        public EventOrganiserViewModel(int id)
        {
            MiidEntities db = new MiidEntities();

            var eo = db.EventOrganisers.Where(x => x.ID == id).First();

            var events = db.Events.Where(c => c.EventOrganiserID == id).ToList();

            this.MyEvents = new List<EventViewModel>();

            foreach (var ev in events)
            {
                
                //EventViewModel mod = new EventViewModel(ev.ID, id, true, true);//no tickets overload
                EventViewModel mod = new EventViewModel(ev.ID, id, true);//with tickets overload
                this.MyEvents.Add(mod);
            }

            this.EventOrganiser = eo;
        }

    }
}