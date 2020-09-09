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
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

namespace MiidWeb.Models
{
    public class PurchaseTicketViewModel
    {

        public class TicketTuple
        {

            public int ID { get; set; }
            public int Qty { get; set; }
        }

        public SelectList AvailableMonths { get; set; }

        public SelectList AvailableDays { get; set; }

        public int Id { get; set; }
        private MiidEntities db = new MiidEntities();
        public Event Event { get; set; }

        public string EventBannerURL { get; set; }

        public string SeatPlanImage { get; set; }

        public List<TicketClassViewModel> TicketClasses { get; set; }

        public List<Ticket> MyPurchasedTickets { get; set; }

        public List<Ticket> MyPendingTickets { get; set; }

        public SelectList TenderTypeList { get; set; }

        public int SelectedTenderTypeID { get; set; }

        public SelectList TicketClassSelectList { get; set; }

        //    public SelectList TicketQuantityList { get; set; }

        //    public int SelectedTicketQuantity { get; set; }


        public decimal? MyMoneyCurrentBalance { get; set; }

        public decimal PendingTicketsTotalCost { get; set; }

        public string BuyerFirstName { get; set; }
        public string BuyerLastName { get; set; }
        public string BuyerEmail { get; set; }

        public decimal AdminFee { get; set; }


        public string PromoCode { get; set; }

        public decimal DiscountAmount { get; set; }

        public int? DiscountedTicketClassID { get; set; }

        public string FriendEmailTicketClassList { get; set; }

        public PurchaseTicketViewModel()
        { }

        public PurchaseTicketViewModel(int EventID, string AvailableDays, bool v1, bool v2)
        {
            Setup(EventID, AvailableDays, v1, v2);
        }

        private void Setup(int eventID, string availableDays, bool v1, bool v2)
        {
            Setup(eventID);
            DateTime ticketClassStartDate = DateTime.Parse(availableDays);

            List<TicketClassViewModel> allowedList = new List<TicketClassViewModel>();
            foreach (var tf in this.TicketClasses.Where(x => x.TicketClass.StartDate.Value.ToString("yyyy-MM-dd") == ticketClassStartDate.ToString("yyyy-MM-dd")))
            {
                allowedList.Add(tf);
            }
            this.TicketClasses = null;
            this.TicketClasses = new List<TicketClassViewModel>();
            if (allowedList != null && allowedList.Count() > 0)
            {
                this.TicketClasses.AddRange(allowedList);
            }
        }

        public PurchaseTicketViewModel(int EventID)
        {
            Setup(EventID);
        }

