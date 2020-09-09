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
using MiidWeb.Helpers;

namespace MiidWeb.Models
{

    public class TicketClassLiteViewModel
    {
        

        public int ID { get; set; }
        public string Code { get; set; }
        public Nullable<int> EventID { get; set; }
        public string Description { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string ImageURL { get; set; }
        public Nullable<System.DateTime> DateTimeUpdated { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> RunningQuantity { get; set; }
        public Nullable<int> MaxOnlineSaleQuantity { get; set; }
        public Nullable<int> StatusID { get; set; }

        public string EventName { get; set; }

    }






    public class TicketClassViewModel
    {
        public TicketClass TicketClass { get; set; }

        

        public TicketClass Code { get; set; }

        public string TicketClassDescription { get; set; }
        public SelectList TicketQuantityList { get; set; }

        public int SelectedTicketQuantity { get; set; }
        public int EventID { get; set; }
        public int TicketsSold { get; set; }
        public int TicketsReserved { get; set; }
        public int TicketCount { get; set; }
        public decimal TotalCost { get; set; }


        public bool BoxOffice { get; set; }
        public int OldQuantity { get; set; }
        public int RunningQuantity { get; set; }
        public int StartDateDay { get; set; }
        public int StartDateMonth { get; set; }
        public int StartDateYear { get; set; }
        public string StartDateTimeTime { get; set; }

        public IEnumerable<SelectListItem> StartDateDays { get; set; }
        public IEnumerable<SelectListItem> StartDateMonths { get; set; }
        public IEnumerable<SelectListItem> StartDateYears { get; set; }
        public IEnumerable<SelectListItem> StatusOptions { get; set; }

        public IEnumerable<SelectListItem> StartDateTimeTimes { get; set; }


        public int EndDateDay { get; set; }
        public int EndDateMonth { get; set; }
        public int EndDateYear { get; set; }

        public string EndDateTimeTime { get; set; }
        public IEnumerable<SelectListItem> EndDateDays { get; set; }
        public IEnumerable<SelectListItem> EndDateMonths { get; set; }
        public IEnumerable<SelectListItem> EndDateYears { get; set; }
        public IEnumerable<SelectListItem> EndDateTimeTimes { get; set; }

        public Event Event { get; set; }
        public List<TicketSeatTuple> ChosenRowSeats { get; set; }

        public List<TicketClassSeatRange> TicketClassSeatRanges { get; set; }
        public bool IsOnline { get; set; }

        public TicketClassViewModel()
        {

        }
        public TicketClassViewModel(int id, int selectedTicketCount = 0, List<TicketSeatTuple> chosenrowseats=null)
        {
            int baseYear = int.Parse(ConfigRepo.Get("baseYear").ToString());

            this.ChosenRowSeats = chosenrowseats;
            
            var db = new MiidEntities();
            

            this.TicketClass = db.TicketClasses.Find(id);

            this.TicketClassSeatRanges = db.TicketClassSeatRanges.Where(x => x.TicketClassID == id).ToList();

            this.Event = db.Events.Find(this.TicketClass.EventID);
            this.BoxOffice = TicketClass.BoxOffice??false;
            this.IsOnline = TicketClass.IsOnline??false;
            this.OldQuantity = TicketClass.Quantity??0;
            this.TicketsSold = db.Tickets.Where(x => x.TicketClassID == id && (x.StatusID == 5 || x.StatusID == 4 )).Count(); //status 5&4 - only count purchased and used tickets and refunds pending
            this.TotalCost = (decimal)((TicketClass.Price??0.00M) * (this.TicketCount));

            this.StartDateDay = ((DateTime)this.TicketClass.StartDate).Day;
            this.StartDateMonth = ((DateTime)this.TicketClass.StartDate).Month;
            this.StartDateYear = ((DateTime)this.TicketClass.StartDate).Year;
            this.StartDateTimeTime = ((DateTime)this.TicketClass.StartDate).ToString("HH:mm");


            this.EndDateDay = ((DateTime)this.TicketClass.EndDate).Day;
            this.EndDateMonth = ((DateTime)this.TicketClass.EndDate).Month;
            this.EndDateYear = ((DateTime)this.TicketClass.EndDate).Year;
            this.EndDateTimeTime = ((DateTime)this.TicketClass.EndDate).ToString("HH:mm");

            this.StartDateMonths = GetMonthDropDown();
            this.StartDateDays = GetNumberDropDown(1, 31);
            this.StartDateYears = GetNumberDropDown(baseYear, baseYear + 5);

            this.EndDateMonths = GetMonthDropDown();
            this.EndDateDays = GetNumberDropDown(1, 31);
            this.EndDateYears = GetNumberDropDown(baseYear, baseYear + 5);

            this.StartDateTimeTimes = GetTimeDropDown(0, 23);
            this.EndDateTimeTimes = GetTimeDropDown(0, 23);

            this.StatusOptions = GetStatus();

            int purchasedID = StatusHelper.StatusID("Ticket", "Purchased"); 
            
            var ticketsSold = db.Tickets.Where(x => x.TicketClassID == id && x.EndUserID != null && (x.StatusID == 5 || x.StatusID == 4));
            if (ticketsSold != null && ticketsSold.Count() > 0)
            {
                this.TicketsSold = ticketsSold.Count();
            }
            List<SelectListItem> ticketcounter = new List<SelectListItem>();
            int xy = 0;
            while (xy <= 20 && xy <= this.TicketClass.RunningQuantity)
            {
                if (xy != selectedTicketCount)
                {
                    ticketcounter.Add(new SelectListItem { Text = xy.ToString(), Value = xy.ToString() });
                }
                else
                {
                    ticketcounter.Add(new SelectListItem { Text = xy.ToString(), Value = xy.ToString(), Selected=true });
                }
                xy++;
            }
            this.RunningQuantity = this.TicketClass.RunningQuantity??0;

            this.TicketQuantityList = new SelectList(ticketcounter, "Value", "Text", selectedTicketCount);

            this.SelectedTicketQuantity = selectedTicketCount;
            this.TicketCount = selectedTicketCount;

            //Commented out to work on line
            this.ChosenRowSeats = chosenrowseats;
            if (ChosenRowSeats != null && ChosenRowSeats.Count() > 0)
            {
                this.TicketCount = chosenrowseats.Count();
            }
            else
            {
                this.TicketCount = this.SelectedTicketQuantity;
            }
            this.TotalCost = (int)(this.TicketClass.Price * this.TicketCount);


        }



        private static List<SelectListItem> GetStatus()
        {
            var db = new MiidEntities();
            var statusList = new List<SelectListItem>();
            foreach (var s in db.Status.Where(x => x.Context == "TicketClass").ToList())
            {
                statusList.Add(new SelectListItem(){ Value = s.ID.ToString(), Text = s.Code });
            }

            return statusList;
        }

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
                    date1.Add(new SelectListItem() { Value = "0" + x.ToString() + ":15", Text = "0" + x.ToString() + ":15" });
                    date1.Add(new SelectListItem() { Value = "0" + x.ToString() + ":30", Text = "0" + x.ToString() + ":30" });
					date1.Add(new SelectListItem() { Value = "0" + x.ToString() + ":45", Text = "0" + x.ToString() + ":45" });
				}
                else
                {
                    date1.Add(new SelectListItem() { Value = x.ToString() + ":00", Text = x.ToString() + ":00" });
                    date1.Add(new SelectListItem() { Value = x.ToString() + ":15", Text = x.ToString() + ":15" });
                    date1.Add(new SelectListItem() { Value = x.ToString() + ":30", Text = x.ToString() + ":30" });
                }
                x++;
            }

