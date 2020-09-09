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
using FluentValidation.Attributes;
using MiidWeb.Rules;
using System.ComponentModel.DataAnnotations;
using MiidWeb.Repositories;
using System.Configuration;

namespace MiidWeb.Models
{
    [Validator(typeof(EventViewValidator))]
    public class EventViewModel
    {
        private MiidEntities db = new MiidEntities();
        public Event Event { get; set; }

        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateToCompareAgainst { get; set; }

        public List<EventImage> EventImages { get; set; }

        public List<VendorViewModel> Vendors { get; set; }

        public List<TicketClassViewModel> TicketClasses { get; set; }

        public EventOrganiser EventOrganiser { get; set; }

        public EventCategory EventCategory { get; set; }

        public EndUser EndUser { get; set; }

        public List<TicketViewModel> LoggedInUserTickets { get; set; }

        public CalendarViewModel CalendarViewModel { get; set; }

        public int TicketsSold { get; set; }

        public decimal RevenueGenerated { get; set; }
        public int SubdomainID { get; set; }
        public int DaysTilEvent { get; set; }
        public int HoursTilEvent { get; set; }
        public int MinutesTilEvent { get; set; }

        public string EventLogoURL { get; set; }

        public string EventBannerURL { get; set; }
        public string SeatPlanImage { get; set; }

        public DateTime? TicketCutoffDate { get; set; }
        public int TicketCutoffDay { get; set; }
        public int TicketCutoffMonth { get; set; }
        public int TicketCutoffYear { get; set; }
        public string TicketCutoffTime { get; set; }

        public IEnumerable<SelectListItem> TicketCutoffDays { get; set; }
        public IEnumerable<SelectListItem> TicketCutoffMonths { get; set; }
        public IEnumerable<SelectListItem> TicketCutoffYears { get; set; }
        public IEnumerable<SelectListItem> TicketCutoffTimes { get; set; }
        public PagedList.IPagedList<Event> events { get; set; }

        public int StartDateDay { get; set; }
        public int StartDateMonth { get; set; }
        public int StartDateYear { get; set; }
        public string StartDateTimeTime { get; set; }

        public IEnumerable<SelectListItem> StartDateDays { get; set; }
        public IEnumerable<SelectListItem> StartDateMonths { get; set; }
        public IEnumerable<SelectListItem> StartDateYears { get; set; }

        public IEnumerable<SelectListItem> StartDateTimeTimes { get; set; }


        public int EndDateDay { get; set; }
        public int EndDateMonth { get; set; }
        public int EndDateYear { get; set; }

        public string EndDateTimeTime { get; set; }
        public IEnumerable<SelectListItem> EndDateDays { get; set; }
        public IEnumerable<SelectListItem> EndDateMonths { get; set; }
        public IEnumerable<SelectListItem> EndDateYears { get; set; }
        public IEnumerable<SelectListItem> EndDateTimeTimes { get; set; }

        public string NewVendorCode { get; set; }

        public List<VendorEventViewModel> VendorPOSTransactions { get; set; }

        public decimal TotalVendorRevenueGenerated { get; set; }

        public decimal TotalTicketSaleRevenueGenerated { get; set; }


        public int DaysRemaining { get; set; }
        public int HoursRemaining { get; set; }
        public int MinutesRemaining { get; set; }


        public bool? IsMultiDayEvent { get; set; }
        public bool? IsCashless { get; set; }

        public bool? LimitOneTicketPerUser { get; set; }
        private static List<SelectListItem> GetMonthDropDown()
        {
            var date2 = new List<SelectListItem>();
            int x = 1;

            while (x <= 12)
            {

                DateTime d = new DateTime(2000, x, 1);
                date2.Add(new SelectListItem() { Value = x.ToString(), Text = d.ToString("MMMM") });
                x++;
            }
            return date2;
        }

        private static List<SelectListItem> GetNumberDropDown(int x, int y)
        {
            List<SelectListItem> date1 = new List<SelectListItem>();

            while (x <= y)
            {
                date1.Add(new SelectListItem() { Value = x.ToString(), Text = x.ToString() });
                x++;
            }

            return date1;
        }