        private void Setup(int EventID, string SelectedTicketsString = null, string SelectedSeatsString = null)
        {
            
            this.Event = db.Events.Include(x => x.EventCategory).Include(x => x.EventOrganiser).Include(x => x.Status).Where(x => x.ID == EventID).First();


            var EventImages = db.EventImages.Where(x => x.EventID == EventID).ToList();


            if (EventImages.Where(x => x.ImageAltText == "banner").Count() > 0)
            {
                this.EventBannerURL = EventImages.Where(x => x.ImageAltText == "banner").First().ImageURL;
            }

            if (EventImages != null && EventImages.Where(x => x.ImageAltText == "seatingplan").Count() > 0)
            {
                this.SeatPlanImage = EventImages.Where(x => x.ImageAltText == "seatingplan").First().ImageURL;
            }


            int active = MiidWeb.Helpers.StatusHelper.StatusID("TicketClass", "Active");

            var DbTicketClasses = db.TicketClasses.Where(x => x.EventID == EventID && x.StatusID == active && x.RunningQuantity > 0).ToList();

            this.TicketClasses = new List<TicketClassViewModel>();

            foreach (var ticketClass in DbTicketClasses)
            {
                int selectedTicketCount = 0;

                if (SelectedTicketsString != null)
                {
                    string[] selected = SelectedTicketsString.Split(';');
                    List<TicketTuple> chosen = new List<TicketTuple>();
                    foreach (string s in selected)
                    {
                        if (!String.IsNullOrEmpty(s))
                        {
                            chosen.Add(new TicketTuple { ID = int.Parse(s.Split(':')[0]), Qty = int.Parse(s.Split(':')[1]) });
                        }
                    }
                    var find = chosen.Where(x => x.ID == ticketClass.ID);
                    if (find != null && find.Count() > 0)
                    {
                        selectedTicketCount = find.First().Qty;
                    }

                    this.TicketClasses.Add(new TicketClassViewModel(ticketClass.ID, selectedTicketCount));
                }

                if (SelectedSeatsString != null)
                {
                    string[] selected = SelectedSeatsString.Split(';');
                    List<TicketSeatTuple> chosen = new List<TicketSeatTuple>();
                    foreach (string s in selected)
                    {
                        if (!String.IsNullOrEmpty(s))
                        {
                            chosen.Add(new TicketSeatTuple { TicketClassID = int.Parse(s.Split(':')[0]), RowID = int.Parse(s.Split(':')[1]), SeatNumber = int.Parse(s.Split(':')[2]),TicketClassSeatID = int.Parse(s.Split(':')[3]) });
                        }
                    }
                    var find = chosen.Where(x => x.TicketClassID == ticketClass.ID);
                    if (find != null && find.Count() > 0)
                    {
                        selectedTicketCount = find.Count();
                    }

                    //this adds up the ticketclasses but we need to also record the row/seats
                    this.TicketClasses.Add(new TicketClassViewModel(ticketClass.ID, selectedTicketCount, chosen.Where(x => x.TicketClassID == ticketClass.ID).ToList()));

                }
                if (SelectedSeatsString == null && SelectedTicketsString == null)
                {
                    this.TicketClasses.Add(new TicketClassViewModel(ticketClass.ID, selectedTicketCount));
                }

            }
            this.TenderTypeList = new SelectList(
          new List<SelectListItem>
            {
                    new SelectListItem { Text = "EFT", Value = "1"},
                    new SelectListItem { Text = "Credit Card", Value = "2"},
                    new SelectListItem { Text = "Instant EFT", Value = "3"},
            }, "Value", "Text");

            this.SelectedTenderTypeID = 1;
			
			
			 List<SelectListItem> ticketcounter = new List<SelectListItem>();
            ticketcounter.Add(new SelectListItem { Value = "0", Text = "-- Select Area --", Selected=true });

            foreach (var tc1 in this.TicketClasses)
            {
                string descprice = String.Format("{0} - (R {1})", tc1.TicketClass.Description, tc1.TicketClass.Price);
                ticketcounter.Add(new SelectListItem { Value = tc1.TicketClass.ID.ToString(), Text = descprice });
               
             
            }


            List<SelectListItem> availableMonths = new List<SelectListItem>();
            availableMonths.Add(new SelectListItem { Text = "-- Select Month  --", Value = "0"});
            foreach (var xy in MonthsAvailable(this.Event.ID))
            {
               
               availableMonths.Add(new SelectListItem { Text = xy.ToString(), Value = xy.ToString() });
              
            }

            
            this.AvailableMonths = new SelectList(availableMonths, "Value", "Text");



            this.TicketClassSelectList = new SelectList(ticketcounter, "Value", "Text");
			
        }

        public List<string> MonthsAvailable(int EventID)
        {
            List<string> result = new List<string>();
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("MonthsAvailable", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };

            sqlDataAdapter.SelectCommand.Parameters.Add("@EventID", SqlDbType.Int).Value = EventID;
            
            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];

            sqlConnection.Close();
            if (item != null)
            {
                foreach (DataRow row in item.Rows)
                {

                    result.Add( row["MonthAvailable"].ToString());
                    
                }
            }
            return result;
        }

        public class DayTuple
        {
            public string AvailableDay { get; set; }
            public DateTime StartDate { get; set; }
        }

        public SelectList DaysAvailable(int EventID, string SelectedMonth)
        {
            List<DayTuple> result = new List<DayTuple>();
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("DaysAvailable", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };

            sqlDataAdapter.SelectCommand.Parameters.Add("@EventID", SqlDbType.Int).Value = EventID;
            sqlDataAdapter.SelectCommand.Parameters.Add("@SelectedMonth", SqlDbType.VarChar).Value = SelectedMonth;
            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];

            sqlConnection.Close();
            if (item != null)
            {
                foreach (DataRow row in item.Rows)
                {

                    result.Add(new DayTuple { AvailableDay = row["DayAvailable"].ToString(), StartDate = DateTime.Parse(row["StartDate"].ToString()) });

                }
            }

            List<SelectListItem> availableDays = new List<SelectListItem>();

            foreach (var xy in result.OrderBy(x=>(int.Parse(x.AvailableDay))))
            {

                availableDays.Add(new SelectListItem { Text = xy.AvailableDay.ToString(), Value = xy.StartDate.ToString("yyyy-MM-dd") });

            }