            return date1;
        }

      

        public TicketClassViewModel(int EventID, bool newTC, int baseYear)
        {
            var db = new MiidEntities();
            
            this.EventID = EventID;
            this.Event = db.Events.Find(EventID);
            this.TicketClass = new TicketClass();
            this.OldQuantity = TicketClass.Quantity ?? 0;

            if (!newTC)
            {
                this.TicketClass = db.TicketClasses.Include(x => x.Event).Include(x => x.Status).Where(x => x.ID == EventID).First();

                this.StartDateDay = ((DateTime)this.TicketClass.StartDate).Day;
                this.StartDateMonth = ((DateTime)this.TicketClass.StartDate).Month;
                this.StartDateYear = ((DateTime)this.TicketClass.StartDate).Year;
                this.StartDateTimeTime = ((DateTime)this.TicketClass.StartDate).ToString("HH:mm");


                this.EndDateDay = ((DateTime)this.TicketClass.EndDate).Day;
                this.EndDateMonth = ((DateTime)this.TicketClass.EndDate).Month;
                this.EndDateYear = ((DateTime)this.TicketClass.EndDate).Year;
                this.EndDateTimeTime = ((DateTime)this.TicketClass.EndDate).ToString("HH:mm");

            }
            else//default to event date start end
            {
                this.StartDateDay = ((DateTime)this.Event.StartDateTime).Day;
                this.StartDateMonth = ((DateTime)this.Event.StartDateTime).Month;
                this.StartDateYear = ((DateTime)this.Event.StartDateTime).Year;
                this.StartDateTimeTime = ((DateTime)this.Event.StartDateTime).ToString("HH:mm");


                this.EndDateDay = ((DateTime)this.Event.EndDateTime).Day;
                this.EndDateMonth = ((DateTime)this.Event.EndDateTime).Month;
                this.EndDateYear = ((DateTime)this.Event.EndDateTime).Year;
                this.EndDateTimeTime = ((DateTime)this.Event.EndDateTime).ToString("HH:mm");

            }
            this.StartDateMonths = GetMonthDropDown();
            this.StartDateDays = GetNumberDropDown(1, 31);
            this.StartDateYears = GetNumberDropDown(baseYear, baseYear + 5);

            this.EndDateMonths = GetMonthDropDown();
            this.EndDateDays = GetNumberDropDown(1, 31);
            this.EndDateYears = GetNumberDropDown(baseYear, baseYear + 5);

            this.StartDateTimeTimes = GetTimeDropDown(0, 23);
            this.EndDateTimeTimes = GetTimeDropDown(0, 23);


            List<SelectListItem> ticketcounter = new List<SelectListItem>();
            int xy = 0;
            while (xy <= 10 && xy <= this.TicketClass.RunningQuantity)
            {
                ticketcounter.Add(new SelectListItem { Text = xy.ToString(), Value = xy.ToString() });
                xy++;
            }


            this.TicketQuantityList = new SelectList( ticketcounter, "Value", "Text");

            this.SelectedTicketQuantity = 0;
        }

    }




    public class TicketClassLoggedOutViewModel
    {
        public int ID { get; set; }
     
        public Nullable<int> EventID { get; set; }
     
        public Nullable<int> Price { get; set; }
       
   
        public int TicketCount { get; set; }
        public decimal TotalCost { get; set; }

        public List<TicketSeatTuple> ChosenRowSeats { get; set; }

        public List<TicketClassSeatRange> TicketClassSeatRanges { get; set; }

        public TicketClassLoggedOutViewModel()
        {

        }
        public TicketClassLoggedOutViewModel(int id)
        {
          
            var db = new MiidEntities();


            var TicketClass = db.TicketClasses.Where(x => x.ID == id).First();

            this.EventID = TicketClass.EventID;
            this.ID = TicketClass.ID;
         this.Price = (int)TicketClass.Price;
         

          

       
        }

        public TicketClassLoggedOutViewModel(int id, List<TicketSeatTuple> chosenrowseats)
        {

            var db = new MiidEntities();


            var TicketClass = db.TicketClasses.Where(x => x.ID == id).First();

            this.EventID = TicketClass.EventID;
            this.ID = TicketClass.ID;
            this.Price = (int) TicketClass.Price;

            this.ChosenRowSeats = chosenrowseats;

            this.TicketCount = chosenrowseats.Count();
            this.TotalCost = (int)(this.Price * this.TicketCount);
        }





    }




}