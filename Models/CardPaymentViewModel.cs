using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MiidWeb.Helpers;
using MiidWeb.Repositories;

namespace MiidWeb.Models
{
    public class CardPaymentViewModel
    {

        private MiidEntities db = new MiidEntities();
      
        public int ID { get; set; }
        public int EndUserID { get; set; }
        public decimal? TotalAmountInRands { get; set; }
        public string ItemName { get; set; }
        public int ItemQuantity { get; set; }
        public int ItemPrice { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public int SecurityCode { get; set; }
        public string NameOnCard { get; set; }

        public string Email { get; set; }
                
        public string UniquePaymentID { get; set; }
        public DateTime PaymentDate { get; set; }

        public string Purpose { get; set; }
        public string PurchaseSessionID { get; set; }

        public PurchaseTicketViewModel PurchaseTicketViewModel { get; set; }
        
        public decimal AdminFee { get; set; }
        //check Ivery for mandatory fields

        public CardPaymentViewModel() 
        { }

        public CardPaymentViewModel(int EndUserID, string PayingFor, PurchaseTicketViewModel PurchaseTicketViewModel, string purchaseSessionID)
        {

            this.Purpose = PayingFor;
            this.PurchaseTicketViewModel = PurchaseTicketViewModel;
            this.PurchaseSessionID = purchaseSessionID;

            // TODO: Complete member initialization
            this.EndUserID = EndUserID;
            var user = db.EndUsers.Find(EndUserID);
            if (user != null)
            {
                this.NameOnCard = String.Format("{0} {1} ", user.Firstname, user.Surname);//ideally Title Initials Surname
                this.ID = user.ID;
                //this.PaymentDate = DateTime.Now;
                this.UniquePaymentID = MiidWeb.Helpers.PaymentHelper.UniqueRefNo(user.Firstname, user.Surname);
                this.ItemName = PayingFor;
                this.ItemQuantity = 1;
                this.Email = user.Email;

                if (PayingFor == "Ticket Purchase")
                {
               



                    this.TotalAmountInRands = (decimal)PurchaseTicketViewModel.TicketClasses.Sum(x => x.TotalCost);
                    this.AdminFee = Helpers.ConfiguredFeeHelper.FeeAmount("CardPayment", (decimal)this.TotalAmountInRands, int.Parse(GlobalVariables.SubdomainID));
                }
                else
                {
                    this.TotalAmountInRands = 0;
                }
                               
            }
        
        }
        public CardPaymentViewModel(int EndUserID, string PayingFor, int TopUpAmount, string purchaseSessionID)
        {

            this.Purpose = PayingFor;
            this.PurchaseTicketViewModel = PurchaseTicketViewModel;
            this.PurchaseSessionID = purchaseSessionID;

            // TODO: Complete member initialization
            this.EndUserID = EndUserID;
            var user = db.EndUsers.Find(EndUserID);
            if (user != null)
            {
                this.NameOnCard = String.Format("{0} {1} ", user.Firstname, user.Surname);//ideally Title Initials Surname
                this.ID = user.ID;
                //this.PaymentDate = DateTime.Now;
                this.UniquePaymentID = MiidWeb.Helpers.PaymentHelper.UniqueRefNo(user.Firstname, user.Surname);
                this.ItemName = PayingFor;
                this.ItemQuantity = 1;
                this.Email = user.Email;

                this.TotalAmountInRands = TopUpAmount;
                this.AdminFee = Helpers.ConfiguredFeeHelper.FeeAmount("CardPayment", (decimal)this.TotalAmountInRands, int.Parse(GlobalVariables.SubdomainID));



            }

        }


        public void SaveAsBankTransaction(string pgReference=null)
        {
            string ttype = "";

            if (this.Purpose == "Ticket Purchase")
            {
                ttype = "Card Payment Ticket Purchase";
            }
            else
            {
                ttype = "Card Payment Topup";
            }

            int ttypeid = Helpers.StatusHelper.TransactionTypeID(ttype);

            var db = new MiidEntities();
            var bankTran = new BankTransaction
            {
                Amount = this.TotalAmountInRands,
                Description = pgReference ?? this.UniquePaymentID,
                Reference = this.UniquePaymentID,
                ConfirmationDate = DateTime.Now,
                EndUserID = this.EndUserID,
                DepositorName = String.Format("{0}", this.NameOnCard),
                TransactionDate = DateTime.Now,
                UpdatedByUserName = "End User",
                TransactionTypeID = ttypeid,
                AdminFee = this.AdminFee,
                StatusID = StatusHelper.StatusID("BankTransaction", "waiting on payment")//Check when this is updated?
            };

            db.BankTransactions.Add(bankTran);
            db.SaveChanges();
            //Db Alert
            ExportCsvRepository.ExportCSVAndEmail("AreThereMoreTicketsThanBank", bankTran.Reference ?? "", "info@miid.co.za", String.Format("WARNING. duplicate transaction: {0}", bankTran.Reference ?? ""), "See attachment", String.Format("AreThereMoreTicketsThanBank_{0}.csv", DateTime.Now.ToString("yyyyMMddHHmmss")));

        }

    }
}