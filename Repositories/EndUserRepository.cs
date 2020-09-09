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
using MiidWeb.Helpers;


namespace MiidWeb.Repositories
{
    public static class EndUserRepository
    {

        public static string GetTagStatusForUserID(int EndUserID)
        {
            string result = "User Not Found, Tag Not Found";

            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("GetTagStatusForUserID", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };
            sqlDataAdapter.SelectCommand.Parameters.Add("@EndUserID", SqlDbType.Int).Value = EndUserID;
           

            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];
            if (item != null)
            {
                foreach (DataRow row in item.Rows)
                {
                    result = row[0].ToString();
                }
            }
            sqlConnection.Close();
            return result;


        }


        public static EndUser GetByUserID(int ID)
        {
            MiidEntities db = new MiidEntities();

            return db.EndUsers.Find(ID);


        }

        public static EndUser GetByUserName(string UserName)
        {
            MiidEntities db = new MiidEntities();
            var users = db.EndUsers.Where(x => x.Email == UserName);

            if (users != null && users.Count() > 0)
            {
                return users.First();
            }
            return null;

        }

        public static bool EmailVerified(string UserName)
        {
            var user = GetByUserName(UserName);

            if (user != null)
                return user.EmailVerified ?? false;

            return false;

        }

        public static bool AmIAnEventOrganiser(string EmailAddress)
        {
            var db = new MiidEntities();

            var eos = db.EventOrganisers.Where(x => x.Email == EmailAddress);

            if (eos != null && eos.Count() > 0)
            {
                return true;
            }
            return false;


        }

        public static bool AmIAnEventOrganiser(string EmailAddress, bool AmILockedOut)
        {
            var db = new MiidEntities();

            var eos = db.EventOrganisers.Where(x => x.Email == EmailAddress);

            if (eos != null && eos.Count() > 0)
            {
                if (eos.First().StatusID == StatusHelper.StatusID("EO", "Inactive"))
                {
                    return true;
                }
            }
            return false;


        }


        public static List<MyEventItem> GetMyEventsOnly(int EndUserID, DateTime Date, int PlusMonths)
        {
            List<MyEventItem> myEvents = new List<MyEventItem>();
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
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



        public static List<CalendarItem> GetMyEvents(int EndUserID, DateTime Date, int PlusMonths)
        {
            List<CalendarItem> myEvents = new List<CalendarItem>();
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
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

        public static void UpdateBankDetailsForRefund(RefundTicketViewModel model)
        {
            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(model.EndUserID);
                if (user != null)
                {
                    user.Bank = model.Bank;
                    user.BranchCode = model.BranchCode;
                    user.AccountHolderName = model.AccountHolderName;
                    user.AccountNumber = model.AccountNumber;
                    user.Notes = model.Notes;
                    
                    user.AccountTypeName = model.AccountType;
                    user.Country = model.Country;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();

                }


            }
        }
        public static void ClearBankDetailsAfterRefund(Int64 userid)
        {
            using (var db = new MiidEntities())
            {
                var user = db.EndUsers.Find(userid);
                if (user != null)
                {
                    user.Bank = ""; //model.Bank;
                    user.BranchCode = ""; //model.BranchCode;
                    user.AccountHolderName = ""; //model.AccountHolderName;
                    user.AccountNumber = ""; //model.AccountNumber;
                    //user.BankAccountType = ""; //model.AccountType;
                    user.AccountTypeName = ""; //model.AccountType;
                    user.Country = ""; //model.Country;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();

                }


            }
        }

        public static bool DoesAspNetUserExist(string Email)
        {
            bool result = false;
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("DoesAspNetUserExist", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };
            sqlDataAdapter.SelectCommand.Parameters.Add("@Email", SqlDbType.VarChar).Value = Email;



            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];
            if (item != null)
            {
                foreach (DataRow row in item.Rows)
                {
                    result = bool.Parse(row[0].ToString());
                }
            }

            return result;
        }

