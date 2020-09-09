using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MiidWeb;
using MiidWeb.Helpers;
using MiidWeb.Models;
using MiidWeb.Repositories;

namespace MiidWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BankTransactionsController : Controller
    {
        private MiidEntities db = new MiidEntities();
        private void GetLayoutFiles()
        {

            string host = HttpContext.Request.Url.Host;

         //   string company = host.Split('.')[0].ToString();
            string company = host;

            //   if (company.Contains("www")) {  company = "training.miid.co.za"; }
           if (company.Contains("localhost")) { company = "miid.co.za"; }

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
        // GET: BankTransactions
        public ActionResult Index()
        {
            return View(db.BankTransactions.ToList());
        }


        public ActionResult IndexApprovals(string ttype)
        {
            List<BankTransactionViewModel> list = new List<BankTransactionViewModel>();
            int PendingStatusID = 0;
            int TransactionTypeID = 0;
            ViewBag.Context = ttype;

            switch (ttype)
            {

                case "manualeft":

                    PendingStatusID = StatusHelper.StatusID("BankTransaction", "waiting on payment");
                    TransactionTypeID = StatusHelper.TransactionTypeID("Manual EFT Topup");

                    foreach (var bt in db.BankTransactions.Where(x => x.StatusID == PendingStatusID && x.TransactionTypeID == TransactionTypeID).ToList())
                    {
                        list.Add(new BankTransactionViewModel(bt));

                    }
                    break;

                case "withdrawal":

                    PendingStatusID = StatusHelper.StatusID("MiiFunds Withdrawal Request", "Pending Admin Payout");
                    TransactionTypeID = StatusHelper.TransactionTypeID("MiiFunds Withdrawal");



                    foreach (var bt in db.BankTransactions.Where(x => x.StatusID == PendingStatusID && x.TransactionTypeID == TransactionTypeID).ToList())
                    {
                        list.Add(new BankTransactionViewModel(bt));

                    }
                    break;
                case "ticketpurchase":
                    PendingStatusID = StatusHelper.StatusID("BankTransaction", "Pending Ticket Purchase");
                    TransactionTypeID = StatusHelper.TransactionTypeID("Manual EFT Ticket Purchase");



                    foreach (var bt in db.BankTransactions.Where(x => x.StatusID == PendingStatusID && x.TransactionTypeID == TransactionTypeID).ToList())
                    {
                        list.Add(new BankTransactionViewModel(bt));

                    }
                    break;
                default: break;

            }
            return View(list);
        }

        [HttpPost]
        public ActionResult IndexApprovals(FormCollection form)
        {

            int ApprovedStatusID = 0;
            int CancelledStatusID = 0;
            int TransactionTypeID = 0;
            int AmountSign = 1;


            string context = form["hdnContext"].ToString();

            switch (context)
            {

                case "manualeft":
                    ApprovedStatusID = StatusHelper.StatusID("BankTransaction", "payment received");
                    CancelledStatusID = StatusHelper.StatusID("BankTransaction", "cancelled");

                    TransactionTypeID = StatusHelper.TransactionTypeID("TopUp");
                    AmountSign = 1;//positive
                    break;
                case "withdrawal":
                    ApprovedStatusID = StatusHelper.StatusID("MiiFunds Withdrawal Request", "Admin Paid Out");
                    TransactionTypeID = StatusHelper.TransactionTypeID("MiiFunds Withdrawal Paid Out");
                    AmountSign = -1; //negative
                    break;
                case "ticketpurchase":
                    ApprovedStatusID = StatusHelper.StatusID("BankTransaction", "Approved Ticket Purchase");
                    TransactionTypeID = StatusHelper.TransactionTypeID("TopUp");
                    AmountSign = 1;//positive
                    break;
                default: break;
            }

            int x = 1;

            int PermissionCount = int.Parse(form["hdnRowCount"].ToString());

            while (x <= PermissionCount)
            {
                int BankTransactionID = int.Parse(form["hdn" + x.ToString()]);
                decimal NewAmount = decimal.Parse(form["Amount" + x.ToString()]);
                string Note = form["Note" + x.ToString()];
                var Selected = form["chk" + x.ToString()];
                var Cancelled = form["chkcncl" + x.ToString()];

                var bt = db.BankTransactions.Find(BankTransactionID);


                if (Selected == "true,false" || Selected == "on") //then true
                {
                    

                    if (context != "manualeft" || (context == "manualeft" && !(Cancelled == "true,false" || Cancelled == "on")))
                    {
                       
                        bt.Note = Note;
                        bt.StatusID = ApprovedStatusID;
                        bt.Amount = NewAmount;
                        bt.ConfirmationDate = DateTime.Now;
                        db.Entry(bt).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                    
                        bt.Note = Note;
                        bt.StatusID = CancelledStatusID;
                        bt.Amount = NewAmount;
                        db.Entry(bt).State = EntityState.Modified;
                        db.SaveChanges();

                    }

                    if (context == "manualeft" && !(Cancelled == "true,false" || Cancelled == "on"))
                    {
                        //Update / Create a MiiFunds Transaction
                        decimal lastBalance = 0.00M;
                        if (db.MyMoneys.Where(k => k.EndUserID == bt.EndUserID).OrderByDescending(y => y.ID).Count() > 0)
                        {
                            MyMoney last = db.MyMoneys.Where(k => k.EndUserID == bt.EndUserID).OrderByDescending(y => y.ID).First();
                            lastBalance = last.RunningBalance ?? 0.00M;
                        }



                        MyMoney money = new MyMoney
                        {
                            Amount = bt.Amount * (decimal)AmountSign,
                            DateTimeUpdated = DateTime.Now,
                            Description = bt.Description,
                            EndUserID = bt.EndUserID,
                            RunningBalance = lastBalance + (bt.Amount * (decimal)AmountSign),
                            TransactionTypeID = TransactionTypeID,
                            TransactionDate = DateTime.Now,
                            EmailNotificationStatus = "P",//pending
                            Reference = bt.Reference,
                            Note = Note

                        };

                        db.MyMoneys.Add(money);
                        db.SaveChanges();
                        db.Entry(money).GetDatabaseValues();

                       

                    }
                   

                        if (context == "withdrawal")
                    {
                        //Update / Create a MiiFunds Transaction

                        if (db.MyMoneys.Where(k => k.EndUserID == bt.EndUserID && k.Reference == bt.Reference).Count() > 0)
                        {
                            MyMoney money = db.MyMoneys.Where(k => k.EndUserID == bt.EndUserID && k.Reference == bt.Reference).First();
                            money.Description = "MiiFunds Withdrawal";
                            money.TransactionTypeID = TransactionTypeID;
                            money.DateTimeUpdated = DateTime.Now;
                            money.EmailNotificationStatus = "P";
                            money.Note = Note;
                            db.Entry(money).State = EntityState.Modified;
                            db.SaveChanges();
                            db.Entry(money).GetDatabaseValues();
                        }
                       


                    }

                    if (context == "ticketpurchase")
                    { 
                        //update ticket status to confirmed
                        List<TicketViewModel> boughtTickets = TicketRepository.ConfirmTickets(bt.Description);
                        string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                        GetLayoutFiles();
                        string subdomain = GlobalVariables.Company;
                        // MyMoneyRepository.SendConfirmationManualEFT_TicketPurchase(bt.EndUserID, (int)(bt.Amount??0), bt.DepositorName, bt.TransactionDate, boughtTickets, baseUrl,subdomain);
                        MyMoneyRepository.SendConfirmationManualEFT_TicketPurchase(bt.EndUserID, (int)(bt.Amount ?? 0), bt.Reference, bt.TransactionDate, boughtTickets, baseUrl, subdomain);
                    }

                }
                else
                {
                    if (Cancelled == "true,false" || Cancelled == "on")
                    {

                        EndUserRepository.SendCancelManualEftEmail(bt);

                    }

                }
                x++;
            }

            if (context == "manualeft" || context == "withdrawal")
            {
                EndUserRepository.SendBatchNotification(context);
            }

            return RedirectToAction("IndexApprovals", new { ttype = context});
        }



        // GET: BankTransactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankTransaction bankTransaction = db.BankTransactions.Find(id);
            if (bankTransaction == null)
            {
                return HttpNotFound();
            }
            return View(bankTransaction);
        }

        // GET: BankTransactions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BankTransactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TransactionDate,ConfirmationDate,EndUserID,Description,TransactionTypeID,StatusID,Amount,UpdatedByUserName,DepositorName")] BankTransaction bankTransaction)
        {
            if (ModelState.IsValid)
            {
                db.BankTransactions.Add(bankTransaction);
                db.SaveChanges();

                ExportCsvRepository.ExportCSVAndEmail("AreThereMoreTicketsThanBank", bankTransaction.Reference ?? "","info@miid.co.za", String.Format("WARNING. duplicate transaction: {0}", bankTransaction.Reference ?? ""), "See attachment", String.Format("AreThereMoreTicketsThanBank_{0}.csv", DateTime.Now.ToString("yyyyMMddHHmmss")));

                return RedirectToAction("Index");
            }

            return View(bankTransaction);
        }

        // GET: BankTransactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankTransaction bankTransaction = db.BankTransactions.Find(id);
            if (bankTransaction == null)
            {
                return HttpNotFound();
            }
            return View(bankTransaction);
        }

        // POST: BankTransactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TransactionDate,ConfirmationDate,EndUserID,Description,TransactionTypeID,StatusID,Amount,UpdatedByUserName,DepositorName")] BankTransaction bankTransaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bankTransaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bankTransaction);
        }

        // GET: BankTransactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankTransaction bankTransaction = db.BankTransactions.Find(id);
            if (bankTransaction == null)
            {
                return HttpNotFound();
            }
            return View(bankTransaction);
        }

        // POST: BankTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BankTransaction bankTransaction = db.BankTransactions.Find(id);
            db.BankTransactions.Remove(bankTransaction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
