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

        public static void CreateLoginForEventOrganiser(string Email)
        {
            List<MyEventItem> myEvents = new List<MyEventItem>();
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MiidConnectionString"].ConnectionString);
            sqlConnection.Open();
            DataSet dataSet = new DataSet();

            var command = new SqlCommand("CreateLoginForEventOrganiser", sqlConnection)
                 {
                     CommandType = CommandType.StoredProcedure
                 };
            
            command.Parameters.Add("@Email", SqlDbType.VarChar).Value = Email;

            
            command.ExecuteNonQuery();
            

            sqlConnection.Close();

            return;
        }

        public static EventOrganiser GetEventOrganiserByEmail(string Email)
        {
            MiidEntities db = new MiidEntities();

            var eo  = db.EventOrganisers.Where(x => x.Email == Email).First();

           
            return eo;
        }


    }
}