        public static List<FriendViewModel> GetMyFriends(int EndUserID)
        {
            MiidEntities db = new MiidEntities();

            List<FriendViewModel> myFriends = new List<FriendViewModel>();

            var friendsInitiating = db.Friends.Where(x => x.InitiatingUserID == EndUserID && x.DateAccepted != null).ToList();

            var friendsAccepting = db.Friends.Where(x => x.AcceptingUserID == EndUserID && x.DateAccepted != null).ToList();

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

        public static void SendCancelManualEftEmail(BankTransaction bt)
        {
            MyMoneyRepository.SendCancelManualEFTEmailNotification(bt.EndUserID, bt);
        }

        public static Controllers.UserModel GetUser(string UserID)
        {
            MiidEntities db = new MiidEntities();

            EndUser u = db.EndUsers.Find(int.Parse(UserID));

            if (u != null)
            {
                Controllers.UserModel um = new Controllers.UserModel();
                um.FirstName = u.Firstname;
                um.LastName = u.Surname;
                um.ProfilePicURL = u.ProfilePicURL;
                um.Id = u.ID;
                um.Email = u.Email;
                return um;
            }
            else
                return null;
        }

        public static Controllers.UserModel GetUser(string email, bool useEmail)
        {
            try
            {

                MiidEntities db = new MiidEntities();

                var us = db.EndUsers.Where(x => x.Email == email);

                if (us != null && us.Count() > 0)
                {
                    var u = us.First();
                    Controllers.UserModel um = new Controllers.UserModel();
                    um.FirstName = u.Firstname;
                    um.LastName = u.Surname;
                    um.ProfilePicURL = u.ProfilePicURL;
                    um.Id = u.ID;
                    um.Email = email;
                    return um;

                }
                else
                {
                    //create placeholder user
                    EndUser u = new EndUser();
                    u.Email = email;
                    u.Firstname = email;
                    u.Surname = email;
                    u.IDNumber = "0000000000000";
                    u.Cell = "0000000000";
                    u.StatusID = 1;
                    u.ProfilePicURL = "avatar_blank.png";

                    db.EndUsers.Add(u);
                    db.SaveChanges();
                    Controllers.UserModel um = new Controllers.UserModel();
                    um.FirstName = u.Firstname;
                    um.LastName = u.Surname;
                    um.ProfilePicURL = u.ProfilePicURL;
                    um.Id = u.ID;
                    um.Email = email;
                    return um;
                }
            }
            catch (Exception e)
            {

                return null;
            }

        }


        public static bool AreYouRegisteredBuddy(int EndUserID)
        {
            bool result = false;
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("AreYouRegisteredBuddy", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };
            sqlDataAdapter.SelectCommand.Parameters.Add("@EndUserID", SqlDbType.Int).Value = EndUserID;


            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];
            if (item != null)
            {
                foreach (DataRow row in item.Rows)
                {
                    result = bool.Parse(row[0].ToString());
                }
            }
            sqlConnection.Close();
            return result;

        }

        public static bool IsUserUpToDate(int EndUserID)
        {
            MiidEntities db = new MiidEntities();

            bool result = false;
            int NullFieldCount = 0;

            var user = db.EndUsers.Find(EndUserID);

            //            Name
            //Surname
            //ID number
            //Phone Number
            //Date of Birth
            //T & C's


            if (user != null)
            {
                if (String.IsNullOrEmpty(user.Firstname) || user.Firstname == user.Email) { NullFieldCount++; };
                if (String.IsNullOrEmpty(user.Surname) || user.Surname == user.Email) { NullFieldCount++; };
                if (String.IsNullOrEmpty(user.Cell) || user.Cell.StartsWith("00000")) { NullFieldCount++; };
                if (String.IsNullOrEmpty(user.IDNumber) || user.IDNumber.StartsWith("00000")) { NullFieldCount++; };
                //if (String.IsNullOrEmpty(user.City)) { NullFieldCount++; };
                if (user.DateOfBirth == null) { NullFieldCount++; };

                if (user.TermsAndConditionsAccepted == null || user.TermsAndConditionsAccepted == false) { NullFieldCount++; };
                // if (String.IsNullOrEmpty(user.KnownConditions)) { NullFieldCount++; };
                // if (String.IsNullOrEmpty(user.MedicalAidCompany)) { NullFieldCount++; };
                // if (String.IsNullOrEmpty(user.MedicalAidNumber)) { NullFieldCount++; };
                // if (String.IsNullOrEmpty(user.Medication)) { NullFieldCount++; };
                // if (String.IsNullOrEmpty(user.NextOfKin)) { NullFieldCount++; };
                // if (String.IsNullOrEmpty(user.NextOfKinTelephone)) { NullFieldCount++; };
                // if (String.IsNullOrEmpty(user.ProfilePicURL)) { NullFieldCount++; };
                // if (String.IsNullOrEmpty(user.Province)) { NullFieldCount++; };
                // if (String.IsNullOrEmpty(user.StreetAddress)) { NullFieldCount++; };
                // if (String.IsNullOrEmpty(user.Suburb)) { NullFieldCount++; };
                // if (String.IsNullOrEmpty(user.Telephone)) { NullFieldCount++; };

            }

            if (NullFieldCount == 0) result = true;

            return result;
        }


