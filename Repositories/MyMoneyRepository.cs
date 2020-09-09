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
using MiidWeb.Helpers;
using MiidWeb.api;
using System.IO;

namespace MiidWeb.Repositories
{
    public static class MyMoneyRepository
    {

        public static int GetSubdomainID()
        {
            string host = HttpContext.Current.Request.Url.Host;

            //  string company = host.Split('.')[0].ToString();
            string company = host;

            //  if (company.Contains("www")) {  company = "training.miid.co.za"; }
             if (company.Contains("localhost")) { company = "miid.co.za"; }

            GlobalVariables.Company = company;

            return LookupHelper.GetSubdomainID(company);

        }


        public static decimal MyAvailableFunds(int userid)
        {

            // decimal AdminFee = ConfiguredFeeHelper.FeeAmount("MiiFundsWithdrawal", 0.00M, int.Parse(GlobalVariables.SubdomainID));


            decimal AvailableFunds = CalculateMyAvailableFundsFromAllTransactions(userid);
            //if(AvailableFunds > AdminFee) return AvailableFunds - AdminFee;
            // else
            return AvailableFunds;


        }
        public static decimal CalculateMyAvailableFundsFromAllTransactions(int enduserID)
        {
            try
            {
                decimal result = 0.00M;
                SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
                sqlConnection.Open();
                DataSet dataSet = new DataSet();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
                {
                    SelectCommand = new SqlCommand("CalculateMyAvailableFundsFromAllTransactions", sqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    }
                };


                sqlDataAdapter.SelectCommand.Parameters.Add("@enduserID", SqlDbType.Int).Value = enduserID;


                sqlDataAdapter.Fill(dataSet, "dsResult");
                DataTable item = dataSet.Tables["dsResult"];
                if (item != null)
                {
                    foreach (DataRow row in item.Rows)
                    {
                        result = decimal.Parse(row[0].ToString());
                    }
                }
                sqlConnection.Close();

                return result;
            }
            catch (Exception e)
            {

                return 0.00M;
            }

        }

        public static decimal Purchase(int EndUserID, decimal Amount, string Description)
        {
            decimal RunningBalance = 0.0M;

            decimal AdminFee = Helpers.ConfiguredFeeHelper.FeeAmount("MiiFundsTicketPurchase", Amount, int.Parse(GlobalVariables.SubdomainID));

            using (MiidEntities db = new MiidEntities())
            {
                //get running balance
                var myMoneys = db.MyMoneys.Include(m => m.TransactionType).Include(m => m.EndUser).Where(x => x.EndUserID == EndUserID).OrderByDescending(x => x.TransactionDate).Take(1);

                if (myMoneys.Count() > 0)
                {
                    RunningBalance = (decimal)myMoneys.First().RunningBalance - (Amount + AdminFee);
                }
                else
                {
                    RunningBalance = 0 - (Amount + AdminFee);
                }

                var endUser = db.EndUsers.Find(EndUserID);
                var UniquePaymentID = Helpers.PaymentHelper.UniqueRefNo(endUser.Firstname, endUser.Surname);

                MyMoney myMoney = new MyMoney();
                myMoney.EndUserID = EndUserID;
                myMoney.TransactionTypeID = db.TransactionTypes.Where(x => x.Description.ToLower() == "purchase").First().ID;
                myMoney.TransactionDate = DateTime.Now;
                myMoney.Amount = -(Amount + AdminFee);
                myMoney.RunningBalance = RunningBalance;
                myMoney.Description = Description;
                myMoney.Reference = UniquePaymentID;

                db.MyMoneys.Add(myMoney);
                db.SaveChanges();

            };


            return RunningBalance;

        }

        public static decimal SuccessfulTopup(int EndUserID, decimal AmountInclFee, string Reference, string Description)
        {
            decimal RunningBalance = 0.0M;
            //Instant EFT Topup - deduct fee
            string path = HttpContext.Current.Server.MapPath("~/Uploads");
            System.IO.File.AppendAllText(Path.Combine(path, "IveriLog.txt"), "Successful Topup line 68");

            using (MiidEntities db = new MiidEntities())
            {

                BankTransaction bt = db.BankTransactions.Where(x => x.Reference == Reference).First();
                decimal Fee = (decimal)(bt.AdminFee ?? 0.0M);

                //LogHelper.Log(String.Format("decimal Fee={0}", Fee), "SuccessfulTopup");

                //get running balance
                var myMoneys = db.MyMoneys.Include(m => m.TransactionType).Include(m => m.EndUser).Where(x => x.EndUserID == EndUserID).OrderByDescending(x => x.TransactionDate).Take(1);

                if (myMoneys.Count() > 0)
                {
                    RunningBalance = (decimal)myMoneys.First().RunningBalance + AmountInclFee - Fee;
                    //LogHelper.Log(String.Format("RunningBalance={0}", Fee), "SuccessfulTopup");
                }
                else
                {
                    RunningBalance = 0 + AmountInclFee - Fee;
                    //LogHelper.Log(String.Format("RunningBalance={0}", Fee), "SuccessfulTopup");
                }

                var endUser = db.EndUsers.Find(EndUserID);
                //var UniquePaymentID = Helpers.PaymentHelper.UniqueRefNo(endUser.Firstname, endUser.Surname);

                MyMoney myMoney = new MyMoney();
                myMoney.EndUserID = EndUserID;
                myMoney.TransactionTypeID = StatusHelper.TransactionTypeID("Instant EFT Topup");
                myMoney.TransactionDate = DateTime.Now;
                myMoney.Amount = AmountInclFee - Fee;
                myMoney.RunningBalance = RunningBalance;
                myMoney.Description = Description;
                myMoney.Reference = Reference;

                db.MyMoneys.Add(myMoney);
                db.SaveChanges();
                //LogHelper.Log(String.Format(" AmountInclFee{0}", myMoney.Amount), "SuccessfulTopup");
                //LogHelper.Log(String.Format(" myMoney.Amount={0}", myMoney.Amount), "SuccessfulTopup");

            };


            return RunningBalance;

        }


        #region Email Notifications

        #region MiiFunds Emails
        public static void SendConfirmationMiifundsWithdrawalRequest(int EndUserID, decimal RequestedAmount, int p3, decimal Fee)
        {
            //Send mail to admin

            StringBuilder adminbody = new StringBuilder();
            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(EndUserID);
                //Send mail to user

                adminbody.AppendLine(String.Format("Hi, {0}", "MiiD Finance Admin"));
                adminbody.AppendLine(String.Format("Mii-Funds withdrawal request received from end user: {0} {1} - {2}", user.Firstname, user.Surname, user.Email));

                adminbody.AppendLine(String.Format("Amount: ZAR {0}", RequestedAmount.ToString()));
                adminbody.AppendLine(String.Format("Fee: ZAR {0}", Fee.ToString()));
                adminbody.AppendLine(String.Format("Make payment and change status on Withdrawal confirmation screen."));


                EmailHelper.SendMail("logs@miid.co.za", String.Format("Mii-Funds withdrawal request"), adminbody.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
                //EmailHelper.SendMail("jonathanmwallis1808@gmail.com", String.Format("Mii-Funds withdrawal request"), adminbody.ToString());
                //EmailHelper.SendMail("dsouchon@gmail.com", String.Format("Mii-Funds withdrawal request"), adminbody.ToString());

            }



            StringBuilder userbody = new StringBuilder();

            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(EndUserID);
                //Send mail to user

                userbody.AppendLine(String.Format("Hi, {0}", user.Firstname));
                userbody.AppendLine(String.Format("Thank you for your Mii-Funds withdrawal request"));
                userbody.AppendLine(String.Format("You will receive confirmation as soon as we have made the payment into your registered bank account."));
                userbody.AppendLine(String.Format("Amount requested: ZAR {0}", RequestedAmount.ToString()));
                userbody.AppendLine(String.Format("Fee: ZAR {0}", Fee.ToString()));
                userbody.AppendLine(String.Format("Thank you!"));
                userbody.AppendLine(String.Format("MiiD Team"));

                EmailHelper.SendMail(user.Email, "Mii-Funds withdrawal request received", userbody.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
            }
        }

        public static void SendConfirmationMiifundsWithdrawalApprovedPaidOut(int EndUserID, decimal RequestedAmount, decimal Fee, string Note)
        {
            RequestedAmount += Fee; //Remove fee from amount
            RequestedAmount = Math.Abs(RequestedAmount);

            //Send mail to admin

            StringBuilder adminbody = new StringBuilder();
            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(EndUserID);
                //Send mail to user

                adminbody.AppendLine(String.Format("Hi, {0}", "MiiD Finance Admin"));
                adminbody.AppendLine(String.Format("Mii-Funds withdrawal request received from end user: {0} {1} - {2} PAID OUT", user.Firstname, user.Surname, user.Email));

                adminbody.AppendLine(String.Format("Amount: ZAR {0}", RequestedAmount.ToString("0.00")));
                adminbody.AppendLine(String.Format("Fee deducted from MiiFunds: ZAR {0}", Fee.ToString("0.00")));
                adminbody.AppendLine(String.Format("Note: {0}", Note));


                EmailHelper.SendMail("logs@miid.co.za", String.Format("Mii-Funds withdrawal paid out"), adminbody.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
                //EmailHelper.SendMail("jonathanmwallis1808@gmail.com", String.Format("Mii-Funds withdrawal request paid out"), adminbody.ToString());
                //EmailHelper.SendMail("dsouchon@gmail.com", String.Format("Mii-Funds withdrawal request paid out"), adminbody.ToString());

            }
            StringBuilder userbody = new StringBuilder();

            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(EndUserID);
                //Send mail to user

                userbody.AppendLine(String.Format("Hi, {0}", user.Firstname));
                userbody.AppendLine(String.Format("Your Mii-Funds withdrawal request has been paid out"));
                userbody.AppendLine(String.Format("into your registered bank account."));
                userbody.AppendLine(String.Format("Amount paid: ZAR {0}", RequestedAmount.ToString("0.00")));
                userbody.AppendLine(String.Format("Note: {0}", Note));
                userbody.AppendLine(String.Format("Fee deducted from MiiFunds: ZAR {0}", Fee.ToString("0.00")));
                userbody.AppendLine(String.Format("Thank you!"));
                userbody.AppendLine(String.Format("MiiD Team"));

                EmailHelper.SendMail(user.Email, "Mii-Funds withdrawal request paid out", userbody.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
            }
        }

