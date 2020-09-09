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
    public static class EndUserRepository
    {

       


        public static List<MyEventItem> GetMyEventsOnly(int EndUserID, DateTime Date, int PlusMonths)
        {
            List<MyEventItem> myEvents = new List<MyEventItem>();
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MiidConnectionString"].ConnectionString);
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("GetMyEventsOnly", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };
            sqlDataAdapter.SelectCommand.Parameters.Add("@EndUserID", SqlDbType.Int).Value = EndUserID;
            sqlDataAdapter.SelectCommand.Parameters.Add("@Date", SqlDbType.DateTime).Value = Date;
            sqlDataAdapter.SelectCommand.Parameters.Add("@PlusMonths", SqlDbType.Int).Value = PlusMonths;
            
            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];
            if (item != null)
            {
                foreach (DataRow row in item.Rows)
                {
                    DateTime start; DateTime end;
                    DateTime? start1 = null; DateTime? end1 = null;
                    if (DateTime.TryParse(row["StartDateTime"].ToString(), out start))
                    {
                        start = start;
                    }
                    if (DateTime.TryParse(row["EndDateTime"].ToString(), out end))
                    {
                        end = end;
                    }

                    MyEventItem MyEventsViewModel = new MyEventItem()
                    {
                        
                        
                        EventID = int.Parse(row["EventID"].ToString()),
                        ShortDescription = row["ShortDescription"].ToString(),
                        StartDateTime = start,
                        EndDateTime = end,
                        TicketCount = int.Parse(row["TicketCount"].ToString()),
                        ImageURL = row["ImageURL"].ToString(),
                        ImageAltText = row["ImageAltText"].ToString(),
                        EventName = row["EventName"].ToString(),
                        LongDescription = row["LongDescription"].ToString()

                    };
                    myEvents.Add(MyEventsViewModel);
                }
            }
            sqlConnection.Close();
            return myEvents;
        }
	


        public static List<CalendarItem> GetMyEvents(int EndUserID, DateTime Date, int PlusMonths )
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
            sqlDataAdapter.SelectCommand.Parameters.Add("@PlusMonths", SqlDbType.Int).Value = PlusMonths;
            
            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];
            if (item != null)
            {
                foreach (DataRow row in item.Rows)
                {
                    DateTime start; DateTime end;
                    DateTime? start1 = null; DateTime? end1 = null;
                    if (DateTime.TryParse(row["StartDateTime"].ToString(), out start))
                    {
                        start1 = start;
                    }
                    if (DateTime.TryParse(row["EndDateTime"].ToString(), out end))
                    {
                        end1 = end;
                    }

                    CalendarItem MyEventsViewModel = new CalendarItem()
                    {
                        DayNo = row["DayNo"].ToString(),
                        CalendarDate = DateTime.Parse(row["CalendarDate"].ToString()),
                        EventID = int.Parse(row["EventID"].ToString()),
                        ShortDescription = row["ShortDescription"].ToString(),
                        StartDateTime = start1,
                        EndDateTime = end1,
                        TicketCount = int.Parse(row["TicketCount"].ToString()),
                        ImageURL = row["ImageURL"].ToString(),
                        ImageAltText = row["ImageAltText"].ToString(),
                        EventName = row["EventName"].ToString(),
                        LongDescription = row["LongDescription"].ToString()

                    };
                    myEvents.Add(MyEventsViewModel);
                }
            }
            sqlConnection.Close();
            return myEvents;
        }


        public static List<FriendViewModel> GetMyFriends(int EndUserID)
        {
            MiidEntities db = new MiidEntities();

            List<FriendViewModel> myFriends = new List<FriendViewModel>();

            var friendsInitiating = db.Friends.Where(x => x.InitiatingUserID == EndUserID && x.DateAccepted != null).ToList();

            var friendsAccepting = db.Friends.Where(x =>x.AcceptingUserID == EndUserID && x.DateAccepted != null).ToList();

            foreach (var friend in friendsInitiating)
            {
                var friendViewModel = new FriendViewModel();
                friendViewModel.Friend = db.EndUsers.Where(x => x.ID == friend.AcceptingUserID).First();
                friendViewModel.Friendship = friend;
                myFriends.Add(friendViewModel);
            }
            foreach (var friend in friendsAccepting)
            {
                var friendViewModel = new FriendViewModel();
                friendViewModel.Friend = db.EndUsers.Where(x => x.ID == friend.InitiatingUserID).First();
                friendViewModel.Friendship = friend;
                myFriends.Add(friendViewModel);
            }

            return myFriends;
        }


        public static Controllers.UserModel GetUser(string p)
        {
            MiidEntities db = new MiidEntities();

            EndUser u = db.EndUsers.Find(int.Parse(p));

            if (u != null)
            {
                Controllers.UserModel um = new Controllers.UserModel();
                um.FirstName = u.Firstname;
                um.LastName = u.Surname;

                return um;
            }
            else
                return null;
        }


        public static bool IsUserUpToDate(int EndUserID)
        {
            MiidEntities db = new MiidEntities();

            bool result = false;
            int NullFieldCount = 0;

            var user = db.EndUsers.Find(EndUserID);

            if (user != null)
            {
                if (String.IsNullOrEmpty(user.Cell)) { NullFieldCount++; };
                if (String.IsNullOrEmpty(user.City)) { NullFieldCount++; };
                if (user.DateOfBirth == null) { NullFieldCount++; };
                if (String.IsNullOrEmpty(user.IDNumber)) { NullFieldCount++; };
                if (String.IsNullOrEmpty(user.KnownConditions)) { NullFieldCount++; };
                if (String.IsNullOrEmpty(user.MedicalAidCompany)) { NullFieldCount++; };
                if (String.IsNullOrEmpty(user.MedicalAidNumber)) { NullFieldCount++; };
                if (String.IsNullOrEmpty(user.Medication)) { NullFieldCount++; };
                if (String.IsNullOrEmpty(user.NextOfKin)) { NullFieldCount++; };
                if (String.IsNullOrEmpty(user.NextOfKinTelephone)) { NullFieldCount++; };
                if (String.IsNullOrEmpty(user.ProfilePicURL)) { NullFieldCount++; };
                if (String.IsNullOrEmpty(user.Province)) { NullFieldCount++; };
                if (String.IsNullOrEmpty(user.StreetAddress)) { NullFieldCount++; };
                if (String.IsNullOrEmpty(user.Suburb)) { NullFieldCount++; };
                if (String.IsNullOrEmpty(user.Telephone)) { NullFieldCount++; };
                
            }

            if (NullFieldCount == 0) result = true;

            return result;
        }

    }
}