            this.AvailableDays = new SelectList(availableDays, "Value", "Text");
            return this.AvailableDays;
        }

        

         public SelectList GetTicketsAvailable(int EventID, string SelectedDate)
        {
            List<DayTuple> result = new List<DayTuple>();
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("GetTicketsAvailable", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };

            sqlDataAdapter.SelectCommand.Parameters.Add("@EventID", SqlDbType.Int).Value = EventID;
            sqlDataAdapter.SelectCommand.Parameters.Add("@SelectedDate", SqlDbType.VarChar).Value = SelectedDate;
            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];

            sqlConnection.Close();
            if (item != null)
            {
                foreach (DataRow row in item.Rows)
                {

                    result.Add(new DayTuple { AvailableDay = row["DayAvailable"].ToString(), StartDate = DateTime.Parse(row["StartDate"].ToString()) });

                }
            }

            List<SelectListItem> availableDays = new List<SelectListItem>();

            foreach (var xy in result)
            {

                availableDays.Add(new SelectListItem { Text = xy.AvailableDay.ToString(), Value = xy.StartDate.ToString("yyyy-MM-dd") });

            }


            this.AvailableDays = new SelectList(availableDays, "Value", "Text");
            return this.AvailableDays;
        }



        public PurchaseTicketViewModel ApplyPromoCode(PurchaseTicketViewModel model, string PromoCode = "")
        {
            PurchaseTicketViewModel modelOut = model;
            bool PromoCodeUsed = false;
            string outPromoCode = PromoCode;

            List<TicketClassViewModel> outPutList = new List<TicketClassViewModel>();

            foreach (var t in model.TicketClasses.Where(x => x.TicketCount > 0 || x.SelectedTicketQuantity>0).ToList())
            {
                TicketClassViewModel output = new TicketClassViewModel();
                output = t;
                PromoCodeViewModel vm = TicketRepository.PromoCodeAvailable(PromoCode, t.TicketClass.ID);
                if (vm.DiscountPercentage > 0 && !PromoCodeUsed)
                {
                    
                    decimal DiscountPercentage = vm.DiscountPercentage;
                    outPromoCode = vm.First8;
                    decimal discountDecimal = (DiscountPercentage / 100.00M);
                    this.DiscountAmount = (decimal)(t.TicketClass.Price * discountDecimal);
                    this.DiscountedTicketClassID = t.TicketClass.ID;
                    //now reduce the cost of just one of the tickets in this class
                    output.TotalCost -= DiscountAmount;

                    PromoCodeUsed = true;
                }
                outPutList.Add(output);
            }
            modelOut.TicketClasses = outPutList;
            modelOut.PromoCode = outPromoCode;
            return modelOut;
        }


        public PurchaseTicketViewModel(int EventID, int? EndUserID = null, string SelectedTicketsString = null, string SelectedSeatsString =null)
        {




            Setup(EventID, SelectedTicketsString, SelectedSeatsString);

            int EndUserID1 = EndUserID ?? 0;

            if (db.MyMoneys.Where(x => x.EndUserID == EndUserID1) != null && db.MyMoneys.Where(x => x.EndUserID == EndUserID1).Count() > 0)
            {
                this.MyMoneyCurrentBalance = db.MyMoneys.Where(x => x.EndUserID == (int)EndUserID).OrderByDescending(x => x.ID).Take(1).First().RunningBalance;
            }
            else
            {
                this.MyMoneyCurrentBalance = 0;
            }


            if (EndUserID != null) //then get my tickets
            {
                int Purchased = Helpers.StatusHelper.StatusID("Ticket", "Purchased");
                int Pending = Helpers.StatusHelper.StatusID("Ticket", "Pending");
                   MyPurchasedTickets = db.Tickets.Include(x=>x.TicketClassSeats).Where(x => x.EndUserID == EndUserID && x.StatusID == Purchased).ToList();
                MyPendingTickets = db.Tickets.Include(x => x.TicketClassSeats).Include(x => x.TicketClass).Where(x => x.EndUserID == EndUserID && x.StatusID == Pending).ToList();
            bool PromoCodeUsed = false;


                foreach (var t in MyPendingTickets.ToList())
                {
                    PromoCodeViewModel vm = TicketRepository.PromoCodeAvailable(this.PromoCode, t.TicketClass.ID);
                  

                    if (vm.DiscountPercentage > 0 && !PromoCodeUsed)
                    {
                        decimal DiscountPercentage = vm.DiscountPercentage;
                        decimal DiscountedPrice = (decimal)t.TicketClass.Price * (DiscountPercentage / 100);
                        this.DiscountAmount = (decimal)t.TicketClass.Price - DiscountedPrice;
                        PendingTicketsTotalCost += DiscountedPrice;
                        PromoCodeUsed = true;
                        string promoCodeOut = vm.First8;
                        this.PromoCode = promoCodeOut;
                    }
                    else
                    {
                        PendingTicketsTotalCost += (decimal)t.TicketClass.Price;
                    }
                }
            }



        }

    }



    public class PurchaseTicketLoggedOutViewModel
    {
        public int Id { get; set; }
        private MiidEntities db = new MiidEntities();
        public Event Event { get; set; }

        public string EventBannerURL { get; set; }
        public string PromoCode { get; set; }
        public List<TicketClassLoggedOutViewModel> TicketClasses { get; set; }

        public string FriendEmailTicketClassList { get; set; }

        public PurchaseTicketLoggedOutViewModel()
        { }

        public PurchaseTicketLoggedOutViewModel(int EventID, string _PromoCode, string SelectedTicketSeats)
        {
            Setup(EventID);
            this.PromoCode = _PromoCode;

            if (!String.IsNullOrEmpty(SelectedTicketSeats))
            {
                Setup(EventID, SelectedTicketSeats);
            }
        }

        private void Setup(int EventID)
        {
            this.Event = db.Events.Include(x => x.EventCategory).Include(x => x.EventOrganiser).Include(x => x.Status).Where(x => x.ID == EventID).First();


            var EventImages = db.EventImages.Where(x => x.EventID == EventID).ToList();


            if (EventImages.Where(x => x.ImageAltText == "banner").Count() > 0)
            {
                this.EventBannerURL = EventImages.Where(x => x.ImageAltText == "banner").First().ImageURL;
            }

            int active = MiidWeb.Helpers.StatusHelper.StatusID("TicketClass", "Active");

            var DbTicketClasses = db.TicketClasses.Where(x => x.EventID == EventID && x.StatusID == active && x.RunningQuantity > 0).ToList();

            this.TicketClasses = new List<TicketClassLoggedOutViewModel>();

            foreach (var ticketClass in DbTicketClasses)
            {
                this.TicketClasses.Add(new TicketClassLoggedOutViewModel(ticketClass.ID));

            }

        }


        private void Setup(int EventID, string SelectedSeatsString = null)
        {
            this.Event = db.Events.Include(x => x.EventCategory).Include(x => x.EventOrganiser).Include(x => x.Status).Where(x => x.ID == EventID).First();


            var EventImages = db.EventImages.Where(x => x.EventID == EventID).ToList();


            if (EventImages.Where(x => x.ImageAltText == "banner").Count() > 0)
            {
                this.EventBannerURL = EventImages.Where(x => x.ImageAltText == "banner").First().ImageURL;
            }

            int active = MiidWeb.Helpers.StatusHelper.StatusID("TicketClass", "Active");

            var DbTicketClasses = db.TicketClasses.Where(x => x.EventID == EventID && x.StatusID == active && x.RunningQuantity > 0).ToList();

            this.TicketClasses = new List<TicketClassLoggedOutViewModel>();

            foreach (var ticketClass in DbTicketClasses)
            {
                int selectedTicketCount = 0;
                

                if (SelectedSeatsString != null)
                {
                    string[] selected = SelectedSeatsString.Split(';');
                    List<TicketSeatTuple> chosen = new List<TicketSeatTuple>();
                    foreach (string s in selected)
                    {
                        if (!String.IsNullOrEmpty(s))
                        {
                            chosen.Add(new TicketSeatTuple { TicketClassID = int.Parse(s.Split(':')[0]), RowID = int.Parse(s.Split(':')[1]), SeatNumber = int.Parse(s.Split(':')[2]), TicketClassSeatID = int.Parse(s.Split(':')[3]) });
                        }
                    }
                    var find = chosen.Where(x => x.TicketClassID == ticketClass.ID);
                    if (find != null && find.Count() > 0)
                    {
                        selectedTicketCount = find.Count();
                    }

                    //this adds up the ticketclasses but we need to also record the row/seats
                    this.TicketClasses.Add(new TicketClassLoggedOutViewModel(ticketClass.ID, chosen.Where(x => x.TicketClassID == ticketClass.ID).ToList()));

                }
                if (SelectedSeatsString == null)
                {
                    this.TicketClasses.Add(new TicketClassLoggedOutViewModel(ticketClass.ID));
                }

            }
        

        }


    }


}