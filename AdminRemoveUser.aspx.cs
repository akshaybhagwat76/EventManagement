using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MiidWeb.Repositories;
namespace MiidWeb
{
    public partial class AdminRemoveUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.IsInRole("Admin"))
            { 
                Response.Redirect("/");
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
                sqlConnection.Open();
                DataSet dataSet = new DataSet();

                var command = new SqlCommand("RemoveUser", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add("@Email", SqlDbType.VarChar).Value = TextBox1.Text;


                command.ExecuteNonQuery();


                sqlConnection.Close();

                Label1.Text = "Success";
            }
            catch (Exception ee)
            {
                Label1.Text = ee.Message;
            
            }
        }
    }
}