        public static void SendConfirmationMiiFunds_TicketPurchase(int EndUserID, int TotalAmountInRands, DateTime PaymentDate, List<TicketViewModel> boughtTickets, string serverPath, string subdomain)
        {
            //Send mail to admin
            var reportApi = new ReportApi();
            var ticketReort = new MemoryStream();
            string ticketPath;
            StringBuilder adminbody = new StringBuilder();
            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(EndUserID);
                //Send mail to user

                adminbody.AppendLine(String.Format("Hi, {0}", "MiiD Finance Admin"));
                adminbody.AppendLine(String.Format("MiiD Ticket Purchase via MiiFunds performed by end user: {0} {1} - {2}", user.Firstname, user.Surname, user.Email));

                adminbody.AppendLine("Tickets Purchased:");
                foreach (var t in boughtTickets)
                {
                    adminbody.AppendLine(String.Format("{0} - {1} - From: {2} to: {5}- R{3}- Ticket No: {4}", t.Event.EventName, t.TicketClass.Description, t.TicketClass.StartDate, t.TicketClass.Price.ToString(), t.Ticket.Hash, t.TicketClass.EndDate));

                }
                adminbody.AppendLine(String.Format("Total Amount: ZAR {0}", TotalAmountInRands.ToString()));



                EmailHelper.SendMail("logs@miid.co.za", String.Format("MiiFunds Ticket Purchase"), adminbody.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
                //EmailHelper.SendMail("jonathanmwallis1808@gmail.com", String.Format("MiiFunds Ticket Purchase"), adminbody.ToString());
                //EmailHelper.SendMail("dsouchon@gmail.com", String.Format("MiiFunds Ticket Purchase"), adminbody.ToString());
            }



            StringBuilder userbody = new StringBuilder();

            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(EndUserID);
                //Send mail to user

                //userbody.AppendLine(String.Format("Hi, {0}", user.Firstname));
                //userbody.AppendLine(String.Format("Thank you for your MiiD Ticket Purchase using MiiFunds"));
                //userbody.AppendLine(String.Format("The payment was successful."));
                //userbody.AppendLine("Tickets Purchased:");
                //foreach (var t in boughtTickets)
                //{
                //    userbody.AppendLine(String.Format("{0} - {1} - From: {2} to: {5}- R{3}- Ticket No: {4}", t.Event.EventName, t.TicketClass.Description, t.TicketClass.StartDate, t.TicketClass.Price.ToString(), t.Ticket.Hash, t.TicketClass.EndDate));

                //}
                //userbody.AppendLine(String.Format("Total Amount: ZAR {0}", TotalAmountInRands.ToString()));
                //userbody.AppendLine(String.Format("Thank you!"));
                //userbody.AppendLine(String.Format("MiiD Team"));

                //EmailHelper.SendMail(user.Email, "MiiD Ticket Purchase using MiiFunds Successful", userbody.ToString());

                var SUBDOMAIN = db.Subdomains.Find(int.Parse(GlobalVariables.SubdomainID));
                string fileName = HttpContext.Current.Server.MapPath(String.Format(@"~\Content\EmailTemplates\{0}", SUBDOMAIN.EmailTicketPurchaseMiiFunds));


                string body = System.IO.File.ReadAllText(fileName);

                body = body.Replace("<Firstname>", user.Firstname);
                body = body.Replace("<Email>", user.Email);
                body = body.Replace("<Amount>", TotalAmountInRands.ToString());

                body = body.Replace("<TicketClass>", boughtTickets.First().TicketClass.Code);
                body = body.Replace("<StartDate>", ((DateTime)boughtTickets.First().TicketClass.StartDate).ToString("dd MMM yyyy HH:mm"));
                body = body.Replace("<EndDate>", ((DateTime)boughtTickets.First().TicketClass.EndDate).ToString("dd MMM yyyy HH:mm"));
                body = body.Replace("<TicketNumber>", boughtTickets.First().Ticket.TicketNumber);
                body = body.Replace("<EventName>", boughtTickets.First().Event.EventName);

                List<string> ticketPaths = new List<string>();

                foreach (var bought in boughtTickets.Take(1))//The first ticket ID is enough to get the rest
                {
                    ticketPaths.Add(ticketPath = reportApi.PopulateTicketReportMulti(bought.Ticket.ID, serverPath, HttpContext.Current.Server.MapPath("/"), subdomain));
                }
                foreach (var bought in boughtTickets)
                {
                    reportApi.PopulateTicketReport(bought.Ticket.ID, serverPath, HttpContext.Current.Server.MapPath("/"), subdomain);
                }
                EmailHelper.SendMail(user.Email, "MiiD Ticket Purchase using MiiFunds Successful", body.ToString(), null, null, ticketPaths, ConfigRepo.GetSubdomainID());


            }
        }