        private static List<SelectListItem> GetTimeDropDown(int x, int y)
        {
            List<SelectListItem> date1 = new List<SelectListItem>();

            while (x <= y)
            {
                if (x < 10)
                {
                    date1.Add(new SelectListItem() { Value = "0" + x.ToString() + ":00", Text = "0" + x.ToString() + ":00" });
                }
                else
                {
                    date1.Add(new SelectListItem() { Value = x.ToString() + ":00", Text = x.ToString() + ":00" });
                }
                x++;
            }

            return date1;
        }

        public EventViewModel()
        { }


        public EventViewModel(bool PlainForSearch, int id)
        {
            this.Event = db.Events.Find(id);
            this.EventOrganiser = db.EventOrganisers.Find(this.Event.EventOrganiserID);
            this.EventImages = db.EventImages.Where(x => x.EventID == id).ToList();
            this.SubdomainID = this.Event.SubdomainID ?? 0;
            if (this.EventImages != null && this.EventImages.Where(x => x.ImageAltText == "logo").Count() > 0)
            {
                this.EventLogoURL = this.EventImages.Where(x => x.ImageAltText == "logo").First().ImageURL;
            }

            if (this.EventImages != null && this.EventImages.Where(x => x.ImageAltText == "banner").Count() > 0)
            {
                this.EventBannerURL = this.EventImages.Where(x => x.ImageAltText == "banner").First().ImageURL;
            }

            if (this.EventImages != null && this.EventImages.Where(x => x.ImageAltText == "seatingplan").Count() > 0)
            {
                this.SeatPlanImage = this.EventImages.Where(x => x.ImageAltText == "seatingplan").First().ImageURL;
            }

        }

        public EventViewModel(bool DateDropDowns)
        {
            int baseYear = int.Parse(ConfigRepo.Get("baseYear").ToString());


            this.TicketCutoffMonths = GetMonthDropDown();
            this.TicketCutoffDays = GetNumberDropDown(1, 31);
            this.TicketCutoffYears = GetNumberDropDown(baseYear, baseYear + 5);

            this.StartDateMonths = GetMonthDropDown();
            this.StartDateDays = GetNumberDropDown(1, 31);
            this.StartDateYears = GetNumberDropDown(baseYear, baseYear + 5);

            this.EndDateMonths = GetMonthDropDown();
            this.EndDateDays = GetNumberDropDown(1, 31);
            this.EndDateYears = GetNumberDropDown(baseYear, baseYear + 5);

            this.StartDateTimeTimes = GetTimeDropDown(0, 23);
            this.EndDateTimeTimes = GetTimeDropDown(0, 23);
            this.TicketCutoffTimes = GetTimeDropDown(0, 23);

            this.Event = new Event();

        }
        public EventViewModel(int EventID, int LoggedInUserID)
        {

            int baseYear = int.Parse(ConfigRepo.Get("baseYear").ToString());


            this.Event = db.Events.Include(x => x.EventCategory).Include(x => x.EventOrganiser).Include(x => x.Status).Where(x => x.ID == EventID).First();
            this.SubdomainID = this.Event.SubdomainID ?? 0;
            this.EventCategory = Event.EventCategory;

            this.EventOrganiser = Event.EventOrganiser;

            this.Vendors = new List<VendorViewModel>();

            this.EventImages = db.EventImages.Where(x => x.EventID == EventID).ToList();

            if (this.EventImages.Where(x => x.ImageAltText == "logo").Count() > 0)
            {
                this.EventLogoURL = this.EventImages.Where(x => x.ImageAltText == "logo").First().ImageURL;
            }

            if (this.EventImages.Where(x => x.ImageAltText == "banner").Count() > 0)
            {
                this.EventBannerURL = this.EventImages.Where(x => x.ImageAltText == "banner").First().ImageURL;
            }
            if (this.EventImages != null && this.EventImages.Where(x => x.ImageAltText == "seatingplan").Count() > 0)
            {
                this.SeatPlanImage = this.EventImages.Where(x => x.ImageAltText == "seatingplan").First().ImageURL;
            }
            var ve = db.VendorEvents.Include(x => x.Vendor).Where(x => x.EventID == EventID).ToList();

            foreach (var ven in ve)
            {

                this.Vendors.Add(new VendorViewModel(ven.Vendor.VendorCode, EventID));
            }
            //here and below only active
            var TicketClasses = db.TicketClasses.Include(x => x.Status).Where(x => x.EventID == EventID).ToList();



            this.TicketClasses = new List<TicketClassViewModel>();
            foreach (var tc in TicketClasses)
            {
                var tcvm = new TicketClassViewModel(tc.ID);
                this.TicketClasses.Add(tcvm);
            }


            this.LoggedInUserTickets = new List<TicketViewModel>();

            foreach (var ticketClass in this.TicketClasses)
            {
                int Expired = Helpers.StatusHelper.StatusID("Ticket", "Expired");

                //x.StatusID == Purchased && 
                var myTickets = db.Tickets.Where(x => x.EndUserID == LoggedInUserID
                && x.StatusID != Expired
                && x.TicketClassID == ticketClass.TicketClass.ID).ToList();
                var list = new List<TicketViewModel>();

                foreach (var t in myTickets)
                {
                    list.Add(new TicketViewModel(t.ID));
                }

                this.LoggedInUserTickets.AddRange(list);
            }

            this.EndUser = db.EndUsers.Find(LoggedInUserID);

        }


