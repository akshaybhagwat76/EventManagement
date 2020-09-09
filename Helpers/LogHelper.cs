using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MiidWeb.Models;
using System.IO;
using System.Net;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using MiidWeb.Repositories;

namespace MiidWeb.Helpers
{
    public static class LogHelper
    {
        public static int ID { get; set; }
        public static string Context { get; set; }
        public static string Description { get; set; }
        public static DateTime DateCreated { get; set; }
        public static string UserName { get; set; }

      

        public static void Log(string message, string context)
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath("/");
                System.IO.File.AppendAllText(Path.Combine(path, "MiiDLogTraining.txt"), String.Format("{0}:  {1}: {2}", context, message, DateTime.Now));
            }
            catch (Exception cc)
            {
                LogToDatabase(cc.ToString());
            }

        }

        private static void LogToDatabase(string Message)
        {
             
            List<MyEventItem> myEvents = new List<MyEventItem>();
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("LogToDatabase", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };
            sqlDataAdapter.SelectCommand.Parameters.Add("@Message", SqlDbType.VarChar).Value = Message;
            
            sqlDataAdapter.Fill(dataSet, "dsResult");
            DataTable item = dataSet.Tables["dsResult"];
            if (item != null)
            {
               
            }
            sqlConnection.Close();
            return;
        


    }



    //public void LogData()
    //{
    //    //MiidEntities db = new MiidEntities();

    //    //var d = new DataLog();
    //    //d.Context = this.Context;
    //    //d.Description = this.Description;
    //    //d.DateCreated = DateTime.Now;
    //    //d.UserName = this.UserName;

    //    //db.DataLogs.Add(d);
    //    //db.SaveChanges();

    //    return;
    //}



}
}