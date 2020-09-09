using MiidWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace MiidWeb.Repositories
{
  public class ReportRepository
  {


        public List<TicketReportModel> GetTicketReportDataMulti(int TicketId, string serverPath = "")
        {
           

            List<int> ticketIds = GetAllTicketsInThisPurchase(TicketId);
            foreach (var id in ticketIds)
            {
                //make sure qrcode exists
                TicketRepository.SaveTicketQRCode(id);

            }


            List<TicketReportModel> reportData = new List<TicketReportModel>();
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("ReportTicketGetMulti", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };
            sqlDataAdapter.SelectCommand.Parameters.Add("@TicketId", SqlDbType.VarChar).Value = TicketId;

            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];
            if (item != null)
            {
                foreach (DataRow row in item.Rows)
                {
                    reportData.Add(new TicketReportModel
                    {
                        Firstname = row["Firstname"].ToString(),
                        Surname = row["Surname"].ToString(),
                        IDNumber = row["IDNumber"].ToString(),
                        TicketClass = row["TicketClass"].ToString(),

                        EventName = row["EventName"].ToString(),
                        TicketClassStartDate = DateTime.Parse(row["TicketClassStartDate"].ToString()),
                        StartDateTime = DateTime.Parse(row["StartDateTime"].ToString()),
                        StreetAddress = row["StreetAddress"].ToString(),
                        Suburb = row["Suburb"].ToString(),
                        UserName = row["UserName"].ToString(),
                        SeatNumber = row["SeatNumber"].ToString(),
                        RowNumber = row["RowNumber"].ToString(),



                        TicketId = row["TicketNumber"].ToString(),
                        ImageURL = String.Format("{0}//{1}//{2}", serverPath, "Uploads", row["ImageUrl"].ToString()),
                        QrCode = String.Format("{0}//{1}//{2}", serverPath, "MiiDTicketPDFs", row["QrCode"].ToString()),
                        Image = new byte[0]
                    });
                }
            }


            sqlConnection.Close();
            return reportData;
        }

        public List<int> GetAllTicketsInThisPurchase(int ticketId)
        {
            List<int> reportData = new List<int>();
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("GetAllTicketsInThisPurchase", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };
            sqlDataAdapter.SelectCommand.Parameters.Add("@TicketId", SqlDbType.VarChar).Value = ticketId;

            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];
            if (item != null)
            {
                foreach (DataRow row in item.Rows)
                {
                    reportData.Add(int.Parse(row["TicketID"].ToString()));
                }
            }


            sqlConnection.Close();
            return reportData;
        }

        public List<TicketReportModel> GetTicketReportData(int TicketId, string serverPath="")
    {
            //make sure qrcode exists
            TicketRepository.SaveTicketQRCode(TicketId);

      List<TicketReportModel> reportData = new List<TicketReportModel>();
      SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
      sqlConnection.Open();
      DataSet dataSet = new DataSet();
      SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
      {
        SelectCommand = new SqlCommand("ReportTicketGet", sqlConnection)
        {
          CommandType = CommandType.StoredProcedure
        }
      };
      sqlDataAdapter.SelectCommand.Parameters.Add("@TicketId", SqlDbType.VarChar).Value = TicketId;

      sqlDataAdapter.Fill(dataSet, "dsResult");
      DataTable item = dataSet.Tables["dsResult"];
      if (item != null)
      {
                foreach (DataRow row in item.Rows)
                {
                    reportData.Add(new TicketReportModel
                    {
                        Firstname = row["Firstname"].ToString(),
                        Surname = row["Surname"].ToString(),
                        IDNumber = row["IDNumber"].ToString(),
                        TicketClass = row["TicketClass"].ToString(),

                        EventName = row["EventName"].ToString(),
                        TicketClassStartDate = DateTime.Parse(row["TicketClassStartDate"].ToString()),
                        StartDateTime = DateTime.Parse(row["StartDateTime"].ToString()),
                        StreetAddress = row["StreetAddress"].ToString(),
                        Suburb = row["Suburb"].ToString(),
                        UserName = row["UserName"].ToString(),
                        SeatNumber = row["SeatNumber"].ToString(),
                        RowNumber = row["RowNumber"].ToString(),



                        TicketId = row["TicketNumber"].ToString(),
                        //ImageURL = Path.Combine(serverPath, "Uploads/" + row["ImageUrl"].ToString()),
                        //QrCode = Path.Combine(serverPath, "MiiDTicketPDFs/" + row["QrCode"].ToString()),
                        //ImageURL = "http://www.google.com/intl/en_ALL/images/logo.gif",
                        ImageURL = String.Format("{0}//{1}//{2}", serverPath, "Uploads", row["ImageUrl"].ToString()),
                        QrCode = String.Format("{0}//{1}//{2}", serverPath, "MiiDTicketPDFs", row["QrCode"].ToString()),

                        Image = new byte[0]
                    });
                }
            }

       
      sqlConnection.Close();
      return reportData;
    }


        public List<string> PosIDsForEvent(int EventID)
        {

            List<string> reportData = new List<string>();
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("PosIDsForEvent", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };
            sqlDataAdapter.SelectCommand.Parameters.Add("@EventID", SqlDbType.VarChar).Value = EventID;

            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];
            if (item != null)
            {
                foreach (DataRow row in item.Rows)
                {

                    string Report_StoresMarkForDeletion = row["PosID"].ToString();
                        


                    reportData.Add(Report_StoresMarkForDeletion);
                }
            }
            sqlConnection.Close();
            return reportData;
        }



    }
}
