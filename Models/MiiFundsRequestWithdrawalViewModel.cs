using MiidWeb.Helpers;
using MiidWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiidWeb.Models
{
    public class MiiFundsRequestWithdrawalViewModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public decimal? TotalFunds { get; set; }
        public decimal? AvailableFunds { get; set; }
        public decimal? AdminFee { get; set; }
        public decimal RequestedAmount { get; set; }
        public string IDNumber { get; set; }
        public int Password { get; set; }

        public int EndUserID { get; set; }

        public bool TermsAndConditions { get; set; }


        public String Country{ get; set; }
        public String Bank{ get; set; }
        public String BranchCode{ get; set; }
        public String AccountNumber{ get; set; }
        public String AccountType{ get; set; }
        public String AccountHolderName{ get; set; }
        public String Notes{ get; set; }


        public MiiFundsRequestWithdrawalViewModel()
        {

        }

        public MiiFundsRequestWithdrawalViewModel(int userid)
        {

            this.EndUserID = userid;

            using(var db = new MiidEntities())
            {


                EndUser user = db.EndUsers.Find(userid);

                this.FirstName = user.Firstname;
                this.Surname = user.Surname;

                this.AdminFee = ConfiguredFeeHelper.FeeAmount("MiiFundsWithdrawal", 0.00M, int.Parse(GlobalVariables.SubdomainID));
                
                var moneys = db.MyMoneys.Where(x => x.EndUserID == user.ID).OrderByDescending(x => x.ID);

                if (moneys != null && moneys.Count() > 0)
                {
                    this.AvailableFunds = MyMoneyRepository.MyAvailableFunds(user.ID) -this.AdminFee;
                    this.TotalFunds = MyMoneyRepository.MyAvailableFunds(user.ID);
                
                }

            }
        }

       

    }
}