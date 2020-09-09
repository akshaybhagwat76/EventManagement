using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MiidWeb;
using MiidWeb.Models;
using MiidWeb.Repositories;
using System.Web.Routing;
using System.IO;
using MiidWeb.Helpers;
using System.Drawing;
using System.Configuration;
using PagedList;
using MiidWeb.Navigation;
using Newtonsoft.Json;
using System.Text;

namespace MiidWeb.Controllers
{
    public class EventImageModel
    {
        public int ID { get; set; }
        public int EventID { get; set; }

    }


    public class EventsController : BaseController
    {
        private MiidEntities db = new MiidEntities();

        private void GetLayoutFiles()
        {

            string host = HttpContext.Request.Url.Host;

            //  string company = host.Split('.')[0].ToString();
            string company = host;

            //  if (company.Contains("www")) {  company = "training.miid.co.za"; }
             if (company.Contains("localhost")) { company = "miid.co.za"; }
            if (company.ToLower().Contains("gigCulture.miid.co.za")) { company = "GigCulture.miid.co.za"; }
            if (company.ToLower().Contains("farmyardpark.miid.co.za")) { company = "FarmYardPark.miid.co.za"; }
            if (company.ToLower().Contains("www.lulatickets.co.za")) { company = "lulatickets.co.za"; }
            if (company.ToLower().Contains("lulatickets.co.za")) { company = "lulatickets.co.za"; }

            GlobalVariables.Company = company;

            int subDomainID = LookupHelper.GetSubdomainID(company);
            using (var db = new MiidEntities())
            {
                Subdomain subDomain = db.Subdomains.Find(subDomainID);
                if (subDomain != null)
                {
                    GlobalVariables.SubdomainID = subDomain.ID.ToString();
                    GlobalVariables.Text1 = subDomain.Text1;
                    GlobalVariables.Text2 = subDomain.Text2;
                    GlobalVariables.Text3 = subDomain.Text3;
                    GlobalVariables.Text4 = subDomain.Text4;
                    GlobalVariables.Text5 = subDomain.Text5;
                    GlobalVariables.Text6 = subDomain.Text6;
                    GlobalVariables.LoginParaOne = subDomain.LoginParaOne;
                    GlobalVariables.LoginParatwo = subDomain.LoginParatwo;
                    GlobalVariables.HomeParaOne = subDomain.HomeParaOne;
                    GlobalVariables.HomeParaTwo = subDomain.HomeParatwo;
                    GlobalVariables.HomeImageOne = subDomain.HomeImageOne;
                    GlobalVariables.HomeImagetwo = subDomain.HomeImagetwo;
                    GlobalVariables.HomeImagethree = subDomain.HomeImagethree;
                    GlobalVariables.HomeImageFour = subDomain.HomeImageFour;
                    GlobalVariables.HomeImageOneLink = subDomain.HomeImageOneLink;
                    GlobalVariables.HomeImageTwoLink = subDomain.HomeImageTwoLink;
                    GlobalVariables.HomeImageThreeLink = subDomain.HomeImageThreeLink;
                    GlobalVariables.HomeImageFourLink = subDomain.HomeImageFourLink;
                    GlobalVariables.HomeImageOneText = subDomain.HomeImageOneText;
                    GlobalVariables.HomeImageTwoText = subDomain.HomeImageTwoText;
                    GlobalVariables.HomeImageFourText = subDomain.HomeImageThreeText;
                    GlobalVariables.HomeImageFourLink = subDomain.HomeImageFourText;
                    GlobalVariables.HomeCardOneHeading = subDomain.HomeCardOneHeading;
                    GlobalVariables.HomeCardTwoHeading = subDomain.HomeCardTwoHeading;
                    GlobalVariables.HomeCardThreeHeading = subDomain.HomeCardThreeHeading;
                    GlobalVariables.HomeCardOnePara = subDomain.HomeCardOnePara;
                    GlobalVariables.HomeCardTwoPara = subDomain.HomeCardTwoPara;
                    GlobalVariables.HomeCardThreePara = subDomain.HomeCardThreePara;
                    GlobalVariables.CallToActionOne = subDomain.CallToActionOne;
                    GlobalVariables.CallToActionTwo = subDomain.CallToActionTwo;
                    GlobalVariables.CallToActionThree = subDomain.CallToActionThree;
                    GlobalVariables.CallToActionAll = subDomain.CallToActionAll;

                    GlobalVariables.PurchaseContainerHeading = subDomain.PurchaseContainerHeading;
                    GlobalVariables.PurchaseInfo = subDomain.PurchaseInfo;
                    GlobalVariables.PurchaseDetails = subDomain.PurchaseDetails;
                    GlobalVariables.PurchaseExtra = subDomain.PurchaseExtra;
                    GlobalVariables.CallToActionPurchase = subDomain.CallToActionPurchase;

                    GlobalVariables.PurchaseTermsOne = subDomain.PurchaseTermsOne;
                    GlobalVariables.PurchaseTermsTwo = subDomain.PurchaseTermsTwo;
                    GlobalVariables.PurchaseTermsThree = subDomain.PurchaseTermsThree;

                    GlobalVariables.PurchaseSoldOut = subDomain.PurchaseSoldOut;
                    GlobalVariables.Currency = subDomain.Currency;
                    GlobalVariables.CustomTerms = subDomain.CustomTerms;

                    GlobalVariables.PurchaseNoAvailiblity = subDomain.PurchaseNoAvailiblity;
                    GlobalVariables.PurchaseFilter = subDomain.PurchaseFilter;
                    GlobalVariables.PurchaseFindTickets = subDomain.PurchaseFindTickets;
                    GlobalVariables.PurchaseRowNumber = subDomain.PurchaseRowNumber;
                    GlobalVariables.PurchaseSeatNumber = subDomain.PurchaseSeatNumber;
                    GlobalVariables.PurchaseTotalQunatity = subDomain.PurchaseTotalQunatity;
                    GlobalVariables.PurchaseSubTotal = subDomain.PurchaseSubTotal;
                    GlobalVariables.PurchaseTotal = subDomain.PurchaseTotal;
                    GlobalVariables.PurchaseArea = subDomain.PurchaseArea;
                    GlobalVariables.SeatingPlanImage = subDomain.SeatingPlanImage;

                    GlobalVariables.MyEventsHeading = subDomain.MyEventsHeading;
                    GlobalVariables.MyEventsParaOne = subDomain.MyEventsParaOne;
                    GlobalVariables.MyEventsParaTwo = subDomain.MyEventsParaTwo;
                    GlobalVariables.MyEventsParaHelpHeading = subDomain.MyEventsParaHelpHeading;
                    GlobalVariables.MyEventsHelpOne = subDomain.MyEventsHelpOne;
                    GlobalVariables.MyEventsHelpTwo = subDomain.MyEventsHelpTwo;
                    GlobalVariables.MyEventsHelpThree = subDomain.MyEventsHelpThree;
                    GlobalVariables.MyEventsDownLoadHeading = subDomain.MyEventsDownLoadHeading;
                    GlobalVariables.ButtonDownload = subDomain.ButtonDownload;
                    GlobalVariables.ButtonPending = subDomain.ButtonPending;
                    GlobalVariables.MyEventsHelpHeadingOne = subDomain.MyEventsHelpHeadingOne;
                    GlobalVariables.MyEventsHelpHeadingTwo = subDomain.MyEventsHelpHeadingTwo;
                    GlobalVariables.MyEventsHelpHeadingThree = subDomain.MyEventsHelpHeadingThree;

                    GlobalVariables.CreateFirstHeading = subDomain.CreateFirstHeading;
                    GlobalVariables.CreateFirstParaOne = subDomain.CreateFirstParaOne;
                    GlobalVariables.CreateFirstFirstName = subDomain.CreateFirstFirstName;
                    GlobalVariables.CreateFirstFirstNameSub = subDomain.CreateFirstFirstNameSub;
                    GlobalVariables.CreateFirstLastName = subDomain.CreateFirstLastName;
                    GlobalVariables.CreateFirstLastNameSub = subDomain.CreateFirstLastNameSub;
                    GlobalVariables.CreateFirstCell = subDomain.CreateFirstCell;
                    GlobalVariables.CreateFirstCellSub = subDomain.CreateFirstCellSub;
                    GlobalVariables.CreateFirstID = subDomain.CreateFirstID;
                    GlobalVariables.CreateFirstIDSub = subDomain.CreateFirstIDSub;
                    GlobalVariables.CreateFirstEmail = subDomain.CreateFirstEmail;
                    GlobalVariables.CreateFirstSeucrePurchase = subDomain.CreateFirstSeucrePurchase;
                    GlobalVariables.Terms = subDomain.Terms;

                    GlobalVariables.ChooseTenderHeadingOne = subDomain.ChooseTenderHeadingOne;
                    GlobalVariables.ChooseTenderHeadingTwo = subDomain.ChooseTenderHeadingTwo;
                    GlobalVariables.ChooseTenderParaOne = subDomain.ChooseTenderParaOne;
                    GlobalVariables.ChooseTenderParaTwo = subDomain.ChooseTenderParaTwo;
                    GlobalVariables.ChooseTenderButtonTwo = subDomain.ChooseTenderButtonTwo;
                    GlobalVariables.ChooseTenderButtonTwo = subDomain.ChooseTenderButtonTwo;
                    GlobalVariables.ChooseTenderParaSubOne = subDomain.ChooseTenderParaSubOne;
                    GlobalVariables.ChooseTenderParaSubTwo = subDomain.ChooseTenderParaSubTwo;

                    GlobalVariables.ButtonAllEvents = subDomain.ButtonAllEvents;
                    GlobalVariables.ShowPassword = subDomain.ShowPassword;
                    GlobalVariables.ForgotpasswordText = subDomain.ForgotpasswordText;
                    GlobalVariables.RegisterParaOne = subDomain.RegisterParaOne;
                    GlobalVariables.RegisterPasswordLength = subDomain.RegisterPasswordLength;

                    GlobalVariables.VoucherHeading = subDomain.VoucherHeading;
                    GlobalVariables.VoucherPara = subDomain.VoucherPara;

                    GlobalVariables.CardPaymentHeading = subDomain.CardPaymentHeading;
                    GlobalVariables.ButtonBack = subDomain.ButtonBack;
                    GlobalVariables.ButtonConfirm = subDomain.ButtonConfirm;
                    GlobalVariables.CardPaymentPara = subDomain.CardPaymentPara;



                }
            }
            if (company == "miid")
            {
                GlobalVariables.LayoutLogin = "~/Views/Shared/_LayoutLogin.cshtml";
                GlobalVariables.LayoutAdmin = "~/Views/Shared/_LayoutAdmin.cshtml";
                GlobalVariables.Layout = MiidWeb.Repositories.TicketRepository.GetLayoutFile("Layout");

            }
            else
            {
                GlobalVariables.LayoutLogin = String.Format("~/Views/Shared/_LayoutLogin{0}.cshtml", company);
                GlobalVariables.LayoutAdmin = String.Format("~/Views/Shared/_LayoutAdmin{0}.cshtml", company);
                GlobalVariables.Layout = String.Format("~/Views/Shared/_Layout{0}.cshtml", company);

            }


           

        }

