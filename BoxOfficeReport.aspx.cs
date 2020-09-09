using MiidWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiidWeb
{
    public partial class BoxOfficeReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int EventID = int.Parse(Request.QueryString["EI"]);
            string PosID = Request.QueryString["PI"];
            string FromDate = Request.QueryString["FD"];
            string ToDate = Request.QueryString["TD"];
        


            DateTime FromDate1;
            DateTime.TryParse(FromDate, out FromDate1);
            DateTime ToDate1;
            DateTime.TryParse(ToDate, out ToDate1);
            

            GridView1.DataSource = DataDumpRepository.GetDataDump("BoxOfficeReport", PosID, EventID, FromDate1, ToDate1);
            GridView1.DataBind();
        }
    }
}