        public EventViewModel(int EventID)
        {

            int baseYear = int.Parse(ConfigRepo.Get("baseYear").ToString());


            this.Event = db.Events.Include(x => x.EventCategory).Include(x => x.EventOrganiser).Include(x => x.Status).Where(x => x.ID == EventID).First();
            this.SubdomainID = this.Event.SubdomainID ?? 0;
            this.EventCategory = Event.EventCategory;

            this.EventOrganiser = Event.EventOrganiser;

            this.Vendors = new List<VendorViewModel>();

            this.EventImages = db.EventImages.Where(x => x.EventID == EventID).ToList();

            if (this.EventImages.Where(x => x.ImageAltText == "logo").Count() > 0)
            {
                this.EventLogoURL = this.EventImages.Where(x => x.ImageAltText == "logo").First().ImageURL;
            }

            if (this.EventImages.Where(x => x.ImageAltText == "banner").Count() > 0)
            {
                this.EventBannerURL = this.EventImages.Where(x => x.ImageAltText == "banner").First().ImageURL;
            }
            if (this.EventImages != null && this.EventImages.Where(x => x.ImageAltText == "seatingplan").Count() > 0)
            {
                this.SeatPlanImage = this.EventImages.Where(x => x.ImageAltText == "seatingplan").First().ImageURL;
            }
            var ve = db.VendorEvents.Include(x => x.Vendor).Where(x => x.EventID == EventID).ToList();

            foreach (var ven in ve)
            {

                this.Vendors.Add(new VendorViewModel(ven.Vendor.VendorCode, EventID));
            }



            var TicketClasses = db.TicketClasses.Include(x => x.Status).Where(x => x.EventID == EventID).ToList();

            this.TicketClasses = new List<TicketClassViewModel>();
            foreach (var tc in TicketClasses)
            {
                var tcvm = new TicketClassViewModel(tc.ID);
                this.TicketClasses.Add(tcvm);
            }



        }


