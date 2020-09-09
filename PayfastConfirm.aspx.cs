using MiidWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiidWeb
{
    public partial class PayfastConfirm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            try { 
            if (!IsPostBack)
            { 
             string purpose = Request.QueryString["pps"];

             if (purpose == "ticket")
             {
                 pnlTicketPurchase.Visible = true;
             }
             else
             {
                 pnlMiiFundsTopup.Visible = true;
             }
            
            
            }
            }

            catch (Exception gg)
            {
                LogHelper.Log("Payfast Confirm", gg.ToString());

            }
        }
    }
}