using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using MiidWeb.Models;

namespace MiidWeb.Repositories
{
    public static class SiteUserRepository
    {


      

        public static void ResetLogin(string Email)
        {
            StringBuilder result = new StringBuilder();
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("ResetLogin", sqlConnection)
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
                    result.AppendLine(row[0].ToString());
                }
            }
            sqlConnection.Close();
            return;
        }

        public static EndUser GetUserFromUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public static string CreateLoginForQuickAddUser(string Email)
        {
            try
            {
                StringBuilder result = new StringBuilder();
                SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
                sqlConnection.Open();
                DataSet dataSet = new DataSet();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
                {
                    SelectCommand = new SqlCommand("CreateLoginForQuickAddUser", sqlConnection)
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
                        result.AppendLine(row[0].ToString());
                    }
                }
                sqlConnection.Close();
                return result.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static void UpdateBankDetails(MiiFundsRequestWithdrawalViewModel model)
        {
            StringBuilder result = new StringBuilder();
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("UpdateBankDetails", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };


            sqlDataAdapter.SelectCommand.Parameters.Add("@EndUserID", SqlDbType.VarChar).Value = model.EndUserID;
            sqlDataAdapter.SelectCommand.Parameters.Add("@Country", SqlDbType.VarChar).Value = model.Country?? "Country";
            sqlDataAdapter.SelectCommand.Parameters.Add("@Bank", SqlDbType.VarChar).Value = model.Bank ?? "Bank";
            sqlDataAdapter.SelectCommand.Parameters.Add("@BranchCode", SqlDbType.VarChar).Value = model.BranchCode ?? "BranchCode";
            sqlDataAdapter.SelectCommand.Parameters.Add("@AccountNumber", SqlDbType.VarChar).Value = model.AccountNumber ?? "AccountNumber";
            sqlDataAdapter.SelectCommand.Parameters.Add("@AccountType", SqlDbType.VarChar).Value = model.AccountType ?? "AccountType";
            sqlDataAdapter.SelectCommand.Parameters.Add("@AccountHolderName", SqlDbType.VarChar).Value = model.AccountHolderName ?? "AccountHolderName";
            sqlDataAdapter.SelectCommand.Parameters.Add("@Notes", SqlDbType.VarChar).Value = model.Notes ?? "Notes";



            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];
            if (item != null)
            {
                foreach (DataRow row in item.Rows)
                {
                    result.AppendLine(row[0].ToString());
                }
            }
            sqlConnection.Close();
            return;
        }
    }
}