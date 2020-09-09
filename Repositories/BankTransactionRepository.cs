using MiidWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MiidWeb.Repositories
{
    public static class BankTransactionRepository
    {

        public static BankTransaction UpdateBankTransactionStatus(string Status, string UniquePaymentID, string UpdatedByUserName)
        {

            try
            {

                int NewStatusID = StatusHelper.StatusID("BankTransaction", Status);

                var db = new MiidEntities();


                var bankTrans = db.BankTransactions.Where(x => x.Description == UniquePaymentID || x.Reference == UniquePaymentID);

                if (bankTrans != null && bankTrans.Count() > 0)
                {
                    var bankTran = bankTrans.First();
                    bankTran.StatusID = NewStatusID;
                    bankTran.UpdatedByUserName = UpdatedByUserName;// "Payfast System Response";

                    if (Status == "Approved" || Status == "Approved Ticket Purchase")
                    {
                        bankTran.ConfirmationDate = DateTime.Now;
                    }

                    db.Entry(bankTran).State = EntityState.Modified;

                    db.SaveChanges();
                    return bankTran;
                }

                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public static BankTransaction GetByUniquePaymentID(string UniquePaymentID)
        {
            using (var db = new MiidEntities())
            {

                var bts = db.BankTransactions.Include(x=>x.EndUser).Where(x => x.Reference == UniquePaymentID);
                if (bts != null && bts.Count() > 0)
                {
                    return bts.First();

                }
                else
                {
                    return null;
                }
            
            }
        }

        public static BankTransaction GetByPaygenius(string Description)
        {
            using (var db = new MiidEntities())
            {

                var bts = db.BankTransactions.Include(x => x.EndUser).Where(x => x.Description == Description);
                if (bts != null && bts.Count() > 0)
                {
                    return bts.First();

                }
                else
                {
                    return null;
                }

            }
        }


        public static bool DoesUniquePaymentIDExist(string uniquePaymentID)
        {
            return GetByUniquePaymentID(uniquePaymentID) != null;
        }
    }
}