        public static void SendConfirmationMiiFunds_TicketPurchaseWS(int EndUserID, int TotalAmountInRands, DateTime PaymentDate, List<TicketViewModel> boughtTickets, string serverPath, string subdomain)
        {
            //Send mail to admin
            var reportApi = new ReportApi();
            var ticketReort = new MemoryStream();
            string ticketPath;
            StringBuilder adminbody = new StringBuilder();
            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(EndUserID);
                //Send mail to user

                adminbody.AppendLine(String.Format("Hi, {0}", "MiiD Finance Admin"));
                adminbody.AppendLine(String.Format("MiiD Ticket Purchase via MiiFunds performed by end user: {0} {1} - {2}", user.Firstname, user.Surname, user.Email));

                adminbody.AppendLine("Tickets Purchased:");
                foreach (var t in boughtTickets)
                {
                    adminbody.AppendLine(String.Format("{0} - {1} - From: {2} to: {5}- R{3}- Ticket No: {4}", t.Event.EventName, t.TicketClass.Description, t.TicketClass.StartDate, t.TicketClass.Price.ToString(), t.Ticket.Hash, t.TicketClass.EndDate));

                }
                adminbody.AppendLine(String.Format("Total Amount: ZAR {0}", TotalAmountInRands.ToString()));



                EmailHelper.SendMail("logs@miid.co.za", String.Format("MiiFunds Ticket Purchase"), adminbody.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
                //EmailHelper.SendMail("jonathanmwallis1808@gmail.com", String.Format("MiiFunds Ticket Purchase"), adminbody.ToString());
                //EmailHelper.SendMail("dsouchon@gmail.com", String.Format("MiiFunds Ticket Purchase"), adminbody.ToString());
            }



            StringBuilder userbody = new StringBuilder();

            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(EndUserID);
                //Send mail to user

                //userbody.AppendLine(String.Format("Hi, {0}", user.Firstname));
                //userbody.AppendLine(String.Format("Thank you for your MiiD Ticket Purchase using MiiFunds"));
                //userbody.AppendLine(String.Format("The payment was successful."));
                //userbody.AppendLine("Tickets Purchased:");
                //foreach (var t in boughtTickets)
                //{
                //    userbody.AppendLine(String.Format("{0} - {1} - From: {2} to: {5}- R{3}- Ticket No: {4}", t.Event.EventName, t.TicketClass.Description, t.TicketClass.StartDate, t.TicketClass.Price.ToString(), t.Ticket.Hash, t.TicketClass.EndDate));

                //}
                //userbody.AppendLine(String.Format("Total Amount: ZAR {0}", TotalAmountInRands.ToString()));
                //userbody.AppendLine(String.Format("Thank you!"));
                //userbody.AppendLine(String.Format("MiiD Team"));

                //EmailHelper.SendMail(user.Email, "MiiD Ticket Purchase using MiiFunds Successful", userbody.ToString());

                var SUBDOMAIN = db.Subdomains.Find(int.Parse(GlobalVariables.SubdomainID));
                string fileName = HttpContext.Current.Server.MapPath(String.Format(@"~\Content\EmailTemplates\{0}", SUBDOMAIN.EmailTicketPurchaseMiiFunds));



                string body = System.IO.File.ReadAllText(fileName);

                body = body.Replace("<Firstname>", user.Firstname);
                body = body.Replace("<Email>", user.Email);
                body = body.Replace("<Amount>", TotalAmountInRands.ToString());

                body = body.Replace("<TicketClass>", boughtTickets.First().TicketClass.Code);
                body = body.Replace("<StartDate>", ((DateTime)boughtTickets.First().TicketClass.StartDate).ToString("yyyy-MM-dd HH:mm"));
                body = body.Replace("<EndDate>", ((DateTime)boughtTickets.First().TicketClass.EndDate).ToString("yyyy-MM-dd HH:mm"));
                body = body.Replace("<TicketNumber>", boughtTickets.First().Ticket.TicketNumber);
                body = body.Replace("<EventName>", boughtTickets.First().Event.EventName);

                List<string> ticketPaths = new List<string>();

                foreach (var bought in boughtTickets.Take(1))
                {
                    ticketPaths.Add(ticketPath = reportApi.PopulateTicketReportMulti(bought.Ticket.ID, serverPath, HttpContext.Current.Server.MapPath("/"), subdomain));
                }
                foreach (var bought in boughtTickets)
                {
                    reportApi.PopulateTicketReport(bought.Ticket.ID, serverPath, HttpContext.Current.Server.MapPath("/"), subdomain);
                }
                EmailHelper.SendMail(user.Email, "MiiD Ticket Purchase using MiiFunds Successful", body.ToString(), null, null, ticketPaths, ConfigRepo.GetSubdomainID());


            }
        }



        public static string DoIQualifyForADiscount(int userID, int? eventID, int? vendorID, decimal? fulltransactionAmount)
        {

            try
            {
                string result = "0";
                SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
                sqlConnection.Open();
                DataSet dataSet = new DataSet();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
                {
                    SelectCommand = new SqlCommand("DoIQualifyForADiscount", sqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    }
                };


                sqlDataAdapter.SelectCommand.Parameters.Add("@userID", SqlDbType.Int).Value = userID;
                sqlDataAdapter.SelectCommand.Parameters.Add("@eventID", SqlDbType.Int).Value = eventID;
                sqlDataAdapter.SelectCommand.Parameters.Add("@vendorID", SqlDbType.Int).Value = vendorID;
                sqlDataAdapter.SelectCommand.Parameters.Add("@fulltransactionAmount", SqlDbType.Decimal).Value = fulltransactionAmount;


                sqlDataAdapter.Fill(dataSet, "dsResult");
                DataTable item = dataSet.Tables["dsResult"];
                if (item != null)
                {
                    foreach (DataRow row in item.Rows)
                    {
                        result = row[0].ToString();
                    }
                }
                sqlConnection.Close();

                return result;
            }
            catch (Exception e)
            {

                return e.Message;
            }

        }



        public static string SaveQuickAddTransaction(int TransactionAmount, int EndUserID, bool IsCreditTransaction, string StaffUserName)
        {

            try
            {
                StringBuilder result = new StringBuilder();
                SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
                sqlConnection.Open();
                DataSet dataSet = new DataSet();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
                {
                    SelectCommand = new SqlCommand("SaveQuickAddTransaction", sqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    }
                };


                sqlDataAdapter.SelectCommand.Parameters.Add("@IsCreditTransaction", SqlDbType.Bit).Value = IsCreditTransaction;
                sqlDataAdapter.SelectCommand.Parameters.Add("@TransactionAmount", SqlDbType.Int).Value = TransactionAmount;
                sqlDataAdapter.SelectCommand.Parameters.Add("@EndUserID", SqlDbType.Int).Value = EndUserID;
                sqlDataAdapter.SelectCommand.Parameters.Add("@StaffUserName", SqlDbType.VarChar).Value = StaffUserName;
                

                sqlDataAdapter.Fill(dataSet, "dsResult");
                DataTable item = dataSet.Tables["dsResult"];
                if (item != null)
                {
                    foreach (DataRow row in item.Rows)
                    {
                        result.AppendLine(row[0].ToString());
                    }
                }
                sqlConnection.Close();

                return result.ToString();
            }
            catch (Exception e)
            {

                return e.Message;
            }

        }


        #endregion

        #region Card Payment Emails
        public static void SendConfirmationCardPayment_TicketPurchase(int EndUserID, int TotalAmountInRands, string UniquePaymentID, DateTime PaymentDate, List<TicketViewModel> boughtTickets, string EventName, string serverPath, string subdomain)
        {
            //Send mail to admin
            decimal fee = 0;
            int amount = 0;
            StringBuilder adminbody = new StringBuilder();
            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(EndUserID);
                //Send mail to user

                var bts = db.BankTransactions.Where(x => x.Reference == UniquePaymentID);
                BankTransaction bt = new BankTransaction();
                if (bts != null && bts.Count() > 0) bt = bts.First();
                fee = (decimal)bt.AdminFee;
                amount = (int)bt.Amount;

                adminbody.AppendLine(String.Format("Hi, {0}", "MiiD Finance Admin"));
                adminbody.AppendLine(String.Format("MiiD Card Payment Ticket purchase performed by end user: {0} {1} - {2}", user.Firstname, user.Surname, user.Email));
                adminbody.AppendLine(String.Format("Unique Payment No: {0}", UniquePaymentID));
                adminbody.AppendLine(String.Format("Amount: ZAR {0}", TotalAmountInRands.ToString()));
                adminbody.AppendLine(String.Format("Fee: ZAR {0}", fee.ToString()));
                adminbody.AppendLine(String.Format("Automatically Approved by Iveri System Response."));
                adminbody.AppendLine("Tickets Purchased:");
                foreach (var t in boughtTickets)
                {
                    adminbody.AppendLine(String.Format("{0} - {1} - From: {2} to: {5}- R{3}- Ticket No: {4}", t.Event.EventName, t.TicketClass.Description, t.TicketClass.StartDate, t.TicketClass.Price.ToString(), t.Ticket.Hash, t.TicketClass.EndDate));

                }

                EmailHelper.SendMail("logs@miid.co.za", String.Format("Card Payment Ticket Purchase: UQP: {0}", UniquePaymentID), adminbody.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
                //EmailHelper.SendMail("jonathanmwallis1808@gmail.com", String.Format("Card Payment Ticket Purchase: UQP: {0}", UniquePaymentID), adminbody.ToString());
                //EmailHelper.SendMail("dsouchon@gmail.com", String.Format("Card Payment Ticket Purchase: UQP: {0}", UniquePaymentID), adminbody.ToString());
            }



            StringBuilder userbody = new StringBuilder();

            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(EndUserID);
                //Send mail to user
                var reportApi = new ReportApi();
                var ticketReort = new MemoryStream();
                string ticketPath;


                userbody.AppendLine(String.Format("Hi, {0}", user.Firstname));
                userbody.AppendLine(String.Format("Thank you for your MiiD Ticket Purchase via Card Payment"));
                userbody.AppendLine(String.Format("The payment with Unique Payment No #{0} was successful.", UniquePaymentID));
                userbody.AppendLine("Tickets Purchased:");
                foreach (var t in boughtTickets)
                {
                    userbody.AppendLine(String.Format("{0} - {1} - From: {2} to: {5}- R{3}- Ticket No: {4}", t.Event.EventName, t.TicketClass.Description, t.TicketClass.StartDate, t.TicketClass.Price.ToString(), t.Ticket.Hash, t.TicketClass.EndDate));

                }

                //userbody.AppendLine(String.Format("Total Amount: ZAR {0}", TotalAmountInRands.ToString()));
                //userbody.AppendLine(String.Format("Thank you!"));
                //userbody.AppendLine(String.Format("MiiD Team"));
                var SUBDOMAIN = db.Subdomains.Find(int.Parse(GlobalVariables.SubdomainID));
                string fileName = HttpContext.Current.Server.MapPath(String.Format(@"~\Content\EmailTemplates\{0}", SUBDOMAIN.EmailTicketPurchase));



                string body = System.IO.File.ReadAllText(fileName);

                body = body.Replace("<Firstname>", user.Firstname);
                body = body.Replace("<Email>", user.Email);
                body = body.Replace("<Amount>", amount.ToString());
                body = body.Replace("<Fee>", fee.ToString());
                body = body.Replace("<UniquePaymentID>", UniquePaymentID);
                body = body.Replace("<TicketClass>", boughtTickets.First().TicketClass.Code);
                body = body.Replace("<StartDate>", ((DateTime)boughtTickets.First().TicketClass.StartDate).ToString("yyyy-MM-dd HH:mm"));
                body = body.Replace("<EndDate>", ((DateTime)boughtTickets.First().TicketClass.EndDate).ToString("yyyy-MM-dd HH:mm"));
                body = body.Replace("<TicketNumber>", boughtTickets.First().Ticket.TicketNumber);
                body = body.Replace("<EventName>", EventName);


                List<string> ticketPaths = new List<string>();

                foreach (var bought in boughtTickets.Take(1))
                {
                    ticketPaths.Add(reportApi.PopulateTicketReportMulti(bought.Ticket.ID, serverPath, HttpContext.Current.Server.MapPath("/"), subdomain));
                }

                EmailHelper.SendMail(user.Email, "MiiD Card Payment Ticket Purchase Successful", body.ToString(), null, null, ticketPaths, ConfigRepo.GetSubdomainID());
            }
        }

        public static decimal GetCalculatedBalance(int ID)
        {
            try
            {
                decimal result = 0.00M;
                SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
                sqlConnection.Open();
                DataSet dataSet = new DataSet();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
                {
                    SelectCommand = new SqlCommand("GetCalculatedBalance", sqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    }
                };


                sqlDataAdapter.SelectCommand.Parameters.Add("@ID", SqlDbType.Int).Value = ID;

                sqlDataAdapter.Fill(dataSet, "dsResult");
                DataTable item = dataSet.Tables["dsResult"];
                if (item != null)
                {
                    foreach (DataRow row in item.Rows)
                    {
                        result = decimal.Parse(row[0].ToString());
                    }
                }
                sqlConnection.Close();

                return result;
            }
            catch (Exception e)
            {
                Helpers.LogHelper.Log(e.Message + String.Format("Parameters- @ID: {0}", ID), "GetCalculatedBalance");
                return 0.00M;
            }
        }


        public static bool TransferMiiFunds(int fromAccountID, int toAccountID, decimal transferAmount, int parentID)
        {
            try
            {
                bool result = false;
                SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
                sqlConnection.Open();
                DataSet dataSet = new DataSet();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
                {
                    SelectCommand = new SqlCommand("TransferMiiFunds", sqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    }
                };


                sqlDataAdapter.SelectCommand.Parameters.Add("@fromAccountID", SqlDbType.Int).Value = fromAccountID;
                sqlDataAdapter.SelectCommand.Parameters.Add("@toAccountID", SqlDbType.Int).Value = toAccountID;
                sqlDataAdapter.SelectCommand.Parameters.Add("@transferAmount", SqlDbType.Int).Value = transferAmount;
                sqlDataAdapter.SelectCommand.Parameters.Add("@parentID", SqlDbType.Int).Value = parentID;

                sqlDataAdapter.Fill(dataSet, "dsResult");
                DataTable item = dataSet.Tables["dsResult"];
                if (item != null)
                {
                    foreach (DataRow row in item.Rows)
                    {
                        result = bool.Parse(row[0].ToString());
                    }
                }
                sqlConnection.Close();

                return result;
            }
            catch (Exception e)
            {
                Helpers.LogHelper.Log(e.Message + String.Format("Parameters- @FromAccountID: {0}, @ToAcountID: {1}, @Amount: {2}", fromAccountID, toAccountID, transferAmount), "TransferMiiFunds");
                return false;
            }
        }

        public static void SendConfirmationCardPayment(int EndUserID, int TotalAmountInRands, string UniquePaymentID, DateTime PaymentDate)
        {

            //Send mail to admin
            decimal fee = 0;
            int amount = 0;

            StringBuilder adminbody = new StringBuilder();
            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(EndUserID);
                //Send mail to user

                var bts = db.BankTransactions.Where(x => x.Reference == UniquePaymentID);
                BankTransaction bt = new BankTransaction();
                if (bts != null && bts.Count() > 0) bt = bts.First();
                fee = (decimal)bt.AdminFee;
                amount = (int)bt.Amount;

                adminbody.AppendLine(String.Format("Hi, {0}", "MiiD Finance Admin"));
                adminbody.AppendLine(String.Format("Mii-Funds Card Payment Topup performed by end user: {0} {1} - {2}", user.Firstname, user.Surname, user.Email));
                adminbody.AppendLine(String.Format("Unique Payment No: {0}", UniquePaymentID));
                adminbody.AppendLine(String.Format("Amount incl fee: ZAR {0}", TotalAmountInRands.ToString()));
                adminbody.AppendLine(String.Format("Fee: ZAR {0}", fee.ToString()));
                adminbody.AppendLine(String.Format("Automatically Approved by IVeri System Response."));


                EmailHelper.SendMail("logs@miid.co.za", String.Format("Card Payment Topup: UQP: {0}", UniquePaymentID), adminbody.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
                //EmailHelper.SendMail("jonathanmwallis1808@gmail.com", String.Format("Card Payment Topup: UQP: {0}", UniquePaymentID), adminbody.ToString());
                //EmailHelper.SendMail("dsouchon@gmail.com", String.Format("Card Payment Topup: UQP: {0}", UniquePaymentID), adminbody.ToString());
            }



            StringBuilder userbody = new StringBuilder();

            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(EndUserID);
                //Send mail to user

                //userbody.AppendLine(String.Format("Hi, {0}", user.Firstname));
                //userbody.AppendLine(String.Format("Thank you for your Mii-Funds Topup via Card Payment"));
                //userbody.AppendLine(String.Format("The payment with Unique Payment No #{0} was successful.", UniquePaymentID));
                //userbody.AppendLine(String.Format("Amount: ZAR {0}", TotalAmountInRands.ToString()));
                //userbody.AppendLine(String.Format("Thank you!"));
                //userbody.AppendLine(String.Format("MiiD Team"));
                var SUBDOMAIN = db.Subdomains.Find(int.Parse(GlobalVariables.SubdomainID));
                string fileName = HttpContext.Current.Server.MapPath(String.Format(@"~\Content\EmailTemplates\{0}", SUBDOMAIN.EmailMiifundsTopup));



                string body = System.IO.File.ReadAllText(fileName);

                body = body.Replace("<Firstname>", user.Firstname);
                body = body.Replace("<Email>", user.Email);
                body = body.Replace("<Amount>", amount.ToString());
                body = body.Replace("<Fee>", fee.ToString("0.00"));
                body = body.Replace("<UniquePaymentID>", UniquePaymentID);


                EmailHelper.SendMail(user.Email, "MiiD Card Payment Topup Successful", body.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
            }
        }

        public static void SendFailureCardPayment(int EndUserID, int TotalAmountInRands, string UniquePaymentID, DateTime PaymentDate)
        {

            //Send mail to admin

            StringBuilder adminbody = new StringBuilder();
            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(EndUserID);
                //Send mail to user
                String message = message = String.Format("Mii-Funds Topup via Card payment performed by end user: {0} {1} - {2} Failed", user.Firstname, user.Surname, user.Email);
                String subject = "Card payment Miifunds Topup Failed";

                if (UniquePaymentID.ToUpper().Contains("_TT"))
                {
                    message = String.Format("Ticket Purchase via Card payment performed by end user: {0} {1} - {2} Failed", user.Firstname, user.Surname, user.Email);
                    subject = "Card payment Ticket Purchase failed";
                }


                adminbody.AppendLine(String.Format("Hi, {0}", "MiiD Finance Admin"));
                adminbody.AppendLine(message);
                adminbody.AppendLine(String.Format("Unique Payment No: {0}", UniquePaymentID));
                adminbody.AppendLine(String.Format("Amount: ZAR {0}", TotalAmountInRands.ToString()));

                adminbody.AppendLine(String.Format("Automatically Failed by Payfast System Response."));


                EmailHelper.SendMail("logs@miid.co.za", String.Format("Card Payment Failed: UQP: {0}", UniquePaymentID), adminbody.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
                //EmailHelper.SendMail("jonathanmwallis1808@gmail.com", String.Format("Instant EFT Topup Failed: UQP: {0}", UniquePaymentID), adminbody.ToString());
                //EmailHelper.SendMail("dsouchon@gmail.com", String.Format("Instant EFT Topup Failed: UQP: {0}", UniquePaymentID), adminbody.ToString());
            }



            StringBuilder userbody = new StringBuilder();

            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(EndUserID);
                //Send mail to user

                String message = "Thank you for your Mii-Funds Topup via Card payment";
                String subject = "Card payment Miifunds Topup Failed";
                if (UniquePaymentID.ToUpper().Contains("_TT"))
                {
                    message = "Thank you for your Ticket Purchase via Card payment";
                    subject = "Card payment Ticket Purchase failed";
                }


                userbody.AppendLine(String.Format("Hi, {0}", user.Firstname));
                userbody.AppendLine(String.Format(message));
                userbody.AppendLine(String.Format("Unfortunately the payment with Unique Payment No #{0} was unsuccessful.", UniquePaymentID));
                userbody.AppendLine(String.Format("Amount: ZAR {0}", TotalAmountInRands.ToString()));
                userbody.AppendLine(String.Format("Kind regards"));
                userbody.AppendLine(String.Format("MiiD Team"));

                EmailHelper.SendMail(user.Email, subject, userbody.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
            }
        }


        #endregion

        #region Manual EFT Emails
        public static void SendConfirmationManualEFT(int EndUserID, decimal TotalAmountInRands, string UniquePaymentID, DateTime PaymentDate, decimal fee)
        {

            //Send mail to admin

            StringBuilder adminbody = new StringBuilder();
            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(EndUserID);
                //Send mail to user

                adminbody.AppendLine(String.Format("Hi, {0}", "MiiD Finance Admin"));
                adminbody.AppendLine(String.Format("Mii-FundsTopup request received from end user: {0} {1} - {2}", user.Firstname, user.Surname, user.Email));
                adminbody.AppendLine(String.Format("Unique Payment No: {0}", UniquePaymentID));
                adminbody.AppendLine(String.Format("Amount: ZAR {0}", TotalAmountInRands.ToString()));
                adminbody.AppendLine(String.Format("Fee: ZAR {0}", fee.ToString()));
                adminbody.AppendLine(String.Format("Check bank statement and approve."));


                EmailHelper.SendMail("logs@miid.co.za", String.Format("Manual EFT Topup Request: UQP: {0}", UniquePaymentID), adminbody.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
                //EmailHelper.SendMail("jonathanmwallis1808@gmail.com", String.Format("Manual EFT Topup Request: UQP: {0}", UniquePaymentID), adminbody.ToString());
                //EmailHelper.SendMail("dsouchon@gmail.com", String.Format("Manual EFT Topup Request: UQP: {0}", UniquePaymentID), adminbody.ToString());
            }



            StringBuilder userbody = new StringBuilder();

            using (var db = new MiidEntities())
            {
                var reportApi = new ReportApi();
                var ticketReort = new MemoryStream();
                var user = db.EndUsers.Find(EndUserID);

                var SUBDOMAIN = db.Subdomains.Find(int.Parse(GlobalVariables.SubdomainID));

                var NoFee = 0;

                string fileName = HttpContext.Current.Server.MapPath(String.Format(@"~\Content\EmailTemplates\{0}", SUBDOMAIN.ManauEFTtopUpRequest));

                string body = System.IO.File.ReadAllText(fileName);

                body = body.Replace("<Firstname>", user.Firstname);
                body = body.Replace("<Email>", user.Email);
                body = body.Replace("<Amount>", TotalAmountInRands.ToString());
                body = body.Replace("<Fee>", NoFee.ToString("0.00"));
                body = body.Replace("<UniquePaymentID>", UniquePaymentID);


                List<string> ticketPaths = new List<string>();


                EmailHelper.SendMail(user.Email, "MiiD: Your manual eft top-up request has been received", body.ToString(), null, null, ticketPaths, ConfigRepo.GetSubdomainID());

            }
        }

        //GIG CULTURE CHANGES HAPPENED HERE for manual EFT
        public static void SendConfirmationManualEFT_TicketPurchase(int EndUserID, int TotalAmountInRands, string UniquePaymentID, DateTime PaymentDate, List<TicketViewModel> boughtTickets, string serverPath, string subdomain)
        {


            SendConfirmationInstantEFT_TicketPurchase(EndUserID, TotalAmountInRands, UniquePaymentID, PaymentDate, boughtTickets, "", serverPath, subdomain, true);

            /*
            //Send mail to admin
            EndUser WhoBoughtTheseTickets = TicketRepository.WhoBoughtTheseTickets(UniquePaymentID);

            StringBuilder adminbody = new StringBuilder();
            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(EndUserID);
                //Send mail to user

                adminbody.AppendLine(String.Format("Hi, {0}", "MiiD Finance Admin"));
                adminbody.AppendLine(String.Format("Manual EFT Ticket purchase confirmed (funds received) for end user: {0} {1} - {2}", user.Firstname, user.Surname, user.Email));
                adminbody.AppendLine(String.Format("Unique Payment No: {0}", UniquePaymentID));
                adminbody.AppendLine("Tickets Purchased:");
                foreach (var t in boughtTickets)
                {
                    adminbody.AppendLine(String.Format("{0} - {1} - From: {2} to: {5}- R{3}- Ticket No: {4}", t.Event.EventName, t.TicketClass.Description, t.TicketClass.StartDate, t.TicketClass.Price.ToString(), t.Ticket.Hash, t.TicketClass.EndDate));

                }
                adminbody.AppendLine(String.Format("Total Amount: ZAR {0}", TotalAmountInRands.ToString()));



                EmailHelper.SendMail("logs@miid.co.za", String.Format("Manual EFT Ticket Purchase Confirmed: UQP: {0}", UniquePaymentID), adminbody.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
                //EmailHelper.SendMail("jonathanmwallis1808@gmail.com", String.Format("Manual EFT Ticket Purchase Confirmed: UQP: {0}", UniquePaymentID), adminbody.ToString());
                //EmailHelper.SendMail("dsouchon@gmail.com", String.Format("Manual EFT Ticket Purchase Confirmed: UQP: {0}", UniquePaymentID), adminbody.ToString());
            }



            StringBuilder userbody = new StringBuilder();





            using (var db = new MiidEntities())
            {
                var reportApi = new ReportApi();
                var ticketReort = new MemoryStream();
                var user = db.EndUsers.Find(EndUserID);

                


                List<int> recipientids = new List<int>();
                foreach (var b in boughtTickets)
                {
                    recipientids.Add(b.Ticket.EndUserID ?? 0);

                }
                List<int> uniquerecipientids = new List<int>();
                uniquerecipientids.AddRange(recipientids.Distinct().Where(x => x.ToString() != "0"));

                string fileName = HttpContext.Current.Server.MapPath(String.Format(@"~\Content\EmailTemplates\{0}", "ticket_purchase.html"));

                if (GlobalVariables.SubdomainID != null)
                {
                    var SUBDOMAIN = db.Subdomains.Find(int.Parse(GlobalVariables.SubdomainID));
                    fileName = HttpContext.Current.Server.MapPath(String.Format(@"~\Content\EmailTemplates\{0}", SUBDOMAIN.EmailTicketPurchase));
                }
                if (WhoBoughtTheseTickets.ID != EndUserID)
                {
                    fileName = HttpContext.Current.Server.MapPath(String.Format(@"~\Content\EmailTemplates\{0}", "FriendTicketEmail_training.miid.co.za"));
                    if (GlobalVariables.SubdomainID != null)
                    {
                        var SUBDOMAIN = db.Subdomains.Find(int.Parse(GlobalVariables.SubdomainID));
                        fileName = HttpContext.Current.Server.MapPath(String.Format(@"~\Content\EmailTemplates\FriendTicketEmail_{0}.html", SUBDOMAIN.SubdomainName.ToLower()));
                    }
                }

                string bodytext = System.IO.File.ReadAllText(fileName);
                string body = "";
                List<TicketViewModel> myboughtTickets = new List<TicketViewModel>();

                bool AreYouRegisteredBuddy = false;





                var NoFee = 0;

            

                body = body.Replace("<Firstname>", user.Firstname);
                body = body.Replace("<Email>", user.Email);
                body = body.Replace("<Amount>", TotalAmountInRands.ToString());
                body = body.Replace("<Fee>", NoFee.ToString("0.00"));
                body = body.Replace("<UniquePaymentID>", UniquePaymentID);
                body = body.Replace("<TicketClass>", boughtTickets.First().TicketClass.Code);
                body = body.Replace("<StartDate>", ((DateTime)boughtTickets.First().TicketClass.StartDate).ToString("yyyy-MM-dd HH:mm"));
                body = body.Replace("<EndDate>", ((DateTime)boughtTickets.First().TicketClass.EndDate).ToString("yyyy-MM-dd HH:mm"));
                body = body.Replace("<TicketNumber>", boughtTickets.First().Ticket.TicketNumber);
                body = body.Replace("<EventName>", boughtTickets.First().Event.EventName);

                List<string> ticketPaths = new List<string>();

                foreach (var bought in boughtTickets.Take(1))
                {
                    ticketPaths.Add(reportApi.PopulateTicketReportMulti(bought.Ticket.ID, serverPath, HttpContext.Current.Server.MapPath("/"), subdomain));
                }
                foreach (var bought in boughtTickets)
                {
                    reportApi.PopulateTicketReport(bought.Ticket.ID, serverPath, HttpContext.Current.Server.MapPath("/"), subdomain);
                }
                EmailHelper.SendMail(user.Email, "MiiD Ticket Purchase via Manual EFT Successful", body.ToString(), null, null, ticketPaths, ConfigRepo.GetSubdomainID());

            }
            */
        }
        public static void SendConfirmationManualEFT_TicketReservation(int EndUserID, decimal TotalAmountInRands, string UniquePaymentID, DateTime PaymentDate, List<TicketViewModel> boughtTickets)
        {
            //Send mail to admin

            StringBuilder adminbody = new StringBuilder();
            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(EndUserID);
                //Send mail to user


                //Send mail to user

                var SUBDOMAIN = db.Subdomains.Find(int.Parse(GlobalVariables.SubdomainID));
                string fileName = HttpContext.Current.Server.MapPath(String.Format(@"~\Content\EmailTemplates\{0}", SUBDOMAIN.ManualEftProRata));
                string body = System.IO.File.ReadAllText(fileName);
                //Send mail to user
                body = body.Replace("<Firstname>", user.Firstname);
                body = body.Replace("<Surname>", user.Surname);
                body = body.Replace("<Email>", user.Email);
                body = body.Replace("<Phone>", user.Cell);
                body = body.Replace("<Amount>", TotalAmountInRands.ToString());
                body = body.Replace("<UniquePaymentID>", UniquePaymentID);
                body = body.Replace("<Date>", PaymentDate.ToString());



                var reportApi = new ReportApi();
                var ticketReort = new MemoryStream();


                List<string> ticketPaths = new List<string>();



                EmailHelper.SendMail("logs@miid.co.za", "Pro forma invoice for tickets", body.ToString(), null, null, ticketPaths, ConfigRepo.GetSubdomainID());


                //EmailHelper.SendMail("jonathanmwallis1808@gmail.com", String.Format("Manual EFT Ticket Reservation : UQP: {0}", UniquePaymentID), adminbody.ToString());
                //EmailHelper.SendMail("dsouchon@gmail.com", String.Format("Manual EFT Ticket Reservation: UQP: {0}", UniquePaymentID), adminbody.ToString());
            }



            StringBuilder userbody = new StringBuilder();

            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(EndUserID);
                //Send mail to user

                var SUBDOMAIN = db.Subdomains.Find(int.Parse(GlobalVariables.SubdomainID));
                string fileName = HttpContext.Current.Server.MapPath(String.Format(@"~\Content\EmailTemplates\{0}", SUBDOMAIN.ManualEftProRata));
                string body = System.IO.File.ReadAllText(fileName);
                //Send mail to user
                body = body.Replace("<Firstname>", user.Firstname);
                body = body.Replace("<Surname>", user.Surname);
                body = body.Replace("<Email>", user.Email);
                body = body.Replace("<Phone>", user.Cell);
                body = body.Replace("<Amount>", TotalAmountInRands.ToString());
                body = body.Replace("<UniquePaymentID>", UniquePaymentID);

                body = body.Replace("<Date>", PaymentDate.ToString());
                DateTime today = DateTime.Today;
                body = body.Replace("<Date2>", today.ToString());



                var reportApi = new ReportApi();
                var ticketReort = new MemoryStream();


                List<string> ticketPaths = new List<string>();



                EmailHelper.SendMail(user.Email, "Pro forma invoice for ticket purchase - Mi-id Online ticketing", body.ToString(), null, null, ticketPaths, ConfigRepo.GetSubdomainID());

            }
        }

        public static void SendConfirmationManualEFTReceievedAndToppedUp(int EndUserID, int TotalAmountInRands, decimal fee, string Note)
        {


            StringBuilder userbody = new StringBuilder();

            using (var db = new MiidEntities())
            {
                var reportApi = new ReportApi();
                var ticketReort = new MemoryStream();
                var user = db.EndUsers.Find(EndUserID);

                var SUBDOMAIN = db.Subdomains.Find(int.Parse(GlobalVariables.SubdomainID));

                var NoFee = 0;

                string fileName = HttpContext.Current.Server.MapPath(String.Format(@"~\Content\EmailTemplates\{0}", SUBDOMAIN.ManauEFTtopUpConfirmed));

                string body = System.IO.File.ReadAllText(fileName);

                body = body.Replace("<Firstname>", user.Firstname);
                body = body.Replace("<Email>", user.Email);
                body = body.Replace("<Amount>", TotalAmountInRands.ToString());
                body = body.Replace("<Fee>", NoFee.ToString("0.00"));



                List<string> ticketPaths = new List<string>();


                EmailHelper.SendMail(user.Email, "MiiD: Your manual eft top-up confirmed", body.ToString(), null, null, ticketPaths, ConfigRepo.GetSubdomainID());

            }
        }


        public static void SendCancelManualEFTEmailNotification(int EndUserID, BankTransaction bt)
        {


            StringBuilder userbody = new StringBuilder();

            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(EndUserID);
                //Send mail to user

                userbody.AppendLine(String.Format("Hi, {0}", user.Firstname));
                userbody.AppendLine(String.Format("Your manual EFT topup was cancelled."));
                userbody.AppendLine(String.Format("The amount was {0}", bt.Amount.ToString()));
                userbody.AppendLine(String.Format("Transaction date {0}", bt.TransactionDate.ToString()));
                userbody.AppendLine(String.Format(@"
                
                Note: {0}", bt.Note));

                userbody.AppendLine(String.Format("Please contact support if you have any queries"));
                userbody.AppendLine(String.Format("MiiD Team"));

                EmailHelper.SendMail(user.Email, "Manual EFT Topup Cancelled", userbody.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
            }
        }

        public static bool UniqueIDExistsOnPostransaction(string uniqueID)
        {
          
           
                try
                {
                bool result = false;
                SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
                    sqlConnection.Open();
                    DataSet dataSet = new DataSet();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
                    {
                        SelectCommand = new SqlCommand("UniqueIDExistsOnPostransaction", sqlConnection)
                        {
                            CommandType = CommandType.StoredProcedure
                        }
                    };


                    sqlDataAdapter.SelectCommand.Parameters.Add("@uniqueID", SqlDbType.VarChar).Value = uniqueID;


                    sqlDataAdapter.Fill(dataSet, "dsResult");
                    DataTable item = dataSet.Tables["dsResult"];
                    if (item != null)
                    {
                        foreach (DataRow row in item.Rows)
                        {
                            result = bool.Parse(row[0].ToString());
                        }
                    }
                    sqlConnection.Close();

                    return result;
                }
                catch (Exception e)
                {

                    return false;
                }

            
        }



        #endregion

        #region Instant EFT Emails

        public static bool HasEmailBeenSentForThisTicket(string UniquePaymentID)
        {
            bool result = false;

            //int TicketPurchased = MiidWeb.Helpers.StatusHelper.StatusID("Ticket", "Purchased");

            //using (var db = new MiidEntities())
            //{
            //    var purchasedTickets = db.Tickets.Where(f => f.UniquePaymentID == UniquePaymentID && f.StatusID == TicketPurchased).ToList();
            //    if (purchasedTickets != null && purchasedTickets.Count() > 0)
            //    {
            //        result = true;
            //    }

            //}
            return result;
        }
        public static bool HasEmailBeenSentForThisTopup(string UniquePaymentID)
        {
            bool result = false;
            int TransactionTypeID = StatusHelper.TransactionTypeID("Instant EFT Topup");

            using (var db = new MiidEntities())
            {
                var instantEFTtopups = db.MyMoneys.Where(f => f.Reference == UniquePaymentID && f.TransactionTypeID == TransactionTypeID).ToList();
                if (instantEFTtopups != null && instantEFTtopups.Count() > 0)
                {
                    result = true;
                }

            }
            return result;
        }

        //GIG CULTURE CHANGES HAPPENED HERE
        public static void SendConfirmationInstantEFT_TicketPurchase(int EndUserID, int TotalAmountInRands, string UniquePaymentID, DateTime PaymentDate, List<TicketViewModel> boughtTickets, string EventName, string serverPath, string subdomain, bool ItsActuallyManualEFT = false)
        {
            //Please check if Event is single user per ticket event or not
            //What kind of event is it?

            Event eventmodel = TicketRepository.GetEventFromUniquePaymentID(UniquePaymentID);
            int _subdomainID = GetSubdomainID();
            try
            {
                EndUser WhoBoughtTheseTickets = TicketRepository.WhoBoughtTheseTickets(UniquePaymentID);



                var reportApi = new ReportApi();
                var ticketReort = new MemoryStream();
                string ticketPath;

                decimal fee = 0;
                int amount = 0;
                var db1 = new MiidEntities();
                //Send mail to admin
                var bts = db1.BankTransactions.Where(x => x.Reference == UniquePaymentID);
                BankTransaction bt = new BankTransaction();
                if (bts != null && bts.Count() > 0) bt = bts.First();
                fee = (decimal)(bt.AdminFee ?? 0.00M);
                amount = (int)bt.Amount;

                StringBuilder adminbody = new StringBuilder();

                string adminsubject = "MiiD Ticket Purchase via Instant EFT performed by end user:";
                string adminsubject2 = "Instant EFT Ticket Purchase:";

                string purchasersubject = "MiiD Ticket Purchase via Instant EFT Successful";

                Subdomain subdomainObject = new Subdomain();
                if (ItsActuallyManualEFT)
                {
                    subdomainObject = LookupHelper.GetSubdomainFromEventID(eventmodel.ID);
                    if (subdomainObject != null)
                    { _subdomainID = subdomainObject.ID; }
                    adminsubject = "MiiD Ticket Purchase via Manual EFT performed by end user:";
                    adminsubject2 = "Manual EFT Ticket Purchase:";
                    purchasersubject = "MiiD Ticket Purchase via Manual EFT Successful";
                }


                using (var db = new MiidEntities())
                {


                    var user = db.EndUsers.Find(EndUserID);
                    //Send mail to user

                    adminbody.AppendLine(String.Format("Hi, {0}", "MiiD Finance Admin"));
                    adminbody.AppendLine(String.Format(adminsubject + " {0} {1} - {2}", user.Firstname, user.Surname, user.Email));
                    adminbody.AppendLine(String.Format("Unique Payment No: {0}", UniquePaymentID));
                    adminbody.AppendLine("Tickets Purchased:");
                    foreach (var t in boughtTickets)
                    {
                        adminbody.AppendLine(String.Format("{0} - {1} - From: {2} to: {5}- R{3}- Ticket No: {4}", t.Event.EventName, t.TicketClass.Description, t.TicketClass.StartDate, t.TicketClass.Price.ToString(), t.Ticket.Hash, t.TicketClass.EndDate));

                    }
                    adminbody.AppendLine(String.Format("Amount: ZAR {0}", TotalAmountInRands.ToString()));
                    adminbody.AppendLine(String.Format("Service Fee: ZAR {0}", fee.ToString("0.00")));
                    adminbody.AppendLine(String.Format("Automatically Approved by Payfast System Response."));


                    EmailHelper.SendMail("logs@miid.co.za", String.Format(adminsubject2 + " UQP: {0}", UniquePaymentID), adminbody.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
                    //EmailHelper.SendMail("jonathanmwallis1808@gmail.com", String.Format("Instant EFT Ticket Purchase: UQP: {0}", UniquePaymentID), adminbody.ToString());
                    //EmailHelper.SendMail("dsouchon@gmail.com", String.Format("Instant EFT Ticket Purchase: UQP: {0}", UniquePaymentID), adminbody.ToString());
                }



                StringBuilder userbody = new StringBuilder();

                using (var db = new MiidEntities())
                {
                    var buyer = db.EndUsers.Find(EndUserID);




                    List<int> recipientids = new List<int>();
                    foreach (var b in boughtTickets)
                    {
                        recipientids.Add(b.Ticket.EndUserID ?? 0);

                    }
                    List<int> uniquerecipientids = new List<int>();
                    uniquerecipientids.AddRange(recipientids.Distinct().Where(x => x.ToString() != "0"));

                    string fileName = HttpContext.Current.Server.MapPath(String.Format(@"~\Content\EmailTemplates\{0}", "ticket_purchase.html"));
                    
                    if (_subdomainID >0)
                    {
                        var SUBDOMAIN = db.Subdomains.Find(_subdomainID);
                        fileName = HttpContext.Current.Server.MapPath(String.Format(@"~\Content\EmailTemplates\{0}", SUBDOMAIN.EmailTicketPurchase));
                    }


                    string bodytext = System.IO.File.ReadAllText(fileName);
                    string body = "";
                    List<TicketViewModel> myboughtTickets = new List<TicketViewModel>();

                    bool AreYouRegisteredBuddy = false;

                    //buyer gets one multi email
                    //BUYER GET A MULTI REPORT
                    body = bodytext;
                    body = body.Replace("<Firstname>", buyer.Firstname);
                    body = body.Replace("<Email>", buyer.Email);
                    body = body.Replace("<Amount>", amount.ToString());
                    body = body.Replace("<Fee>", fee.ToString("0.00"));
                    body = body.Replace("<UniquePaymentID>", UniquePaymentID);
                    body = body.Replace("<TicketClass>", boughtTickets.First().TicketClass.Code);
                    body = body.Replace("<StartDate>", ((DateTime)boughtTickets.First().TicketClass.StartDate).ToString("yyyy-MM-dd HH:mm"));
                    body = body.Replace("<EndDate>", ((DateTime)boughtTickets.First().TicketClass.EndDate).ToString("yyyy-MM-dd HH:mm"));
                    body = body.Replace("<TicketNumber>", boughtTickets.First().Ticket.TicketNumber);



                    List<string> buyerticketPaths = new List<string>();
                    //Generate Multi ticket for buyer
                    buyerticketPaths.Add(reportApi.PopulateTicketReportMulti(boughtTickets.First().Ticket.ID, serverPath, HttpContext.Current.Server.MapPath("/"), subdomain));
                    EmailHelper.SendMail(buyer.Email, purchasersubject, body.ToString(), null, null, buyerticketPaths, ConfigRepo.GetSubdomainID());

                    //Only send friend emails for single ticket per user events
                    if (eventmodel.LimitOneTicketPerUser ?? false == true)
                    {
                        fileName = HttpContext.Current.Server.MapPath(String.Format(@"~\Content\EmailTemplates\{0}", "FriendTicketEmail_training.miid.co.za"));

                        if (_subdomainID > 0)
                        {

                            var SUBDOMAIN = db.Subdomains.Find(_subdomainID);
                            fileName = HttpContext.Current.Server.MapPath(String.Format(@"~\Content\EmailTemplates\FriendTicketEmail_{0}.html", SUBDOMAIN.SubdomainName.ToLower()));
                        }


                        foreach (var userid in uniquerecipientids)
                        {

                            bodytext = System.IO.File.ReadAllText(fileName);
                            body = bodytext;

                            AreYouRegisteredBuddy = EndUserRepository.AreYouRegisteredBuddy(userid);

                            if (AreYouRegisteredBuddy)
                            {

                                var friend = db.EndUsers.Find(userid);

                                myboughtTickets = boughtTickets.Where(x => x.Ticket.EndUserID == userid).ToList();

                                body = body.Replace("<Firstname>", buyer.Firstname);
                                body = body.Replace("<Email>", buyer.Email);

                                body = body.Replace("<TicketClass>", myboughtTickets.First().TicketClass.Code);
                                body = body.Replace("<StartDate>", ((DateTime)myboughtTickets.First().TicketClass.StartDate).ToString("yyyy-MM-dd HH:mm"));
                                body = body.Replace("<EndDate>", ((DateTime)myboughtTickets.First().TicketClass.EndDate).ToString("yyyy-MM-dd HH:mm"));
                                body = body.Replace("<TicketNumber>", myboughtTickets.First().Ticket.TicketNumber);


                                List<string> ticketPaths = new List<string>();

                                foreach (var bought in myboughtTickets.Take(1))
                                {
                                    if (bought.Event != null) { EventName = bought.Event.EventName; }

                                    body = body.Replace("<EventName>", EventName);

                                    //TODO : check/change to make sure it only sends all the tickets belonging to this userid not the payment ref
                                    //Check complete: it is fine as it is 30 Jan 2020
                                    //IF YOU ARE NOT THE BUYER, but a Registered Friend: GET THE SINGLE REPORT in the email


                                    ticketPaths.Add(reportApi.PopulateTicketReport(bought.Ticket.ID, serverPath, HttpContext.Current.Server.MapPath("/"), subdomain));

                                }



                                EmailHelper.SendMail(friend.Email, "You received a ticket. See attached.", body.ToString(), null, null, ticketPaths, ConfigRepo.GetSubdomainID());


                            }
                            else //You are not registered buddy: you just get a download link forcing you to register!
                            {
                                string DownloadTicketLink = "";


                                bodytext = System.IO.File.ReadAllText(fileName);
                                body = bodytext;

                                buyer = db.EndUsers.Find(userid);

                                myboughtTickets = boughtTickets.Where(x => x.Ticket.EndUserID == userid).ToList();

                                if (String.IsNullOrEmpty(buyer.Firstname)) { body = body.Replace("<Firstname>", buyer.Email); }
                                else
                                {
                                    body = body.Replace("<Firstname>", buyer.Firstname);
                                }
                                body = body.Replace("<Email>", buyer.Email);
                                body = body.Replace("<UniquePaymentID>", UniquePaymentID);

                                body = body.Replace("<TicketClass>", myboughtTickets.First().TicketClass.Code);
                                body = body.Replace("<StartDate>", ((DateTime)myboughtTickets.First().TicketClass.StartDate).ToString("yyyy-MM-dd HH:mm"));
                                body = body.Replace("<EndDate>", ((DateTime)myboughtTickets.First().TicketClass.EndDate).ToString("yyyy-MM-dd HH:mm"));
                                body = body.Replace("<TicketNumber>", myboughtTickets.First().Ticket.TicketNumber);

                                body = body.Replace("<LinkToTicket>", DownloadTicketLink);

                                List<string> ticketPaths = new List<string>();


                                foreach (var bought in myboughtTickets)
                                {
                                    reportApi.PopulateTicketReport(bought.Ticket.ID, serverPath, HttpContext.Current.Server.MapPath("/"), subdomain);
                                    if (bought.Event != null) { EventName = bought.Event.EventName; }

                                    body = body.Replace("<EventName>", EventName);
                                }

                                EmailHelper.SendMail(buyer.Email, "You received a ticket.", body.ToString(), null, null, null, ConfigRepo.GetSubdomainID());

                            }
                        }
                    }
                    else
                    {
                        foreach (var userid in uniquerecipientids)
                        {
                            myboughtTickets = boughtTickets.Where(x => x.Ticket.EndUserID == userid).ToList();

                            foreach (var bought in myboughtTickets)
                            {
                                reportApi.PopulateTicketReport(bought.Ticket.ID, serverPath, HttpContext.Current.Server.MapPath("/"), subdomain);
                               
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {

                LogHelper.Log("Payfast Notify", ee.Message);

            }

        }

        public static void SendConfirmationPaygenius_TicketPurchase(int EndUserID, int TotalAmountInRands, string UniquePaymentID, DateTime PaymentDate, List<TicketViewModel> boughtTickets, string EventName, string serverPath, string subdomain)
        {

            try
            {
                var reportApi = new ReportApi();
                var ticketReort = new MemoryStream();
                string ticketPath;

                decimal fee = 0;
                int amount = 0;
                var db1 = new MiidEntities();
                //Send mail to admin
                var bts = db1.BankTransactions.Where(x => x.Reference == UniquePaymentID);
                BankTransaction bt = new BankTransaction();
                if (bts != null && bts.Count() > 0) bt = bts.First();
                fee = (decimal)(bt.AdminFee ?? 0.00M);
                amount = (int)bt.Amount;

                StringBuilder adminbody = new StringBuilder();
                using (var db = new MiidEntities())
                {



                    var user = db.EndUsers.Find(EndUserID);
                    //Send mail to user

                    adminbody.AppendLine(String.Format("Hi, {0}", "MiiD Finance Admin"));
                    adminbody.AppendLine(String.Format("MiiD Ticket Purchase via Paygenius performed by end user: {0} {1} - {2}", user.Firstname, user.Surname, user.Email));
                    adminbody.AppendLine(String.Format("Unique Payment No: {0}", UniquePaymentID));
                    adminbody.AppendLine("Tickets Purchased:");
                    foreach (var t in boughtTickets)
                    {
                        adminbody.AppendLine(String.Format("{0} - {1} - From: {2} to: {5}- R{3}- Ticket No: {4}", t.Event.EventName, t.TicketClass.Description, t.TicketClass.StartDate, t.TicketClass.Price.ToString(), t.Ticket.Hash, t.TicketClass.EndDate));

                    }
                    adminbody.AppendLine(String.Format("Amount: ZAR {0}", TotalAmountInRands.ToString()));
                    adminbody.AppendLine(String.Format("Service Fee: ZAR {0}", fee.ToString("0.00")));
                    adminbody.AppendLine(String.Format("Automatically Approved by Payfast System Response."));


                    EmailHelper.SendMail("logs@miid.co.za", String.Format("Paygenius Ticket Purchase: UQP: {0}", UniquePaymentID), adminbody.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
                    //EmailHelper.SendMail("jonathanmwallis1808@gmail.com", String.Format("Instant EFT Ticket Purchase: UQP: {0}", UniquePaymentID), adminbody.ToString());
                    //EmailHelper.SendMail("dsouchon@gmail.com", String.Format("Instant EFT Ticket Purchase: UQP: {0}", UniquePaymentID), adminbody.ToString());
                }



                StringBuilder userbody = new StringBuilder();

                using (var db = new MiidEntities())
                {
                    var user = db.EndUsers.Find(EndUserID);
                    //Send mail to user

                    //userbody.AppendLine(String.Format("Hi, {0}", user.Firstname));
                    //userbody.AppendLine(String.Format("Thank you for your MiiD Ticket."));
                    //userbody.AppendLine(String.Format("The payment with Unique Payment No #{0} was successful.", UniquePaymentID));
                    //userbody.AppendLine("Tickets Purchased:");
                    //foreach (var t in boughtTickets)
                    //{
                    //    userbody.AppendLine(String.Format("{0} - {1} - From: {2} to: {5}- R{3}- Ticket No: {4}", t.Event.EventName, t.TicketClass.Description, t.TicketClass.StartDate, t.TicketClass.Price.ToString(), t.Ticket.Hash, t.TicketClass.EndDate));

                    //}
                    //userbody.AppendLine(String.Format("Total Amount: ZAR {0}", TotalAmountInRands.ToString()));
                    //userbody.AppendLine(String.Format("Thank you!"));
                    //userbody.AppendLine(String.Format("MiiD Team"));
                    var SUBDOMAIN = db.Subdomains.Find(int.Parse(GlobalVariables.SubdomainID));
                    string fileName = HttpContext.Current.Server.MapPath(String.Format(@"~\Content\EmailTemplates\{0}", SUBDOMAIN.EmailTicketPurchaseEFT));

                    string body = System.IO.File.ReadAllText(fileName);

                    body = body.Replace("<Firstname>", user.Firstname);
                    body = body.Replace("<Email>", user.Email);
                    body = body.Replace("<Amount>", amount.ToString());
                    body = body.Replace("<Fee>", fee.ToString("0.00"));
                    body = body.Replace("<UniquePaymentID>", UniquePaymentID);
                    body = body.Replace("<TicketClass>", boughtTickets.First().TicketClass.Code);
                    body = body.Replace("<StartDate>", ((DateTime)boughtTickets.First().TicketClass.StartDate).ToString("yyyy-MM-dd HH:mm"));
                    body = body.Replace("<EndDate>", ((DateTime)boughtTickets.First().TicketClass.EndDate).ToString("yyyy-MM-dd HH:mm"));
                    body = body.Replace("<TicketNumber>", boughtTickets.First().Ticket.TicketNumber);
                    body = body.Replace("<EventName>", EventName);

                    List<string> ticketPaths = new List<string>();

                    foreach (var bought in boughtTickets.Take(1))
                    {
                        ticketPaths.Add(reportApi.PopulateTicketReportMulti(bought.Ticket.ID, serverPath, HttpContext.Current.Server.MapPath("/"), subdomain));
                    }
                    foreach (var bought in boughtTickets)
                    {
                        reportApi.PopulateTicketReport(bought.Ticket.ID, serverPath, HttpContext.Current.Server.MapPath("/"), subdomain);
                    }
                    EmailHelper.SendMail(user.Email, "MiiD Ticket Purchase via Paygenius Successful", body.ToString(), null, null, ticketPaths, ConfigRepo.GetSubdomainID());

                }
            }
            catch (Exception ee)
            {

                LogHelper.Log("Payfast Notify", ee.Message);

            }

        }



        public static void SendConfirmationPaygenius_TicketPurchaseExpired(int EndUserID, int TotalAmountInRands, string UniquePaymentID, DateTime PaymentDate, List<TicketViewModel> boughtTickets, string EventName, string serverPath, string subdomain)
        {

            try
            {
                var reportApi = new ReportApi();
                var ticketReort = new MemoryStream();
                string ticketPath;

                decimal fee = 0;
                int amount = 0;
                var db1 = new MiidEntities();
                //Send mail to admin
                var bts = db1.BankTransactions.Where(x => x.Reference == UniquePaymentID);
                BankTransaction bt = new BankTransaction();
                if (bts != null && bts.Count() > 0) bt = bts.First();
                fee = (decimal)(bt.AdminFee ?? 0.00M);
                amount = (int)bt.Amount;

                StringBuilder adminbody = new StringBuilder();
                using (var db = new MiidEntities())
                {



                    var user = db.EndUsers.Find(EndUserID);
                    //Send mail to user

                    adminbody.AppendLine(String.Format("Hi, {0}", "MiiD Finance Admin"));
                    adminbody.AppendLine(String.Format("MiiD Ticket Purchase via Paygenius performed by end user: {0} {1} - {2}", user.Firstname, user.Surname, user.Email));
                    adminbody.AppendLine(String.Format("Unique Payment No: {0}", UniquePaymentID));
                    adminbody.AppendLine("TICKETS ALREADY EXPIRED - PHONE TO REBOOK OR REFUND:");
                    foreach (var t in boughtTickets)
                    {
                        adminbody.AppendLine(String.Format("{0} - {1} - From: {2} to: {5}- R{3}- Ticket No: {4}", t.Event.EventName, t.TicketClass.Description, t.TicketClass.StartDate, t.TicketClass.Price.ToString(), t.Ticket.Hash, t.TicketClass.EndDate));

                    }
                    adminbody.AppendLine(String.Format("Amount: ZAR {0}", TotalAmountInRands.ToString()));
                    adminbody.AppendLine(String.Format("Service Fee: ZAR {0}", fee.ToString("0.00")));
                    adminbody.AppendLine(String.Format("Automatically Approved by Payfast System Response."));


                    EmailHelper.SendMail("logs@miid.co.za", String.Format("TICKETS ALREADY EXPIRED: Paygenius Ticket Purchase: UQP: {0}", UniquePaymentID), adminbody.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
                    //EmailHelper.SendMail("jonathanmwallis1808@gmail.com", String.Format("Instant EFT Ticket Purchase: UQP: {0}", UniquePaymentID), adminbody.ToString());
                    //EmailHelper.SendMail("dsouchon@gmail.com", String.Format("Instant EFT Ticket Purchase: UQP: {0}", UniquePaymentID), adminbody.ToString());
                }



                StringBuilder userbody = new StringBuilder();

                using (var db = new MiidEntities())
                {
                    var user = db.EndUsers.Find(EndUserID);
                    //Send mail to user

                    //userbody.AppendLine(String.Format("Hi, {0}", user.Firstname));
                    //userbody.AppendLine(String.Format("Thank you for your MiiD Ticket."));
                    //userbody.AppendLine(String.Format("The payment with Unique Payment No #{0} was successful.", UniquePaymentID));
                    //userbody.AppendLine("Tickets Purchased:");
                    //foreach (var t in boughtTickets)
                    //{
                    //    userbody.AppendLine(String.Format("{0} - {1} - From: {2} to: {5}- R{3}- Ticket No: {4}", t.Event.EventName, t.TicketClass.Description, t.TicketClass.StartDate, t.TicketClass.Price.ToString(), t.Ticket.Hash, t.TicketClass.EndDate));

                    //}
                    //userbody.AppendLine(String.Format("Total Amount: ZAR {0}", TotalAmountInRands.ToString()));
                    //userbody.AppendLine(String.Format("Thank you!"));
                    //userbody.AppendLine(String.Format("MiiD Team"));
                    var SUBDOMAIN = db.Subdomains.Find(int.Parse(GlobalVariables.SubdomainID));
                    string fileName = HttpContext.Current.Server.MapPath(String.Format(@"~\Content\EmailTemplates\{0}", SUBDOMAIN.EmailTicketPurchaseEFTExpired));

                    string body = System.IO.File.ReadAllText(fileName);

                    body = body.Replace("<Firstname>", user.Firstname);
                    body = body.Replace("<Email>", user.Email);
                    body = body.Replace("<Amount>", amount.ToString());
                    body = body.Replace("<Fee>", fee.ToString("0.00"));
                    body = body.Replace("<UniquePaymentID>", UniquePaymentID);
                    body = body.Replace("<TicketClass>", boughtTickets.First().TicketClass.Code);
                    body = body.Replace("<StartDate>", ((DateTime)boughtTickets.First().TicketClass.StartDate).ToString("yyyy-MM-dd HH:mm"));
                    body = body.Replace("<EndDate>", ((DateTime)boughtTickets.First().TicketClass.EndDate).ToString("yyyy-MM-dd HH:mm"));
                    body = body.Replace("<TicketNumber>", boughtTickets.First().Ticket.TicketNumber);
                    body = body.Replace("<EventName>", EventName);


                    EmailHelper.SendMail(user.Email, "MiiD Payment Received but Tickets already EXPIRED", body.ToString(), null, null, null, ConfigRepo.GetSubdomainID());

                }
            }
            catch (Exception ee)
            {

                LogHelper.Log("Payfast Notify", ee.Message);

            }

        }



        public static void SendConfirmationInstantEFT(int EndUserID, int TotalAmountInRands, string UniquePaymentID, DateTime PaymentDate)
        {
            decimal fee = 0;
            int amount = 0;
            var db1 = new MiidEntities();
            //Send mail to admin
            var bts = db1.BankTransactions.Where(x => x.Reference == UniquePaymentID);
            BankTransaction bt = new BankTransaction();
            if (bts != null && bts.Count() > 0) bt = bts.First();
            fee = (decimal)bt.AdminFee;
            amount = (int)bt.Amount;
            //Send mail to admin

            StringBuilder adminbody = new StringBuilder();
            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(EndUserID);
                //Send mail to user

                adminbody.AppendLine(String.Format("Hi, {0}", "MiiD Finance Admin"));
                adminbody.AppendLine(String.Format("Mii-Funds Instant EFT Topup performed by end user: {0} {1} - {2}", user.Firstname, user.Surname, user.Email));
                adminbody.AppendLine(String.Format("Unique Payment No: {0}", UniquePaymentID));
                adminbody.AppendLine(String.Format("Amount: ZAR {0}", TotalAmountInRands.ToString()));
                adminbody.AppendLine(String.Format("Automatically Approved by Payfast System Response."));


                EmailHelper.SendMail("logs@miid.co.za", String.Format("Instant EFT Topup: UQP: {0}", UniquePaymentID), adminbody.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
                //EmailHelper.SendMail("jonathanmwallis1808@gmail.com", String.Format("Instant EFT Topup: UQP: {0}", UniquePaymentID), adminbody.ToString());
                //EmailHelper.SendMail("dsouchon@gmail.com", String.Format("Instant EFT Topup: UQP: {0}", UniquePaymentID), adminbody.ToString());
            }



            StringBuilder userbody = new StringBuilder();

            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(EndUserID);
                //Send mail to user

                //userbody.AppendLine(String.Format("Hi, {0}", user.Firstname));
                //userbody.AppendLine(String.Format("Thank you for your Mii-Funds Topup via Instant EFT"));
                //userbody.AppendLine(String.Format("The payment with Unique Payment No #{0} was successful.", UniquePaymentID));
                //userbody.AppendLine(String.Format("Amount: ZAR {0}", TotalAmountInRands.ToString()));
                //userbody.AppendLine(String.Format("Thank you!"));
                //userbody.AppendLine(String.Format("MiiD Team"));
                var SUBDOMAIN = db.Subdomains.Find(int.Parse(GlobalVariables.SubdomainID));
                string fileName = HttpContext.Current.Server.MapPath(String.Format(@"~\Content\EmailTemplates\{0}", SUBDOMAIN.EmailMiifundsTopupEFT));

                string body = System.IO.File.ReadAllText(fileName);

                body = body.Replace("<Firstname>", user.Firstname);
                body = body.Replace("<Email>", user.Email);
                body = body.Replace("<Amount>", amount.ToString());
                body = body.Replace("<Fee>", fee.ToString("0.00"));
                body = body.Replace("<UniquePaymentID>", UniquePaymentID);




                EmailHelper.SendMail(user.Email, "Instant EFT Topup Successful", body.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
            }
        }

        public static void SendFailureInstantEFT(int EndUserID, int TotalAmountInRands, string UniquePaymentID, DateTime PaymentDate)
        {

            //Send mail to admin

            StringBuilder adminbody = new StringBuilder();
            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(EndUserID);
                //Send mail to user

                adminbody.AppendLine(String.Format("Hi, {0}", "MiiD Finance Admin"));
                adminbody.AppendLine(String.Format("Mii-Funds Instant EFT Topup performed by end user: {0} {1} - {2} Failed", user.Firstname, user.Surname, user.Email));
                adminbody.AppendLine(String.Format("Unique Payment No: {0}", UniquePaymentID));
                adminbody.AppendLine(String.Format("Amount: ZAR {0}", TotalAmountInRands.ToString()));
                adminbody.AppendLine(String.Format("Automatically Failed by Payfast System Response."));


                EmailHelper.SendMail("logs@miid.co.za", String.Format("Instant EFT Topup Failed: UQP: {0}", UniquePaymentID), adminbody.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
                //EmailHelper.SendMail("jonathanmwallis1808@gmail.com", String.Format("Instant EFT Topup Failed: UQP: {0}", UniquePaymentID), adminbody.ToString());
                //EmailHelper.SendMail("dsouchon@gmail.com", String.Format("Instant EFT Topup Failed: UQP: {0}", UniquePaymentID), adminbody.ToString());
            }



            StringBuilder userbody = new StringBuilder();

            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(EndUserID);
                //Send mail to user

                userbody.AppendLine(String.Format("Hi, {0}", user.Firstname));
                userbody.AppendLine(String.Format("Thank you for your Mii-Funds Topup via Instant EFT"));
                userbody.AppendLine(String.Format("Unfortunately the payment with Unique Payment No #{0} was unsuccessful.", UniquePaymentID));
                userbody.AppendLine(String.Format("Amount: ZAR {0}", TotalAmountInRands.ToString()));
                userbody.AppendLine(String.Format("Kind regards"));
                userbody.AppendLine(String.Format("MiiD Team"));

                EmailHelper.SendMail(user.Email, "Instant EFT Topup Failed", userbody.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
            }
        }

        public static void SendFailurePaygenius(int EndUserID, int TotalAmountInRands, string UniquePaymentID, DateTime PaymentDate)
        {

            //Send mail to admin

            StringBuilder adminbody = new StringBuilder();
            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(EndUserID);
                //Send mail to user

                adminbody.AppendLine(String.Format("Hi, {0}", "MiiD Finance Admin"));
                adminbody.AppendLine(String.Format("Paygenius ticket purchase performed by end user: {0} {1} - {2} Failed", user.Firstname, user.Surname, user.Email));
                adminbody.AppendLine(String.Format("Unique Payment No: {0}", UniquePaymentID));
                adminbody.AppendLine(String.Format("Amount: ZAR {0}", TotalAmountInRands.ToString()));
                adminbody.AppendLine(String.Format("Automatically Failed by Paygenius System Response."));


                EmailHelper.SendMail("logs@miid.co.za", String.Format("Paygenius ticket purchase Failed: UQP: {0}", UniquePaymentID), adminbody.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
                //EmailHelper.SendMail("jonathanmwallis1808@gmail.com", String.Format("Instant EFT Topup Failed: UQP: {0}", UniquePaymentID), adminbody.ToString());
                //EmailHelper.SendMail("dsouchon@gmail.com", String.Format("Instant EFT Topup Failed: UQP: {0}", UniquePaymentID), adminbody.ToString());
            }



            StringBuilder userbody = new StringBuilder();

            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(EndUserID);
                //Send mail to user

                userbody.AppendLine(String.Format("Hi, {0}", user.Firstname));
                userbody.AppendLine(String.Format("Thank you for your Paygenius ticket purchase"));
                userbody.AppendLine(String.Format("Unfortunately the payment with Unique Payment No #{0} was unsuccessful.", UniquePaymentID));
                userbody.AppendLine(String.Format("Amount: ZAR {0}", TotalAmountInRands.ToString()));
                userbody.AppendLine(String.Format("Kind regards"));
                userbody.AppendLine(String.Format("MiiD Team"));

                EmailHelper.SendMail(user.Email, "Paygenius ticket purchase Failed", userbody.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
            }
        }




        #endregion

        #endregion

















    }
}