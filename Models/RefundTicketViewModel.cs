using MiidWeb.Helpers;
using MiidWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiidWeb.Models
{


    public class RefundTicketViewModelLite
    {

        public int ID { get; set; }//TicketID
        public int EndUserID { get; set; }//EndUserID

        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }

        public String Country { get; set; }
        public String Bank { get; set; }
        public String BranchCode { get; set; }
        public String AccountNumber { get; set; }
        public String AccountType { get; set; }
        public String AccountHolderName { get; set; }
        public String Notes { get; set; }

        public decimal TicketPurchasePrice { get; set; }
        public string TicketClassName { get; set; }
        public string EventName { get; set; }

        public string RequestStatus  { get; set; }
        public string TicketStatus { get; set; }

        public Nullable<System.DateTime> DatetimePurchased { get; set; }
        public Nullable<System.DateTime> DatetimeReserved { get; set; }
        public Nullable<System.DateTime> DatetimeRedeemed { get; set; }
        public Nullable<System.DateTime> DateRefundPaidOut { get; set; }
        public Nullable<System.DateTime> DateRefundRequested { get; set; }

        public RefundTicketViewModelLite() { }

        public RefundTicketViewModelLite(int TicketID, int EndUserID) {


            this.ID = TicketID;
            this.EndUserID = EndUserID;

            var tv = new TicketViewModel(TicketID);
            var user = EndUserRepository.GetByUserID(EndUserID);

            this.AccountHolderName = user.AccountHolderName;
            this.AccountNumber = user.AccountNumber;
            this.AccountType = user.AccountTypeName;
            this.Notes = user.Notes;
            this.Bank = user.Bank;
            this.BranchCode = user.BranchCode;
            this.Country = user.Country;
            this.FirstName = user.Firstname;
            this.Surname = user.Surname;
            this.Email = user.Email;
            this.DateRefundPaidOut = tv.Ticket.DateRefundPaidOut;
            this.DateRefundRequested = tv.Ticket.DateRefundRequested;
            this.DatetimePurchased = tv.Ticket.DatetimePurchased;
            this.DatetimeReserved = tv.Ticket.DatetimeReserved;
            this.DatetimeRedeemed = tv.Ticket.DatetimeRedeemed;
            this.TicketClassName = tv.TicketClass.Description;
            this.EventName = tv.Event.EventName;
            this.TicketStatus = "Reserved";
            if (this.DatetimeRedeemed == null && this.DatetimePurchased != null)
            {
                this.TicketStatus = "Purchased";
            }
            if (this.DatetimeRedeemed != null && this.DatetimePurchased != null)
            {
                this.TicketStatus = "Used";
            }

        }

    }

    public class RefundTicketViewModel
    {

        public List<TicketViewModel> refundableTickets { get; set; }
        public List<TicketViewModel> requestedToRefundTickets { get; set; }

        public int ID { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public decimal? TotalTicketValue { get; set; }
        public decimal? AvailableTicketValue { get; set; }
        public decimal? AdminFee { get; set; }
        public decimal RequestedAmount { get; set; }
        public string IDNumber { get; set; }
        public int Password { get; set; }

        public int EndUserID { get; set; }

        public bool TermsAndConditions { get; set; }


        public String Country { get; set; }
        public String Bank { get; set; }
        public String BranchCode { get; set; }
        public String AccountNumber { get; set; }
        public String AccountType { get; set; }
        public String AccountHolderName { get; set; }
        public String Notes { get; set; }


        public RefundTicketViewModel()
        {

        }

        public RefundTicketViewModel(string UserName, List<TicketViewModel> requestedToRefundTickets)
        {

            this.EndUserID = EndUserRepository.GetByUserName(UserName).ID;

            using (var db = new MiidEntities())
            {


                EndUser user = db.EndUsers.Find(this.EndUserID);

                this.FirstName = user.Firstname;
                this.Surname = user.Surname;

                this.AdminFee = ConfiguredFeeHelper.FeeAmount("MiiFundsWithdrawal", 0.00M, int.Parse(GlobalVariables.SubdomainID));

               
                this.AvailableTicketValue = TicketRepository.GetRefundableTickets(UserName).Sum(x=>x.Ticket.TicketPurchasePrice);
                    
               

            }
        }



    }
}