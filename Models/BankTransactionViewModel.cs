using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiidWeb.Models
{
    public class BankTransactionViewModel
    {

        public int ID { get; set; }

        private MiidEntities db = new MiidEntities();
        public BankTransaction BankTransaction { get; set; }
        public Status Status { get; set; }

        public TransactionType TransactionType { get; set; }

        public EndUser EndUser { get; set; }

        public bool Approve { get; set; }

        public BankTransactionViewModel()
        { 
        
        
        
        }

        public BankTransactionViewModel(BankTransaction BankTransaction)
        {
            this.BankTransaction = BankTransaction;
            this.ID = BankTransaction.ID;
            this.EndUser = db.EndUsers.Find(this.BankTransaction.EndUserID);
            this.Status = db.Status.Find(this.BankTransaction.StatusID);
            this.TransactionType = db.TransactionTypes.Find(this.BankTransaction.TransactionTypeID);

        }
    }
}