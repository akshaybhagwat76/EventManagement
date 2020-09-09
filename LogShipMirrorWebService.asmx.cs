using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace MiidWeb
{
    /// <summary>
    /// Summary description for LogShipMirrorWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class LogShipMirrorWebService : System.Web.Services.WebService
    {

        private MiidEntities db = new MiidEntities();




        [WebMethod]
        public void TopupMyTables()
        {
            DateTime BatchDate = DateTime.Now;
            LS_InsertMyMoney(BatchDate);

        
        }
        public void LS_InsertMyMoney(DateTime BatchDate)
        {
            //MiidWebService principalService = new MiidWebService();
            //List<MyMoney> List = principalService.LS_GetMyMoneys(BatchDate);
            ////TODO CHECK FOR UPDATES
            ////db.MyMoneys.AddRange(List.Where(x=>x.);

            //var peopleList1 = db.MyMoneys;

            //List.Where(p => !peopleList1.Any(p2 => p2.ID == p.ID));

            //db.SaveChanges();
        }

        public void LS_InsertTickets(DateTime BatchDate)
        {
            //MiidWebService principalService = new MiidWebService();
            //List<Ticket> List = principalService.LS_GetTickets(BatchDate);
            ////TODO CHECK FOR UPDATES

            //var allTickets = db.Tickets;
            
            ////Insert the new ones            
            //db.Tickets.AddRange(List.Where(p => !allTickets.Any(p2 => p2.ID == p.ID)));
            
            ////Find and update all the updated ones
            //foreach(var updatedTicket in List.Where(p => allTickets.Any(x=>x.DateTimeUpdated < p.DateTimeUpdated)))
            //{
            //    var ticket = db.Tickets.Find(updatedTicket.ID);
            //    ticket.DatetimePurchased = updatedTicket.DatetimePurchased;
            //    ticket.DatetimeRedeemed = updatedTicket.DatetimeRedeemed;
            //    ticket.DatetimeReserved = updatedTicket.DatetimeReserved;
            //    ticket.DateTimeUpdated = updatedTicket.DateTimeUpdated;
            //    ticket.EndUserID = updatedTicket.EndUserID;
            //    ticket.StatusID = updatedTicket.StatusID;
            //    ticket.TicketClassID = updatedTicket.TicketClassID;
            //    ticket.TicketClassID = updatedTicket.TicketClassID;
            //    ticket.TicketClassID = updatedTicket.TicketClassID;
            //    ticket.TicketClassID = updatedTicket.TicketClassID;
            
            
            //}



            db.SaveChanges();
        }

        public void LS_InsertNFCTags(DateTime BatchDate)
        {
            //MiidWebService principalService = new MiidWebService();
            //List<NFCTag> List = principalService.LS_GetNFCTags(BatchDate);
            ////TODO CHECK FOR UPDATES
            //db.NFCTags.AddRange(List);
            //db.SaveChanges();
        }


    }
}
