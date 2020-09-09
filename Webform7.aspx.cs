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
	public partial class WebForm7 : System.Web.UI.Page
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
				this.SearchCustomers();
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
					string sql = "SELECT ID, EventName FROM Event";
					if (!string.IsNullOrEmpty(txtSearch2.Text.Trim()))
					{
						sql += " WHERE EventName LIKE @EventName + '%'";
						cmd.Parameters.AddWithValue("@EventName", txtSearch2.Text.Trim());




					}

					cmd.CommandText = sql;
					cmd.Connection = con;
					using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
					{
						DataTable dt1 = new DataTable();
						sda.Fill(dt1);
						GridViewtickets.DataSource = dt1;
						GridViewtickets.DataBind();
						GridViewtickets.Attributes.Add("class", "table table-bordered table-striped");

						txtSearch2.Attributes.Add("class", "form-control");

					}
				}
			}



		}

		private void SearchCustomers()
		{
			string constr = ConfigRepo.Get("MiidConnectionString");
			using (SqlConnection con = new SqlConnection(constr))
			{
				using (SqlCommand cmd = new SqlCommand())
				{
					string sql = "SELECT TicketClass.EventID, Ticket.StatusID, TicketClass.Code, TicketClass.Description, Ticket.TicketClassID,  Ticket.TicketPurchasePrice,  Ticket.TicketNumber,   EndUser.Firstname, EndUser.Surname,  EndUser. Email, Ticket.EndUserID,  EndUser.Telephone  FROM ((Ticket INNER JOIN EndUser ON Ticket.EndUserID = EndUser.ID) INNER JOIN TicketClass ON Ticket.TicketClassID = TicketClass.ID)";
					if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
					{
						sql += " WHERE EventID LIKE @EventID + '%'";
						cmd.Parameters.AddWithValue("@EventID", txtSearch.Text.Trim());




					}

					cmd.CommandText = sql;
					cmd.Connection = con;
					using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
					{
						DataTable dt1 = new DataTable();
						sda.Fill(dt1);
						gvCustomers.DataSource = dt1;
						gvCustomers.DataBind();
						gvCustomers.Attributes.Add("class", "Table Table-stripped");
				
						txtSearch.Attributes.Add("class", "form-control");

					}
				}
			}



		}

		protected void Search(object sender, EventArgs e)
		{
			this.SearchCustomers();
			this.SearchEvent();
		}

		
		

		protected void OnPaging(object sender, GridViewPageEventArgs e)
		{
			GridViewtickets.PageIndex = e.NewPageIndex;
			this.SearchEvent();
		}










	}

}