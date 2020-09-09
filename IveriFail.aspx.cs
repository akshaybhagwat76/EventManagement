using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace MiidWeb
{
    public partial class IveriFail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string status;
            string description;
            string transactionType;
            string amount;
            string authorisationCode;
            string merchantReference;

            status = Request.Form["LITE_PAYMENT_CARD_STATUS"].ToString();
            description = Request.Form["LITE_RESULT_DESCRIPTION"].ToString();
            transactionType = Request.Form["LITE_AUTHORISATION"].ToString();
            if (transactionType == "TRUE")
            {
                transactionType = "Authorisation";
                
            }
            else
            {
                transactionType = "Sale";
                
            }

            amount = Request.Form["LITE_ORDER_AMOUNT"].ToString();
            authorisationCode = Request.Form["LITE_ORDER_AUTHORISATIONCODE"].ToString();
            merchantReference = Request.Form["ECOM_CONSUMERORDERID"].ToString();

            string text = String.Format(@"Iveri Fail Page Loaded {0} - Status {1} Description - {2} transaction type - {3} - reference {4}
            ", DateTime.Now.ToString(), status, description, transactionType, merchantReference);
            string path = Server.MapPath("~/Uploads");
            System.IO.File.AppendAllText(Path.Combine(path, "IveriLog.txt"), text);

        }
    }
}