        public EventViewModel(int EventID, int EventOrganiserID, bool ForEventOrganiser)
        {
            int baseYear = int.Parse(ConfigRepo.Get("baseYear").ToString());

            this.Event = db.Events.Include(x => x.EventCategory).Include(x => x.EventOrganiser).Include(x => x.Status).Where(x => x.ID == EventID).First();
            this.SubdomainID = this.Event.SubdomainID ?? 0;
            this.StartDateDay = ((DateTime)this.Event.StartDateTime).Day;
            this.StartDateMonth = ((DateTime)this.Event.StartDateTime).Month;
            this.StartDateYear = ((DateTime)this.Event.StartDateTime).Year;
            this.StartDateTimeTime = ((DateTime)this.Event.StartDateTime).ToString("HH:mm");


            this.EndDateDay = ((DateTime)this.Event.EndDateTime).Day;
            this.EndDateMonth = ((DateTime)this.Event.EndDateTime).Month;
            this.EndDateYear = ((DateTime)this.Event.EndDateTime).Year;
            this.EndDateTimeTime = ((DateTime)this.Event.EndDateTime).ToString("HH:mm");


            this.TicketCutoffDay = ((DateTime)this.Event.TicketCutoffDate).Day;
            this.TicketCutoffMonth = ((DateTime)this.Event.TicketCutoffDate).Month;
            this.TicketCutoffYear = ((DateTime)this.Event.TicketCutoffDate).Year;
            this.TicketCutoffTime = ((DateTime)this.Event.TicketCutoffDate).ToString("HH:mm");



            this.EventCategory = Event.EventCategory;

            this.EventOrganiser = Event.EventOrganiser;

            this.ShortDescription = Event.ShortDescription;
            this.LongDescription = Event.LongDescription;

            this.EventImages = db.EventImages.Where(x => x.EventID == EventID).ToList();

            if (this.EventImages.Where(x => x.ImageAltText == "logo").Count() > 0)
            {
                this.EventLogoURL = this.EventImages.Where(x => x.ImageAltText == "logo").First().ImageURL;
            }

            if (this.EventImages.Where(x => x.ImageAltText == "banner").Count() > 0)
            {
                this.EventBannerURL = this.EventImages.Where(x => x.ImageAltText == "banner").First().ImageURL;
            }
            if (this.EventImages != null && this.EventImages.Where(x => x.ImageAltText == "seatingplan").Count() > 0)
            {
                this.SeatPlanImage = this.EventImages.Where(x => x.ImageAltText == "seatingplan").First().ImageURL;
            }

            var ve = db.VendorEvents.Include(x => x.Vendor).Where(x => x.EventID == EventID).ToList();

            this.Vendors = new List<VendorViewModel>();

            if (ve.Count() > 0)
            {
                foreach (var ven in ve)
                {
                    var vendor = db.Vendors.Find(ven.VendorID);
                    if (vendor != null)
                    {
                        this.Vendors.Add(new VendorViewModel(ven.Vendor.VendorCode, EventID));
                    }
                }
            }

            this.TicketsSold = 0;

            this.TicketClasses = new List<TicketClassViewModel>();
            var TicketClassesList = db.TicketClasses.Include(x => x.Status).Where(x => x.EventID == EventID).ToList();
            foreach (var tc in TicketClassesList)
            {
                var tcvm = new TicketClassViewModel(tc.ID);
                int? thisClassTicketCount = db.Tickets.Where(x => x.TicketClassID == tc.ID && (x.StatusID == 5 || x.StatusID == 4 || x.StatusID == 5010)).Count();
                tcvm.TicketsSold = thisClassTicketCount ?? 0;
                tcvm.TotalCost = (decimal)(tcvm.TicketsSold * tc.Price ?? 0.0M);
                tcvm.TicketClass = tc;

                decimal totalDiscount = 0.0M;

                var promocodes = db.PromoCodes.Where(x => x.TicketClassID == tc.ID && x.DateUsed != null && x.TicketID != null);

                if (promocodes != null && promocodes.Count() > 0)
                {
                    totalDiscount = promocodes.Count() * ((decimal)(promocodes.First().DiscountPercentage / 100.0M) * (decimal)(tc.Price ?? 0));
                }
                tcvm.TotalCost -= totalDiscount;
                this.TicketClasses.Add(tcvm);

                this.TicketsSold += thisClassTicketCount ?? 0;

                int thisClassTicketCount1 = thisClassTicketCount ?? 0;

                int thisClassPrice = (int)tc.Price;

                this.RevenueGenerated += (decimal)(thisClassTicketCount1 * thisClassPrice) - (totalDiscount);
            }

            TimeSpan ts = (DateTime)this.Event.StartDateTime - DateTime.Now;

            this.DaysTilEvent = ts.Days;

            this.TicketCutoffMonths = GetMonthDropDown();
            this.TicketCutoffDays = GetNumberDropDown(1, 31);
            this.TicketCutoffYears = GetNumberDropDown(baseYear, baseYear + 5);

            this.StartDateMonths = GetMonthDropDown();
            this.StartDateDays = GetNumberDropDown(1, 31);
            this.StartDateYears = GetNumberDropDown(baseYear, baseYear + 5);

            this.EndDateMonths = GetMonthDropDown();
            this.EndDateDays = GetNumberDropDown(1, 31);
            this.EndDateYears = GetNumberDropDown(baseYear, baseYear + 5);

            this.StartDateTimeTimes = GetTimeDropDown(0, 23);
            this.EndDateTimeTimes = GetTimeDropDown(0, 23);
            this.TicketCutoffTimes = GetTimeDropDown(0, 23);

            this.Vendors = new List<VendorViewModel>();
            var vendorevents = db.VendorEvents.Where(x => x.EventID == this.Event.ID);

            if (vendorevents.Count() > 0)
            {
                foreach (var v in vendorevents)
                {
                    var vendor = db.Vendors.Find(v.VendorID);
                    this.Vendors.Add(new VendorViewModel(v.Vendor.VendorCode, EventID));
                }
            }
            this.IsMultiDayEvent = Event.IsMultiDayEvent;
            this.IsCashless = Event.IsCashless;

        }

