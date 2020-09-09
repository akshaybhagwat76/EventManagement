using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace MiidWeb.Repositories
{
    public class NFCTagRepository
    {
        public static NFCTag GetByTagNumber(string TagNumber)
        {

            var db = new MiidEntities();
            var tag = new NFCTag();

            var tags1 = db.NFCTags.Where(x => x.TagNumber == TagNumber);

            if (tags1.Count() > 0)
            {


                tag = tags1.First();

                
            }

            return tag;

        }

        public static void CancelMyTags(string userName)
        {
       
            StringBuilder result = new StringBuilder();
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("CancelMyTags", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };


            sqlDataAdapter.SelectCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userName;




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

        public static string AddNewTag(NFCTag model)
        {

            try
            {  
                StringBuilder result = new StringBuilder();
                SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
                sqlConnection.Open();
                DataSet dataSet = new DataSet();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
                {
                    SelectCommand = new SqlCommand("ActivateTag", sqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    }
                };


                sqlDataAdapter.SelectCommand.Parameters.Add("@TagNumber", SqlDbType.VarChar).Value = model.TagNumber;
                sqlDataAdapter.SelectCommand.Parameters.Add("@ActivationCode", SqlDbType.VarChar).Value = model.ActivationCode;
                sqlDataAdapter.SelectCommand.Parameters.Add("@TagPin", SqlDbType.Int).Value = model.TagPin;
                sqlDataAdapter.SelectCommand.Parameters.Add("@EndUserID", SqlDbType.Int).Value = model.EndUserID;


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
            catch (Exception e)
            {

                return "error: "+e.Message;
            }
            
        }

        public static string GetMyDadsIDNumber(int nfcTagID, bool returnID)
        {
            try
            {
                string result = "";
                SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
                sqlConnection.Open();
                DataSet dataSet = new DataSet();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
                {
                    SelectCommand = new SqlCommand("GetMyDadsIDNumber", sqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    }
                };


                sqlDataAdapter.SelectCommand.Parameters.Add("@nfcTagID", SqlDbType.Int).Value = nfcTagID;
                
              
                sqlDataAdapter.Fill(dataSet, "dsResult");
                DataTable item = dataSet.Tables["dsResult"];
                if (item != null)
                {
                    foreach (DataRow row in item.Rows)
                    {
                        if (returnID)
                        {
                            result = (row[0].ToString());
                        }
                        else
                        {
                            result = (row[1].ToString());
                        }
                    }
                }
                sqlConnection.Close();

                return result;
            }
            catch (Exception e)
            {

                return "error: " + e.Message;
            }
        }

        public  static string SyncTagApi(int endUserID, string nfcTag)
        {
            StringBuilder result = new StringBuilder();
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("SyncTagApi", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };


            sqlDataAdapter.SelectCommand.Parameters.Add("@enduserid", SqlDbType.Int).Value = endUserID;
            sqlDataAdapter.SelectCommand.Parameters.Add("@nfctag", SqlDbType.VarChar).Value = nfcTag;


            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];
            if (item != null)
            {
                foreach (DataRow row in item.Rows)
                {
                    result.AppendLine(row["Result"].ToString());
                }
            }
            sqlConnection.Close();

            return result.ToString();
        }
    }
    
}