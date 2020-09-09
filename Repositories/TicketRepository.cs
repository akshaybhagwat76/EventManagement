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
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using System.Globalization;
using MiidWeb.Helpers;

namespace MiidWeb.Repositories
{
    public static class TicketRepository
    {

        public static void AssignTicketConfig_ReserveFriendTickets (string buyerEmail, string friendEmailTicketClassList, string uniquePaymentReference)
        {

            int endUserID = EndUserRepository.GetByUserName(buyerEmail).ID;

            List<RefundTicketViewModelLite> list = new List<RefundTicketViewModelLite>();


            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("AssignTicketConfig_ReserveFriendTickets", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };

            sqlDataAdapter.SelectCommand.Parameters.Add("@endUserID", SqlDbType.Int).Value = endUserID;
            sqlDataAdapter.SelectCommand.Parameters.Add("@friendEmailTicketClassList", SqlDbType.VarChar).Value = friendEmailTicketClassList;
            sqlDataAdapter.SelectCommand.Parameters.Add("@uniquePaymentReference", SqlDbType.VarChar).Value = uniquePaymentReference;


            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];

            sqlConnection.Close();


            return;

        }



        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }


        public static Image GenerateQrCode(string input)
        {

            Zen.Barcode.CodeQrBarcodeDraw qrcode = Zen.Barcode.BarcodeDrawFactory.CodeQr;

            Image qrimage = qrcode.Draw(input, 50);

            return qrimage;
        }

        public static Image GenerateBarCode(string input)
        {
            Zen.Barcode.Code128BarcodeDraw Barcode = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;

            return Barcode.Draw(input, 500);
        }

        public static string GetHost(HttpApplicationState appState)
        {

            return (string)appState["HostURL"];
        }

        public static string GetSubdomainID()
        {
            string host = "";

            if (HttpContext.Current != null)
            {
                host = HttpContext.Current.Request.Url.Host;

            }
            else
            {
                host = GetHost(HttpContext.Current.Application);
            }

            // string company = host.Split('.')[0].ToString();
            string company = host;

            //   if (company.Contains("www")) {  company = "training.miid.co.za"; }
           if (company.Contains("localhost")) { company = "miid.co.za"; }
            GlobalVariables.Company = company;

            int subDomainID = LookupHelper.GetSubdomainID(company);

            return subDomainID.ToString();

        }
        public static string GetLayoutFile(string FileName)
        {

            string host = HttpContext.Current.Request.Url.Host;

            //  string company = host.Split('.')[0].ToString();
            string company = host;

            //  if (company.Contains("www")) {  company = "training.miid.co.za"; }
           if (company.Contains("localhost")) { company = "miid.co.za"; }


            if (company == "miid")
            {
                switch (FileName)
                {

                    case "LayoutEventOrganiser": return "~/Views/Shared/_LayoutEventOrganiser.cshtml"; break;
                    case "LayoutLanding": return "~/Views/Shared/_LayoutLandingPages.cshtml"; break;
                    case "LayoutLogin": return "~/Views/Shared/_LayoutLogin.cshtml"; break;
                    case "LayoutAdmin": return "~/Views/Shared/_LayoutAdmin.cshtml"; break;
                    case "Layout": return "~/Views/Shared/_Layout.cshtml"; break;
                    default: return "~/Views/Shared/_Layout.cshtml"; break;

                }


            }
            else
            {
                switch (FileName)
                {
                    case "LayoutLogin":
                        return String.Format("~/Views/Shared/_LayoutLogin{0}.cshtml", company); break;
                    case "LayoutAdmin":
                        //case "LayoutAdmin":
                        return "~/Views/Shared/_LayoutAdmin.cshtml"; break;
                    //  return String.Format("~/Views/Shared/_LayoutAdmin{0}.cshtml", company); break;
                    case "Layout":
                        return String.Format("~/Views/Shared/_Layout{0}.cshtml", company); break;

                    case "LayoutEventOrganiser":
                        return "~/Views/Shared/_LayoutEventOrganiser.cshtml"; break;
                    default: return String.Format("~/Views/Shared/_Layout{0}.cshtml", company); break;

                    case "LayoutLanding":
                        return "~/Views/Shared/_LayoutLandingPages{0}.cshtml"; break;

                }

            }

        }

        public static bool UsePayGenius()
        {
            string host = HttpContext.Current.Request.Url.Host;

            //   string company = host.Split('.')[0].ToString();
            string company = host;

            //  if (company.Contains("www")) {  company = "training.miid.co.za"; }
           if (company.Contains("localhost")) { company = "miid.co.za"; }

            switch (company)
            {
                case "goodtree": return true; break;
                case "Bokbeursie": return true; break;
                case "bokbeursie": return true; break;
                default: return false;
            }

        }

        public static void SendRefundToCustomer(int EndUserID)
        {
            //Send mail to admin

            StringBuilder adminbody = new StringBuilder();
            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(EndUserID);
                //Send mail to user

                adminbody.AppendLine(String.Format("Hi, {0}", "MiiD Finance Admin"));
                adminbody.AppendLine(String.Format("Mii-Funds refund request has been sent for end user: {0} {1} - {2}", user.Firstname, user.Surname, user.Email));

               
                adminbody.AppendLine(String.Format("Please check refund list."));


                EmailHelper.SendMail("logs@miid.co.za", String.Format("Refund request"), adminbody.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
                //EmailHelper.SendMail("jonathanmwallis1808@gmail.com", String.Format("Mii-Funds withdrawal request"), adminbody.ToString());
                //EmailHelper.SendMail("dsouchon@gmail.com", String.Format("Mii-Funds withdrawal request"), adminbody.ToString());

            }



            StringBuilder userbody = new StringBuilder();

            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(EndUserID);
                //Send mail to user

                userbody.AppendLine(String.Format("Hi, {0},<br>", user.Firstname));
                userbody.AppendLine(String.Format("Your ticket refund requeset has been received.<br>"));
                userbody.AppendLine(String.Format("Should your refund request meet all the requirement your refund will be processed.<br>"));
                userbody.AppendLine(String.Format("Please note that the Event Oranisers has final approval on refunds.<br>"));
                userbody.AppendLine(String.Format("The refund process can take up to ten days.<br>"));

                userbody.AppendLine(String.Format("Thank you.<br>"));
                userbody.AppendLine(String.Format("MiiD Team"));

                EmailHelper.SendMail(user.Email, "Ticket refund request received", userbody.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
            }
        }

        public static List<RefundTicketViewModelLite> GetRequestedTicketRefunds(int EventID)
        {
            List<RefundTicketViewModelLite> list = new List<RefundTicketViewModelLite>();

            
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("GetRequestedTicketRefunds", sqlConnection)
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

                    list.Add(new RefundTicketViewModelLite(int.Parse(row["TicketID"].ToString()), int.Parse(row["EndUserID"].ToString())));

                }
            }
            


            return list;
        }

        public static void SetRequestedTicketRefunds(List<int> ticketsToRefund)
        {
            int refundrequestedsatusid = StatusHelper.StatusID("Ticket", "Refund Requested");
            using (var db = new MiidEntities())
            {
                foreach (var t in ticketsToRefund)
                {
                    var ticket = db.Tickets.Find(t);
                    ticket.StatusID = refundrequestedsatusid;
                    ticket.DateRefundRequested = DateTime.Now;
                    db.Entry(ticket).State = EntityState.Modified;
                    db.SaveChanges();

                }


            }
        }

        public static List<TicketViewModel> GetRefundableTickets(string UserName)
        {
            List<TicketViewModel> result = new List<TicketViewModel>();
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("GetRefundableTickets", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };

            sqlDataAdapter.SelectCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = UserName;
          
            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];

            sqlConnection.Close();


            if (item != null)
            {
                foreach (DataRow row in item.Rows)
                {

                    result.Add(new TicketViewModel(int.Parse(row["TicketID"].ToString())));
                    
                }
            }
            return result;
        }

        public static void TicketRefunded(int TicketID)
        {
            using (var db = new MiidEntities())
            {
                var ticket = db.Tickets.Find(TicketID);
                ticket.StatusID = StatusHelper.StatusID("Ticket", "Refund Paid Out");
                ticket.DateRefundPaidOut = DateTime.Now;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
            }

                
        }

        public static string GetSubdomainView(string FileName)
        {

            string host = HttpContext.Current.Request.Url.Host;

            //  string company = host.Split('.')[0].ToString();
            string company = host;

            //   if (company.Contains("www")) {  company = "training.miid.co.za"; }
           if (company.Contains("localhost")) { company = "miid.co.za"; }


            if (company == "miid")
            {
                return FileName;

            }
            else
            {

                return String.Format("{0}{1}", FileName, company);


            }

        }




        public static void SaveTicketQRCode(int TicketID)
        {
            if (TicketID > 0)
            {
                var db = new MiidEntities();
                var ticket = db.Tickets.Find(TicketID);

                string server = HttpContext.Current.Server.MapPath("~/MiiDTicketPDFs/");
                string seqno = ticket.TicketNumber.ToString();

                string path = String.Format("{0}QrCode_{1}.jpg", server, seqno);
                string storeUploadDirectory = Path.Combine(server, seqno);


                if (!Directory.Exists(storeUploadDirectory))
                {
                    Directory.CreateDirectory(storeUploadDirectory);
                }
                if (!File.Exists(path))
                {
                    var imageData = GenerateQrCode("TIC" + ticket.TicketNumber);
                    byte[] bytearray = ImageToByte(imageData);
                    File.WriteAllBytes(path, bytearray);
                }

                return;
            }
        }

        public static DataTable GetTicketClassesForEvent(string EventCode, bool IsBoxOffice)
        {
            StringBuilder result = new StringBuilder();
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("GetTicketClassesForEvent", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };

            sqlDataAdapter.SelectCommand.Parameters.Add("@EventCode", SqlDbType.VarChar).Value = EventCode;
            sqlDataAdapter.SelectCommand.Parameters.Add("@IsBoxOffice", SqlDbType.Bit).Value = IsBoxOffice;
            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];

            sqlConnection.Close();
            return item;
        }




        public static PromoCodeViewModel PromoCodeAvailable(string First8, int TicketClassID)
        {
            PromoCodeViewModel result = new PromoCodeViewModel();
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("PromoCodeAvailable", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };

            sqlDataAdapter.SelectCommand.Parameters.Add("@First8", SqlDbType.VarChar).Value = First8;
            sqlDataAdapter.SelectCommand.Parameters.Add("@TicketClassID", SqlDbType.Int).Value = TicketClassID;
            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];

            sqlConnection.Close();


            if (item != null)
            {
                foreach (DataRow row in item.Rows)
                {

                    result.DiscountPercentage = decimal.Parse(row["result"].ToString());
                    result.First8 = row["First8"].ToString();

                }
            }
            return result;
        }

        public static bool HaveMyTicketsExpired(string reference)
        {
            using (var db = new MiidEntities())
            {
                int Expired = Helpers.StatusHelper.StatusID("Ticket", "Expired");

                var expiredTickets = db.Tickets.Where(x => x.StatusID == Expired && x.UniquePaymentID == reference);
                if (expiredTickets != null && expiredTickets.Count() > 0)
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
        }

        public static string PromoCodeUpdate(string First8, int EndUserID, int TicketClassID)
        {
            StringBuilder result = new StringBuilder();
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("PromoCodeUpdate", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };

            sqlDataAdapter.SelectCommand.Parameters.Add("@First8", SqlDbType.VarChar).Value = First8;
            sqlDataAdapter.SelectCommand.Parameters.Add("@TicketClassID", SqlDbType.Int).Value = TicketClassID;
            sqlDataAdapter.SelectCommand.Parameters.Add("@EndUserID", SqlDbType.Int).Value = EndUserID;
            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];

            sqlConnection.Close();
            return item.Rows[0].ToString();
        }




        public static bool HasPurchasedATicketForThisEvent(int UserID, int EventID)
        {
            bool result = false;


            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("HasPurchasedATicketForThisEvent", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };

            sqlDataAdapter.SelectCommand.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;
            sqlDataAdapter.SelectCommand.Parameters.Add("@EventID", SqlDbType.Int).Value = EventID;

            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];
            if (item != null)
            {
                foreach (DataRow row in item.Rows)
                {

                    result = bool.Parse(row["result"].ToString());

                }
            }
            sqlConnection.Close();

            return result;
        }

        public static int ExpireTickets(string reference)
        {
            int result = 0;


            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("ExpireTickets", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };

            sqlDataAdapter.SelectCommand.Parameters.Add("@reference", SqlDbType.VarChar).Value = reference;
           
            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];
            if (item != null)
            {
                foreach (DataRow row in item.Rows)
                {

                    result = int.Parse(row["result"].ToString());

                }
            }
            sqlConnection.Close();

            return result;
        }

        public static Ticket PurchaseTicketWithCashBoxOffice(int TicketClassID, int BoxOfficeUserID, int PosID)
        {



            int ticketcount = 0;

            string ConfirmationMessage = "";

            using (MiidEntities db = new MiidEntities())
            {

                var ticketClass = db.TicketClasses.Find(TicketClassID);

                decimal costOfTotalPurchase = (decimal)ticketClass.Price;
                bool fail = false;

                if (ticketClass.RunningQuantity < 1)
                {
                    ConfirmationMessage = String.Format("No Tickets Left in {1} Ticket Class. Please change quantity requested.\n", ticketClass.Description);
                    fail = true;
                }

                if (!fail)
                {

                    var Ticket = new Ticket();
                    Ticket.EndUserID = BoxOfficeUserID;
                    Ticket.DatetimePurchased = DateTime.Now;
                    Ticket.DatetimeReserved = DateTime.Now;
                    Ticket.TicketClassID = ticketClass.ID;
                    Ticket.Hash = System.Guid.NewGuid().ToString();
                    Ticket.UniquePaymentID = String.Format("Pos ID: {0}", PosID);

                    List<string> ticketsthissession = new List<string>();

                    Ticket.TicketNumber = GenerateTicketNumber(ticketsthissession);

                    Ticket.StatusID = Helpers.StatusHelper.StatusID("Ticket", "Purchased");

                    Ticket.TicketPurchasePrice = ticketClass.Price;

                    db.Tickets.Add(Ticket);

                    db.SaveChanges();
                    db.Entry(Ticket).GetDatabaseValues();

                    var TicketClass = db.TicketClasses.Find(ticketClass.ID);
                    TicketClass.RunningQuantity -= 1;//Decrement Qty Available
                    db.SaveChanges();


                    //Db Alert
                    ExportCsvRepository.ExportCSVAndEmail("AreThereMoreTicketsThanBank", Ticket.UniquePaymentID ?? "", "info@miid.co.za", String.Format("WARNING. duplicate transaction: {0}", Ticket.UniquePaymentID ?? ""), "See attachment", String.Format("AreThereMoreTicketsThanBank_{0}.csv", DateTime.Now.ToString("yyyyMMddHHmmss")));


                    return Ticket;
                }


            }

            return null;
        }


        public static PurchaseTicketResultViewModel PurchaseTicketsWithMiiFunds(PurchaseTicketViewModel tcvm, int EndUserID)
        {

            PurchaseTicketResultViewModel fail = new PurchaseTicketResultViewModel();
            fail.ErrorList = new List<ConfirmationError>();
            fail.HasErrors = true;

            int ticketcount = 0;

            string ConfirmationMessage = "";

            using (MiidEntities db = new MiidEntities())
            {

                MyMoney miiMoney = new MyMoney();

                var miiMoneyList = db.MyMoneys.Where(x => x.EndUserID == EndUserID).OrderByDescending(x => x.TransactionDate);

                if (miiMoneyList.Count() > 0)
                {
                    miiMoney = miiMoneyList.First();
                }
                decimal availableFunds = (decimal)MyMoneyRepository.MyAvailableFunds(EndUserID);// (miiMoney.RunningBalance ?? 0.0M);

                decimal costOfTotalPurchase = tcvm.TicketClasses.Sum(x => x.TotalCost);

                fail.TotalTicketCost = (int)costOfTotalPurchase;

                if (availableFunds < costOfTotalPurchase)
                {
                    decimal topupRequiredAmount = costOfTotalPurchase - availableFunds;

                    ConfirmationMessage = String.Format("Insufficient Funds. Please top up. Your current balance is: R{0}, total ticket cost is: R{1}, minimum top up required is: R{2}", availableFunds.ToString("0.00"), costOfTotalPurchase.ToString("0.00"), topupRequiredAmount.ToString("0.00"));
                    fail.ErrorList.Add(new ConfirmationError() { Code = "Funds", Description = ConfirmationMessage, Action = "IndexByUserID", Controller = "MyMoneys" });
                }


                NFCTag band = new NFCTag();

                var ActiveBandStatusIDs = db.Status.Where(x => x.Context == "NFCTag" && x.Description == "Active");
                int ActiveBandStatusID = 0;
                if (ActiveBandStatusIDs.Count() > 0) ActiveBandStatusID = ActiveBandStatusIDs.First().ID;

                var bandList = db.NFCTags.Where(x => x.StatusID == ActiveBandStatusID && x.EndUserID == EndUserID).ToList();//Active only

                if (bandList.Count() == 0)
                {
                    //no active bands
                    ConfirmationMessage = "No active band. Please activate.";
                    //fail.ErrorList.Add(new ConfirmationError() { Code = "Band", Description = ConfirmationMessage, Action = "IndexByUserID", Controller = "NFCTags" });
                }

                foreach (var tc in tcvm.TicketClasses)
                {
                    var tc1 = db.TicketClasses.Find(tc.TicketClass.ID);
                    if (tc1.RunningQuantity < tc.TicketCount)
                    {
                        ConfirmationMessage = String.Format("Only {0} Tickets Left in {1} Ticket Class. Please change quantity requested.\n", tc1.Quantity, tc.TicketClass.Description);
                        fail.ErrorList.Add(new ConfirmationError() { Code = "Sold Out", Description = ConfirmationMessage, Action = "PurchaseTickets", Controller = "Events", ID = (int)tc.TicketClass.EventID });
                    }
                }
                //No errors - you can purchase
                if (fail.ErrorList.Count() == 0)
                {
                    fail.HasErrors = false;

                    //Do purchase 
                    var RunningBalance = MyMoneyRepository.Purchase(EndUserID, costOfTotalPurchase, tcvm.TicketClasses.First().TicketClass.Event.EventName);
                    List<string> TicketsThisSession = new List<string>();

                    fail.purchasedTickets = new List<TicketViewModel>();

                    //Issue Tickets
                    foreach (var tc in tcvm.TicketClasses)
                    {
                        if (tc.ChosenRowSeats != null)
                        {

                            foreach (var chosen in tc.ChosenRowSeats)
                            {
                                var Ticket = new Ticket();
                                Ticket.EndUserID = EndUserID;
                                Ticket.DatetimePurchased = DateTime.Now;
                                Ticket.DatetimeReserved = DateTime.Now;
                                Ticket.TicketClassID = tc.TicketClass.ID;

                                Ticket.TicketPurchasePrice = tc.TicketClass.Price;

                                Ticket.Hash = System.Guid.NewGuid().ToString();
                                Ticket.TicketNumber = GenerateTicketNumber(TicketsThisSession);
                                TicketsThisSession.Add(Ticket.TicketNumber);
                                Ticket.StatusID = Helpers.StatusHelper.StatusID("Ticket", "Purchased");

                                bool discountApplied = false;
                                if (tcvm.DiscountedTicketClassID == tc.TicketClass.ID)
                                {
                                    if (!discountApplied)
                                    {
                                        Ticket.TicketPurchasePrice -= tcvm.DiscountAmount;
                                        discountApplied = true;
                                    }
                                }


                                db.Tickets.Add(Ticket);
                                ticketcount++;
                                db.SaveChanges();
                                db.Entry(Ticket).GetDatabaseValues();

                                var TicketClass = db.TicketClasses.Find(tc.TicketClass.ID);
                                TicketClass.RunningQuantity -= 1;//Decrement Qty Available

                                fail.purchasedTickets.Add(new TicketViewModel(Ticket.ID));
                                //Assign Ticket 
                                var TicketSeat = new TicketClassSeat();
                                TicketSeat.TicketID = Ticket.ID;
                                TicketSeat.SeatNumber = chosen.SeatNumber;
                                TicketSeat.TicketClassSeatRangeID = chosen.RowID;
                                db.TicketClassSeats.Add(TicketSeat);//DANIEL 5050 - MIIFUNDS TICKET SEAT PURCHASE
                                db.SaveChanges();

                                //Db Alert
                                ExportCsvRepository.ExportCSVAndEmail("AreThereMoreTicketsThanBank", Ticket.UniquePaymentID ?? "", "info@miid.co.za", String.Format("WARNING. duplicate transaction: {0}", Ticket.UniquePaymentID ?? ""), "See attachment", String.Format("AreThereMoreTicketsThanBank_{0}.csv", DateTime.Now.ToString("yyyyMMddHHmmss")));


                            }
                        }
                        else
                        {  //its a free seating event
                            while (ticketcount < tc.TicketCount)
                            {
                                var Ticket = new Ticket();
                                Ticket.EndUserID = EndUserID;
                                Ticket.DatetimePurchased = DateTime.Now;
                                Ticket.DatetimeReserved = DateTime.Now;
                                Ticket.TicketClassID = tc.TicketClass.ID;
                                Ticket.Hash = System.Guid.NewGuid().ToString();
                                Ticket.TicketNumber = GenerateTicketNumber(TicketsThisSession);
                                TicketsThisSession.Add(Ticket.TicketNumber);
                                Ticket.StatusID = Helpers.StatusHelper.StatusID("Ticket", "Purchased");

                                Ticket.TicketPurchasePrice = tc.TicketClass.Price;
                                bool discountApplied = false;
                                if (tcvm.DiscountedTicketClassID == tc.TicketClass.ID)
                                {
                                    if (!discountApplied)
                                    {
                                        Ticket.TicketPurchasePrice -= tcvm.DiscountAmount;
                                        discountApplied = true;
                                    }
                                }
                                db.Tickets.Add(Ticket);
                                ticketcount++;
                                db.SaveChanges();
                                db.Entry(Ticket).GetDatabaseValues();

                                var TicketClass = db.TicketClasses.Find(tc.TicketClass.ID);
                                TicketClass.RunningQuantity -= 1;//Decrement Qty Available

                                fail.purchasedTickets.Add(new TicketViewModel(Ticket.ID));

                                //Db Alert
                                ExportCsvRepository.ExportCSVAndEmail("AreThereMoreTicketsThanBank", Ticket.UniquePaymentID ?? "", "info@miid.co.za", String.Format("WARNING. duplicate transaction: {0}", Ticket.UniquePaymentID ?? ""), "See attachment", String.Format("AreThereMoreTicketsThanBank_{0}.csv", DateTime.Now.ToString("yyyyMMddHHmmss")));


                            }

                            ticketcount = 0;//reset
                        }
                        db.SaveChanges();
                    }
                }
            }
            return fail;

        }

        public static bool DoesEventHasTicketSeatingPlan(int eventID)
        {
            bool result = false;


            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("DoesEventHasTicketSeatingPlan", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };

            sqlDataAdapter.SelectCommand.Parameters.Add("@EventID", SqlDbType.VarChar).Value = eventID;
            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];
            if (item != null)
            {
                foreach (DataRow row in item.Rows)
                {

                    result = bool.Parse(row["result"].ToString());

                }
            }
            sqlConnection.Close();

            return result;
        }

        public static int DeleteExpiredReservedTickets(int EventID, int EndUserID, int ExpiryMinutes)
        {
            var db = new MiidEntities();
            var myTickets = db.Tickets.Include(x => x.TicketClass).Where(x => x.EndUserID == EndUserID).ToList();
            myTickets = myTickets.Where(x => x.TicketClass.EventID == EventID).ToList();

            int reservedStatusID = Helpers.StatusHelper.StatusID("Ticket", "Reserved");
            int expirecount = 0;

            if (myTickets != null && myTickets.Count() > 0)
            {
                foreach (var t in myTickets)
                {
                    var Ticket = db.Tickets.Find(t.ID);

                    TimeSpan ts = DateTime.Now - (DateTime)Ticket.DatetimeReserved;

                    if (Ticket.StatusID == reservedStatusID && ts.TotalMinutes >= ExpiryMinutes)
                    {


                        var TicketClass = db.TicketClasses.Find(t.TicketClassID);
                        TicketClass.RunningQuantity += 1;//Increase Qty Available 
                        db.SaveChanges();

                        db.Entry(Ticket).GetDatabaseValues();

                        Ticket.StatusID = Helpers.StatusHelper.StatusID("Ticket", "Expired");
                        db.Entry(Ticket).State = EntityState.Modified;

                        db.SaveChanges();
                        DeleteSeatsForExpiredTicket(db, Ticket);

                        expirecount++;
                    }
                }
            }
            return expirecount;

        }

        private static void DeleteSeatsForExpiredTicket(MiidEntities db, Ticket Ticket)
        {
            //Check if this expired Ticket has reserved seats associated
            var tss = db.TicketClassSeats.Where(x => x.TicketID == Ticket.ID);
            if (tss != null && tss.Count() > 0)
            {
                foreach (var seat in tss)
                {
                    db.TicketClassSeats.Remove(seat);

                }
                db.SaveChanges();
            }
        }

        public static bool DoIHaveTicketsInReservedState(int EventID, int EndUserID, int ExpiryMinutes)
        {
            DeleteExpiredReservedTicketsAll();

            var db = new MiidEntities();

            var myTickets = db.Tickets.Include(x => x.TicketClass).Where(x => x.EndUserID == EndUserID).ToList();
            myTickets = myTickets.Where(x => x.TicketClass.EventID == EventID).ToList();

            int reservedStatusID = Helpers.StatusHelper.StatusID("Ticket", "Reserved");
            int expirecount = 0;

            if (myTickets != null && myTickets.Count() > 0)
            {
                foreach (var t in myTickets)
                {
                    var Ticket = db.Tickets.Find(t.ID);

                    TimeSpan ts = DateTime.Now - (DateTime)Ticket.DatetimeReserved;

                    if (Ticket.StatusID == reservedStatusID && ts.TotalMinutes <= ExpiryMinutes)
                    {

                        expirecount++;
                    }
                }
            }


            return expirecount > 0;


        }

        public static PurchaseTicketResultViewModel ReserveTicketsForDirectPurchase(PurchaseTicketViewModel tcvm, int EndUserID, string UniqueRefNo, string TenderType = "EFT")
        {
            PurchaseTicketResultViewModel ptr = new PurchaseTicketResultViewModel();
            ptr.ErrorList = new List<ConfirmationError>();
            ptr.HasErrors = true;
            var user = EndUserRepository.GetByUserID(EndUserID);
            ptr.UniquePaymentID = UniqueRefNo;// MiidWeb.Helpers.PaymentHelper.UniqueRefNo(user.Firstname, user.Surname) + "_TT";

            int ticketcount = 0;

            string ConfirmationMessage = "";

            using (SomethingSomethingEntities db = new SomethingSomethingEntities())
            {
                var buyer = db.EndUsers.Find(EndUserID);
                foreach (var tc in tcvm.TicketClasses)
                {
                    var tc1 = db.TicketClasses.Find(tc.TicketClass.ID);
                    if (tc1.RunningQuantity < tc.TicketCount)
                    {
                        ConfirmationMessage = String.Format("Only {0} Tickets Left in {1} Ticket Class. Please change quantity requested.\n", tc1.Quantity, tc.TicketClass.Description);
                        ptr.EventID = tc1.EventID ?? 0;
                        ptr.ErrorList.Add(new ConfirmationError() { Code = "Sold Out", Description = ConfirmationMessage, Action = "PurchaseTickets", Controller = "Events", ID = (int)tcvm.Event.ID });
                    }
                }
                //Does a BankTransaction already exist with this unique payment ID
                //This indicates that the tickets have already been purchased and session needs to be expired
                bool errorBankTransactionExists = BankTransactionRepository.DoesUniquePaymentIDExist(ptr.UniquePaymentID);

                if (errorBankTransactionExists)
                {

                    ptr.EventID = (int)tcvm.Event.ID;
                    if (TenderType == "EFT")//Manual EFT
                    {
                        ptr.ErrorList.Add(new ConfirmationError() { Code = "Session Expired", Description = "Tickets already reserved but you may have clicked back button too early. Please start again if you do not receive reservation confirmation.", Action = "PurchaseTickets", Controller = "Events", ID = (int)tcvm.Event.ID });
                    }
                    else
                    {
                        ptr.ErrorList.Add(new ConfirmationError() { Code = "Session Expired", Description = "Tickets already reserved but you may have clicked back button too early. Please start again if you do not receive purchase confirmation.", Action = "PurchaseTickets", Controller = "Events", ID = (int)tcvm.Event.ID });
                    }
                }

                //No errors - you can purchase
                if (ptr.ErrorList.Count() == 0)
                {
                    ptr.HasErrors = false;
                    ptr.reservedTickets = new List<TicketViewModel>();


                    List<string> TicketsThisSession = new List<string>();

                    //Reserve Tickets
                    foreach (var tc in tcvm.TicketClasses)
                    {

                        if (tc.ChosenRowSeats != null)
                        {

                            foreach (var chosen in tc.ChosenRowSeats)
                            {
                                var Ticket = new Ticket();
                                Ticket.EndUserID = EndUserID;
                                Ticket.DatetimePurchased = null;
                                Ticket.DatetimeReserved = DateTime.Now;
                                Ticket.TicketClassID = tc.TicketClass.ID;
                                Ticket.Hash = System.Guid.NewGuid().ToString();
                                Ticket.TicketNumber = GenerateTicketNumber(TicketsThisSession);
                                TicketsThisSession.Add(Ticket.TicketNumber);
                                Ticket.StatusID = Helpers.StatusHelper.StatusID("Ticket", "Reserved");
                                Ticket.UniquePaymentID = ptr.UniquePaymentID;
                                Ticket.TicketPurchasePrice = tc.TicketClass.Price;

                                bool discountApplied = false;
                                if (tcvm.DiscountedTicketClassID == tc.TicketClass.ID)
                                {
                                    if (!discountApplied)
                                    {
                                        Ticket.TicketPurchasePrice -= tcvm.DiscountAmount;
                                        discountApplied = true;
                                    }
                                }

                                db.Tickets.Add(Ticket);
                                ticketcount++;
                                db.SaveChanges();
                                db.Entry(Ticket).GetDatabaseValues();

                                var TicketClass = db.TicketClasses.Find(tc.TicketClass.ID);
                                TicketClass.RunningQuantity -= 1;//Decrement Qty Available

                                //Assign Ticket - This was already done on reservation
                                //var TicketSeat = new TicketClassSeat();
                                //TicketSeat.TicketID = Ticket.ID;
                                //TicketSeat.SeatNumber = chosen.SeatNumber;
                                //TicketSeat.TicketClassSeatRangeID = chosen.RowID;

                                //DANIEL now find the existing reserved seat and add the TicketID
                                var reservedSeat = db.TicketClassSeats.Find(chosen.TicketClassSeatID);
                                if (reservedSeat != null)
                                {
                                    reservedSeat.TicketID = Ticket.ID;
                                    db.SaveChanges();
                                }
                                //Note when a ticket is unreserved this row is deleted from DB

                                //Db Alert
                                ExportCsvRepository.ExportCSVAndEmail("AreThereMoreTicketsThanBank", Ticket.UniquePaymentID ?? "", "info@miid.co.za", String.Format("WARNING. duplicate transaction: {0}", Ticket.UniquePaymentID ?? ""), "See attachment", String.Format("AreThereMoreTicketsThanBank_{0}.csv", DateTime.Now.ToString("yyyyMMddHHmmss")));

                            }
                        }
                        else
                        {


                            ticketcount = 0;

                            while (ticketcount < tc.TicketCount)
                            {
                                var Ticket = new Ticket();
                                Ticket.EndUserID = EndUserID;
                                Ticket.DatetimePurchased = null;
                                Ticket.DatetimeReserved = DateTime.Now;
                                Ticket.TicketClassID = tc.TicketClass.ID;
                                Ticket.Hash = System.Guid.NewGuid().ToString();
                                Ticket.TicketNumber = GenerateTicketNumber(TicketsThisSession);
                                TicketsThisSession.Add(Ticket.TicketNumber);
                                Ticket.StatusID = Helpers.StatusHelper.StatusID("Ticket", "Reserved");
                                Ticket.UniquePaymentID = ptr.UniquePaymentID;
                                Ticket.TicketPurchasePrice = tc.TicketClass.Price;

                                bool discountApplied = false;
                                if (tcvm.DiscountedTicketClassID == tc.TicketClass.ID)
                                {
                                    if (!discountApplied)
                                    {
                                        Ticket.TicketPurchasePrice -= tcvm.DiscountAmount;
                                        discountApplied = true;
                                    }
                                }

                                db.Tickets.Add(Ticket);
                                ticketcount++;

                                var TicketClass = db.TicketClasses.Find(tc.TicketClass.ID);
                                TicketClass.RunningQuantity -= 1;//Decrement Qty Available//DANIEL DANIEL check logic here to manage reserved tickets
                                db.SaveChanges();
                                db.Entry(Ticket).GetDatabaseValues();
                                ptr.reservedTickets.Add(new TicketViewModel(Ticket.ID));

                                //Db Alert
                                ExportCsvRepository.ExportCSVAndEmail("AreThereMoreTicketsThanBank", Ticket.UniquePaymentID ?? "", "info@miid.co.za", String.Format("WARNING. duplicate transaction: {0}", Ticket.UniquePaymentID ?? ""), "See attachment", String.Format("AreThereMoreTicketsThanBank_{0}.csv", DateTime.Now.ToString("yyyyMMddHHmmss")));


                            }
                            //Now if there are friends to transfer tickets to
                            if (!String.IsNullOrEmpty(tcvm.FriendEmailTicketClassList))
                            {
                                TicketRepository.AssignTicketConfig_ReserveFriendTickets(buyer.Email, tcvm.FriendEmailTicketClassList, UniqueRefNo);

                            }


                        }
                    }

                }
            }

            return ptr;
        }


        public static List<TicketViewModel> ConfirmTickets(string uniquePaymentID)
        {

            List<TicketViewModel> tvmList = new List<TicketViewModel>();
            int TicketReserved = MiidWeb.Helpers.StatusHelper.StatusID("Ticket", "Reserved");
            int TicketPurchased = MiidWeb.Helpers.StatusHelper.StatusID("Ticket", "Purchased");

            using (var db = new MiidEntities())
            {
                var pendingTickets = db.Tickets.Where(f => f.UniquePaymentID == uniquePaymentID).ToList();
                pendingTickets.ForEach(a => a.DatetimePurchased = DateTime.Now);
                pendingTickets.ForEach(a => a.StatusID = TicketPurchased);
                db.SaveChanges();

                foreach (var ticket in pendingTickets.ToList())
                {

                    var tvm = new TicketViewModel(ticket.ID);
                    //
                    StampPromoCode(ticket.EndUserID, ticket.TicketClassID, ticket.ID);

                    tvmList.Add(tvm);



                }

            }



            return tvmList;
        }

        private static void StampPromoCode(int? endUserID, int? ticketClassID, int iD)
        {
            using (MiidEntities db = new MiidEntities())
            {
                var mypromos = db.PromoCodes.Where(x => x.DateUsed != null && x.TicketClassID == ticketClassID && x.EndUserID == endUserID && x.TicketID == null);
                if (mypromos != null && mypromos.Count() > 0)
                {
                    var promocode = mypromos.First();
                    promocode.TicketID = iD;
                    db.Entry(promocode).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        internal static List<Ticket> GetMyReservedTickets(string uniquePaymentID)
        {
            using (MiidEntities db = new MiidEntities())
            {
                var pendingTickets = db.Tickets.Where(x => x.UniquePaymentID == uniquePaymentID && x.StatusID == MiidWeb.Helpers.StatusHelper.StatusID("Ticket", "Reserved"));
                return pendingTickets.ToList();
            }
        }

        //Clears tickets that have not been purchased
        public static void CancelTickets(int EndUserID)
        {
            int TicketPurchased = MiidWeb.Helpers.StatusHelper.StatusID("Ticket", "Purchased");

            using (var db = new MiidEntities())
            {
                var pendingTickets = db.Tickets.Where(f => f.EndUserID == EndUserID && f.StatusID != TicketPurchased).ToList();

                /*
                pendingTickets.ForEach(a => a.EndUserID= null);
                pendingTickets.ForEach(a => a.DatetimePurchased = null);
                pendingTickets.ForEach(a => a.DatetimeReserved = null);
                pendingTickets.ForEach(a => a.StatusID = 1); //Active
                */

                foreach (var p in pendingTickets)
                {
                    db.Tickets.Remove(p);
                }
                db.SaveChanges();

            }


        }


        public static void ReleaseExpiredSeats(string reference = "")
        {
            bool result = false;


            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("ReleaseExpiredSeats", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };
            sqlDataAdapter.SelectCommand.Parameters.Add("@reference", SqlDbType.VarChar).Value = reference;

            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];
            if (item != null)
            {
                foreach (DataRow row in item.Rows)
                {

                    result = bool.Parse(row["result"].ToString());

                }
            }
            sqlConnection.Close();

            return;

        }

        public static void DeleteExpiredReservedTicketsAll()
        {
            bool result = false;


            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("DeleteExpiredReservedTicketsAll", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };


            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];
            if (item != null)
            {
                foreach (DataRow row in item.Rows)
                {

                    result = bool.Parse(row["result"].ToString());

                }
            }
            sqlConnection.Close();

            return;

        }


        public static void ClearExpiredTicketSeats(string reference = null)
        {
            bool result = false;

            string procName = "ClearExpiredTicketSeats";
            if (reference != null)
            {
                procName = "ClearExpiredTicketSeatsByRef";
            }
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand(procName, sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };
            if (reference != null)
            {
                sqlDataAdapter.SelectCommand.Parameters.Add("@reference", SqlDbType.VarChar).Value = reference;
            }

            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];
            if (item != null)
            {
                foreach (DataRow row in item.Rows)
                {

                    result = bool.Parse(row["result"].ToString());

                }
            }
            sqlConnection.Close();

            return;

        }

        public static string GenerateTicketNumber(List<string> TicketsThisSession)
        {
            var db = new MiidEntities();
            string keystring = "";
            List<Ticket> ticketlist = db.Tickets.ToList();
            List<string> keys = new List<string>();
            foreach (var pi in ticketlist)
            {
                keys.Add(pi.TicketNumber.ToString());
            }

            Random ran = new Random(DateTime.Now.Millisecond);

            int key = 0; int key2;
            do
            {
                key = ran.Next(1000000000, int.MaxValue);
                key2 = ran.Next(1000, int.MaxValue);

                keystring = String.Format("{0}{1}", key.ToString().Substring(0, 6), key2.ToString().Substring(0, 4));

            } while (keys.Contains(keystring) || TicketsThisSession.Contains(keystring));

            return keystring;
        }

        public static EndUser WhoBoughtTheseTickets(string uniquePaymentID)
        {
            EndUser who = new EndUser();

            using (var db = new MiidEntities())
            {
                var banktrans = db.BankTransactions.Where(x => x.Reference == uniquePaymentID);
                if (banktrans != null && banktrans.Count() > 0)
                {
                    int enduserid = banktrans.First().EndUserID;
                    who= db.EndUsers.Find(enduserid);
                }
            
            }
            return who;
        }

        public  static Event GetEventFromUniquePaymentID(string uniquePaymentID)
        {
            int result = 0;

            string procName = "GetEventFromUniquePaymentID";
          
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand(procName, sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };
            if (uniquePaymentID != null)
            {
                sqlDataAdapter.SelectCommand.Parameters.Add("@uniquePaymentID", SqlDbType.VarChar).Value = uniquePaymentID;
            }

            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];
            if (item != null)
            {
                foreach (DataRow row in item.Rows)
                {

                    result = int.Parse(row["EventID"].ToString());

                }
            }
            sqlConnection.Close();

            using (var db = new MiidEntities(true))
            {
                return db.Events.Find(result);
            }
        }
    }

    public static class ConfirmationStatusItem
    {
        public static string Code { get; set; }
        public static string Description { get; set; }

    }
}