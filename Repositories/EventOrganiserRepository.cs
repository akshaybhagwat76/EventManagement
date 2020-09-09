using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MiidWeb;
using MiidWeb.Models;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;


namespace MiidWeb.Repositories
{
	public static class EventOrganiserRepository
	{


        public static string GetCurrentEventsSp(string UserName, string Password)
        {
            StringBuilder result = new StringBuilder();

            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("GetCurrentEventsSp", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };

            sqlDataAdapter.SelectCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = UserName;
            sqlDataAdapter.SelectCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = Password;


            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];
            if (item != null)
            {
                foreach (DataRow row in item.Rows)
                {
                    result.Append(row[0].ToString());
                    result.Append("|");
                }
            }
            sqlConnection.Close();

            return result.ToString();
        }

        public static List<EOTicketReport> GetEOTicketReport(int EventID = 0, DateTime? FromDate = null, DateTime? ToDate = null, string FirstName = null, string Surname = null, string IDNumber = null, string TicketNumber = null, DateTime? ticketstartdate = null)
		{
			List<EOTicketReport> myEvents = new List<EOTicketReport>();
			SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
			sqlConnection.Open();
			DataSet dataSet = new DataSet();
			SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
			{
				SelectCommand = new SqlCommand("EOTicketReport", sqlConnection)
				{
					CommandType = CommandType.StoredProcedure
				}
			};
			sqlDataAdapter.SelectCommand.Parameters.Add("@EventID", SqlDbType.Int).Value = EventID;
			sqlDataAdapter.SelectCommand.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = FromDate;
			sqlDataAdapter.SelectCommand.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate;
			sqlDataAdapter.SelectCommand.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = FirstName;
			sqlDataAdapter.SelectCommand.Parameters.Add("@Surname", SqlDbType.VarChar).Value = Surname;
			sqlDataAdapter.SelectCommand.Parameters.Add("@IDNumber", SqlDbType.VarChar).Value = IDNumber;
			sqlDataAdapter.SelectCommand.Parameters.Add("@TicketNumber", SqlDbType.VarChar).Value = TicketNumber;
			sqlDataAdapter.SelectCommand.Parameters.Add("@TicketStartDate", SqlDbType.DateTime).Value = ticketstartdate;



			sqlDataAdapter.Fill(dataSet, "dsResult");
			DataTable item = dataSet.Tables["dsResult"];
			if (item != null)
			{
				foreach (DataRow row in item.Rows)
				{

					EOTicketReport EOTicketReport = new EOTicketReport()
					{

						TicketID = int.Parse(row["TicketID"].ToString()),
						FirstName = row["FirstName"].ToString(),
						Surname = row["Surname"].ToString(),
						IDNumber = row["IDNumber"].ToString(),
						Email = row["Email"].ToString(),
						Cell = row["Cell"].ToString(),
						TicketClassName = row["TicketClassName"].ToString(),
						TicketNumber = row["TicketNumber"].ToString(),
						DatetimePurchased = DateTime.Parse(row["DatetimePurchased"].ToString()),
						TicketStatus = row["TicketStatus"].ToString(),
						TicketStartDate = DateTime.Parse(row["TicketStartDate"].ToString()),
						TicketEndDate = DateTime.Parse(row["TicketEndDate"].ToString()),



					};
					myEvents.Add(EOTicketReport);
				}
			}
			sqlConnection.Close();
			return myEvents;
		}

