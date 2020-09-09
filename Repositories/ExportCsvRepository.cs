using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.IO;

using System.Text;

using System.Configuration;
using MiidWeb.Helpers;

namespace MiidWeb.Repositories
{
    public static class ExportCsvRepository
    {


        


        public static string ExportCSV(string sqlString)
        {
            string constr = ConfigRepo.Get("MiidConnectionString");
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(sqlString))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);


                            StringBuilder sb = new StringBuilder();

                            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                                              Select(column => column.ColumnName);
                            sb.AppendLine(string.Join(",", columnNames));
                            foreach (DataRow row in dt.Rows)
                            {
                                IEnumerable<string> fields = row.ItemArray.Select(field =>
                                  string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                                sb.AppendLine(string.Join(",", fields));
                            }



                            return sb.ToString();

                        }
                    }
                }
            }
        }

        public static string ExportCSV(string sqlString, bool IsStoreProc, string TicketIDList)
        {
            string constr = ConfigRepo.Get("MiidConnectionString");
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(sqlString))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {

                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                        sda.SelectCommand.Parameters.Add("@TicketIDList", SqlDbType.VarChar).Value = TicketIDList;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);


                            StringBuilder sb = new StringBuilder();

                            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                                              Select(column => column.ColumnName);
                            sb.AppendLine(string.Join(",", columnNames));
                            foreach (DataRow row in dt.Rows)
                            {
                                IEnumerable<string> fields = row.ItemArray.Select(field =>
                                  string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                                sb.AppendLine(string.Join(",", fields));
                            }



                            return sb.ToString();

                        }
                    }
                }
            }
        }

        public static string ExportCSV(string sqlString, string Parameter)//AreThereMoreTicketsThanBank
        {
            string constr = ConfigRepo.Get("MiidConnectionString");
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(sqlString))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {

                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                        sda.SelectCommand.Parameters.Add("@Parameter", SqlDbType.VarChar).Value = Parameter;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);


                            StringBuilder sb = new StringBuilder();

                            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                                              Select(column => column.ColumnName);
                            sb.AppendLine(string.Join(",", columnNames));
                            foreach (DataRow row in dt.Rows)
                            {
                                IEnumerable<string> fields = row.ItemArray.Select(field =>
                                  string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                                sb.AppendLine(string.Join(",", fields));
                            }



                            return sb.ToString();

                        }
                    }
                }
            }
        }




        public static void ExportCSVAndEmail(string procName, string parameter, string contactEmail, string subject, string body, string attachmentFileName)
        {
            string csv = ExportCSV(procName, parameter);

            if (String.IsNullOrEmpty(csv)) return;

            List<string> attachmentList = new List<string>();

            //attachmentFileName = String.Format("{0}{1}", System.Guid.NewGuid().ToString().Replace('-', '_'), attachmentFileName);

            string myTempFile = Path.Combine(@"c:\inetpub\wwwroot\training.miid.co.za\uploads\", attachmentFileName);
            using (StreamWriter sw = new StreamWriter(myTempFile))
            {
                sw.Write(csv);
                attachmentList.Add(myTempFile);

                EmailHelper.SendMail(contactEmail, subject, body, null,null, attachmentList, ConfigRepo.GetSubdomainID());
            }

          

           // System.IO.File.Delete(myTempFile);
        }
    }

}