        public static EndUser GetByTagNumber(string TagNumber)
        {

            var db = new MiidEntities();
            var endUser = new EndUser();

            var tags1 = db.NFCTags.Where(x => x.TagNumber == TagNumber);

            if (tags1.Count() > 0)
            {


                var tag = tags1.First();

                if (tag != null)
                {
                    endUser = db.EndUsers.Find(tag.EndUserID);
                }
            }

            return endUser;

        }

        public static void SendBatchNotification(string context)
        {
            var db = new MiidEntities();
            var endUser = new EndUser();
            int TopUpTransactionTypeID = StatusHelper.TransactionTypeID("TOPUP");
            int WithDrawalTransactionTypeID = StatusHelper.TransactionTypeID("MiiFunds Withdrawal Paid Out");
            decimal fee = 0.00M;

            switch (context)
            {
                case "manualeft":
                    var monies = db.MyMoneys.Where(x => x.EmailNotificationStatus == "P" && x.TransactionTypeID == TopUpTransactionTypeID);
                    foreach (var money in monies.ToList())
                    {
                        fee = Helpers.ConfiguredFeeHelper.FeeAmount("ManualEFT", 0.00M, int.Parse(GlobalVariables.SubdomainID));
                        MyMoneyRepository.SendConfirmationManualEFTReceievedAndToppedUp(money.EndUserID, (int)(money.Amount ?? 0.0M), fee, money.Note);
                        var updateMM = db.MyMoneys.Find(money.ID);
                        updateMM.EmailNotificationStatus = "D";
                        db.Entry(updateMM).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    break;
                case "withdrawal":
                    var monies2 = db.MyMoneys.Where(x => x.EmailNotificationStatus == "P" && x.TransactionTypeID == WithDrawalTransactionTypeID);
                    fee = Helpers.ConfiguredFeeHelper.FeeAmount("MiiFundsWithdrawal", 0.00M, int.Parse(GlobalVariables.SubdomainID));
                    foreach (var money1 in monies2.ToList())
                    {
                        MyMoneyRepository.SendConfirmationMiifundsWithdrawalApprovedPaidOut(money1.EndUserID, money1.Amount ?? 0.0M, fee, money1.Note);
                        var updateMM = db.MyMoneys.Find(money1.ID);
                        updateMM.EmailNotificationStatus = "D";
                        db.Entry(updateMM).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    break;
                default: break;
            }
        }

        public static void SetEmailConfirmedOnAspNet(string Email)
        {
            List<MyEventItem> myEvents = new List<MyEventItem>();
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();

            var command = new SqlCommand("SetEmailConfirmedOnAspNet", sqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("@Email", SqlDbType.VarChar).Value = Email;


            command.ExecuteNonQuery();


            sqlConnection.Close();

            return;
        }

        public static bool IsUserIDValid(int EndUserID)
        {
            bool result = false;
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("IsIDNumberValid", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };
            sqlDataAdapter.SelectCommand.Parameters.Add("@EndUserID", SqlDbType.Int).Value = EndUserID;


            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];
            if (item != null)
            {
                foreach (DataRow row in item.Rows)
                {
                    result = bool.Parse(row[0].ToString());
                }
            }
            sqlConnection.Close();
            return result;
        }

        public static List<int> EventIDsIHaveTicketsFor(int loggedInUserID)
        {
            List<int> result = new List<int>();
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("EventIDsIHaveTicketsFor", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };
            sqlDataAdapter.SelectCommand.Parameters.Add("@EndUserID", SqlDbType.Int).Value = loggedInUserID;


            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];
            if (item != null)
            {
                foreach (DataRow row in item.Rows)
                {
                    result.Add(int.Parse(row[0].ToString()));
                }
            }
            sqlConnection.Close();
            return result;
        }
    }
}