        public EventViewModel(int EventID, int EventOrganiserID, bool ForEventOrganiser, bool NoTickets)
        {
            this.Event = db.Events.Include(x => x.EventCategory).Include(x => x.EventOrganiser).Include(x => x.Status).Where(x => x.ID == EventID).First();
            this.SubdomainID = this.Event.SubdomainID ?? 0;
            int baseYear = int.Parse(ConfigRepo.Get("baseYear").ToString());

            this.Event = db.Events.Include(x => x.EventCategory).Include(x => x.EventOrganiser).Include(x => x.Status).Where(x => x.ID == EventID).First();

            this.StartDateDay = ((DateTime)this.Event.StartDateTime).Day;
            this.StartDateMonth = ((DateTime)this.Event.StartDateTime).Month;
            this.StartDateYear = ((DateTime)this.Event.StartDateTime).Year;
            this.StartDateTimeTime = ((DateTime)this.Event.StartDateTime).ToString("HH:mm");


            this.EndDateDay = ((DateTime)this.Event.EndDateTime).Day;
            this.EndDateMonth = ((DateTime)this.Event.EndDateTime).Month;
            this.EndDateYear = ((DateTime)this.Event.EndDateTime).Year;
            this.EndDateTimeTime = ((DateTime)this.Event.EndDateTime).ToString("HH:mm");


            this.TicketCutoffDay = ((DateTime)this.Event.TicketCutoffDate).Day;
            this.TicketCutoffMonth = ((DateTime)this.Event.TicketCutoffDate).Month;
            this.TicketCutoffYear = ((DateTime)this.Event.TicketCutoffDate).Year;
            this.TicketCutoffTime = ((DateTime)this.Event.TicketCutoffDate).ToString("HH:mm");


            this.EventCategory = Event.EventCategory;

            this.EventOrganiser = Event.EventOrganiser;

            this.ShortDescription = Event.ShortDescription;
            this.LongDescription = Event.LongDescription;

            this.EventImages = db.EventImages.Where(x => x.EventID == EventID).ToList();

            if (this.EventImages.Where(x => x.ImageAltText == "logo").Count() > 0)
            {
                this.EventLogoURL = this.EventImages.Where(x => x.ImageAltText == "logo").First().ImageURL;
            }

            var ve = db.VendorEvents.Include(x => x.Vendor).Where(x => x.EventID == EventID).ToList();

            this.Vendors = new List<VendorViewModel>();

            if (ve.Count() > 0)
            {
                foreach (var ven in ve)
                {
                    var vendor = db.Vendors.Find(ven.VendorID);
                    if (vendor != null)
                    {
                        this.Vendors.Add(new VendorViewModel(ven.Vendor.VendorCode, EventID));
                    }
                }
            }


            TimeSpan ts = (DateTime)this.Event.StartDateTime - DateTime.Now;

            this.DaysTilEvent = ts.Days;

            this.HoursTilEvent = ts.Hours;// -(ts.Days * 24);

            this.MinutesTilEvent = ts.Minutes;// -(ts.Days * 24 * 60) - (this.HoursTilEvent * 60);


            this.TicketCutoffMonths = GetMonthDropDown();
            this.TicketCutoffDays = GetNumberDropDown(1, 31);
            this.TicketCutoffYears = GetNumberDropDown(2015, 2020);

            this.StartDateMonths = GetMonthDropDown();
            this.StartDateDays = GetNumberDropDown(1, 31);
            this.StartDateYears = GetNumberDropDown(2015, 2020);

            this.EndDateMonths = GetMonthDropDown();
            this.EndDateDays = GetNumberDropDown(1, 31);
            this.EndDateYears = GetNumberDropDown(2015, 2020);

            this.StartDateTimeTimes = GetTimeDropDown(0, 23);
            this.EndDateTimeTimes = GetTimeDropDown(0, 23);
            this.TicketCutoffTimes = GetTimeDropDown(0, 23);

            this.Vendors = new List<VendorViewModel>();
            var vendorevents = db.VendorEvents.Where(x => x.EventID == this.Event.ID);

            if (vendorevents.Count() > 0)
            {
                foreach (var v in vendorevents)
                {
                    var vendor = db.Vendors.Find(v.VendorID);
                    this.Vendors.Add(new VendorViewModel(v.Vendor.VendorCode, EventID));
                }
            }

            VendorEventRepository repo = new VendorEventRepository();

            this.VendorPOSTransactions = repo.GetAllVendorTransactions(this.Event.ID);
            this.TotalVendorRevenueGenerated = repo.GetAllVendorTransactionsTotal(this.Event.ID);

        }


