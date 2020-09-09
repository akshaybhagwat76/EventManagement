using MiidWeb.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiidWeb
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {

                Response.Redirect("Home");
            }
            if (!User.IsInRole("Admin"))
            {
                Response.Redirect("Home");
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ReportApi repo = new ReportApi();
            int ticketId = int.Parse(TextBox1.Text);

            string serverPath = "https://miid.co.za";
            string drivepath = @"c:\inetpub\wwwrooot\training.miid.co.za";
            try
            {
                Label2.Text = repo.PopulateTicketReport(ticketId, serverPath, drivepath, subdomain: "miid");
            }
            catch (Exception e2)
            {
                Label2.Text = e2.Message;
            }
           
        }
    }
}