		public static List<EORemarketingReport> GetRemarketinReport(int EventID = 0, DateTime? FromDate = null, DateTime? ToDate = null, string FirstName = null, string Surname = null, string IDNumber = null, string TicketNumber = null, DateTime? ticketstartdate = null)
		{
			List<EORemarketingReport> myEvents = new List<EORemarketingReport>();
			SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
			sqlConnection.Open();
			DataSet dataSet = new DataSet();
			SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
			{
				SelectCommand = new SqlCommand("EORemarketingReport", sqlConnection)
				{
					CommandType = CommandType.StoredProcedure
				}
			};
			sqlDataAdapter.SelectCommand.Parameters.Add("@EventID", SqlDbType.Int).Value = EventID;
			sqlDataAdapter.SelectCommand.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = FromDate;
			sqlDataAdapter.SelectCommand.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate;
			sqlDataAdapter.SelectCommand.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = FirstName;
			sqlDataAdapter.SelectCommand.Parameters.Add("@Surname", SqlDbType.VarChar).Value = Surname;
			sqlDataAdapter.SelectCommand.Parameters.Add("@IDNumber", SqlDbType.VarChar).Value = IDNumber;
			sqlDataAdapter.SelectCommand.Parameters.Add("@TicketNumber", SqlDbType.VarChar).Value = TicketNumber;
			sqlDataAdapter.SelectCommand.Parameters.Add("@TicketStartDate", SqlDbType.DateTime).Value = ticketstartdate;



			sqlDataAdapter.Fill(dataSet, "dsResult");
			DataTable item = dataSet.Tables["dsResult"];
			if (item != null)
			{
				foreach (DataRow row in item.Rows)
				{

					EORemarketingReport EORemarketingReport = new EORemarketingReport()
					{

						TicketID = int.Parse(row["TicketID"].ToString()),
						FirstName = row["FirstName"].ToString(),
						Surname = row["Surname"].ToString(),
						IDNumber = row["IDNumber"].ToString(),
						Email = row["Email"].ToString(),
						Cell = row["Cell"].ToString(),
						TicketClassName = row["TicketClassName"].ToString(),
						TicketNumber = row["TicketNumber"].ToString(),
						
						TicketStatus = row["TicketStatus"].ToString(),
						TicketStartDate = DateTime.Parse(row["TicketStartDate"].ToString()),
						TicketEndDate = DateTime.Parse(row["TicketEndDate"].ToString()),



					};
					myEvents.Add(EORemarketingReport);
				}
			}
			sqlConnection.Close();
			return myEvents;
		}

		public static void CreateLoginForEventOrganiser(string Email)//, string NewStatus = "Active")
		{
			List<MyEventItem> myEvents = new List<MyEventItem>();
			SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
			sqlConnection.Open();
			DataSet dataSet = new DataSet();

			var command = new SqlCommand("CreateLoginForEventOrganiser", sqlConnection)
			{
				CommandType = CommandType.StoredProcedure
			};

			command.Parameters.Add("@Email", SqlDbType.VarChar).Value = Email;


			command.ExecuteNonQuery();


			sqlConnection.Close();

			//using (var db = new MiidEntities())
			//{
			//    var endUser = new EndUser();
			//    endUser.Email = Email;
			//    endUser.StatusID = Helpers.StatusHelper.StatusID("EO", NewStatus);
			//    db.EndUsers.Add(endUser);
			//    db.SaveChanges();
			//}
			return;
		}


		public static EventOrganiser GetEventOrganiserByEmail(string Email)
		{
			MiidEntities db = new MiidEntities();

			var eos = db.EventOrganisers.Where(x => x.Email == Email);

			if (eos.Count() > 0)
			{

				return eos.First();
			}

			return null;

		}

		public static List<EOEventSummaryItem> GetMyEventSummaries(int EventOrganiserID)
		{
			List<EOEventSummaryItem> myEvents = new List<EOEventSummaryItem>();
			SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
			sqlConnection.Open();
			DataSet dataSet = new DataSet();
			SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
			{
				SelectCommand = new SqlCommand("EventSummaryByEventOrganiser", sqlConnection)
				{
					CommandType = CommandType.StoredProcedure
				}
			};
			sqlDataAdapter.SelectCommand.Parameters.Add("@EventOrganiserID", SqlDbType.Int).Value = EventOrganiserID;


			sqlDataAdapter.Fill(dataSet, "dsResult");
			DataTable item = dataSet.Tables["dsResult"];
			if (item != null)
			{
				foreach (DataRow row in item.Rows)
				{
					DateTime start;

					if (DateTime.TryParse(row["StartDateTime"].ToString(), out start))
					{
						start = start;
					}


					EOEventSummaryItem MyEventsViewModel = new EOEventSummaryItem()
					{


						EventID = int.Parse(row["EventID"].ToString()),
						DaysTillEvent = int.Parse(row["DaysTillEvent"].ToString()),
						StartDateTime = start,
						TicketCount = int.Parse(row["TicketCount"].ToString()),
						EventName = row["EventName"].ToString(),
						TicketClassName = row["EventName"].ToString(),
						TicketValue = decimal.Parse(row["TicketValue"].ToString())


					};
					myEvents.Add(MyEventsViewModel);
				}
			}
			sqlConnection.Close();
			return myEvents;
		}

	}

}
	


    
