using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MiidWeb.Repositories;

namespace MiidWeb
{
	public partial class WebForm8 : System.Web.UI.Page
	{




		public string con = "";


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

			if (!this.IsPostBack)
			{
				
				this.SearchEvent();
			}
		}

		private void SearchEvent()
		{
			string constr = ConfigRepo.Get("MiidConnectionString");
			using (SqlConnection con = new SqlConnection(constr))
			{
				using (SqlCommand cmd = new SqlCommand())
				{
					string sql = "select * from PayFastTransactions p join vwTickets t on t.UniquePaymentID = p.MPaymentID Where StatusID = '5008' ORDER BY Date DESC";
					

					cmd.CommandText = sql;
					cmd.Connection = con;
					using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
					{
						DataTable dt1 = new DataTable();
						sda.Fill(dt1);
						GridViewtickets.DataSource = dt1;
						GridViewtickets.DataBind();
						GridViewtickets.Attributes.Add("class", "table table-bordered table-striped");

						
					}
				}
			}



		}

		
		protected void Search(object sender, EventArgs e)
		{
		
			this.SearchEvent();
		}




		protected void OnPaging(object sender, GridViewPageEventArgs e)
		{
			GridViewtickets.PageIndex = e.NewPageIndex;
			this.SearchEvent();
		}










	}

}