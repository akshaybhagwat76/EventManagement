using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.Entity;
using MiidWeb.Models;
using System.Text;
using System.IO;
using MiidWeb.Repositories;
using MiidWeb.Helpers;
using System.Data.SqlClient;
using System.Configuration;

namespace MiidWeb
{
    /// <summary>
    /// Summary description for MiidWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class MiidWebService : System.Web.Services.WebService
    {
        private MiidEntities db = new MiidEntities();



        #region Box office

        [WebMethod]
        public int LoginEventOrganiserBoxOffice(string UserName, string Password, int EventID)
        {

            int result = 0;
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("LoginEventOrganiserBoxOffice", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };

            sqlDataAdapter.SelectCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = UserName;
            sqlDataAdapter.SelectCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = Password;

            sqlDataAdapter.SelectCommand.Parameters.Add("@EventID", SqlDbType.Int).Value = EventID;
            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];
            if (item != null)
            {
                foreach (DataRow row in item.Rows)
                {
                    result = int.Parse(row[0].ToString());
                }
            }
            sqlConnection.Close();

            return result;
        }


        public class TicketClassTuple
        {
            public int ID { get; set; }
            public string TicketClassName { get; set; }
            public int Count { get; set; }
            public bool Available { get; set; }
        }
        [WebMethod]
        public string PurchaseBoxOfficeTickets(string TicketClassList, int BoxOfficeUserID, int PosID)
        {

            using (var db = new MiidEntities())
            {
                List<Ticket> item = new List<Ticket>();
                string[] ticketclasses = TicketClassList.Split(';');

                //Verify availability
                List<string> ListTicketClass = new List<string>();
                foreach (string s in ticketclasses)
                {
                    if(!String.IsNullOrEmpty(s))
                    ListTicketClass.Add(s);
                }
                List<TicketClassTuple> selectedCounts = new List<TicketClassTuple>();
                List<TicketClassTuple> checkedCounts = new List<TicketClassTuple>();
                var grouped = ListTicketClass
                .GroupBy(s => s)
                .Select(g => new { Symbol = g.Key, Count = g.Count() });

                foreach (var TC in grouped)
                {
                   
                    selectedCounts.Add(new TicketClassTuple { ID = int.Parse(TC.Symbol), Count = TC.Count });
                }

                Boolean fail = false;
                foreach (var t in selectedCounts)
                {
                    var tc = db.TicketClasses.Find(t.ID);

                    if (t.Count <= db.TicketClasses.Find(t.ID).RunningQuantity)
                    {
                        checkedCounts.Add(new TicketClassTuple { ID = t.ID, Count = t.Count, TicketClassName = tc.Description, Available = true });
                    }
                    else
                    {
                        checkedCounts.Add(new TicketClassTuple { ID = t.ID, Count = t.Count, TicketClassName = tc.Description, Available = false });
                        fail = true;
                    }

                }


                if (item != null && !fail)
                {
                    int x = 0;
                    TicketLiteViewModel[] statuses = new TicketLiteViewModel[ticketclasses.Length];//ds removed -1
                    foreach (string row in ticketclasses)
                    {
                        if (!String.IsNullOrEmpty(row))
                        {
                            statuses[x] = new TicketLiteViewModel(TicketRepository.PurchaseTicketWithCashBoxOffice(int.Parse(row), BoxOfficeUserID, PosID));
                            x++;
                        }
                    }
                    return JsonConvert.SerializeObject(statuses, Newtonsoft.Json.Formatting.Indented);
                }
                else
                {
                    return String.Format("{0}|{1}", "Error purchasing tickets", JsonConvert.SerializeObject(checkedCounts.Where(x => x.Available == false), Newtonsoft.Json.Formatting.Indented));

                }





            }
        }

        [WebMethod]
        public string GetTicketClassesForEvent(string EventCode, bool IsBoxOffice)
        {
            using (var db = new MiidEntities())
            {
                DataTable item = TicketRepository.GetTicketClassesForEvent(EventCode, IsBoxOffice);

                if (item != null)
                {
                    int x = 0;
                    TicketClassLiteViewModel[] statuses = new TicketClassLiteViewModel[item.Rows.Count];
                    foreach (DataRow row in item.Rows)
                    {

                        statuses[x] = new TicketClassLiteViewModel
                        {
                            ID = int.Parse(row["ID"].ToString()),
                            EventName = row["EventName"].ToString(),
                            EventID = int.Parse(row["EventID"].ToString()),
                            Code = row["Code"].ToString(),
                            Description = row["Description"].ToString(),
                            StartDate = DateTime.Parse(row["StartDate"].ToString()),
                            EndDate = DateTime.Parse(row["EndDate"].ToString()),
                            Price = decimal.Parse(row["Price"].ToString()),
                            RunningQuantity = int.Parse(row["RunningQuantity"].ToString()),
                            Quantity = int.Parse(row["Quantity"].ToString()),

                        };
                        x++;
                    }
                    return JsonConvert.SerializeObject(statuses, Newtonsoft.Json.Formatting.Indented);
                }

                return null;

            }

        }

        #endregion


        #region Ticketing App

        [WebMethod]
        public string SyncTagApi(int EndUserID, string NfcTag)
        {

            return NFCTagRepository.SyncTagApi(EndUserID, NfcTag);
        }


        [WebMethod]
        public string GetTagStatusForUserID(string EndUserID)
        {

            return EndUserRepository.GetTagStatusForUserID(int.Parse(EndUserID));
        }


        [WebMethod]
        public string GetUserForTag(string TagNumber)
        {
            string UserDetails = "";


            var tags = db.NFCTags.Where(x => x.TagNumber == TagNumber);

            if (tags.Count() > 0)
            {
                var tag = tags.First();

                if (tag != null)
                {
                    var endUser = db.EndUsers.Find(tag.EndUserID);
                    if (endUser != null)
                    {

                        EndUserViewModel endUserVM = new EndUserViewModel();

                        endUserVM.EndUser = endUser;
                        endUserVM.UpcomingEvents = new MyEventsViewModel(endUser.ID, DateTime.Now, 3);//3 months ahead

                        UserDetails = String.Format("Name: {0}, Surname: {1}, , Contact: {2}, Next of Kin: {3} , Next of Kin Contact: {4}", endUser.Firstname, endUser.Surname, endUser.Cell, endUser.NextOfKin, endUser. NextOfKinTelephone);


                        //  if (endUserVM.UpcomingEvents.Events.Count() > 0)
                        //{
                        //  foreach (var e in endUserVM.UpcomingEvents.Events.ToList())
                        //{
                        //  var ticketClasses = db.TicketClasses.Where(x => x.EventID == e.EventID);
                        // UserDetails += String.Format("TicketForEvent: {0}, Count: {1}", e.EventName, e.TicketCount);

                        //       }
                        //  }

                        //  return String.Format("Details: {0}", UserDetails);

                        return String.Format(UserDetails);

                    }
                    else
                    {
                        return "User not found.";

                    }

                }
            }
            else
            {

                return "Tag not found.";

            }

            return UserDetails;

        }

        [WebMethod]
        public string LoginEventOrganiser(string UserName, string Password)
        {

            StringBuilder sb = new StringBuilder();

            var eos = db.EventOrganisers.Where(x => x.Email == UserName && x.HandHeldLoginPassword == Password);
            if (eos.Count() > 0)
            {
                var eo = eos.First();
                sb.AppendLine(String.Format("{0} logged in successfully.", eo.CompanyName));



            }
            else
            {
                sb.AppendLine(String.Format("{0} not found.", UserName));
            }



            return sb.ToString();
        }

      

        [WebMethod]
        public string GetCurrentEvents(string UserName, string Password)
        {

            return EventOrganiserRepository.GetCurrentEventsSp(UserName, Password);
            //StringBuilder sb = new StringBuilder();

            //var eos = db.EventOrganisers.Where(x => x.Email == UserName && x.HandHeldLoginPassword == Password);
            //if (eos.Count() > 0)
            //{
            //    var eo = eos.First();


            //    foreach (var ev in CurrentEventsForEventOrganiser(eo.ID, true))
            //    {
            //        sb.Append(ev);
            //        sb.Append("|");

            //    }

            //}
            //else
            //{
            //    sb.AppendLine(String.Format("{0} not found.", UserName));
            //}



            //return sb.ToString();
        }

