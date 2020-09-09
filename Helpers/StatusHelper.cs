using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiidWeb.Helpers
{
    public class StatusHelper
    {
        public static int StatusID(string Context, string Status)
        {

            int result = 0;

            MiidEntities db = new MiidEntities();

            var status = db.Status.Where(u => u.Context.ToLower() == Context.ToLower() && u.Code.ToLower() == Status).FirstOrDefault();

            if (status != null)
            {
                result = status.ID;
            }


            return result;
        }


        public static int TransactionTypeID(string TranTypeCode)
        {

            int result = 0;

            MiidEntities db = new MiidEntities();

            var status = db.TransactionTypes.Where(u => u.TranTypeCode.ToLower() == TranTypeCode.ToLower()).FirstOrDefault();

            if (status != null)
            {
                result = status.ID;
            }


            return result;
        }

        public static string TransactionTypeCode(int TransactionTypeID)
        {

            string result = "";

            MiidEntities db = new MiidEntities();

            var status = db.TransactionTypes.Find(TransactionTypeID);

            if (status != null)
            {
                result = status.TranTypeCode;
            }


            return result;
        }
    }
}