using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MiidWeb.Helpers
{
    public class ControlHelper
    {
        //public static void FillDropDownList(string Query, SelectItemList DropDownName, string valueMember, string displayMember)
        //{

        //    var conn = ConfigRepo.Get("MiidConnectionString");

        //    DataTable dt;

        //    using (var cn = new SqlConnection(conn))
        //    {
        //        cn.Open();

        //        try
        //        {
        //            SqlCommand cmd = new SqlCommand(Query, cn);
        //            SqlDataAdapter sa = new SqlDataAdapter(cmd);
        //            DataSet ds = new DataSet();
        //            sa.Fill(ds);
        //            dt = ds.Tables[0];

        //        }
        //        catch (SqlException e)
        //        {
        //            Console.WriteLine(e.ToString());
        //            return;
        //        }
        //    }

        //    DropDownName.DataSource = dt;
        //    DropDownName.ValueMember = valueMember;
        //    DropDownName.DisplayMember = displayMember;
        //}
    }
}