        private List<string> CurrentEventsForEventOrganiser(int EventOrganiserID, bool Lite)
        {
            List<string> MyEvents = new List<string>();

            using (var db = new MiidEntities())
            {

                var events = db.Events.Where(x => x.EventOrganiserID == EventOrganiserID).ToList();
                foreach (var ev in events)
                {
                    MyEvents.Add(ev.EventName);
                }
            }
                

            return MyEvents;

        }

        private List<EventViewModel> CurrentEventsForEventOrganiser(int EventOrganiserID)
        {

            EventOrganiserViewModel eo = new EventOrganiserViewModel(EventOrganiserID);

            return eo.MyEvents;

        }

        [WebMethod]
        public string TicketValidForEvent(string TagNumber, string EventName)
        {

            StringBuilder sb = new StringBuilder();

            var theEvent = new Event();

            bool ItsATicketCode = false;
            string TicketCode = "";

            var events = db.Events.Where(x => x.EventName == EventName);
            if (events.Count() > 0)
            {
                theEvent = events.First();
            }


            List<TicketViewModel> myValidTickets = new List<TicketViewModel>();

            if (theEvent != null)
            {

                if (TagNumber.Substring(0, 3).ToLower() == "tic")
                {
                    ItsATicketCode = true;
                    TicketCode = TagNumber.Substring(3);
                }

                if (!ItsATicketCode)
                {
                    int ActiveID = StatusHelper.StatusID("NFCtag", "Active");

                    var tags = db.NFCTags.Where(x => x.TagNumber == TagNumber && x.StatusID == ActiveID);

                    if (tags.Count() > 0)
                    {
                        var tag = tags.First();

                        if (tag != null)
                        {

                            int StatusPurchasedID = db.Status.Where(x => x.Context == "Ticket" && x.Code == "Purchased").First().ID;
                            int TicketUsedStatusID = db.Status.Where(x => x.Context == "Ticket" && x.Code == "Used").First().ID;

                            var endUser = db.EndUsers.Find(tag.EndUserID);
                            if (endUser != null)
                            {

                                EndUserViewModel endUserVM = new EndUserViewModel();

                                var MyTickets = db.Tickets.Include(x => x.TicketClass).Where(x => x.EndUserID == endUser.ID && x.StatusID == StatusPurchasedID);

                                foreach (var ticket in MyTickets.ToList())
                                {
                                    if (ticket.TicketClass.EventID == theEvent.ID)
                                    {
                                        myValidTickets.Add(new TicketViewModel(ticket.ID));
                                        break; // Only send one ticket per call
                                    }

                                }
                                if (myValidTickets.Count() > 0)
                                {
                                    sb.AppendLine(String.Format("{0} {1}", endUser.Firstname, endUser.Surname));
                                    sb.AppendLine(String.Format("{0}", endUser.IDNumber));

                                    var Path1 = HttpContext.Current.Server.MapPath("~/images/");//profile pics stored here


                                    foreach (var validTicket in myValidTickets)
                                    {
                                        //sb.AppendLine(String.Format("Event: {0}", validTicket.Event.EventName));
                                        sb.AppendLine(String.Format("{0}", validTicket.TicketClass.Description));
                                        //sb.AppendLine(String.Format("Ticket #: {0}", validTicket.Ticket.TicketNumber));

                                    }

                                    if (endUser.ProfilePicURL != null)
                                    {
                                        if (File.Exists(Path.Combine(Path1, endUser.ProfilePicURL)))
                                        {
                                            sb.AppendLine(ConvertImageToString(Path.Combine(Path1, endUser.ProfilePicURL)));
                                        }

                                    }
                                    else
                                    {
                                        sb.AppendLine(ConvertImageToString(Path.Combine(Path1, "avatar_blank.png")));
                                    }


                                    return sb.ToString();
                                }
                                else
                                {
                                    return "No Valid Tickets";
                                }

                            }
                            else
                            {
                                return "User not found.";

                            }

                        }
                    }
                    else
                    {

                        return "No Active Tags found.";

                    }
                }
                else //Process TicketCode from Barcode
                {

                    int StatusPurchasedID = db.Status.Where(x => x.Context == "Ticket" && x.Code == "Purchased").First().ID;
                    int TicketUsedStatusID = db.Status.Where(x => x.Context == "Ticket" && x.Code == "Used").First().ID;
                    //Find ticket
                    var tickets = db.Tickets.Include(x => x.TicketClass).Where(c => c.TicketNumber == TicketCode && c.StatusID == StatusPurchasedID);

                    var usedtickets = db.Tickets.Include(x => x.TicketClass).Where(c => c.TicketNumber == TicketCode && c.StatusID == TicketUsedStatusID);

                    if (usedtickets != null && usedtickets.Count() > 0)
                    {
                        return "No valid ticket. Already used.";

                    }

                    if (tickets != null && tickets.Count() > 0)
                    {
                        var validTicket = tickets.First();
                        if (validTicket.TicketClass.EventID == theEvent.ID)
                        {

                            var endUser = db.EndUsers.Find(validTicket.EndUserID);
                            if (endUser != null)
                            {

                                EndUserViewModel endUserVM = new EndUserViewModel();


                                sb.AppendLine(String.Format("{0} {1}", endUser.Firstname, endUser.Surname));
                                sb.AppendLine(String.Format("{0}", endUser.IDNumber));

                                var Path1 = HttpContext.Current.Server.MapPath("~/images/");//profile pics stored here


                                sb.AppendLine(String.Format("{0}", validTicket.TicketClass.Description));


                                if (endUser.ProfilePicURL != null)
                                {
                                    if (File.Exists(Path.Combine(Path1, endUser.ProfilePicURL)))
                                    {
                                        sb.AppendLine(ConvertImageToString(Path.Combine(Path1, endUser.ProfilePicURL)));
                                    }

                                }
                                else
                                {
                                    sb.AppendLine(ConvertImageToString(Path.Combine(Path1, "avatar_blank.png")));
                                }


                                return sb.ToString();
                            }
                            else
                            {
                                return "User not found";
                            }


                        }
                        else
                        {

                            return "Ticket Not valid for this Event.";

                        }
                    }
                    //Find ticket class
                }
            }
            else
            {
                return "Event Not Found";

            }

            return "No valid ticket.";

        }