        #region Admin Functions
        [Authorize(Roles = "Admin")]

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";


            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var events = db.Events.Include(t => t.EventOrganiser);

            if (!String.IsNullOrEmpty(searchString))
            {
                events = events.Where(s => s.EventName.Contains(searchString)
                                    
                                

                                       );


            }

            switch (sortOrder)
            {
                
               
                case "date_desc":
                    events = events.OrderByDescending(s => s.EventName);
                    break;
                default:  // Name ascending 
                    events = events.OrderBy(s => s.EventCategoryID);
                    break;
            }

            int pageSize = 30;
            int pageNumber = (page ?? 1);

            return View(events.ToPagedList(pageNumber, pageSize));
        }

        #endregion


        #region End User Functions
        // GET: Events
        [Authorize]

        public ActionResult MiiEvents()
        {

            var LoggedInUserID = Helpers.UserHelper.UserID(User.Identity.Name);

            var events = new Models.EventListViewModel(LoggedInUserID: LoggedInUserID, ShowCurrentEvents: true);

            if ((UserHelper.UserEmailConfirmed(User.Identity.Name)) == "Pending")
            {
                return RedirectToAction("ConfirmEmailLink", "EndUsers", new { email = User.Identity.Name });
            }

            return View(events);



        }



        //public ActionResult Search(int? EventCategoryID = null, int? EventOrganiserID = null, int? regionid = null, DateTime? eventmonth = null, string KeyWordSearchText = "", int? page = 1)
        public ActionResult Search(string KeyWordSearchText = "", int? page = 1)
        {

            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }

            int pageSize = 50;
            int pageNumber = (page ?? 1);

            EventSearchModel searchmodel = new EventSearchModel();
            int subDomainID = int.Parse(MiidWeb.Repositories.TicketRepository.GetSubdomainID());
            var events = new EventListViewModel(KeyWordSearchText, subDomainID);
            searchmodel.Events = events.Events.ToPagedList(pageNumber, pageSize);




            //var events = new Models.EventListViewModel(DateTime.Now, DateTime.Now.AddYears(10), LoggedInUserID, Descr);









            //var events = new Models.EventListViewModel(DateTime.Now, DateTime.Now.AddYears(10), LoggedInUserID, Descr);

