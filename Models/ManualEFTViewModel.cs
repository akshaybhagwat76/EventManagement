using MiidWeb.Helpers;
using MiidWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiidWeb.Models
{
    public class ManualEFTViewModel
    {
        private MiidEntities db = new MiidEntities();
        public int ID { get; set; }
        public int EndUserID { get; set; }
        public string UniquePaymentID { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public decimal TotalAmountInRands { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal ConfiguredFeeAmount { get; set; }

        public bool TermsAndConditions { get; set; }
        public bool ReferenceInSafePlace { get; set; }

        public string Purpose{ get; set; }
        public string PurchaseSessionID { get; set; }

        public PurchaseTicketViewModel PurchaseTicketViewModel { get; set; }
        public decimal ActualAmount { get; set; }

        public ManualEFTViewModel()
        { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="EndUserID"></param>
        /// <param name="Purpose"> MiiFunds Topup, Ticket Purchase</param>
        public ManualEFTViewModel(int EndUserID, string Purpose, PurchaseTicketViewModel pvModel, string purchaseSessionID)
        {
            this.Purpose = Purpose;
            this.PurchaseTicketViewModel = pvModel;
            this.PurchaseSessionID = purchaseSessionID;

            // TODO: Complete member initialization
            this.EndUserID = EndUserID;
            var user = db.EndUsers.Find(EndUserID);
            if (user != null)
            {
                this.FirstName = user.Firstname;
                this.ID = user.ID;
                this.PaymentDate = DateTime.Now;
                this.UniquePaymentID = MiidWeb.Helpers.PaymentHelper.UniqueRefNo(user.Firstname, user.Surname);
                this.Surname = user.Surname;
                if (Purpose == "Ticket Purchase")
                {
                    //this.TotalAmountInRands = (int)pvModel.TicketClasses.Sum(x=>x.TotalCost);
                    this.TotalAmountInRands = (decimal)pvModel.TicketClasses.Where(x => x.TicketCount > 0).Sum(x => x.TotalCost);
                    this.UniquePaymentID = this.UniquePaymentID + "_TT";
                }
                else 
                { 
                this.TotalAmountInRands = 0;
                }

            }
        }

        public void SaveAsBankTransaction()
        {
            string ttype = "";
            string bankstatus = "";

            if (this.Purpose == "Ticket Purchase")
            {
                ttype = "Manual EFT Ticket Purchase";
                bankstatus = "Pending Ticket Purchase";
            }
            else
            {
                ttype = "Manual EFT Topup";
                bankstatus = "waiting on payment";
            }
            int ttypeid = Helpers.StatusHelper.TransactionTypeID(ttype);

            var db = new MiidEntities();
            var bankTran = new BankTransaction
            {
                Amount = this.TotalAmountInRands,
                Description = this.UniquePaymentID,
                Reference = this.UniquePaymentID,
                ConfirmationDate = null,
                EndUserID = this.EndUserID,
                ActualAmount = this.ActualAmount,
                DepositorName = String.Format("{0} {1}", this.FirstName, this.Surname),
                TransactionDate = DateTime.Now,
                UpdatedByUserName = "End User",
                TransactionTypeID = ttypeid,
                AdminFee = this.ConfiguredFeeAmount,
                StatusID = StatusHelper.StatusID("BankTransaction", bankstatus)//Daniel check when this is updated to confirmed
            };

            db.BankTransactions.Add(bankTran);
            db.SaveChanges();
            //Db Alert
            ExportCsvRepository.ExportCSVAndEmail("AreThereMoreTicketsThanBank", bankTran.Reference ?? "", "info@miid.co.za", String.Format("WARNING. duplicate transaction: {0}", bankTran.Reference ?? ""), "See attachment", String.Format("AreThereMoreTicketsThanBank_{0}.csv", DateTime.Now.ToString("yyyyMMddHHmmss")));

        }



    }
}