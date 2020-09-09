using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MiidWeb.Repositories;

namespace MiidWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PromotionUploadController : BaseController
    {
       
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(HttpPostedFileBase upload)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (upload != null && upload.ContentLength > 0)
                    {

                        if (upload.FileName.EndsWith(".csv"))
                        {
                            Stream stream = upload.InputStream;
                            DataTable csvTable = new DataTable();
                            using (CsvReader csvReader =
                                new CsvReader(new StreamReader(stream), true))
                            {
                                csvTable.Load(csvReader);
                            }

                            foreach (DataRow row in csvTable.Rows)
                            {
                                PromotionInsert(row["EventName"].ToString(),
                                    row["UserName"].ToString(),
                                    decimal.Parse(row["DiscountAmount"].ToString()),
                                    row["VendorName"].ToString(),
                                    row["UserStatus"].ToString()
                                    );

                            }

                            return View(csvTable);
                        }
                        else
                        {
                            ModelState.AddModelError("File", "This file format is not supported");
                            return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("File", "Please Upload Your file");
                    }
                }
            }
            catch (Exception e)
            {

                Growl("Miid Promotion Uploads", "Upload error..... :( " + e.Message);

            }
            return View();
        }


        public static void PromotionInsert(string EventName, string UserName, decimal DiscountAmount, string VendorName, string UserStatus)
        {

            StringBuilder result = new StringBuilder();
            SqlConnection sqlConnection = new SqlConnection(ConfigRepo.Get("MiidConnectionString"));
            sqlConnection.Open();
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter()
            {
                SelectCommand = new SqlCommand("PromotionInsert", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                }
            };


            sqlDataAdapter.SelectCommand.Parameters.Add("@EventName", SqlDbType.VarChar).Value = EventName;
            sqlDataAdapter.SelectCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = UserName;
            sqlDataAdapter.SelectCommand.Parameters.Add("@DiscountAmount", SqlDbType.Decimal).Value = DiscountAmount;
            sqlDataAdapter.SelectCommand.Parameters.Add("@VendorName", SqlDbType.VarChar).Value = VendorName;
            sqlDataAdapter.SelectCommand.Parameters.Add("@UserStatus", SqlDbType.VarChar).Value = UserStatus;
            




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