            return View("Search", searchmodel);






        }

        // GET: Events/Details/5
        //[ReturnAfterAuthentication]
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (User.Identity.IsAuthenticated && UserHelper.UserEmailConfirmed(User.Identity.Name) != "Pending")
            {

                var LoggedInUserID = Helpers.UserHelper.UserID(User.Identity.Name);

                TicketRepository.DeleteExpiredReservedTickets((int)id, LoggedInUserID, 3);

                EventViewModel @event = new EventViewModel((int)id, LoggedInUserID);
                @event.CalendarViewModel = new CalendarViewModel(LoggedInUserID, DateTime.Now, 1);
                @event.CalculateTimeRemaining();
                //Flush expired tickets



                if (@event == null)
                {
                    return HttpNotFound();
                }

                //Growl("MiiD", "Your Ticket Purchasing facility is on hold for 5 minutes - account refreshing. Please try again in 5 minutes.");

                if (TicketRepository.DoIHaveTicketsInReservedState((int)id, LoggedInUserID, 3))
                {
                    ViewBag.TicketsOnHoldMessage = "While purchasing your ticket you may have closed your browser or clicked Back instead of selecting Cancel. Your purchasing facility is on hold for 3 minutes. We apologise for the delay however we try our best to give everyone a fair opportunity to purchase a ticket.";
                }
                return View(@event);
            }
            else
            {
                EventViewModel @event = new EventViewModel((int)id);
                return View("DetailsPreLogin", @event);

            }
        }

        #endregion


        #region Purchase Tickets


        //Get
        [Authorize]
        public ActionResult RefundTickets()
        {
            Growl("Important!", "Select your tickets you would like refunded");
            List<TicketViewModel> refundableTickets = TicketRepository.GetRefundableTickets(User.Identity.Name);
            ViewBag.RefundableTickets = refundableTickets;
            ViewBag.EndUserID = EndUserRepository.GetByUserName(User.Identity.Name);
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult RefundTickets(FormCollection form, RefundTicketViewModel model)
        {

            List<int> ticketsToRefund = new List<int>();

            foreach (var key in form.AllKeys)
            {
                if (key.StartsWith("chk"))
                {
                    var Selected = form[key];
                    if (Selected == "true,false" || Selected == "on") //then true
                    {
                        ticketsToRefund.Add(int.Parse(key.Substring(key.IndexOf('_') + 1)));
                    }
                }
                EndUserRepository.UpdateBankDetailsForRefund(model);
                TicketRepository.SetRequestedTicketRefunds(ticketsToRefund);
                Growl("Refund Request Received", "Please allow a few days for processing");

             



            }

            TicketRepository.SendRefundToCustomer(model.EndUserID);


            //Email MIID ADMIN
            Helpers.EmailHelper.SendMail("donavan.wallis@miid.co.za", "Refund Requested for user", "Please check refund list");
            Helpers.EmailHelper.SendMail("jonathan.wallis@miid.co.za", "Refund Requested for user", "Please check refund list");
            Helpers.EmailHelper.SendMail("info@miid.co.za", "Refund Requested for user", "Please check refund list");


            return RedirectToAction("MiiEvents");

        }

        public ActionResult RefundApprovals(int id = 0)
        {
            List<RefundTicketViewModelLite> list = new List<RefundTicketViewModelLite>();

            list = TicketRepository.GetRequestedTicketRefunds(id);

            return View(list);
        }

        [HttpPost]
        public ActionResult RefundApprovals(FormCollection form, List<RefundTicketViewModelLite> list, string userids)
        {

            List<int> ticketsRefunded = new List<int>();

            int RefundPaidOut = 0;
       

            RefundPaidOut = StatusHelper.StatusID("Ticket", "Refund Paid Out");
                 
            

            foreach (var key in form.AllKeys)
            {
                if (key.StartsWith("chk"))
                {
                    var Selected = form[key];
                    if (Selected == "true,false" || Selected == "on") //then true
                    {
                        ticketsRefunded.Add(int.Parse(key.Substring(key.IndexOf('_') + 1)));
                    }
                }
            }

            foreach (var t in ticketsRefunded)
            {
                TicketRepository.TicketRefunded(t);

            }
            foreach (string userid in userids.Split(','))
            {
                Int64 enduserID = 0;
                Int64.TryParse(userid, out enduserID);
                EndUserRepository.ClearBankDetailsAfterRefund(enduserID);
            }

            return RedirectToAction("RefundApprovals");
        }



        public ActionResult PurchaseTicketsPreLogin(int EventID = 0, string ReturnUrl = null, string ConfirmationMessage = null)
        {

            if ((UserHelper.UserEmailConfirmed(User.Identity.Name)) == "Pending")
            {
                Growl("Confirm your email", "We need to link your tickets and/or prepaid wallet to email. Please check your email for your confirmation link");

                return RedirectToAction("ConfirmEmailLink", "EndUsers", new { email = User.Identity.Name });
            }

            Growl("MiiD", "Sign in to purchase tickets");
            Session["EventID"] = EventID;

            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        //[Authorize] Removed DS


        public JsonResult ValidatePromoCode(string id, string promocode)
        {
            decimal value = 0.0M;
            string outputPromoCode = promocode;
            try
            {
                string[] selectedticketTypes = id.Split(';');
                bool OneIsValid = false; int count = 0;
                //"Promotion Code Applied"
                foreach (var tt in selectedticketTypes)
                {
                    if (!String.IsNullOrEmpty(tt))
                    {
                        count++;
                    }
                }
                foreach (var tt in selectedticketTypes)
                {
                    if (!String.IsNullOrEmpty(tt))
                    {
                        PromoCodeViewModel vm = TicketRepository.PromoCodeAvailable(promocode, int.Parse(tt));
                        if (vm != null)
                        {
                            value = vm.DiscountPercentage;
                            outputPromoCode = vm.First8;
                        }
                        OneIsValid = value > 0;

                        if (OneIsValid) break;
                    }
                }
                if (OneIsValid)
                {
                    return Json("Code Valid: Discount applied at checkout ," + outputPromoCode);
                }
                else
                {
                    if (count == 0)
                    {
                        return Json("No Tickets Selected");
                    }
                    else
                    {
                        if (value == 0)
                        {
                            return Json("Code Invalid");
                        }
                        else
                        {
                            return Json("Code Already Used");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }
            //return Json("Promo Code Valid");
        }

        public JsonResult GetTicketClassQuantities(string id)
        {
            List<TicketClassSeat> cats = new List<TicketClassSeat>();
            List<SelectListItem> subdistricts = new List<SelectListItem>();
            var seatRange = new TicketClassViewModel(int.Parse(id), 0);

            return Json(new SelectList(seatRange.TicketQuantityList, "Value", "Text"));

        }

        public JsonResult GetAvailableDays(string id, string eventid)
        {
            PurchaseTicketViewModel pc = new PurchaseTicketViewModel();

            int EventID = int.Parse(eventid);
            string SelectedMonth = id;

            return Json(new SelectList(pc.DaysAvailable(EventID, SelectedMonth), "Value", "Text"));

        }

        public JsonResult GetAvailableTickets(string id, string eventid)
        {
            PurchaseTicketViewModel pc = new PurchaseTicketViewModel();

            int EventID = int.Parse(eventid);
            string SelectedDate = id;

            return Json(new SelectList(pc.GetTicketsAvailable(EventID, SelectedDate), "Value", "Text"));

        }
        [HttpPost]
        public ActionResult GetTickets(int hdnEventID, string AvailableDays)
        {

            PurchaseTicketViewModel model = new PurchaseTicketViewModel();

            if (String.IsNullOrEmpty(AvailableDays) || AvailableDays == "0")
            {
                model = new PurchaseTicketViewModel(hdnEventID);
                model.TicketClasses = null;
            }
            else
            {
                model = new PurchaseTicketViewModel(hdnEventID, AvailableDays, true, true);
            }

            return View("PurchaseTickets", model);
        }


        public JsonResult GetRowNumbers(string id)
        {
            List<TicketClassSeatRange> cats = new List<TicketClassSeatRange>();
            List<SelectListItem> subdistricts = new List<SelectListItem>();
            var ticketClass = db.TicketClasses.Find(int.Parse(id));
            List<int> allseatsinrow = new List<int>();
            List<int> takenseatsinrow = new List<int>();

            cats = db.TicketClassSeatRanges.Where(x => x.TicketClassID == ticketClass.ID).OrderBy(x => x.RowNumber).ToList();

            foreach (var s in cats)
            {
                if (AreAllSeatsBookedInThisRow(s.ID.ToString()))
                {

                }
                else
                {
                    subdistricts.Add(new SelectListItem { Value = s.ID.ToString(), Text = s.RowNumber.ToString() });
                }
            }

            if (subdistricts.Count() == 0)
            {
                subdistricts.Add(new SelectListItem { Value = 0.ToString(), Text = 0.ToString() });
            }

            return Json(new SelectList(subdistricts, "Value", "Text"));
            //This will show available seat numbers for this row
        }

        public bool AreAllSeatsBookedInThisRow(string id)
        {
            List<TicketClassSeat> cats = new List<TicketClassSeat>();
            List<SelectListItem> subdistricts = new List<SelectListItem>();
            var seatRange = db.TicketClassSeatRanges.Find(int.Parse(id));
            List<int> allseatsinrow = new List<int>();
            List<int> takenseatsinrow = new List<int>();

            cats = db.TicketClassSeats.Where(x => x.TicketClassSeatRangeID == seatRange.ID).ToList();
            int xx = seatRange.FromSeatNumber ?? 0;
            while (xx <= seatRange.ToSeatNumber)
            {
                allseatsinrow.Add(xx);
                xx++;
            }

            foreach (var c in cats)
            {
                takenseatsinrow.Add(c.SeatNumber ?? 0);

            }

            if (allseatsinrow.Count == takenseatsinrow.Count)
            {
                return true;
            }

            return false;

        }


        public JsonResult GetSeats(string id)
        {
            List<TicketClassSeat> cats = new List<TicketClassSeat>();
            List<SelectListItem> subdistricts = new List<SelectListItem>();
            var seatRange = db.TicketClassSeatRanges.Find(int.Parse(id));
            List<int> allseatsinrow = new List<int>();
            List<int> takenseatsinrow = new List<int>();

            cats = db.TicketClassSeats.Where(x => x.TicketClassSeatRangeID == seatRange.ID).ToList();
            int xx = seatRange.FromSeatNumber ?? 0;
            while (xx <= seatRange.ToSeatNumber)
            {
                allseatsinrow.Add(xx);
                xx++;
            }

            foreach (var c in cats)
            {
                takenseatsinrow.Add(c.SeatNumber ?? 0);

            }


            var result = allseatsinrow.Where(p => !takenseatsinrow.Any(p2 => p2 == p));
            foreach (var s in result)
            {
                subdistricts.Add(new SelectListItem { Text = s.ToString(), Value = s.ToString() });

            }
            return Json(new SelectList(subdistricts, "Value", "Text"));
            //This will show available seat numbers for this row
        }


        public JsonResult UnReserveSeatTemp(string id)
        {

            List<SelectListItem> subdistricts = new List<SelectListItem>();


            using (var db = new MiidEntities())
            {
                var tcs = db.TicketClassSeats.Find(int.Parse(id));

                db.TicketClassSeats.Remove(tcs);
                db.SaveChanges();
                subdistricts.Add(new SelectListItem { Text = tcs.ID.ToString(), Value = tcs.ID.ToString() });

            }


            return Json(new SelectList(subdistricts, "Value", "Text"));
        }

        public JsonResult ReserveSeatTemp(string id, string ticketClassID, string rownumber)
        {

            List<SelectListItem> subdistricts = new List<SelectListItem>();


            using (var db = new MiidEntities())
            {
                int seatNumberInt = int.Parse(id);
                int intTicketClassID = int.Parse(ticketClassID);
                TicketClassSeat tcs = new MiidWeb.TicketClassSeat();
                tcs.SeatNumber = seatNumberInt;
                int ticketClassSeatRangeID = 0;

                var tcrs = db.TicketClassSeatRanges.Where(x => x.TicketClassID == intTicketClassID && x.RowNumber == rownumber && (x.FromSeatNumber <= seatNumberInt && x.ToSeatNumber >= seatNumberInt));
                if (tcrs != null && tcrs.Count() > 0)
                {
                    tcs.TicketClassSeatRangeID = tcrs.First().ID;
                    ticketClassSeatRangeID = tcrs.First().ID;
                    tcs.DateTimeReserved = DateTime.Now;
                }
                //check if this seat is already reserved
                var tcss = db.TicketClassSeats.Where(x => x.TicketClassSeatRangeID == ticketClassSeatRangeID && x.SeatNumber == seatNumberInt);
                if (tcss != null && tcss.Count() > 0)
                {
                    subdistricts.Add(new SelectListItem { Text = 0.ToString(), Value = 0.ToString() });
                }
                else
                {
                    db.TicketClassSeats.Add(tcs);
                    db.SaveChanges();
                    subdistricts.Add(new SelectListItem { Text = tcs.ID.ToString(), Value = tcs.ID.ToString() });
                }


            }


            return Json(new SelectList(subdistricts, "Value", "Text"));
        }
        public ActionResult GetSeats2(string id)
        {

            List<TicketClassSeat> cats = new List<TicketClassSeat>();
            List<SelectListItem> subdistricts = new List<SelectListItem>();
            var seatRange = db.TicketClassSeatRanges.Find(int.Parse(id));
            List<int> allseatsinrow = new List<int>();
            List<int> takenseatsinrow = new List<int>();

            cats = db.TicketClassSeats.Where(x => x.TicketClassSeatRangeID == seatRange.ID).ToList();
            int xx = seatRange.FromSeatNumber ?? 0;
            while (xx <= seatRange.ToSeatNumber)
            {
                allseatsinrow.Add(xx);
                xx++;
            }

            foreach (var c in cats)
            {
                takenseatsinrow.Add(c.SeatNumber ?? 0);

            }

            StringBuilder sv = new StringBuilder();

            sv.Append("<h4> Row: " + seatRange.RowNumber + "</h4>");

            var result = allseatsinrow.Where(p => !takenseatsinrow.Any(p2 => p2 == p));
            foreach (var s in allseatsinrow)
            {
                if (takenseatsinrow.Contains(s))
                {

                    sv.Append("<div class=\"selected\">" + s.ToString() + "</div>");
                }
                else
                {

                    sv.Append("<div class=\"square\">" + s.ToString() + "</div>");
                }

            }



            return Content(sv.ToString());
            //This will show available seat numbers for this row
        }
        public ActionResult PurchaseTickets(int EventID = 0, string ConfirmationMessage = null, string SelectedTicketsString = null)
        {
            GetLayoutFiles();
            string subdomain = GlobalVariables.Company;
            //TicketRepository.ClearExpiredTicketSeats();//Clears every 15 minutes

            if ((UserHelper.UserEmailConfirmed(User.Identity.Name)) == "Pending")
            {
                Growl("MiiD", "Please confirm your email before attempting to purchase tickets. Thank you.");

                return RedirectToAction("ConfirmEmailLink", "EndUsers", new { email = User.Identity.Name });
            }

            var LoggedInUserID = Helpers.UserHelper.UserID(User.Identity.Name);

            PurchaseTicketViewModel model = new PurchaseTicketViewModel(EventID, LoggedInUserID, SelectedTicketsString);

            string NFCTagResult = "No tag.";

            //Check: Is Band Active?

            var ActiveBandStatusIDs = db.Status.Where(x => x.Context == "NFCTag" && x.Description == "Active");
            int ActiveBandStatusID = 0;
            if (ActiveBandStatusIDs.Count() > 0) ActiveBandStatusID = ActiveBandStatusIDs.First().ID;
            var ActiveNFCTags = db.NFCTags.Where(x => x.EndUserID == LoggedInUserID && x.StatusID == ActiveBandStatusID);

            /* var NFCTags = db.NFCTags.Where(x => x.EndUserID == LoggedInUserID && x.StatusID != ActiveBandStatusID);

             if (ActiveNFCTags.Count() > 0)
             {
                 NFCTagResult = "Tag active.";
                 Session.Add("ErrorMessage", NFCTagResult);
             }
             else
             {
                 if (NFCTags.Count() > 0) NFCTagResult = "Tag not active.";
                 Session.Add("ErrorMessage", NFCTagResult);
             }

             if (NFCTagResult == "No tag.")
             {
                 Session.Add("ErrorMessage", NFCTagResult);
                return RedirectToAction("CreateByUser", "NFCTags");
             }

             if (NFCTagResult == "Tag not active.")
             {
                 Session.Add("ErrorMessage", NFCTagResult);
                 return RedirectToAction("IndexByUserID", "NFCTags");
             }
             */
            //Check Is User Info Updated?
            //Daniel relaxed this rule 24 03 2016
            //if (!EndUserRepository.IsUserUpToDate(LoggedInUserID))
            //{
            //    Growl("Message", "Please complete all your profile fields before purchasing event tickets.");
            //    return RedirectToAction("MiiDetails", "EndUsers", new { id = LoggedInUserID });

            //}

            var List = new SelectList(
      new List<SelectListItem>
      {
        new SelectListItem { Text = "EFT", Value = "1"},
        new SelectListItem { Text = "Credit Card", Value = "2"},
        new SelectListItem { Text = "Instant EFT", Value = "3"},
      }, "Value", "Text");


            ViewBag.TenderType = List;
            ViewBag.ConfirmationMessage = ConfirmationMessage;

            bool hasSeatingPlan = TicketRepository.DoesEventHasTicketSeatingPlan(EventID);
            if (hasSeatingPlan)
            {
                return View("PurchaseTicketsSeatPlan", model);
            }
            else
            {
                if (model.Event.IsMultiDayEvent.HasValue && model.Event.IsMultiDayEvent.Value)
                {
                    model.TicketClasses = null;
                }
                return View("PurchaseTickets", model);
            }
        }

        public ActionResult CreateCookie()
        {

            var cookie = new HttpCookie("cookie_name", "some value");
            Response.AppendCookie(cookie);
            return View();
        }

        public ActionResult ReadCookie()
        {

            var cookie = Request.Cookies["cookie_name"];
            if (cookie != null)
            {
                string value = cookie.Value;
            }
            return View();
        }

        public class FriendTicket {
            public int TicketClassID { get; set; }
            public string FriendEmail { get; set; }
        }
        [HttpPost]

        public ActionResult PurchaseTickets(FormCollection form, string txtPromoCode = "", string hdnPromoCode = "")
        {
            bool isUniqueEmailList = true;
            int EventID = int.Parse(form["EventID"]);
            var Event = db.Events.Include(x => x.TicketClasses).Where(x => x.ID == EventID).First();
            StringBuilder friendticketlist = new StringBuilder();

            List<string> friendEmails = new List<string>();

            bool purchaseforfriends = false;
            foreach (var key in form.AllKeys)
            {
                if(key.ToString().Contains("Friend"))
                //if (form[key].Contains("Friend"))
                {
                    purchaseforfriends = true;
                    break;
                }
            }
            if (purchaseforfriends)
            {
              
               
                    
                    foreach (var tc in Event.TicketClasses)
                    {
                        for (int ff = 1; ff < 20; ff++)
                        {
                            if (form["Friend_TicketClassQty_" + tc.ID.ToString()+"_"+ff] != null && form["Friend_TicketClassQty_" + tc.ID.ToString() + "_" + ff] != "")
                            {
                            string TicketClassID = tc.ID.ToString();
                            string FriendEmail = form["Friend_TicketClassQty_" + tc.ID.ToString() + "_" + ff];

                                friendEmails.Add(FriendEmail);
                                friendticketlist.Append(String.Format("{0}:{1},", FriendEmail, TicketClassID));
                            }
                        }
                    }

                isUniqueEmailList = friendEmails.Distinct().Count() == friendEmails.Count();

                if (!isUniqueEmailList)
                {
                    //Error condition: friend emails are repeated.
                    Growl("Mi-iD", "Only one ticket per email address. Please check for repeated emails.");
                    return RedirectToAction("PurchaseTickets", "Events", new { EventID = EventID} );
                }
            }

            string PromoCode = hdnPromoCode;

            var f = form;

            //check ticket quantity
            int totalTicketQty = 0;
          

         
            foreach (var tc in Event.TicketClasses)
            {
                if (form["TicketClassQty_" + tc.ID.ToString()] != null && form["TicketClassQty_" + tc.ID.ToString()] != "")
                {
                    totalTicketQty += int.Parse(form["TicketClassQty_" + tc.ID.ToString()].ToString());
                }
            }

            if (totalTicketQty > 0)//only 1 ticket allowed
            {

                PurchaseTicketViewModel purchaseTicketViewModel = new PurchaseTicketViewModel();
                purchaseTicketViewModel.TicketClasses = new List<TicketClassViewModel>();





                //New DS
                if (!User.Identity.IsAuthenticated)
                {

                    PurchaseTicketLoggedOutViewModel purchaseTicketViewModelLoggedOut = new PurchaseTicketLoggedOutViewModel(EventID, PromoCode, "");
                    purchaseTicketViewModelLoggedOut.FriendEmailTicketClassList = friendticketlist.ToString();

                    foreach (var tc in Event.TicketClasses)
                    {
                        var qty = form["TicketClassQty_" + tc.ID.ToString()];

                        if (form["TicketClassQty_" + tc.ID.ToString()] != null && form["TicketClassQty_" + tc.ID.ToString()] != "")
                        {
                            var tcvm = new TicketClassLoggedOutViewModel(tc.ID);
                            tcvm.TicketCount = int.Parse(form["TicketClassQty_" + tc.ID.ToString()].ToString());
                            tcvm.TotalCost = (decimal)tc.Price * (decimal)tcvm.TicketCount;
                            purchaseTicketViewModelLoggedOut.TicketClasses.Add(tcvm);
                        }
                    }

                    string cookievalue = JsonConvert.SerializeObject(purchaseTicketViewModelLoggedOut.TicketClasses.Where(x => x.TicketCount > 0), Newtonsoft.Json.Formatting.Indented);
                    var cookie = new HttpCookie("MiiD_selected_tickets", cookievalue);
                    cookie.Expires = DateTime.Now.AddMinutes(15.0);
                    Response.AppendCookie(cookie);

                    var cookie2 = new HttpCookie("MiiD_PromoCode", PromoCode);
                    cookie2.Expires = DateTime.Now.AddMinutes(15.0);
                    Response.AppendCookie(cookie2);

                    if (!String.IsNullOrEmpty(purchaseTicketViewModelLoggedOut.FriendEmailTicketClassList))
                    {
                        var cookie3 = new HttpCookie("MiiD_FriendEmailList", purchaseTicketViewModelLoggedOut.FriendEmailTicketClassList);
                        Response.AppendCookie(cookie3);
                    }

                    Growl("Mi-iD", "Ticket selection will expire in 15 minutes.");

                    return RedirectToAction("LoginAlternative", "LoginAlternative");


                }
                else
                {
                    var LoggedInUserID = Helpers.UserHelper.UserID(User.Identity.Name);
                    purchaseTicketViewModel = new PurchaseTicketViewModel(EventID, LoggedInUserID, null);

                    purchaseTicketViewModel.FriendEmailTicketClassList = friendticketlist.ToString();

                    foreach (var tc in Event.TicketClasses)
                    {
                        var qty = form["TicketClassQty_" + tc.ID.ToString()];

                        if (form["TicketClassQty_" + tc.ID.ToString()] != null && form["TicketClassQty_" + tc.ID.ToString()] != "")
                        {
                            var tcvm = new TicketClassViewModel() { TicketClass = tc };
                            tcvm.TicketCount = int.Parse(form["TicketClassQty_" + tc.ID.ToString()].ToString());
                            tcvm.TotalCost = (decimal)(tc.Price * (decimal)tcvm.TicketCount);
                            purchaseTicketViewModel.TicketClasses.Add(tcvm);
                        }
                    }
                    var endUser = db.EndUsers.Find(LoggedInUserID);

                    purchaseTicketViewModel = purchaseTicketViewModel.ApplyPromoCode(purchaseTicketViewModel, PromoCode);

                    purchaseTicketViewModel.BuyerEmail = endUser.Email;
                    purchaseTicketViewModel.BuyerFirstName = endUser.Firstname;
                    purchaseTicketViewModel.BuyerLastName = endUser.Surname;

                    string PurchaseSessionID = System.Guid.NewGuid().ToString();

                    Session.Add(PurchaseSessionID, purchaseTicketViewModel);


                    if (!EndUserRepository.IsUserUpToDate(LoggedInUserID))
                    {
                        Growl("Ticket Details", "The following details will appear on your ticket");


                        PurchaseTicketLoggedOutViewModel purchaseTicketViewModelLoggedOut = new PurchaseTicketLoggedOutViewModel(EventID, PromoCode, "");
                        purchaseTicketViewModelLoggedOut.FriendEmailTicketClassList = friendticketlist.ToString();

                        foreach (var tc in Event.TicketClasses)
                        {
                            var qty = form["TicketClassQty_" + tc.ID.ToString()];

                            if (form["TicketClassQty_" + tc.ID.ToString()] != null && form["TicketClassQty_" + tc.ID.ToString()] != "")
                            {
                                var tcvm = new TicketClassLoggedOutViewModel(tc.ID);
                                tcvm.TicketCount = int.Parse(form["TicketClassQty_" + tc.ID.ToString()].ToString());
                                tcvm.TotalCost = (int)tc.Price * tcvm.TicketCount;
                                purchaseTicketViewModelLoggedOut.TicketClasses.Add(tcvm);
                            }
                        }

                        string cookievalue = JsonConvert.SerializeObject(purchaseTicketViewModelLoggedOut.TicketClasses.Where(x => x.TicketCount > 0), Newtonsoft.Json.Formatting.Indented);

                        return RedirectToAction("CreateFirst", "EndUsers", new { Email = endUser.Email, Cookie = cookievalue });


                    }

                    return RedirectToAction("ChooseTender", "MyMoneys", new { @sessionid = PurchaseSessionID, purpose = "TicketPurchase" });

                }
            }
            else
            {
                var LoggedInUserID = Helpers.UserHelper.UserID(User.Identity.Name);

                PurchaseTicketViewModel model = new PurchaseTicketViewModel(EventID, LoggedInUserID);
                model.FriendEmailTicketClassList = friendticketlist.ToString();

                string NFCTagResult = "No tag.";

                //Check: Is Band Active?

                var ActiveBandStatusIDs = db.Status.Where(x => x.Context == "NFCTag" && x.Description == "Active");
                int ActiveBandStatusID = 0;
                if (ActiveBandStatusIDs.Count() > 0) ActiveBandStatusID = ActiveBandStatusIDs.First().ID;
                var ActiveNFCTags = db.NFCTags.Where(x => x.EndUserID == LoggedInUserID && x.StatusID == ActiveBandStatusID);


                var List = new SelectList(
                new List<SelectListItem>
                {
                        new SelectListItem { Text = "EFT", Value = "1"},
                        new SelectListItem { Text = "Credit Card", Value = "2"},
                        new SelectListItem { Text = "Instant EFT", Value = "3"},
                }, "Value", "Text");


                ViewBag.TenderType = List;


                if (totalTicketQty == 0)
                {
                    Growl("Miid", "At least one ticket type must be selected for purchase.");
                }
                bool hasSeatingPlan = TicketRepository.DoesEventHasTicketSeatingPlan(EventID);
                if (hasSeatingPlan)
                {
                    return View("PurchaseTicketsSeatPlan", model);
                }
                else
                {
                    if (model.Event.IsMultiDayEvent.HasValue && model.Event.IsMultiDayEvent.Value)
                    {
                        model.TicketClasses = null;
                    }
                    return View("PurchaseTickets", model);
                }

                return View(model);


            }
        }


        [HttpPost]

        public ActionResult PurchaseTicketsSeatPlan(FormCollection form, string txtPromoCode = "")
        {
            string PromoCode = txtPromoCode;
            var f = form;

            //check ticket quantity
            int totalTicketQty = 0;
            int EventID = int.Parse(form["EventID"]);

            var Event = db.Events.Include(x => x.TicketClasses).Where(x => x.ID == EventID).First();
            foreach (var tc in Event.TicketClasses)
            {
                if (form["TicketClassQty_" + tc.ID.ToString()] != null && form["TicketClassQty_" + tc.ID.ToString()] != "")
                {
                    totalTicketQty += int.Parse(form["TicketClassQty_" + tc.ID.ToString()].ToString());
                }
            }
            int maxrows = int.Parse(form["hiddenlinenum"]);

            if (totalTicketQty > 0 || maxrows > 0)//only 1 ticket allowed
            {

                PurchaseTicketViewModel purchaseTicketViewModel = new PurchaseTicketViewModel();
                purchaseTicketViewModel.TicketClasses = new List<TicketClassViewModel>();





                //New DS
                if (!User.Identity.IsAuthenticated)
                {

                    //////////////////////////////////////////////////////////



                    maxrows = int.Parse(form["hiddenlinenum"]);
                    int xz = 1;
                    StringBuilder selectedTicketSeats = new StringBuilder();
                    while (xz <= maxrows)
                    {
                        if (form["ticketclassid_" + xz.ToString()] != null)
                        {
                            selectedTicketSeats.Append(form["ticketclassid_" + xz.ToString()]);
                            selectedTicketSeats.Append(":");
                            selectedTicketSeats.Append(form["rownumberid_" + xz.ToString()]);
                            selectedTicketSeats.Append(":");
                            selectedTicketSeats.Append(form["seatnumberid_" + xz.ToString()]);
                            selectedTicketSeats.Append(":");
                            selectedTicketSeats.Append(form["ticketclassseatid_" + xz.ToString()]);
                            selectedTicketSeats.Append(";");

                        }

                        xz++;
                    }

                    PurchaseTicketLoggedOutViewModel purchaseTicketViewModelLoggedOut = new PurchaseTicketLoggedOutViewModel(EventID, PromoCode, selectedTicketSeats.ToString());


                    string cookievalue = JsonConvert.SerializeObject(purchaseTicketViewModelLoggedOut.TicketClasses.Where(x => x.TicketCount > 0), Newtonsoft.Json.Formatting.Indented);
                    var cookie = new HttpCookie("MiiD_selected_tickets", cookievalue);
                    cookie.Expires = DateTime.Now.AddMinutes(15.0);
                    Response.AppendCookie(cookie);

                    var cookie2 = new HttpCookie("MiiD_PromoCode", PromoCode);
                    cookie2.Expires = DateTime.Now.AddMinutes(15.0);
                    Response.AppendCookie(cookie2);


                    Growl("Mi-iD", "Ticket selection will expire in 15 minutes.");

                    return RedirectToAction("LoginAlternative", "LoginAlternative");



                }
                else
                {
                    var LoggedInUserID = Helpers.UserHelper.UserID(User.Identity.Name);


                    maxrows = int.Parse(form["hiddenlinenum"]);
                    int xz = 1;
                    StringBuilder selectedTicketSeats = new StringBuilder();
                    while (xz <= maxrows)
                    {
                        if (form["ticketclassid_" + xz.ToString()] != null)
                        {
                            selectedTicketSeats.Append(form["ticketclassid_" + xz.ToString()]);
                            selectedTicketSeats.Append(":");
                            selectedTicketSeats.Append(form["rownumberid_" + xz.ToString()]);
                            selectedTicketSeats.Append(":");
                            selectedTicketSeats.Append(form["seatnumberid_" + xz.ToString()]);
                            selectedTicketSeats.Append(":");
                            selectedTicketSeats.Append(form["ticketclassseatid_" + xz.ToString()]);
                            selectedTicketSeats.Append(";");

                        }

                        xz++;
                    }

                    purchaseTicketViewModel = new PurchaseTicketViewModel(EventID, LoggedInUserID, null, selectedTicketSeats.ToString());

                    purchaseTicketViewModel = purchaseTicketViewModel.ApplyPromoCode(purchaseTicketViewModel, PromoCode);

                    var endUser = db.EndUsers.Find(LoggedInUserID);
                    purchaseTicketViewModel.BuyerEmail = endUser.Email;
                    purchaseTicketViewModel.BuyerFirstName = endUser.Firstname;
                    purchaseTicketViewModel.BuyerLastName = endUser.Surname;

                    string PurchaseSessionID = System.Guid.NewGuid().ToString();

                    Session.Add(PurchaseSessionID, purchaseTicketViewModel);


                    if (!EndUserRepository.IsUserUpToDate(LoggedInUserID))
                    {
                        Growl("Ticket Details", "The following details will appear on your ticket");


                        PurchaseTicketLoggedOutViewModel purchaseTicketViewModelLoggedOut = new PurchaseTicketLoggedOutViewModel(EventID, PromoCode, selectedTicketSeats.ToString());
                        foreach (var tc in Event.TicketClasses)
                        {
                            var qty = form["TicketClassQty_" + tc.ID.ToString()];

                            if (form["TicketClassQty_" + tc.ID.ToString()] != null && form["TicketClassQty_" + tc.ID.ToString()] != "")
                            {
                                var tcvm = new TicketClassLoggedOutViewModel(tc.ID);
                                tcvm.TicketCount = int.Parse(form["TicketClassQty_" + tc.ID.ToString()].ToString());
                                tcvm.TotalCost = (int)tc.Price * tcvm.TicketCount;
                                purchaseTicketViewModelLoggedOut.TicketClasses.Add(tcvm);
                            }
                        }

                        string cookievalue = JsonConvert.SerializeObject(purchaseTicketViewModelLoggedOut.TicketClasses.Where(x => x.TicketCount > 0), Newtonsoft.Json.Formatting.Indented);

                        return RedirectToAction("CreateFirst", "EndUsers", new { Email = endUser.Email, Cookie = cookievalue });


                    }

                    return RedirectToAction("ChooseTender", "MyMoneys", new { @sessionid = PurchaseSessionID, purpose = "TicketPurchase" });

                }
            }
            else
            {
                var LoggedInUserID = Helpers.UserHelper.UserID(User.Identity.Name);

                PurchaseTicketViewModel model = new PurchaseTicketViewModel(EventID, LoggedInUserID);

                string NFCTagResult = "No tag.";

                //Check: Is Band Active?

                var ActiveBandStatusIDs = db.Status.Where(x => x.Context == "NFCTag" && x.Description == "Active");
                int ActiveBandStatusID = 0;
                if (ActiveBandStatusIDs.Count() > 0) ActiveBandStatusID = ActiveBandStatusIDs.First().ID;
                var ActiveNFCTags = db.NFCTags.Where(x => x.EndUserID == LoggedInUserID && x.StatusID == ActiveBandStatusID);


                var List = new SelectList(
                new List<SelectListItem>
                {
                        new SelectListItem { Text = "EFT", Value = "1"},
                        new SelectListItem { Text = "Credit Card", Value = "2"},
                        new SelectListItem { Text = "Instant EFT", Value = "3"},
                }, "Value", "Text");


                ViewBag.TenderType = List;


                if (totalTicketQty == 0)
                {
                    Growl("Miid", "At least one ticket type must be selected for purchase.");
                }
                //if (totalTicketQty >1)
                //{
                //    Growl("Miid", "Only one ticket may be purchased per event. ");
                //}

                bool hasSeatingPlan = TicketRepository.DoesEventHasTicketSeatingPlan(EventID);
                if (hasSeatingPlan)
                {
                    return View("PurchaseTicketsSeatPlan", model);
                }
                else
                {
                    if (model.Event.IsMultiDayEvent.HasValue && model.Event.IsMultiDayEvent.Value)
                    {
                        model.TicketClasses = null;
                    }
                    return View("PurchaseTickets", model);
                }

                // return View(model);


            }
        }

        [Authorize]

        public ActionResult PurchaseTickets2(string Json, string promocode, string friendEmailList="")
        {
            Json = Helpers.JsonCleaner.RestoreNames(Json);

            List<TicketClassLoggedOutViewModel> ticketClassesFromCookie = JsonConvert.DeserializeObject<List<TicketClassLoggedOutViewModel>>(Server.UrlDecode(Json));
            int EventID = ticketClassesFromCookie.First().EventID ?? 0;
            int EndUserID = Helpers.UserHelper.UserID(User.Identity.Name);

            var purchaseTicketViewModel = new PurchaseTicketViewModel(EventID, EndUserID);
            purchaseTicketViewModel.TicketClasses = new List<TicketClassViewModel>();
            purchaseTicketViewModel.FriendEmailTicketClassList = friendEmailList;


            foreach (var tc in ticketClassesFromCookie)
            {

                var tvm = new TicketClassViewModel(tc.ID, tc.TicketCount, tc.ChosenRowSeats);
                tvm.TicketCount = tc.TicketCount;
                tvm.TotalCost = (decimal)tc.TotalCost;

                purchaseTicketViewModel.TicketClasses.Add(tvm);
            }
            if (!String.IsNullOrEmpty(promocode))
            {
                purchaseTicketViewModel = purchaseTicketViewModel.ApplyPromoCode(purchaseTicketViewModel, promocode);
            }

            string PurchaseSessionID = System.Guid.NewGuid().ToString();

            Session.Add(PurchaseSessionID, purchaseTicketViewModel);

            DeleteCookie();


            return RedirectToAction("ChooseTender", "MyMoneys", new { @sessionid = PurchaseSessionID, purpose = "TicketPurchase" });

        }

        protected void DeleteCookie()
        {
            HttpCookie currentUserCookie = Request.Cookies["MiiD_selected_tickets"];
            if (currentUserCookie != null)
            {
                Response.Cookies.Remove("MiiD_selected_tickets");
                currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                currentUserCookie.Value = null;
                Response.SetCookie(currentUserCookie);
            }

            HttpCookie currentUserCookie2 = Request.Cookies["MiiD_PromoCode"];
            if (currentUserCookie2 != null)
            {
                Response.Cookies.Remove("MiiD_PromoCode");
                currentUserCookie2.Expires = DateTime.Now.AddDays(-10);
                currentUserCookie2.Value = null;
                Response.SetCookie(currentUserCookie2);
            }

            HttpCookie currentUserCookie3 = Request.Cookies["MiiD_FriendEmailList"];
            if (currentUserCookie3 != null)
            {
                Response.Cookies.Remove("MiiD_FriendEmailList");
                currentUserCookie3.Expires = DateTime.Now.AddDays(-10);
                currentUserCookie3.Value = null;
                Response.SetCookie(currentUserCookie3);
            }

        }


        #endregion


        #region News Feed

        [HttpPost]
        public ActionResult AddNewsFeedComment(FormCollection form)
        {
            var f = form;
            string NewsFeedComment = form["NewsFeedComment"];
            int NewsFeedID = int.Parse(form["NewsFeedID"]);
            int CommenterUserID = int.Parse(form["CommenterUserID"]);
            int UserID = int.Parse(form["UserID"]);

            var newsFeedComment = new NewsFeedComment
            {
                CommenterUserID = CommenterUserID,
                Comment = NewsFeedComment,
                NewsFeedID = NewsFeedID,
                DateCreated = DateTime.Now,
                StatusID = 1
            };

            db.NewsFeedComments.Add(newsFeedComment);
            db.SaveChanges();

            return RedirectToAction("EndUserProfile", new { id = UserID, loggedInUserID = CommenterUserID });
        }

        #endregion


        #region Event Organiser Functions
        [Authorize]

        public ActionResult IndexForEO()
        {

            var eventOrganiser = EventOrganiserRepository.GetEventOrganiserByEmail(User.Identity.Name);

            if (eventOrganiser == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var events = new Models.EventListViewModel(eventOrganiser.ID);

            return View(events);



        }


        public ActionResult CreateTicketIndex(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventCategoryID = new SelectList(db.EventCategories, "ID", "Code", @event.EventCategoryID);
            var eo = EventOrganiserRepository.GetEventOrganiserByEmail(User.Identity.Name);
            if (eo != null)
            {
                ViewBag.EventOrganiserID = eo.ID;
                ViewBag.CompanyName = eo.CompanyName;
            }
            ViewBag.StatusID = 1;

            EventViewModel evm = new EventViewModel(@event.ID, (int)@event.EventOrganiserID, true);
            evm.DateToCompareAgainst = DateTime.Now;
            evm.EventOrganiser = eo;

            return View(evm);
        }







        [Authorize]

        public ActionResult CreateByEO()
        {

            ViewBag.EventCategoryID = new SelectList(db.EventCategories, "ID", "Code");
            var eo = EventOrganiserRepository.GetEventOrganiserByEmail(User.Identity.Name);
            if (eo != null)
            {
                ViewBag.EventOrganiserID = eo.ID;
                ViewBag.CompanyName = eo.CompanyName;
            }
            ViewBag.StatusID = 1;

            EventViewModel evm = new EventViewModel(true);
            evm.DateToCompareAgainst = DateTime.Now;
            evm.EventOrganiser = eo;

            return View(evm);


        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public ActionResult CreateByEO(EventViewModel e, int EventCategoryID, string imageData, string EventLogoURL, int EventOrganiserID, string bannerImageData, string EventBannerURL)
        {

            if (ModelState.IsValid)
            {

                int failcount = 0;

                string[] StartDateTimeStr = e.StartDateTimeTime.Split(':');
                string[] EndDateTimeStr = e.EndDateTimeTime.Split(':');
                string[] TicketCutoffTimeStr = e.TicketCutoffTime.Split(':');

                DateTime? startdateNull = DateHelper.CreateDate(e.StartDateYear, e.StartDateMonth, e.StartDateDay, int.Parse(StartDateTimeStr[0]), int.Parse(StartDateTimeStr[1]), 0);
                DateTime? enddateNull = DateHelper.CreateDate(e.EndDateYear, e.EndDateMonth, e.EndDateDay, int.Parse(EndDateTimeStr[0]), int.Parse(EndDateTimeStr[1]), 0);
                DateTime? tickcutdateNull = DateHelper.CreateDate(e.TicketCutoffYear, e.TicketCutoffMonth, e.TicketCutoffDay, int.Parse(TicketCutoffTimeStr[0]), int.Parse(TicketCutoffTimeStr[1]), 0);

                if (startdateNull != null && enddateNull != null && tickcutdateNull != null)
                {
                    e.Event.StartDateTime = (DateTime)startdateNull;

                    if (DateHelper.DateSequenceViolation(e.DateToCompareAgainst, (DateTime)startdateNull))
                    {
                        //Invalid start date
                        ModelState.AddModelError("StartDateTime", "StartDateTime invalid.");
                        failcount++;
                    }

                    e.Event.EndDateTime = enddateNull;

                    if (DateHelper.DateSequenceViolation(e.DateToCompareAgainst, (DateTime)enddateNull) || DateHelper.DateSequenceViolation((DateTime)e.Event.StartDateTime, (DateTime)enddateNull))
                    {
                        //Invalid start date
                        ModelState.AddModelError("EndDateTime", "EndDateTime invalid.");
                        failcount++;
                    }

                    e.Event.TicketCutoffDate = (DateTime)tickcutdateNull;

                    if (DateHelper.DateSequenceViolation(e.DateToCompareAgainst, (DateTime)tickcutdateNull) || DateHelper.DateSequenceViolation((DateTime)tickcutdateNull, (DateTime)e.Event.EndDateTime))
                    {
                        //Invalid TicketCutOffDate
                        ModelState.AddModelError("TicketCutOffDate", "Ticket CutOff Date invalid.");
                        failcount++;
                    }

                    if (failcount > 0)
                        goto fail;

                    e.Event.EventOrganiserID = EventOrganiserID;

                    var Event = new Event();
                    Event = e.Event;
                    Event.LongDescription = e.LongDescription;
                    Event.ShortDescription = e.ShortDescription;
                    Event.EventCategoryID = EventCategoryID;
                    Event.IsMultiDayEvent = e.IsMultiDayEvent;
                    Event.IsCashless = e.IsCashless;
                  
                    if (!e.IsCashless ?? false) { e.LimitOneTicketPerUser = true; }
                    Event.LimitOneTicketPerUser = e.LimitOneTicketPerUser;
                    db.Events.Add(Event);
                    db.SaveChanges();

                    int EventID = Event.ID;

                    EventImage ei = new EventImage();

                    if (!String.IsNullOrEmpty(imageData))
                    {
                        var bytes = Convert.FromBase64String(imageData);
                        var fileName = String.Format("Logo_{0}_{1}.png", EventID.ToString(), e.Event.EventName.Trim());
                        var filePath = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                        using (var imageFile = new FileStream(filePath, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();
                            imageFile.Dispose();
                        }

                        //delete all other logos for this event
                        foreach (var eiDel in db.EventImages.Where(x => x.ImageAltText == "logo" && x.EventID == EventID).ToList())
                        {
                            db.EventImages.Remove(eiDel);
                        }

                        ei.EventID = EventID;
                        ei.ImageURL = fileName;
                        ei.ImageAltText = "logo";
                        db.EventImages.Add(ei);
                        db.SaveChanges();
                    }

                    //Banner
                    if (!String.IsNullOrEmpty(bannerImageData))
                    {


                        string fileExt = "png";

                        fileExt = bannerImageData.Contains("data:image/jpeg;base64,") ? "jpg" : fileExt;
                        fileExt = bannerImageData.Contains("data:image/jpg;base64,") ? "jpg" : fileExt;
                        fileExt = bannerImageData.Contains("data:image/png;base64,") ? "png" : fileExt;
                        fileExt = bannerImageData.Contains("data:image/gif;base64,") ? "gif" : fileExt;

                        bannerImageData = bannerImageData.Contains("data:image/jpeg;base64,") ? bannerImageData.Replace("data:image/jpeg;base64,", "") : bannerImageData;
                        bannerImageData = bannerImageData.Contains("data:image/jpg;base64,") ? bannerImageData.Replace("data:image/jpg;base64,", "") : bannerImageData;
                        bannerImageData = bannerImageData.Contains("data:image/png;base64,") ? bannerImageData.Replace("data:image/png;base64,", "") : bannerImageData;
                        bannerImageData = bannerImageData.Contains("data:image/gif;base64,") ? bannerImageData.Replace("data:image/gif;base64,", "") : bannerImageData;


                        var bytes = Convert.FromBase64String(bannerImageData);

                        string eventName = "Blank";

                        if (e.Event.EventName != null)
                        {
                            eventName = e.Event.EventName.Replace(" ", String.Empty);//remove  blank spaces
                        }

                        var fileName = String.Format("Banner_{0}_{1}.{2}", e.Event.ID.ToString(), eventName, fileExt);
                        var filePath = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                        using (var imageFile = new FileStream(filePath, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();
                            imageFile.Dispose();
                        }
                        /*
                        Image croppedImage = ImageHelper.resizeImg(filePath, 1000);

                        fileName = String.Format("Banner_crp_{0}_{1}.{2}", e.Event.ID.ToString(), eventName, fileExt);
                        filePath = Path.Combine(Server.MapPath("~/Uploads"), fileName);

                        croppedImage.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                        croppedImage.Dispose();
                        */
                        //delete all other logos for this event
                        foreach (var eiDel in db.EventImages.Where(x => x.ImageAltText == "banner" && x.EventID == e.Event.ID).ToList())
                        {
                            db.EventImages.Remove(eiDel);
                        }
                        EventImage eiB = new EventImage();
                        eiB.EventID = e.Event.ID;
                        eiB.ImageURL = fileName;
                        eiB.ImageAltText = "banner";
                        db.EventImages.Add(eiB);
                        db.SaveChanges();
                    }
                }
                else
                {
                    string errordates = "";
                    if (startdateNull == null)
                    {
                        errordates += " Start Date, ";
                        ModelState.AddModelError("StartDateTime", "StartDateTime invalid.");
                    }
                    if (enddateNull == null)
                    {
                        errordates += " End Date, ";
                        ModelState.AddModelError("EndDateTime", "EndDateTime invalid.");
                    }
                    if (tickcutdateNull == null)
                    {
                        errordates += " Ticket Cut off Date ";
                        ModelState.AddModelError("TicketCutOffDate", "Ticket CutOff Date invalid.");
                    }
                    goto fail;
                }
                Growl("MiiD", "Event created successfully.");
                return RedirectToAction("MyDetails", "EventOrganisers");
            }

            fail:
            ViewBag.EventCategoryID = new SelectList(db.EventCategories, "ID", "Code");
            var eo = EventOrganiserRepository.GetEventOrganiserByEmail(User.Identity.Name);
            if (eo != null)
            {
                ViewBag.EventOrganiserID = eo.ID;
                ViewBag.CompanyName = eo.CompanyName;
            }
            ViewBag.StatusID = 1;

            EventViewModel evm = new EventViewModel(true);
            evm.DateToCompareAgainst = DateTime.Now;
            evm.EventOrganiser = eo;

            return View(evm);
        }
        [Authorize]

        public ActionResult CreateByEONoTickets()
        {

            ViewBag.EventCategoryID = new SelectList(db.EventCategories, "ID", "Code");
            var eo = EventOrganiserRepository.GetEventOrganiserByEmail(User.Identity.Name);
            if (eo != null)
            {
                ViewBag.EventOrganiserID = eo.ID;
                ViewBag.CompanyName = eo.CompanyName;
            }
            ViewBag.StatusID = 1;

            EventViewModel evm = new EventViewModel(true);
            evm.DateToCompareAgainst = DateTime.Now;
            evm.EventOrganiser = eo;

            return View(evm);
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public ActionResult CreateByEONoTickets(EventViewModel e, string imageData, string EventLogoURL, int EventOrganiserID)
        {

            if (ModelState.IsValid)
            {

                int failcount = 0;

                string[] TimeStr = e.StartDateTimeTime.Split(':');
                DateTime date1 = new DateTime(e.StartDateYear, e.StartDateMonth, e.StartDateDay, int.Parse(TimeStr[0]), int.Parse(TimeStr[1]), 0);
                e.Event.StartDateTime = date1;


                if (DateHelper.DateSequenceViolation(e.DateToCompareAgainst, date1))
                {
                    //Invalid start date
                    ModelState.AddModelError("StartDateTime", "StartDateTime invalid.");
                    failcount++;
                }

                TimeStr = e.EndDateTimeTime.Split(':');
                date1 = new DateTime(e.EndDateYear, e.EndDateMonth, e.EndDateDay, int.Parse(TimeStr[0]), int.Parse(TimeStr[1]), 0);
                e.Event.EndDateTime = date1;

                if (DateHelper.DateSequenceViolation(e.DateToCompareAgainst, date1) || DateHelper.DateSequenceViolation((DateTime)e.Event.StartDateTime, date1))
                {
                    //Invalid start date
                    ModelState.AddModelError("EndDateTime", "EndDateTime invalid.");
                    failcount++;
                }

                e.Event.EventOrganiserID = EventOrganiserID;

                var Event = new Event();
                Event = e.Event;

                db.Events.Add(Event);
                db.SaveChanges();

                int EventID = Event.ID;


                EventImage ei = new EventImage();



                return RedirectToAction("MyDetails", "EventOrganisers");
            }




            ViewBag.EventCategoryID = new SelectList(db.EventCategories, "ID", "Code");
            var eo = EventOrganiserRepository.GetEventOrganiserByEmail(User.Identity.Name);
            if (eo != null)
            {
                ViewBag.EventOrganiserID = eo.ID;
                ViewBag.CompanyName = eo.CompanyName;
            }
            ViewBag.StatusID = 1;

            EventViewModel evm = new EventViewModel(true);
            evm.DateToCompareAgainst = DateTime.Now;
            evm.EventOrganiser = eo;

            return View(evm);

        }
        [Authorize]

        public ActionResult EditByEOCentral(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventCategoryID = new SelectList(db.EventCategories, "ID", "Code", @event.EventCategoryID);
            var eo = EventOrganiserRepository.GetEventOrganiserByEmail(User.Identity.Name);
            if (eo != null)
            {
                ViewBag.EventOrganiserID = eo.ID;
                ViewBag.CompanyName = eo.CompanyName;
            }
            ViewBag.StatusID = 1;

            EventViewModel evm = new EventViewModel(@event.ID, (int)@event.EventOrganiserID, true);
            evm.DateToCompareAgainst = DateTime.Now;
            evm.EventOrganiser = eo;

            return View(evm);
        }



        [Authorize]
        public ActionResult EditByEO(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventCategoryID = new SelectList(db.EventCategories, "ID", "Code", @event.EventCategoryID);
            var eo = EventOrganiserRepository.GetEventOrganiserByEmail(User.Identity.Name);
            if (eo != null)
            {
                ViewBag.EventOrganiserID = eo.ID;
                ViewBag.CompanyName = eo.CompanyName;
            }
            ViewBag.StatusID = 1;

            EventViewModel evm = new EventViewModel(@event.ID, (int)@event.EventOrganiserID, true);
            evm.DateToCompareAgainst = DateTime.Now;
            evm.EventOrganiser = eo;

            return View(evm);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult EditByEO(EventViewModel e, string imageData, int EventCategoryID, string EventLogoURL, int EventOrganiserID, string bannerImageData, string EventBannerURL, int SubdomainID,
                       string seatingplanImageData, string SeatPlanImage)
        {
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(imageData))
                {
                    var bytes = Convert.FromBase64String(imageData);

                    string eventName = "Blank";

                    if (e.Event.EventName != null)
                    {
                        eventName = e.Event.EventName.Replace(" ", String.Empty);//remove  blank spaces
                    }

                    var fileName = String.Format("Logo_{0}_{1}.png", e.Event.ID.ToString(), eventName);
                    var filePath = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                    using (var imageFile = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.Write(bytes, 0, bytes.Length);
                        imageFile.Flush();
                        imageFile.Dispose();
                    }

                    //delete all other logos for this event
                    foreach (var eiDel in db.EventImages.Where(x => x.ImageAltText == "logo" && x.EventID == e.Event.ID).ToList())
                    {
                        db.EventImages.Remove(eiDel);
                    }
                    EventImage ei = new EventImage();
                    ei.EventID = e.Event.ID;
                    ei.ImageURL = fileName;
                    ei.ImageAltText = "logo";
                    db.EventImages.Add(ei);
                    db.SaveChanges();
                }

                //Banner
                bannerImageData = SaveImage(e, bannerImageData);
                seatingplanImageData = SaveImage(e, seatingplanImageData,"SeatingPlan");

                int failcount = 0;

                string[] TimeStr = e.StartDateTimeTime.Split(':');
                DateTime date1 = new DateTime(e.StartDateYear, e.StartDateMonth, e.StartDateDay, int.Parse(TimeStr[0]), int.Parse(TimeStr[1]), 0);
                e.Event.StartDateTime = date1;

                if (DateHelper.DateSequenceViolation(e.DateToCompareAgainst, date1))
                {
                    //Invalid start date
                    ModelState.AddModelError("StartDateTime", "StartDateTime invalid.");
                    failcount++;
                }

                TimeStr = e.EndDateTimeTime.Split(':');
                date1 = new DateTime(e.EndDateYear, e.EndDateMonth, e.EndDateDay, int.Parse(TimeStr[0]), int.Parse(TimeStr[1]), 0);
                e.Event.EndDateTime = date1;

                if (DateHelper.DateSequenceViolation(e.DateToCompareAgainst, date1) || DateHelper.DateSequenceViolation((DateTime)e.Event.StartDateTime, date1))
                {
                    //Invalid start date
                    ModelState.AddModelError("EndDateTime", "EndDateTime invalid.");
                    failcount++;
                }

                TimeStr = e.TicketCutoffTime.Split(':');
                date1 = new DateTime(e.TicketCutoffYear, e.TicketCutoffMonth, e.TicketCutoffDay, int.Parse(TimeStr[0]), int.Parse(TimeStr[1]), 0);
                e.Event.TicketCutoffDate = date1;

                if (DateHelper.DateSequenceViolation(e.DateToCompareAgainst, date1) || DateHelper.DateSequenceViolation(date1, (DateTime)e.Event.EndDateTime))
                {
                    //Invalid start date
                    ModelState.AddModelError("TicketCutOffDate", "Ticket CutOff Date invalid.");
                    failcount++;
                }

                if (failcount > 0)
                    goto fail;

                e.Event.EventName = e.Event.EventName;
                e.Event.LongDescription = e.LongDescription;
                e.Event.ShortDescription = e.ShortDescription;
                e.Event.EventOrganiserID = EventOrganiserID;
                e.Event.EventCategoryID = EventCategoryID;
                e.Event.SubdomainID = SubdomainID;
                e.Event.IsMultiDayEvent = e.IsMultiDayEvent;
                e.Event.IsCashless = e.IsCashless;
                e.Event.LimitOneTicketPerUser = e.LimitOneTicketPerUser;

                if (!e.IsCashless ?? false) { e.LimitOneTicketPerUser = true; }

                db.Entry(e.Event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MyDetails", "EventOrganisers");
            }

        fail:
            ViewBag.EventCategoryID = new SelectList(db.EventCategories.OrderBy(x => x.Description), "ID", "Code", e.Event.EventCategoryID);
            var eo = EventOrganiserRepository.GetEventOrganiserByEmail(User.Identity.Name);
            if (eo != null)
            {
                ViewBag.EventOrganiserID = eo.ID;
                ViewBag.CompanyName = eo.CompanyName;
            }
            ViewBag.StatusID = 1;
            EventViewModel evm = new EventViewModel(e.Event.ID, (int)eo.ID, true);
            Growl("MiiD", "Event updated successfully.");
            return View(evm);
        }

        private string SaveImage(EventViewModel e, string bannerImageData, string fileNamePrefix="Banner")
        {
            if (!String.IsNullOrEmpty(bannerImageData))
            {

                string fileExt = "png";

                fileExt = bannerImageData.Contains("data:image/jpeg;base64,") ? "jpg" : fileExt;
                fileExt = bannerImageData.Contains("data:image/jpg;base64,") ? "jpg" : fileExt;
                fileExt = bannerImageData.Contains("data:image/png;base64,") ? "png" : fileExt;
                fileExt = bannerImageData.Contains("data:image/gif;base64,") ? "gif" : fileExt;

                bannerImageData = bannerImageData.Contains("data:image/jpeg;base64,") ? bannerImageData.Replace("data:image/jpeg;base64,", "") : bannerImageData;
                bannerImageData = bannerImageData.Contains("data:image/jpg;base64,") ? bannerImageData.Replace("data:image/jpg;base64,", "") : bannerImageData;
                bannerImageData = bannerImageData.Contains("data:image/png;base64,") ? bannerImageData.Replace("data:image/png;base64,", "") : bannerImageData;
                bannerImageData = bannerImageData.Contains("data:image/gif;base64,") ? bannerImageData.Replace("data:image/gif;base64,", "") : bannerImageData;
                var bytes = Convert.FromBase64String(bannerImageData);

                string eventName = "Blank";

                if (e.Event.EventName != null)
                {
                    eventName = e.Event.EventName.Replace(" ", String.Empty);//remove  blank spaces
                }

                var fileName = String.Format("{3}_{0}_{1}.{2}", e.Event.ID.ToString(), eventName, fileExt, fileNamePrefix);
                var filePath = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                using (var imageFile = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                    imageFile.Dispose();
                }



                //This is where cropping failed
                /*
                Image croppedImage = ImageHelper.resizeImg(filePath, 1000);

                fileName = String.Format("Banner_crp_{0}_{1}.{2}", e.Event.ID.ToString(), eventName, fileExt);
                filePath = Path.Combine(Server.MapPath("~/Uploads"), fileName);

                switch (fileExt)
                {
                    case "png": croppedImage.Save(filePath, System.Drawing.Imaging.ImageFormat.Png); break;
                    case "jpg": croppedImage.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg); break;
                    case "gif": croppedImage.Save(filePath, System.Drawing.Imaging.ImageFormat.Gif); break;
                    default: break;
                }

                croppedImage.Dispose();
                */
                //delete all other logos for this event
                foreach (var eiDel in db.EventImages.Where(x => x.ImageAltText == fileNamePrefix.ToLower() && x.EventID == e.Event.ID).ToList())
                {
                    db.EventImages.Remove(eiDel);
                }
                EventImage eiB = new EventImage();
                eiB.EventID = e.Event.ID;
                eiB.ImageURL = fileName;
                eiB.ImageAltText = fileNamePrefix.ToLower();
                db.EventImages.Add(eiB);
                db.SaveChanges();
            }

            return bannerImageData;
        }

        [Authorize]
        public ActionResult EditByEONoTickets(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventCategoryID = new SelectList(db.EventCategories, "ID", "Code", @event.EventCategoryID);
            var eo = EventOrganiserRepository.GetEventOrganiserByEmail(User.Identity.Name);
            if (eo != null)
            {
                ViewBag.EventOrganiserID = eo.ID;
                ViewBag.CompanyName = eo.CompanyName;
            }
            ViewBag.StatusID = 1;

            EventViewModel evm = new EventViewModel(@event.ID, (int)@event.EventOrganiserID, true);
            evm.DateToCompareAgainst = DateTime.Now;
            evm.EventOrganiser = eo;

            return View(evm);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult EditByEONoTickets(EventViewModel e, string imageData, string EventLogoURL, int EventOrganiserID)
        {
            if (ModelState.IsValid)
            {
                int failcount = 0;

                string[] TimeStr = e.StartDateTimeTime.Split(':');
                DateTime date1 = new DateTime(e.StartDateYear, e.StartDateMonth, e.StartDateDay, int.Parse(TimeStr[0]), int.Parse(TimeStr[1]), 0);
                e.Event.StartDateTime = date1;


                if (DateHelper.DateSequenceViolation(e.DateToCompareAgainst, date1))
                {
                    //Invalid start date
                    ModelState.AddModelError("StartDateTime", "StartDateTime invalid.");
                    failcount++;
                }

                TimeStr = e.EndDateTimeTime.Split(':');
                date1 = new DateTime(e.EndDateYear, e.EndDateMonth, e.EndDateDay, int.Parse(TimeStr[0]), int.Parse(TimeStr[1]), 0);
                e.Event.EndDateTime = date1;

                if (DateHelper.DateSequenceViolation(e.DateToCompareAgainst, date1) || DateHelper.DateSequenceViolation((DateTime)e.Event.StartDateTime, date1))
                {
                    //Invalid start date
                    ModelState.AddModelError("EndDateTime", "EndDateTime invalid.");
                    failcount++;
                }

                if (failcount > 0)
                    goto fail;


                e.Event.LongDescription = e.LongDescription;
                e.Event.ShortDescription = e.ShortDescription;
                e.Event.EventOrganiserID = EventOrganiserID;

                db.Entry(e.Event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MyDetails", "EventOrganisers");
            }

            fail:
            ViewBag.EventCategoryID = new SelectList(db.EventCategories, "ID", "Code", e.Event.EventCategoryID);
            var eo = EventOrganiserRepository.GetEventOrganiserByEmail(User.Identity.Name);
            if (eo != null)
            {
                ViewBag.EventOrganiserID = eo.ID;
                ViewBag.CompanyName = eo.CompanyName;
            }
            ViewBag.StatusID = 1;
            EventViewModel evm = new EventViewModel(e.Event.ID, (int)eo.ID, true);

            return View(evm);
        }
        [Authorize]
        public ActionResult DetailsByEO(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int EventOrganiserID = 0;
            var eo = EventOrganiserRepository.GetEventOrganiserByEmail(User.Identity.Name);
            if (eo != null)
            {
                EventOrganiserID = eo.ID;

            }


            EventViewModel @event = new EventViewModel((int)id, EventOrganiserID, true);
            @event.CalculateTimeRemaining();

            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }
        [Authorize]
        public ActionResult DetailsByEONoTickets(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var LoggedInUserID = Helpers.UserHelper.UserID(User.Identity.Name);

            EventViewModel @event = new EventViewModel((int)id, LoggedInUserID, true, true);


            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        [Authorize]
        public ActionResult Upload(int id = 0, int eventid = 0)
        {

            Session["EventImageid"] = id;
            Session["EventID"] = eventid;

            return View();
        }

        // This action handles the form POST and the upload
        [HttpPost]
        [Authorize]
        public ActionResult Upload(HttpPostedFileBase file, EventImageModel model)
        {

            int EventImageid = model.ID;
            int EventID = model.EventID;

            if (file != null && file.ContentLength > 0)
            {
                //delete existing EventImage if changing
                if (EventImageid != 0)
                {
                    EventImage EventImage = db.EventImages.Find(EventImageid);
                    db.EventImages.Remove(EventImage);
                    db.SaveChanges();

                }

                //var ProjectStep = db.ProjectSteps.Where(p => p.ID == projectphaseid).SingleOrDefault();
                var eventImage = new EventImage();

                // Verify that the user selected a file
                if (file != null && file.ContentLength > 0)
                {

                    var fileName = Path.GetFileName(file.FileName);

                    fileName = fileName.Replace(" ", String.Empty);//remove  blank spaces

                    fileName = "Bnr_" + DateTime.Now.ToString("yyMMdd_hhmmss_") + fileName;

                    var path = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                    file.SaveAs(path);


                    eventImage.Active = true;
                    eventImage.EventID = EventID;
                    eventImage.ImageURL = fileName;
                    eventImage.ImageAltText = "banner";

                    db.EventImages.Add(eventImage);
                    db.SaveChanges();


                }
            }
            // redirect back to the index action to show the form once again

            return RedirectToAction("EditByEO", "Events", new { id = EventID });

        }


        #endregion


        #region CRUD


        // GET: Events/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {

            ViewBag.EventCategoryID = new SelectList(db.EventCategories, "ID", "Code");
            ViewBag.EventOrganiserID = new SelectList(db.EventOrganisers, "ID", "CompanyName");
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ID,EventName,ShortDescription,LongDescription,StatusID,EventOrganiserID,StartDateTime,EndDateTime,StreetAddress,Suburb,City,PostalCode,GoogleMapsLink,GPSCoordinates,AgeLimit,TicketCutoffDate,EventCategoryID")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventCategoryID = new SelectList(db.EventCategories, "ID", "Code", @event.EventCategoryID);
            ViewBag.EventOrganiserID = new SelectList(db.EventOrganisers, "ID", "CompanyName", @event.EventOrganiserID);
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code", @event.StatusID);
            return View(@event);
        }



        // GET: Events/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventCategoryID = new SelectList(db.EventCategories, "ID", "Code", @event.EventCategoryID);
            ViewBag.EventOrganiserID = new SelectList(db.EventOrganisers, "ID", "CompanyName", @event.EventOrganiserID);
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code", @event.StatusID);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ID,EventName,ShortDescription,LongDescription,StatusID,EventOrganiserID,StartDateTime,EndDateTime,StreetAddress,Suburb,City,PostalCode,GoogleMapsLink,GPSCoordinates,AgeLimit,TicketCutoffDate,EventCategoryID")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventCategoryID = new SelectList(db.EventCategories, "ID", "Code", @event.EventCategoryID);
            ViewBag.EventOrganiserID = new SelectList(db.EventOrganisers, "ID", "CompanyName", @event.EventOrganiserID);
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code", @event.StatusID);
            return View(@event);
        }

        // GET: Events/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        #endregion
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    internal class Firstname
    {
    }
}