        [WebMethod]
        public string GetUserEmail(string TagNumber, string EventName)
        {

            StringBuilder sb = new StringBuilder();

            var theEvent = new Event();

            bool ItsATicketCode = false;
            string TicketCode = "";

            var events = db.Events.Where(x => x.EventName == EventName);
            if (events.Count() > 0)
            {
                theEvent = events.First();
            }


            List<TicketViewModel> myValidTickets = new List<TicketViewModel>();

            if (theEvent != null)
            {

                if (TagNumber.Substring(0, 3).ToLower() == "tic")
                {
                    ItsATicketCode = true;
                    TicketCode = TagNumber.Substring(3);
                }

                if (!ItsATicketCode)
                {
                    int ActiveID = StatusHelper.StatusID("NFCtag", "Active");

                    var tags = db.NFCTags.Where(x => x.TagNumber == TagNumber && x.StatusID == ActiveID);

                    if (tags.Count() > 0)
                    {
                        var tag = tags.First();

                        if (tag != null)
                        {

                            int StatusPurchasedID = db.Status.Where(x => x.Context == "Ticket" && x.Code == "Purchased").First().ID;
                            int TicketUsedStatusID = db.Status.Where(x => x.Context == "Ticket" && x.Code == "Used").First().ID;

                            var endUser = db.EndUsers.Find(tag.EndUserID);
                            if (endUser != null)
                            {

                                EndUserViewModel endUserVM = new EndUserViewModel();

                                var MyTickets = db.Tickets.Include(x => x.TicketClass).Where(x => x.EndUserID == endUser.ID && x.StatusID == StatusPurchasedID);

                                foreach (var ticket in MyTickets.ToList())
                                {
                                    if (ticket.TicketClass.EventID == theEvent.ID)
                                    {
                                        myValidTickets.Add(new TicketViewModel(ticket.ID));
                                        break; // Only send one ticket per call
                                    }

                                }
                                if (myValidTickets.Count() > 0)
                                {
                                    string s = endUser.ID.ToString();
                                  
                                    sb.AppendLine(String.Format(s));
                                   

                                    return sb.ToString();
                                }
                                else
                                {
                                    return "No Valid Tickets!!!";
                                }

                            }
                            else
                            {
                                return "User not found.";

                            }

                        }
                    }
                    else
                    {

                        return "No Active Tags found.";

                    }
                }
                else //Process TicketCode from Barcode
                {

                    int StatusPurchasedID = db.Status.Where(x => x.Context == "Ticket" && x.Code == "Purchased").First().ID;
                    int TicketUsedStatusID = db.Status.Where(x => x.Context == "Ticket" && x.Code == "Used").First().ID;
                    //Find ticket
                    var tickets = db.Tickets.Include(x => x.TicketClass).Where(c => c.TicketNumber == TicketCode && c.StatusID == StatusPurchasedID);
                    var usedtickets = db.Tickets.Include(x => x.TicketClass).Where(c => c.TicketNumber == TicketCode && c.StatusID == TicketUsedStatusID);

                    if (usedtickets != null && usedtickets.Count() > 0)
                    {
                        var validTicket = usedtickets.First();
                        if (validTicket.TicketClass.EventID == theEvent.ID)
                        {

                            var endUser = db.EndUsers.Find(validTicket.EndUserID);
                            if (endUser != null)
                            {
                                // this send through EnUserID
                                EndUserViewModel endUserVM = new EndUserViewModel();
                                sb.AppendLine(String.Format("{0}", endUser.ID));
                                return sb.ToString();
                            }
                            else
                            {
                                return "User not found";
                            }



                        }


                    }

                    if (tickets != null && tickets.Count() > 0)
                    {   

                        var validTicket = tickets.First();
                        if (validTicket.TicketClass.EventID == theEvent.ID)
                        {

                            var endUser = db.EndUsers.Find(validTicket.EndUserID);
                            if (endUser != null)
                            {
                                // this send through EnUserID
                                EndUserViewModel endUserVM = new EndUserViewModel();
                                sb.AppendLine(String.Format("{0}", endUser.ID));
                                return sb.ToString();
                            }
                            else
                            {
                                return "User not found";
                            }


                        }
                        else
                        {

                            return "Ticket Not valid for this Event.";

                        }
                    }
                    //Find ticket class
                }
            }
            else
            {
                return "Event Not Found";

            }

            return "No valid ticket.";

        }

        [WebMethod]
        public string AllowDenyAccess(string TagNumber, string ReasonForBlocking, bool Block, string EventName)
        {
            StringBuilder sb = new StringBuilder();

            var theEvent = new Event();
            bool ItsATicketCode = false;
            string TicketCode = "";

            var events = db.Events.Where(x => x.EventName == EventName);
            if (events.Count() > 0)
            {
                theEvent = events.First();
            }


            if (TagNumber.Substring(0, 3).ToLower() == "tic")
            {
                ItsATicketCode = true;
                TicketCode = TagNumber.Substring(3);
            }
            int StatusPurchasedID = 0;
            int TicketUsedStatusID = 0;

            if (ItsATicketCode)
            {
                //Process TicketCode from Barcode

                StatusPurchasedID = db.Status.Where(x => x.Context == "Ticket" && x.Code == "Purchased").First().ID;
                TicketUsedStatusID = db.Status.Where(x => x.Context == "Ticket" && x.Code == "Used").First().ID;
                //Find ticket
                var tickets = db.Tickets.Include(x => x.TicketClass).Where(c => c.TicketNumber == TicketCode && c.StatusID == StatusPurchasedID);
                if (tickets != null && tickets.Count() > 0)
                {



                    foreach (var ticket in tickets.ToList())
                    {
                        if (ticket.TicketClass.EventID == theEvent.ID)
                        {
                            var ticket1 = db.Tickets.Find(ticket.ID);
                            ticket1.StatusID = TicketUsedStatusID;
                            ticket1.DatetimeRedeemed = DateTime.Now;
                            db.Entry(ticket1).State = EntityState.Modified;

                            db.SaveChanges();
                            //Break for 1 ticket
                            sb.AppendLine("Ticket marked as Used");
                            return sb.ToString();
                        }
                        else
                        {
                            sb.AppendLine("Ticket Not valid for this Event.");
                            return sb.ToString();
                        }
                    }
                }
                else
                {
                    sb.AppendLine("No Valid Ticket");
                    return sb.ToString();
                }

            }

            else //TAg is being used not barcode
            {


                if (Block == false)
                {

                    var tags = db.NFCTags.Where(x => x.TagNumber == TagNumber);

                    if (tags.Count() > 0)
                    {
                        var tag = tags.First();

                        if (tag != null)
                        {
                            //Allow access - punch ticket
                            var endUser = db.EndUsers.Find(tag.EndUserID);
                            if (endUser != null)
                            {

                                EndUserViewModel endUserVM = new EndUserViewModel();

                                StatusPurchasedID = db.Status.Where(x => x.Context == "Ticket" && x.Code == "Purchased").First().ID;
                                TicketUsedStatusID = db.Status.Where(x => x.Context == "Ticket" && x.Code == "Used").First().ID;

                                var MyTickets = db.Tickets.Include(x => x.TicketClass).Where(x => x.EndUserID == endUser.ID && x.StatusID == StatusPurchasedID);

                                if (MyTickets.Count() > 0)
                                {
                                    foreach (var ticket in MyTickets.ToList())
                                    {
                                        if (ticket.TicketClass.EventID == theEvent.ID)
                                        {
                                            var ticket1 = db.Tickets.Find(ticket.ID);
                                            ticket1.StatusID = TicketUsedStatusID;
                                            ticket1.DatetimeRedeemed = DateTime.Now;
                                            db.Entry(ticket1).State = EntityState.Modified;

                                            db.SaveChanges();
                                            //Break for 1 ticket
                                            sb.AppendLine("Ticket marked as Used");
                                            return sb.ToString();
                                        }

                                    }
                                }
                                else
                                {
                                    sb.AppendLine("No Valid Ticket");
                                    return sb.ToString();
                                }
                            }
                        }
                    }
                }
            }

            //Log User Rejected
            sb.AppendLine("Customer not allowed entry. Reason logged.");
            return sb.ToString();



        }

        [WebMethod]
        public string GetEventImage(string EventName)
        {

            var events = db.Events.Where(x => x.EventName == EventName);

            if (events.Count() > 0)
            {

                var Event = events.First();
                var images = db.EventImages.Where(x => x.EventID == Event.ID && x.ImageAltText == "banner");
                if (images.Count() > 0)
                {
                    var Image = images.First();
                    var Path1 = HttpContext.Current.Server.MapPath("~/Uploads/");
                    return ConvertImageToString(Path.Combine(Path1, Image.ImageURL));


                }

            }

            return "No Image Found";


        }


        [WebMethod]
        public string GetTicketHoldersForEvent(string UserName, string Password, string EventName)
        {

            StringBuilder sb = new StringBuilder();

            var eos = db.EventOrganisers.Where(x => x.Email == UserName && x.HandHeldLoginPassword == Password);

            if (eos.Count() > 0)
            {
                int purchased = Helpers.StatusHelper.StatusID("Ticket", "Purchased");
                int EventOrganiserID = eos.First().ID;
                //EO Valid now check event
                var Events = db.Events.Where(x => x.EventName == EventName && x.EventOrganiserID == EventOrganiserID);
                if (Events.Count() > 0)
                {
                    int EventID = Events.First().ID;
                    var ticketholders =
                    from events in db.Events
                    join tc in db.TicketClasses on events.ID equals tc.EventID
                    join t in db.Tickets on tc.ID equals t.TicketClassID
                    join eu in db.EndUsers on t.EndUserID equals eu.ID
                    where events.ID == EventID && t.StatusID == purchased
                    select new { TicketFirstName = eu.Firstname, Surname = eu.Surname, Email = eu.Email, t.TicketNumber }; //produces flat sequence

                    foreach (var th in ticketholders)
                    {
                        //  sb.Append(String.Format("{0} {1},{2};", th.TicketFirstName, th.Surname, th.IDNumber));
                        sb.Append(String.Format("{0} {1},{2},{3};", th.TicketFirstName, th.Surname, th.Email, th.TicketNumber));

                    }
                }
                else
                {
                    sb.AppendLine(String.Format("{0} - Event not Found.", EventName));
                }


            }
            else
            {
                sb.AppendLine(String.Format("{0} not found.", UserName));
            }



            return sb.ToString();
        }




