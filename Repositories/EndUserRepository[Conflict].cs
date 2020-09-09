using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using MiidWeb.Models;


namespace MiidWeb.Repositories
{
    public static class EndUserRepository
    {



        public static List<CalendarItem> GetMyEvents(int EndUserID, DateTime Date)
        {
            List<CalendarItem> myEvents = new List<CalendarItem>();
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MiidConnectionString"].ConnectionString);
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("GetMyEvents", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };
            sqlDataAdapter.SelectCommand.Parameters.Add("@EndUserID", SqlDbType.Int).Value = EndUserID;
            sqlDataAdapter.SelectCommand.Parameters.Add("@Date", SqlDbType.DateTime).Value = Date;
            
            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];
            if (item != null)
            {
                foreach (DataRow row in item.Rows)
                {
                    CalendarItem ci = new CalendarItem();
                    
                        ci.DayNo = row["DayNo"].ToString();
                        ci.CalendarDate = DateTime.Parse(row["CalendarDate"].ToString());
                        ci.EventID = int.Parse(row["EventID"].ToString());
                        ci.ShortDescription = row["ShortDescription"].ToString();
                        DateTime sdt = new DateTime (); 
                        if(DateTime.TryParse(row["StartDateTime"].ToString(), out sdt))
                        {
                            ci.StartDateTime = sdt;
                        }
                        DateTime edt = new DateTime (); 
                        if(DateTime.TryParse(row["EndDateTime"].ToString(), out edt))
                        {
                            ci.EndDateTime = edt;
                        }
                        
                        ci.TicketCount = int.Parse(row["TicketCount"].ToString());
                        ci.ImageURL = row["ImageURL"].ToString();
                        ci.ImageAltText = row["ImageAltText"].ToString();

                    
                    myEvents.Add(ci);
                }
            }
            sqlConnection.Close();
            return myEvents;
        }
	

    }
}