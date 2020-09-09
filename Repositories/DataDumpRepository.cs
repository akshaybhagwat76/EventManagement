using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MiidWeb.Repositories
{
    public static class DataDumpRepository
    {

        public static DataTable GetDataDump(string ProcName = "BoxOfficeReport", string PosID = "", int EventID = 0, DateTime? FromDate = null, DateTime? ToDate = null)
        {

            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

          //  SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("DefaultConnection"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();



            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand(ProcName, sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };



            sqlDataAdapter.SelectCommand.Parameters.Add("@EventID", SqlDbType.Int).Value = EventID;
            sqlDataAdapter.SelectCommand.Parameters.Add("@PosID", SqlDbType.VarChar).Value = PosID;
            sqlDataAdapter.SelectCommand.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = FromDate;
            sqlDataAdapter.SelectCommand.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = ToDate;





            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];

            sqlConnection.Close();
            return item;

        }

        public static DataTable GetDataDumpPromoCodeReport(string ProcName, int EventID)
        {
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);


           // SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("DefaultConnection"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();



            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand(ProcName, sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };



            sqlDataAdapter.SelectCommand.Parameters.Add("@EventID", SqlDbType.Int).Value = EventID;





            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];

            sqlConnection.Close();
            return item;

        }
    }
}