        public string ConvertImageToString(string FileName)
        {
            Stream stream = File.OpenRead(FileName);
            var mOriginalData = new byte[stream.Length];
            stream.Read(mOriginalData, 0, mOriginalData.Length);
            stream.Flush();
            stream.Close();

            return Convert.ToBase64String(mOriginalData);
        }

        #endregion


        #region LogShipping

        //[WebMethod]
        //public List<LogShipper> DoLogShipping()
        //{ 
        //    DateTime max = db.LogShippers.Max(x=>x.ShipDate).Value;
        //    var logShip = db.LogShippers.Where(x => x.ShipDate == max).ToList();
        //    DateTime newShipDate = DateTime.Now;

        //    return logShip;

        //}

        //[WebMethod]
        //public List<MyMoney> LS_GetMyMoneys(DateTime BatchDate)
        //{
        //    int LastMaxID = Helpers.LogshipHelper.MaxIDForTable("MyMoney");


        //    LogLogShip(BatchDate, "MyMoney");

        //    return db.MyMoneys.Where(x => x.ID > LastMaxID).ToList();


        //}

        //[WebMethod]
        //public List<Ticket>LS_GetTickets(DateTime BatchDate)
        //{
        //    int LastMaxID = Helpers.LogshipHelper.MaxIDForTable("Ticket");
        //    DateTime LastBatchDate = Helpers.LogshipHelper.MaxBatchDate("Ticket");


        //    LogLogShip(BatchDate, "Ticket");

        //    List<Ticket> insertTickets = db.Tickets.Where(x => x.ID > LastMaxID).ToList();

        //    List<Ticket> updatedTickets = db.Tickets.Where(x => x.DateTimeUpdated >= LastBatchDate).ToList();

        //    insertTickets.AddRange(updatedTickets);

        //    return insertTickets;


        //}

        //[WebMethod]
        //public List<NFCTag> LS_GetNFCTags(DateTime BatchDate)
        //{
        //    int LastMaxID = Helpers.LogshipHelper.MaxIDForTable("NFCTag");


        //    LogLogShip(BatchDate, "NFCTag");

        //    return db.NFCTags.Where(x => x.ID > LastMaxID).ToList();


        //}




        //private void LogLogShip(DateTime BatchDate, string TableName)
        //{
        //    //Add entry to LogShipper
        //    var LogShipper = new LogShipper();
        //    LogShipper.ShipDate = BatchDate;
        //    LogShipper.TableName = TableName;
        //    LogShipper.LastMaxID = db.MyMoneys.Max(x => x.ID);
        //    db.LogShippers.Add(LogShipper);
        //}

        #endregion


        #region Vendor App


        #region Webmethods


        

  [WebMethod]
        public string TransactionListForVendor(int StaffID, int EventID, int VendorID)
        {
            StringBuilder sb = new StringBuilder();

           
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("TransactionListForVendor", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };

            sqlDataAdapter.SelectCommand.Parameters.Add("@StaffID", SqlDbType.Int).Value = StaffID;
            sqlDataAdapter.SelectCommand.Parameters.Add("@EventID", SqlDbType.Int).Value = EventID;
            sqlDataAdapter.SelectCommand.Parameters.Add("@VendorID", SqlDbType.Int).Value = VendorID;
            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];
            if (item != null)
            {
                //return Customer name, transaction amount, time of transactions

                foreach (DataRow row in item.Rows)
                {
                    sb.Append(row["CustomerName"].ToString());
                    sb.Append(";");
                    sb.Append(row["TransactionAmount"].ToString());
                    sb.Append(";");
                    sb.Append(row["TransactionDateTime"].ToString());
                    sb.Append("|");
                }
            }
        
            else
            {
                sb.Append("No Transactions Found.");
            }

            sqlConnection.Close();
              

