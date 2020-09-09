using MiidWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiidWeb
{
    public partial class PromoCodeReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int EventID = int.Parse(Request.QueryString["EI"]);
           
            

            GridView1.DataSource = DataDumpRepository.GetDataDumpPromoCodeReport("ReportPromoCodes", EventID);
            GridView1.DataBind();
        }
    }
}