using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiidWeb.Helpers
{
    public class LookupHelper
    {

        public static Subdomain GetSubdomainFromEventID(int EventID)
        {
            using (MiidEntities db = new MiidEntities(true))
            {
               var event1 =  db.Events.Find(EventID);
                var subdomain = db.Subdomains.Find(event1.SubdomainID);

                return subdomain;
            }
            
        }

        public static int GetSubdomainID(string SubdomainName)
        {

            int result = 0;

            MiidEntities db = new MiidEntities(true);

           SubdomainName = SubdomainName.Replace("www.", "");

            var Subdomain = db.Subdomains.Where(u => u.SubdomainName.ToLower() == SubdomainName.ToLower()).FirstOrDefault();

            if (Subdomain != null)
            {
                result = Subdomain.ID;
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