            return sb.ToString();
            //delimited pairs 1;MichaelBuble|3;Red Bull|

        }

        [WebMethod]
        public string VendorEventRevenue(int StaffID, int EventID, int VendorID)
        {
            StringBuilder sb = new StringBuilder();


            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("VendorEventRevenue2", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };

            sqlDataAdapter.SelectCommand.Parameters.Add("@StaffID", SqlDbType.Int).Value = StaffID;
            sqlDataAdapter.SelectCommand.Parameters.Add("@EventID", SqlDbType.Int).Value = EventID;
            sqlDataAdapter.SelectCommand.Parameters.Add("@VendorID", SqlDbType.Int).Value = VendorID;
            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];
          //  itemstring = item.ToString();
            if (item != null)
            {
                //return Customer name, transaction amount, time of transactions

                foreach (DataRow row in item.Rows)
                {
                   
                    sb.Append(row["TransactionAmount"].ToString());
                   
                   
                }
            }

            else
            {
                sb.Append("No Transactions Found.");
            }

            sqlConnection.Close();


            return sb.ToString();
            //delimited pairs 1;MichaelBuble|3;Red Bull|

        }



        [WebMethod]
        public string RegisterDevice0(string DeviceCode)
        {

            Device device = new Device();
            List<Device> devices = db.Devices.Where(x => x.DeviceAppCode == DeviceCode).ToList();
            if (devices != null && devices.Count() > 0)
            {
                device = devices.First();

            }
            else
            {
                device.DeviceAppCode = DeviceCode;
                device.DateTimeAdded = DateTime.Now;
                device.Description = "New Device Registration";
                db.Devices.Add(device);
                db.SaveChanges();
            }

            return device.ID.ToString();
        }

        //1.
        [WebMethod]
        public string VendorLoginGetEvents1(int VendorPin, string VendorCode)
        {
            StringBuilder sb = new StringBuilder();



            if (!VendorLogin(VendorPin, VendorCode))
            {
                return "Invalid Vendor Credentials. Please try again.";
            }


            var vendors = db.Vendors.Where(x => x.VendorPin == VendorPin && x.VendorCode == VendorCode);
            if (vendors.Count() > 0)
            {
                Vendor vendor = vendors.First();
                if (vendor != null)
                {
                    sb.Append(vendor.ID.ToString() + "; VendorID|");

                    var vendorevents = db.VendorEvents.Where(x => x.VendorID == vendor.ID);

                    if (vendorevents.Count() > 0)
                    {
                        foreach (var ve in vendorevents)
                        {
                            var myEvent = db.Events.Find(ve.EventID);

                            sb.Append(myEvent.ID.ToString());
                            sb.Append(";");
                            sb.Append(myEvent.EventName.ToString());
                            sb.Append("|");
                        }
                    }
                    else
                    {
                        sb.Append("Vendor Not Linked To Any Events.");
                    }

                }

            }
            else
            {
                sb.Append("No Vendors Found.");
            }

            return sb.ToString();
            //delimited pairs 1;MichaelBuble|3;Red Bull|

        }


        //2.
        [WebMethod]
        public string SetEvent2(int EventID, int VendorID, string DeviceCode)
        {
            Device device = new Device();

            device = DeviceRepository.GetByCode(DeviceCode);

            if (device != null)
            {
                device.CurrentEventID = EventID;
                device.CurrentVendorID = VendorID;
                device.DateTimeUpdated = DateTime.Now;

                db.Entry(device).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                device = DeviceRepository.CreateNewDevice(DeviceCode);
                device.CurrentEventID = EventID;
                device.CurrentVendorID = VendorID;
                device.DateTimeUpdated = DateTime.Now;

                db.Entry(device).State = EntityState.Modified;
                db.SaveChanges();
            }

            return "Event Set Successfully";


        }

        //3a Add Staff
        //Send blank staff code to get list of existing staff
        [WebMethod]
        public string AddStaff3a(int VendorID, string StaffCode, int StaffPin, int EventID, int DeviceID, bool OverwritePin)
        {
            StringBuilder sbR = new StringBuilder();
            try
            {


                var allStaffForThisVendorEvent = db.Staffs.Where(x => x.CurrentVendorID == VendorID && x.CurrentEventID == EventID).ToList();

                if (!String.IsNullOrEmpty(StaffCode))
                {

                    var existingStaffCodes = allStaffForThisVendorEvent.Where(x => x.CurrentStaffCode == StaffCode);

                    if (existingStaffCodes == null || existingStaffCodes.Count() == 0)
                    {
                        Staff staff = new Staff { CurrentDevicePin = StaffPin, CurrentVendorID = VendorID, CurrentStaffCode = StaffCode, DateTimeUpdated = DateTime.Now, CurrentDeviceID = DeviceID, CurrentEventID = EventID };
                        db.Staffs.Add(staff);
                        db.SaveChanges();


                    }
                    else //change pin
                    {
                        if (OverwritePin)
                        {
                            Staff staff = existingStaffCodes.Where(x => x.CurrentStaffCode == StaffCode).First();
                            staff.CurrentDevicePin = StaffPin;
                            db.Entry(staff).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            return String.Format("Staff Code {0} already exists.", StaffCode.ToString());
                        }


                    }

                    //get them all again so you include the new one and/or new pins
                    allStaffForThisVendorEvent = db.Staffs.Where(x => x.CurrentVendorID == VendorID && x.CurrentEventID == EventID).ToList();
                }

                foreach (var staff1 in allStaffForThisVendorEvent)
                {
                    sbR.Append(String.Format("{0};{1}|", staff1.CurrentDevicePin, staff1.CurrentStaffCode));
                }

                return sbR.ToString();
            }
            catch (Exception ex)
            {
                return String.Format("Add Staff Failed: {0} - {1} - {2}", ex.Message.ToString(), ex.InnerException.Message.ToString(), ex.InnerException.StackTrace);
            }


        }


        [WebMethod]
        public string GetStaff3c(int VendorID, int EventID)
        {
            StringBuilder sbR = new StringBuilder();
            try
            {


                var allStaffForThisVendorEvent = db.Staffs.Where(x => x.CurrentVendorID == VendorID && x.CurrentEventID == EventID).ToList();

                foreach (var staff1 in allStaffForThisVendorEvent)
                {
                    sbR.Append(String.Format("{0};{1}|", staff1.CurrentDevicePin, staff1.CurrentStaffCode));
                }

                return sbR.ToString();

            }
            catch (Exception ex)
            {
                return String.Format("Get Staff Failed: {0} - {1} - {2}", ex.Message.ToString(), ex.InnerException.Message.ToString(), ex.InnerException.StackTrace);
            }

        }



        [WebMethod]
        public string StartShift3b(int CurrentVendorID, int CurrentDevicePin, string CurrentStaffCode, int EventID, string DeviceCode)
        {
            string StartShiftResult = "";

            string ShiftNo = VendorLoginDeviceToStartShift(DeviceCode, CurrentVendorID, EventID);

            StartShiftResult = String.Format("{0}", ShiftNo.ToString());



            return StartShiftResult;


        }


        [WebMethod]
        public string StaffLogin4(string DeviceCode, int VendorID, int EventID, string StaffCode, int StaffPin, int ShiftNo)
        {
            try
            {
                var staffs = db.Staffs.Where(x => x.CurrentVendorID == VendorID && x.CurrentStaffCode == StaffCode && x.CurrentDevicePin == StaffPin);

                if (staffs != null && staffs.Count() > 0)
                {

                    var staff = staffs.First();
                    int DeviceID = DeviceRepository.GetByCode(DeviceCode).ID;

                    DateTime LoginDate = DateTime.Now;

                    DeviceLogin dl = new DeviceLogin { DeviceID = DeviceID, StaffID = staff.ID, LogTime = LoginDate, LogInTypeID = 1, EventID = EventID, VendorID = VendorID, ShiftNo = ShiftNo };


                    db.DeviceLogins.Add(dl);
                    db.SaveChanges();

                    return String.Format("{0}:{1}", staff.ID, StaffCode);
                }
                else
                {
                    return "Staff Login Failed.";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();

            }


        }


        //TenderAmount5 - no ws

        /// <summary>
        /// Returns user name, pin and available funds
        /// </summary>
        /// <param name="TagNumber"></param>
        /// <returns></returns>
        [WebMethod]
        public string ScanTagForPayment6(string TagNumber)
        {
            string result = "User Not Found.";
            decimal availableFunds = 0.00M;
            int tagPin = 0;
            //retun UserName, PIN, AvailableFunds
            EndUser user = EndUserRepository.GetByTagNumber(TagNumber);
            NFCTag tag = NFCTagRepository.GetByTagNumber(TagNumber);
            int Cancelled = StatusHelper.StatusID("NFCTag", "Cancelled");

            if (tag != null && tag.StatusID == Cancelled)
                return "Error. Active Tag Not Found.";
            if (tag == null)
                return "Error. Tag Not Found.";

            if (user != null)
            {

                var miiFunds = db.MyMoneys.Where(x => x.EndUserID == user.ID);
                int latestTransaction = miiFunds.Max(x => x.ID);
                var miiFund = miiFunds.Where(x => x.ID == latestTransaction).First();

                availableFunds = MyMoneyRepository.CalculateMyAvailableFundsFromAllTransactions(user.ID);//(decimal)miiFund.RunningBalance;

                if (tag != null)
                {
                    tagPin = (int)tag.TagPin;
                    result = String.Format("{0} {1};{2};{3}", user.Firstname, user.Surname, tagPin, availableFunds.ToString("0.00"));
                }


            }

            return result;

        }


        [WebMethod]

        //     use when asking for pin   public string PaymentResult7(int PIN, string TagNumber, int Amount, int EventID, int VendorID, string DeviceCode, int StaffID, int ShiftNo)

        public string PaymentResult7(string TagNumber, int Amount, int EventID, int VendorID, string DeviceCode, int StaffID, int ShiftNo)
        {
            string DeductFundsResult = "";

            int DeviceID = DeviceRepository.GetByCode(DeviceCode).ID;

            int CashLessTransactTypeID = StatusHelper.TransactionTypeID("Cashless Purchase");
            int PromotionDiscountTTID = StatusHelper.TransactionTypeID("Promotion Discount");


            int Cancelled = StatusHelper.StatusID("NFCTag", "Cancelled");
            var tags1 = db.NFCTags.Where(x => x.TagNumber == TagNumber  && x.StatusID != Cancelled);

            if (tags1 == null || tags1.Count() == 0)//PIN may have failed so check if Tag number is valid
            {
                var tags2 = db.NFCTags.Where(x => x.TagNumber == TagNumber && x.StatusID != Cancelled);
                int VendorPIN = db.Vendors.Find(VendorID).VendorPin ?? 0;
                if (tags2 != null && tags2.Count() > 0 )
                {
                    //Vendor override successful so put tag2 into tag1 and proceed
                    tags1 = tags2;
                }
            }

            if (tags1 != null && tags1.Count() > 0)
            {

                var tag = tags1.First();

                POSTagScan posTagScan = LogTagScan(tag);

                int TagScanID = posTagScan.ID;


                var endUser = db.EndUsers.Find(tag.EndUserID);
                if (endUser != null)
                {

                    //DeductFundsResult = String.Format("Name: {0}, Surname: {1},", endUser.Firstname, endUser.Surname);

                    DateTime EndTime = DateTime.Now;

                    if (BalanceCheckAfterDeduct(Amount, endUser.ID, EventID, VendorID) >= 0)
                    {
                        var UniquePaymentID = Helpers.PaymentHelper.UniqueRefNo(endUser.Firstname, endUser.Surname);

                        var _RunningBalance = BalanceCheck(endUser.ID) - Amount;
                        //Sufficient Funds
                        MyMoney transaction = new MyMoney
                        {
                            Amount = Amount * -1,
                            RunningBalance = BalanceCheck(endUser.ID) - Amount,
                            Description = "Vendor POS Payment",
                            TransactionDate = EndTime,
                            TransactionTypeID = CashLessTransactTypeID,
                            EndUserID = endUser.ID,
                            DateTimeUpdated = null,
                            EventID = EventID,
                            VendorID = VendorID,
                            StaffID = StaffID,
                            DeviceID = DeviceID,
                            Reference = UniquePaymentID

                        };
                        db.MyMoneys.Add(transaction);
                        db.SaveChanges();

                        decimal PromotionDiscount = 0;

                        PromotionDiscount = DoIQualifyForADiscount(endUser.ID, EventID, VendorID, Amount);

                        if (PromotionDiscount > 0)
                        {
                            var _RunningBalanceP = BalanceCheck(endUser.ID) + PromotionDiscount;
                            //Sufficient Funds
                            MyMoney transactionP = new MyMoney
                            {
                                Amount = PromotionDiscount * 1,
                                RunningBalance = BalanceCheck(endUser.ID) + PromotionDiscount,
                                Description = "Vendor POS Promotion Discount",
                                TransactionDate = EndTime,
                                TransactionTypeID = PromotionDiscountTTID,
                                EndUserID = endUser.ID,
                                DateTimeUpdated = null,
                                EventID = EventID,
                                VendorID = VendorID,
                                StaffID = StaffID,
                                DeviceID = DeviceID,
                                Reference = UniquePaymentID

                            };
                            db.MyMoneys.Add(transactionP);
                            db.SaveChanges();

                            _RunningBalance = _RunningBalanceP;

                        }

                        POSTransaction posTransaction = new POSTransaction
                        {
                            POSTagScanID = TagScanID,
                            DeviceID = DeviceID,
                            EndTime = EndTime,
                            EventID = EventID,
                            EndUserID = endUser.ID,
                            StaffID = StaffID,
                            VendorID = VendorID,
                            ShiftNo = ShiftNo,
                            Amount = Amount


                        };

                        db.POSTransactions.Add(posTransaction);
                        db.SaveChanges();

                        if (PromotionDiscount > 0)
                        {
                            POSTransaction posTransactionP = new POSTransaction
                            {
                                POSTagScanID = TagScanID,
                                DeviceID = DeviceID,
                                EndTime = EndTime,
                                EventID = EventID,
                                EndUserID = endUser.ID,
                                StaffID = StaffID,
                                VendorID = VendorID,
                                ShiftNo = ShiftNo,
                                Amount = -PromotionDiscount

                            };
                            db.POSTransactions.Add(posTransactionP);
                            db.SaveChanges();


                        }
                        string PromoResult = "";
                        if (PromotionDiscount > 0)
                        {
                            PromoResult = String.Format("Promotion Discount of R {0} applied.", PromotionDiscount);
                        }

                        DeductFundsResult = String.Format("Transaction Successful. R {2} deducted from {0} {1}'s Mii-funds account. {4} Balance remaining: {3}", endUser.Firstname, endUser.Surname, Amount.ToString(), _RunningBalance.ToString(), PromoResult);
                    }
                    else
                    {

                        DeductFundsResult = "Transaction Declined. Insufficient Funds.";
                    }



                }
                else
                {

                    DeductFundsResult = "User Not Found.";
                }


            }
            else
            {
                DeductFundsResult = "Tag or Pin invalid.";
            }
            return DeductFundsResult;

        }


        [WebMethod]
        public string PaymentResult7NoPin(string TagNumber, int Amount, int EventID, int VendorID, string DeviceCode, int StaffID, int ShiftNo, string UniqueID="")
        {

            if (MyMoneyRepository.UniqueIDExistsOnPostransaction(UniqueID))
            {
                return "Transaction has already been approved.";
            }

            string DeductFundsResult = "";

            int DeviceID = DeviceRepository.GetByCode(DeviceCode).ID;

            int CashLessTransactTypeID = StatusHelper.TransactionTypeID("Cashless Purchase");
            int PromotionDiscountTTID = StatusHelper.TransactionTypeID("Promotion Discount");
            int Cancelled = StatusHelper.StatusID("NFCTag", "Cancelled");

            var tags1 = db.NFCTags.Where(x => x.TagNumber == TagNumber && x.StatusID != Cancelled);




            if (tags1 != null && tags1.Count() > 0)
            {

                var tag = tags1.First();

                POSTagScan posTagScan = LogTagScan(tag);

                int TagScanID = posTagScan.ID;


                var endUser = db.EndUsers.Find(tag.EndUserID);
                if (endUser != null)
                {

                    //DeductFundsResult = String.Format("Name: {0}, Surname: {1},", endUser.Firstname, endUser.Surname);

                    DateTime EndTime = DateTime.Now;

                    if (BalanceCheckAfterDeduct(Amount, endUser.ID, EventID, VendorID) >= 0)
                    {
                        var UniquePaymentID = Helpers.PaymentHelper.UniqueRefNo(endUser.Firstname, endUser.Surname);

                        var _RunningBalance = BalanceCheck(endUser.ID) - Amount;
                        //Sufficient Funds
                        MyMoney transaction = new MyMoney
                        {
                            Amount = Amount * -1,
                            RunningBalance = BalanceCheck(endUser.ID) - Amount,
                            Description = "Vendor POS Payment",
                            TransactionDate = EndTime,
                            TransactionTypeID = CashLessTransactTypeID,
                            EndUserID = endUser.ID,
                            DateTimeUpdated = null,
                            EventID = EventID,
                            VendorID = VendorID,
                            StaffID = StaffID,
                            DeviceID = DeviceID,
                            Reference = UniquePaymentID,
                            UniqueID = UniqueID

                        };
                        db.MyMoneys.Add(transaction);
                        db.SaveChanges();

                        decimal PromotionDiscount = 0;

                        PromotionDiscount = DoIQualifyForADiscount(endUser.ID, EventID, VendorID, Amount);

                        if (PromotionDiscount > 0)
                        {
                            var _RunningBalanceP = BalanceCheck(endUser.ID) + PromotionDiscount;
                            //Sufficient Funds
                            MyMoney transactionP = new MyMoney
                            {
                                Amount = PromotionDiscount * 1,
                                RunningBalance = BalanceCheck(endUser.ID) + PromotionDiscount,
                                Description = "Vendor POS Promotion Discount",
                                TransactionDate = EndTime,
                                TransactionTypeID = PromotionDiscountTTID,
                                EndUserID = endUser.ID,
                                DateTimeUpdated = null,
                                EventID = EventID,
                                VendorID = VendorID,
                                StaffID = StaffID,
                                DeviceID = DeviceID,
                                Reference = UniquePaymentID,
                                UniqueID=UniqueID

                            };
                            db.MyMoneys.Add(transactionP);
                            db.SaveChanges();

                            _RunningBalance = _RunningBalanceP;

                        }


                        POSTransaction posTransaction = new POSTransaction
                        {
                            POSTagScanID = TagScanID,
                            DeviceID = DeviceID,
                            EndTime = EndTime,
                            EventID = EventID,
                            EndUserID = endUser.ID,
                            StaffID = StaffID,
                            VendorID = VendorID,
                            ShiftNo = ShiftNo,
                            Amount = Amount,
                            UniqueID = UniqueID
                           

                        };

                        db.POSTransactions.Add(posTransaction);
                        db.SaveChanges();

                        if (PromotionDiscount > 0)
                        {
                            POSTransaction posTransactionP = new POSTransaction
                            {
                                POSTagScanID = TagScanID,
                                DeviceID = DeviceID,
                                EndTime = EndTime,
                                EventID = EventID,
                                EndUserID = endUser.ID,
                                StaffID = StaffID,
                                VendorID = VendorID,
                                ShiftNo = ShiftNo,
                                Amount = -PromotionDiscount    ,
                                UniqueID = UniqueID
                            };
                            db.POSTransactions.Add(posTransactionP);
                            db.SaveChanges();


                        }
                        string PromoResult = "";
                        if (PromotionDiscount > 0)
                        {
                            PromoResult = String.Format("Promotion Discount of R {0} applied.", PromotionDiscount);
                        }


                        DeductFundsResult = String.Format("Transaction Successful. R {2} deducted from {0} {1}'s Mii-funds account. {4} Balance remaining: {3}", endUser.Firstname, endUser.Surname, Amount.ToString(), _RunningBalance.ToString(), PromoResult);
                    }
                    else
                    {

                        DeductFundsResult = "Transaction Declined. Insufficient Funds.";
                    }



                }
                else
                {

                    DeductFundsResult = "User Not Found.";
                }


            }
            else
            {
                DeductFundsResult = "Tag or Pin invalid.";
            }
            return DeductFundsResult;

        }




        [WebMethod]
        public string ScanTagForPaymentRefund6(string TagNumber)
        {
            string result = "User Not Found.";
            decimal availableFunds = 0.00M;
            int tagPin = 0;
            //retun UserName, PIN, AvailableFunds
            EndUser user = EndUserRepository.GetByTagNumber(TagNumber);
            NFCTag tag = NFCTagRepository.GetByTagNumber(TagNumber);

            int Cancelled = StatusHelper.StatusID("NFCTag", "Cancelled");

            if (tag != null && tag.StatusID == Cancelled)
                return "Error. Active Tag Not Found.";
            if (tag == null)
                return "Error. Tag Not Found.";

            if (user != null)
            {

                var miiFunds = db.MyMoneys.Where(x => x.EndUserID == user.ID);
                int latestTransaction = miiFunds.Max(x => x.ID);
                var miiFund = miiFunds.Where(x => x.ID == latestTransaction).First();

                //availableFunds = (decimal)miiFund.RunningBalance;
                availableFunds = MyMoneyRepository.CalculateMyAvailableFundsFromAllTransactions(user.ID);

                if (tag != null)
                {
                    tagPin = (int)tag.TagPin;
                }

                result = String.Format("{0} {1};{2};{3}", user.Firstname, user.Surname, tagPin, availableFunds.ToString("0.00"));
            }

            return result;

        }


        [WebMethod]
        public string PaymentResultRefund7(string TagNumber, int Amount, int EventID, int VendorID, string DeviceCode, int StaffID, int ShiftNo, string UniqueID="")
        {

            if (MyMoneyRepository.UniqueIDExistsOnPostransaction(UniqueID))
            {
                return "Transaction has already been approved.";
            }


            string DeductFundsResult = "";

            int DeviceID = DeviceRepository.GetByCode(DeviceCode).ID;

            int CashLessTransactTypeID = StatusHelper.TransactionTypeID("Cashless Refund");

            int Cancelled = StatusHelper.StatusID("NFCTag", "Cancelled");



            var tags1 = db.NFCTags.Where(x => x.TagNumber == TagNumber && x.StatusID != Cancelled);



            if (tags1 != null && tags1.Count() > 0)
            {

                var tag = tags1.First();

                POSTagScan posTagScan = LogTagScan(tag);

                int TagScanID = posTagScan.ID;


                var endUser = db.EndUsers.Find(tag.EndUserID);
                if (endUser != null)
                {

                    //DeductFundsResult = String.Format("Name: {0}, Surname: {1},", endUser.Firstname, endUser.Surname);

                    DateTime EndTime = DateTime.Now;


                    var UniquePaymentID = Helpers.PaymentHelper.UniqueRefNo(endUser.Firstname, endUser.Surname);

                    var _RunningBalance = BalanceCheck(endUser.ID) + Amount;
                    //Sufficient Funds
                    MyMoney transaction = new MyMoney
                    {
                        Amount = Amount,
                        RunningBalance = BalanceCheck(endUser.ID) + Amount,
                        Description = "Mobile Refund",
                        TransactionDate = EndTime,
                        TransactionTypeID = CashLessTransactTypeID,
                        EndUserID = endUser.ID,
                        DateTimeUpdated = null,
                        EventID = EventID,
                        VendorID = VendorID,
                        StaffID = StaffID,
                        DeviceID = DeviceID,
                        Reference = UniquePaymentID

                    };
                    db.MyMoneys.Add(transaction);
                    db.SaveChanges();

                    POSTransaction posTransaction = new POSTransaction
                    {
                        POSTagScanID = TagScanID,
                        DeviceID = DeviceID,
                        EndTime = EndTime,
                        EventID = EventID,
                        EndUserID = endUser.ID,
                        StaffID = StaffID,
                        VendorID = VendorID,
                        ShiftNo = ShiftNo,
                        Amount = -Amount



                    };

                    db.POSTransactions.Add(posTransaction);
                    db.SaveChanges();

                    DeductFundsResult = String.Format("Transaction Successful. R {2} refunded to {0} {1}'s Mii-funds account. Balance remaining: {3}", endUser.Firstname, endUser.Surname, Amount.ToString(), _RunningBalance.ToString());




                }
                else
                {

                    DeductFundsResult = "User Not Found or Active Tag Not Found.";
                }

            }

            return DeductFundsResult;

        }


        [WebMethod]
        public string AdminTopUp(string TagNumber, int Amount, int EventID, int VendorID, string DeviceCode, int StaffID, int ShiftNo, string UniqueID = "")
        {

            if (MyMoneyRepository.UniqueIDExistsOnPostransaction(UniqueID))
            {
                return "Transaction has already been approved.";
            }


            string DeductFundsResult = "";

            int DeviceID = DeviceRepository.GetByCode(DeviceCode).ID;

            int CashLessTransactTypeID = StatusHelper.TransactionTypeID("Cashless Refund");

            int Cancelled = StatusHelper.StatusID("NFCTag", "Cancelled");



            var tags1 = db.NFCTags.Where(x => x.TagNumber == TagNumber  && x.StatusID != Cancelled);



            if (tags1 != null && tags1.Count() > 0)
            {

                var tag = tags1.First();

                POSTagScan posTagScan = LogTagScan(tag);

                int TagScanID = posTagScan.ID;


                var endUser = db.EndUsers.Find(tag.EndUserID);
                if (endUser != null)
                {

                    //DeductFundsResult = String.Format("Name: {0}, Surname: {1},", endUser.Firstname, endUser.Surname);

                    DateTime EndTime = DateTime.Now;


                    var UniquePaymentID = Helpers.PaymentHelper.UniqueRefNo(endUser.Firstname, endUser.Surname);

                    var _RunningBalance = BalanceCheck(endUser.ID) + Amount;
                    //Sufficient Funds
                    MyMoney transaction = new MyMoney
                    {
                        Amount = Amount,
                        RunningBalance = BalanceCheck(endUser.ID) + Amount,
                        Description = "Mobile top-up",
                        TransactionDate = EndTime,
                        TransactionTypeID = CashLessTransactTypeID,
                        EndUserID = endUser.ID,
                        DateTimeUpdated = null,
                        EventID = EventID,
                        VendorID = VendorID,
                        StaffID = StaffID,
                        DeviceID = DeviceID,
                        Reference = UniquePaymentID

                    };
                    db.MyMoneys.Add(transaction);
                    db.SaveChanges();

                    POSTransaction posTransaction = new POSTransaction
                    {
                        POSTagScanID = TagScanID,
                        DeviceID = DeviceID,
                        EndTime = EndTime,
                        EventID = EventID,
                        EndUserID = endUser.ID,
                        StaffID = StaffID,
                        VendorID = VendorID,
                        ShiftNo = ShiftNo,
                        Amount = -Amount



                    };

                    db.POSTransactions.Add(posTransaction);
                    db.SaveChanges();

                    DeductFundsResult = String.Format("Transaction Successful. R {2} refunded to {0} {1}'s Mii-funds account. Balance remaining: {3}", endUser.Firstname, endUser.Surname, Amount.ToString(), _RunningBalance.ToString());




                }
                else
                {

                    DeductFundsResult = "User Not Found or Active Tag Not Found.";
                }

            }

            return DeductFundsResult;

        }







        //Reprint Slip 8 - no ws

        [WebMethod]
        public string CloseShift9(int VendorPin, string VendorCode, int VendorID, int EventID, string DeviceCode, int ShiftNo, int StaffID)
        {
            int DeviceID = DeviceRepository.GetByCode(DeviceCode).ID;
            string ShiftLogOffResult = "Shift Log Off Successful";

            try
            {
                if (ShiftNo != 0)
                {
                    if (VendorLogin(VendorPin, VendorCode))
                    {

                        StaffLogoff(VendorID, StaffID, EventID, DeviceID, ShiftNo);
                        ShiftLogOffResult = CloseShiftTransactionReport(ShiftNo, EventID, VendorID);
                    }
                    else
                    {
                        ShiftLogOffResult = "Invalid Vendor Credentials";

                    }
                }
                else
                {
                    ShiftLogOffResult = "Shift Already Closed.";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return ShiftLogOffResult;
        }


        #endregion


        #region Internal Methods

        private string CloseShiftTransactionReport(int ShiftNo, int EventID, int VendorID)
        {

            var sb = new StringBuilder();
            var emailBody = new StringBuilder();

            var posTransactions = db.POSTransactions.Where(x => x.EventID == EventID && x.VendorID == VendorID && x.ShiftNo == ShiftNo).ToList();

            decimal total = (decimal)posTransactions.Sum(x => x.Amount);

            if (posTransactions.Count() > 0)
            {
                DateTime closingTime = (DateTime)posTransactions.Max(x => x.EndTime);

                emailBody.AppendLine("<table>");
                foreach (var pos in posTransactions.OrderBy(x => x.EndTime))
                {
                    sb.Append(((DateTime)pos.EndTime).ToString("hh:mm:ss"));
                    sb.Append("|");
                    sb.Append(((decimal)pos.Amount).ToString("0.00"));
                    sb.Append("|");
                    if (pos.Amount >= 0) { sb.Append("SALE"); }
                    else { sb.Append("RFND"); }
                    sb.Append(";");




                    emailBody.AppendLine("<tr><td>");
                    emailBody.Append(((DateTime)pos.EndTime).ToString("hh:mm:ss"));
                    emailBody.Append("</td><td>");
                    emailBody.Append(((decimal)pos.Amount).ToString("0.00"));
                    emailBody.Append("</td><td>");
                    if (pos.Amount >= 0) { sb.Append("SALE"); }
                    else { emailBody.Append("RFND"); }
                    emailBody.Append("</td></tr>");


                }

                emailBody.Append(String.Format("<tr><td>TOTAL: {0}", total.ToString("0.00")));
                emailBody.Append("</td></tr>");
                emailBody.AppendLine("</table>");

                sb.Append(String.Format("TOTAL|{0}|TOTAL", total.ToString("0.00")));

                VendorRepository.SendEODReport(VendorID, EventID, emailBody.ToString(), ShiftNo, closingTime);
            }
            else
            {
                sb.Append("No Pos Transactions.| |;");
                emailBody.Append("No Pos Transactions.");

                VendorRepository.SendEODReport(VendorID, EventID, emailBody.ToString(), ShiftNo, DateTime.Now);
            }
            return sb.ToString();
        }

        private bool VendorLogin(int VendorPin, string VendorCode)
        {
            var vendors = db.Vendors.Where(x => x.VendorPin == VendorPin && x.VendorCode == VendorCode);
            if (vendors.Count() > 0)
            {
                Vendor vendor = vendors.First();
                if (vendor != null)
                {

                    return true;

                }

            }
            return false;

        }


        private string StaffLogoff(int CurrentVendorID, int StaffID, int EventID, int DeviceID, int ShiftNo)
        {
            string StaffLoginResult = "Invalid credentials";



            LogOffDevice(StaffID, DeviceID, DateTime.Now, EventID, CurrentVendorID, ShiftNo);

            StaffLoginResult = "Staff Logged Off Successfully. Shift Number: " + ShiftNo.ToString();


            return StaffLoginResult;


        }

        private decimal BalanceCheckAfterDeduct(decimal Amount, int UserID, int EventID, int VendorID)
        {
            decimal PromotionDiscount = 0;

            PromotionDiscount = DoIQualifyForADiscount(UserID, EventID, VendorID, Amount);
            //If my discount is 20 and the amount is 50 i actually only need 30 in my account
            //so deduct the 20 from the 50 before doing the balance check Plez
            Amount = Amount - PromotionDiscount;

            var miiFunds = db.MyMoneys.Where(x => x.EndUserID == UserID);
            if (miiFunds != null && miiFunds.Count() > 0)
            {
                int latestTransaction = miiFunds.Max(x => x.ID);
                var miiFund = miiFunds.Where(x => x.ID == latestTransaction).First();

                return (decimal)(miiFund.RunningBalance ?? 0.0M) - Amount;
            }
            else
                return -1.0M;
        }

        private decimal DoIQualifyForADiscount(int userID, int? eventID, int? vendorID, decimal? fulltransactionAmount)
        {
            return decimal.Parse(MyMoneyRepository.DoIQualifyForADiscount(userID, eventID, vendorID, fulltransactionAmount));

        }

        private decimal BalanceCheck(int UserID)
        {
            var miiFunds = db.MyMoneys.Where(x => x.EndUserID == UserID);
            int latestTransaction = miiFunds.Max(x => x.ID);
            var miiFund = miiFunds.Where(x => x.ID == latestTransaction).First();

            return (decimal)miiFund.RunningBalance;

        }

        private string VendorLoginDeviceToStartShift(string DeviceCode, int VendorID, int EventID)
        {
            try
            {
                int DeviceID = DeviceRepository.GetByCode(DeviceCode).ID;

                DateTime LoginDate = DateTime.Now;

                DeviceLogin dl = new DeviceLogin { DeviceID = DeviceID, StaffID = null, LogTime = LoginDate, LogInTypeID = 1, EventID = EventID, VendorID = VendorID };
                int LastShiftNo = 0;

                // DateTime startOfDay = LoginDate.Date;
                // DateTime endOfDay = startOfDay.AddDays(1).AddMinutes(-1);

                var deviceLoginsForAllTime = db.DeviceLogins.Where(x => x.VendorID == VendorID && x.EventID == EventID && x.LogInTypeID == 1);
                if (deviceLoginsForAllTime.Count() > 0)
                {
                    LastShiftNo = deviceLoginsForAllTime.Max(x => (int)x.ShiftNo);
                }

                dl.ShiftNo = LastShiftNo + 1;
                db.DeviceLogins.Add(dl);
                db.SaveChanges();

                return String.Format("{0}", ((int)(dl.ShiftNo)).ToString());

            }
            catch (Exception ex)
            {

                return ex.Message.ToString();
            }
        }

        private POSTagScan LogTagScan(NFCTag tag)
        {
            POSTagScan posTagScan = new POSTagScan
            {
                NFCTagID = tag.ID,
                EndUserID = null,
                PinOK = null,
                ScanTime = DateTime.Now

            };

            db.POSTagScans.Add(posTagScan);
            db.SaveChanges();
            return posTagScan;
        }




        private void LogOffDevice(int StaffID, int DeviceID, DateTime LogOffDate, int EventID, int VendorID, int ShiftNo)
        {

            DeviceLogin dl = new DeviceLogin { DeviceID = DeviceID, StaffID = StaffID, LogTime = LogOffDate, LogInTypeID = 0, EventID = EventID, VendorID = VendorID, ShiftNo = ShiftNo };

            db.DeviceLogins.Add(dl);

            db.SaveChanges();
        }

        #endregion

        #endregion



        #region  Daniel Tests - Not For Live Use!

        [WebMethod]
        public string SendConfirmationInstantEFT_TicketPurchase(int EndUserID, int amount_gross, string m_payment_id, string baseUrl)
        {
            try
            {
                var bt = BankTransactionRepository.UpdateBankTransactionStatus("Approved Ticket Purchase", m_payment_id, "Payfast System Response");


                List<TicketViewModel> boughtTickets = TicketRepository.ConfirmTickets(m_payment_id);
                string EventName = boughtTickets.First().Event.EventName;
            
                MyMoneyRepository.SendConfirmationInstantEFT_TicketPurchase(EndUserID, amount_gross, m_payment_id, bt.TransactionDate, boughtTickets, EventName, baseUrl, subdomain: "miid");


                MyMoneyRepository.SendConfirmationManualEFT_TicketPurchase(EndUserID, amount_gross, m_payment_id, bt.TransactionDate, boughtTickets, baseUrl, subdomain: "miid");

                return "Success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        #endregion
    }
}
