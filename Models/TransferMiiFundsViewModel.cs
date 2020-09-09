using MiidWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MiidWeb.Repositories;
using System.Web.Mvc;

namespace MiidWeb.Models
{

    public class LinkedAccount
    {
        public int ChildUserID { get; set; }
        public decimal? AvailableFunds { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
    }

    public class TransferMiiFundsViewModel
    {
        public List<LinkedAccount> LinkedAccounts { get; set; }
        public List<SelectListItem> FromAccounts { get; set; }
        public List<SelectListItem> ToAccounts { get; set; }


        public int FromAccountID { get; set; }
        public int ToAccountID { get; set; }

       
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public decimal? TotalFunds { get; set; }
        public decimal? AvailableFunds { get; set; }

        public decimal TransferAmount { get; set; }
      
        public int EndUserID { get; set; }

      
        public TransferMiiFundsViewModel()
        {

        }

        public TransferMiiFundsViewModel(int userid)
        {

            this.LinkedAccounts = new List<LinkedAccount>();
            this.FromAccounts = new List<SelectListItem>();
            this.ToAccounts = new List<SelectListItem>();


            this.EndUserID = userid;

            using (var db = new MiidEntities())
            {


                EndUser user = db.EndUsers.Find(userid);

                EndUserViewModel parent = new EndUserViewModel(user);

                this.FromAccounts.Add(new SelectListItem() { Value = user.ID.ToString(), Text = String.Format("{0} {1} - R{2}", user.Firstname, user.Surname, MyMoneyRepository.MyAvailableFunds(user.ID)) });
                this.ToAccounts.Add(new SelectListItem() { Value = user.ID.ToString(), Text = String.Format("{0} {1} - R{2}", user.Firstname, user.Surname, MyMoneyRepository.MyAvailableFunds(user.ID)) });


                foreach (var child in parent.MyChildren)
                {
                    this.LinkedAccounts.Add(new LinkedAccount
                    {
                        ChildUserID = child.EndUser.ID,
                        FirstName = child.EndUser.Firstname,
                        Surname = child.EndUser.Surname,
                        AvailableFunds = Repositories.MyMoneyRepository.MyAvailableFunds(child.EndUser.ID),

                    });

                    this.FromAccounts.Add(new SelectListItem() { Value = child.EndUser.ID.ToString(), Text = String.Format("{0} {1} - R{2}", child.EndUser.Firstname, child.EndUser.Surname, MyMoneyRepository.MyAvailableFunds(child.EndUser.ID)) });
                    this.ToAccounts.Add(new SelectListItem() { Value = child.EndUser.ID.ToString(), Text = String.Format("{0} {1} - R{2}", child.EndUser.Firstname, child.EndUser.Surname, MyMoneyRepository.MyAvailableFunds(child.EndUser.ID)) });

                }






                this.FirstName = user.Firstname;
                this.Surname = user.Surname;


                this.AvailableFunds = MyMoneyRepository.MyAvailableFunds(this.EndUserID);


            }
        }



    }
}