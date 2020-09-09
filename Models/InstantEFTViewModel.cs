using MiidWeb.Helpers;
using MiidWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MiidWeb.Models
{
  public class InstantEFTViewModel
  {

    private MiidEntities db = new MiidEntities();


    public int ID { get; set; }

    public int EndUserID { get; set; }
    public string BuyerFirstName { get; set; }
    public string BuyerLastName { get; set; }
    public string BuyerEmail { get; set; }
    public string ItemName { get; set; }
    public decimal TotalAmountInRands { get; set; }

        public decimal ActualAmount { get; set; }

        public decimal TotalMinusFee { get; set; }

    
    public decimal ConfiguredFeeAmount { get; set; }
    public int CustomInteger { get; set; }
    public string CustomString { get; set; }
    public string UniquePaymentID { get; set; }
    public DateTime PaymentDate { get; set; }

    public string Purpose { get; set; }
    public string PurchaseSessionID { get; set; }

    public PurchaseTicketViewModel PurchaseTicketViewModel { get; set; }
    public InstantEFTViewModel()
    { }

    public InstantEFTViewModel(int EndUserID, string PayingFor, PurchaseTicketViewModel PurchaseTicketViewModel, string purchaseSessionID)
    {

      this.Purpose = PayingFor;
      this.PurchaseTicketViewModel = PurchaseTicketViewModel;
      this.PurchaseSessionID = purchaseSessionID;

      // TODO: Complete member initialization
      this.EndUserID = EndUserID;
      var user = db.EndUsers.Find(EndUserID);
      if (user != null)
      {
        this.BuyerFirstName = user.Firstname;
        this.ID = user.ID;
        this.PaymentDate = DateTime.Now;
        this.UniquePaymentID = MiidWeb.Helpers.PaymentHelper.UniqueRefNo(user.Firstname, user.Surname, PayingFor);
        this.BuyerLastName = user.Surname;
        this.BuyerEmail = user.Email;
        this.ItemName = PayingFor;

        if (PayingFor == "Ticket Purchase")
        {
          this.TotalAmountInRands = (decimal)PurchaseTicketViewModel.TicketClasses.Where(x=>x.TicketCount>0).Sum(x => x.TotalCost);
         
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

      if (this.Purpose == "Ticket Purchase")
      {
        ttype = "Instant EFT Ticket Purchase";
      }
      else
      {
        ttype = "Instant EFT Topup";
      }

      int ttypeid = Helpers.StatusHelper.TransactionTypeID(ttype);

      var db = new MiidEntities();
            var bankTran = new BankTransaction
            {
                Amount = this.TotalAmountInRands,
                Description = this.UniquePaymentID,
                Reference = this.UniquePaymentID,
                ConfirmationDate = DateTime.Now,
                EndUserID = this.EndUserID,
                DepositorName = String.Format("{0} {1}", this.BuyerFirstName, this.BuyerLastName),
                TransactionDate = DateTime.Now,
                UpdatedByUserName = "End User",
                TransactionTypeID = ttypeid,
                StatusID = StatusHelper.StatusID("BankTransaction", "waiting on payment"),
                AdminFee = this.ConfiguredFeeAmount,
                ActualAmount = this.ActualAmount,

      };

      db.BankTransactions.Add(bankTran);
      db.SaveChanges();
            //Db Alert
            ExportCsvRepository.ExportCSVAndEmail("AreThereMoreTicketsThanBank", bankTran.Reference ?? "", "info@miid.co.za", String.Format("WARNING. duplicate transaction: {0}", bankTran.Reference ?? ""), "See attachment", String.Format("AreThereMoreTicketsThanBank_{0}.csv", DateTime.Now.ToString("yyyyMMddHHmmss")));

        }
    }
}