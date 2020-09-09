using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiidWeb.Helpers
{
    public static class PaymentHelper
    {

        public static string UniqueRefNo (string FirstName, string Surname, string PayingFor = null)
        {

            if(FirstName.Length<3)
            {
                FirstName = FirstName + "000";
            }

            if(Surname.Length<3)
            {
                Surname = Surname + "000";
            }

            MiidEntities db = new MiidEntities();

            string output = String.Format("{0}{1}{2}",
                    Surname.ToUpper().Substring(0, 3),
                    FirstName.ToUpper().Substring(0, 3),
                    System.Guid.NewGuid().ToString().ToUpper().Substring(0, 6)); 

            //check if by some fluke the unique no already exists - put an index on this column please
            while (db.BankTransactions.Where(x => x.Description.ToUpper() == output).Count() > 0)
            {
                output= String.Format("{0}{1}{2}",
                    Surname.ToUpper().Substring(0, 3),
                    FirstName.ToUpper().Substring(0, 3),
                    System.Guid.NewGuid().ToString().ToUpper().Substring(0, 6));

            }

            if (PayingFor != null && PayingFor == "Ticket Purchase")
            {
                return output + "_TT";
            }

            return output;

        }



    }
}