        public void CalculateTimeRemaining()
        {


            var ticketCut = (DateTime)this.Event.TicketCutoffDate;
            TimeSpan timespan = (ticketCut - DateTime.Now);
            double daysleft = timespan.TotalDays;

            double hoursLeft = (timespan.TotalHours - (daysleft * 24)) % 24;
            double minutesLeft = (timespan.TotalMinutes - (hoursLeft * 60)) % 60;

            this.DaysRemaining = (int)decimal.Floor((decimal)timespan.TotalDays);
            this.HoursRemaining = (int)decimal.Floor((decimal)hoursLeft);
            this.MinutesRemaining = (int)decimal.Floor((decimal)minutesLeft);


        }

    }

    public class EventListViewModel
    {
        private MiidEntities db = new MiidEntities();
        public List<EventViewModel> Events { get; set; }

        public EventListViewModel(string searchString, DateTime FromDate, DateTime ToDate, int LoggedInUserID)
        {
            int baseYear = int.Parse(ConfigRepo.Get("baseYear").ToString());


            this.Events = new List<EventViewModel>();

            var events = !String.IsNullOrEmpty(searchString) ? db.Events.Where(x => x.EventName.Contains(searchString)
            || x.ID.ToString().Contains(searchString)
            || x.EventOrganiser.CompanyName.Contains(searchString)
            || x.City.Contains(searchString)
            || x.EventCategory.Description.Contains(searchString)).OrderByDescending(c => c.StartDateTime) : db.Events;


            foreach (var ev in events)
            {
                this.Events.Add(new EventViewModel(ev.ID, LoggedInUserID));
            }

        }

        public EventListViewModel(int LoggedInUserID, bool ShowCurrentEvents)
        {

            int baseYear = int.Parse(ConfigRepo.Get("baseYear").ToString());


            this.Events = new List<EventViewModel>();

            DateTime date = DateTime.Now.AddDays(-1);

            //var events = db.Events;//.Where(x => x.StartDateTime >= date);

            List<int> eventIDsIHaveTicketsFor = new List<int>();
            eventIDsIHaveTicketsFor = EndUserRepository.EventIDsIHaveTicketsFor(LoggedInUserID);

            foreach (var ev in eventIDsIHaveTicketsFor)
            {
                var evm = new EventViewModel(ev, LoggedInUserID);
                if (evm.LoggedInUserTickets.Count() > 0)
                {
                    this.Events.Add(evm);
                }
            }

        }


        public EventListViewModel(DateTime FromDate, DateTime ToDate, int LoggedInUserID, string Descr)
        {
            this.Events = new List<EventViewModel>();

            var events = db.Events.Where(x => x.StartDateTime >= FromDate && x.StartDateTime <= ToDate && x.LongDescription.ToLower().Contains(Descr.ToLower()));

            var eventsInCategory = db.Events.Include(x => x.EventCategory).Where(x => x.EventCategory.Description.ToLower().Contains(Descr.ToLower()));

            var eventOrganisedBy = db.Events.Include(x => x.EventOrganiser).Where(x => x.EventOrganiser.CompanyName.ToLower().Contains(Descr.ToLower()));


            List<int> eventIDs = new List<int>();

            foreach (var r in events)
            {
                eventIDs.Add(r.ID);
            }
            foreach (var r in eventsInCategory)
            {
                eventIDs.Add(r.ID);
            }
            foreach (var r in eventOrganisedBy)
            {
                eventIDs.Add(r.ID);
            }
            foreach (var ev in eventIDs.Distinct())
            {
                this.Events.Add(new EventViewModel(ev, LoggedInUserID));
            }

        }

        public EventListViewModel(int EventOrganiserID)
        {
            this.Events = new List<EventViewModel>();

            var events = db.Events.Where(x => x.EventOrganiserID == EventOrganiserID).OrderBy(x => x.StatusID).OrderBy(x => x.StartDateTime);


            foreach (var ev in events)
            {
                this.Events.Add(new EventViewModel(ev.ID, EventOrganiserID, true));
            }

        }

        public EventListViewModel(string KeyWordSearchText, int subDomainID = 0)
        {
            this.Events = new List<EventViewModel>();
            List<Event> events = new List<Event>();
            List<Event> eventsInCategory = new List<Event>();
            List<Event> eventOrganisedBy = new List<Event>();

            // var events = db.Events.ToList();

            if (!String.IsNullOrEmpty(KeyWordSearchText))
            {
                events = db.Events.Where(x => x.SubdomainID == subDomainID && x.EventName.ToLower().Contains(KeyWordSearchText.ToLower())).ToList();

                eventsInCategory = db.Events.Include(x => x.EventCategory).Where(x => x.EventCategory.Description.ToLower().Contains(KeyWordSearchText.ToLower())).ToList();

                eventOrganisedBy = db.Events.Include(x => x.EventOrganiser).Where(x => x.EventOrganiser.CompanyName.ToLower().Contains(KeyWordSearchText.ToLower())).ToList();
            }

            List<int> eventIDs = new List<int>();

            foreach (var r in events)
            {
                eventIDs.Add(r.ID);
            }
            foreach (var r in eventsInCategory)
            {
                eventIDs.Add(r.ID);
            }
            foreach (var r in eventOrganisedBy)
            {
                eventIDs.Add(r.ID);
            }
            foreach (var ev in eventIDs.Distinct())
            {
                this.Events.Add(new EventViewModel(ev));
            }


            if (String.IsNullOrEmpty(KeyWordSearchText))
            {
                foreach (var eve in db.Events.Where(x => x.SubdomainID == subDomainID).ToList())
                {
                    this.Events.Add(new EventViewModel(eve.ID));
                }

            }


        }


        public EventListViewModel(EventSearchModel model)
        {
            this.Events = new List<EventViewModel>();

            var events = db.Events.AsEnumerable();

            if (!String.IsNullOrEmpty(model.KeyWordSearchText))
            {
                events = events.Where(x => x.EventName.ToLower().Contains(model.KeyWordSearchText.ToLower())).ToList();
            }

            if (model.RegionID != null)
            {
                events = events.Where(x => x.RegionID == model.RegionID);
            }

            if (model.EventOrganiserID != null)
            {
                events = events.Where(x => x.EventOrganiserID == model.EventOrganiserID);
            }

            if (model.EventCategoryID != null)
            {
                events = events.Where(x => x.EventCategoryID == model.EventCategoryID);
            }

            if (model.EventMonth != null)
            {
                var date = (DateTime)model.EventMonth;

                events = events.Where(x => x.StartDateTime >= date && x.StartDateTime <= date.AddMonths(1));
            }


            foreach (var ev in events)
            {
                this.Events.Add(new EventViewModel(PlainForSearch: true, id: ev.ID));
            }


        }

    }
}
