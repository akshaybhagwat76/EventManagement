
using Microsoft.Reporting.WebForms;
using MiidWeb.Helpers;
using MiidWeb.Models;
using MiidWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiidWeb
{
    public partial class VendorReport : System.Web.UI.Page
    {
        public string con = ConfigRepo.Get("MiidConnectionString");

        protected void Page_PreRender(object sender, EventArgs e)
        {

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (Request.QueryString["Type"] != null)
                {
                    this.Label1.Text = String.Format("Vendor {0} Report", Request.QueryString["Type"].ToString());
                 

                    switch (Request.QueryString["Type"].ToString().ToLower())
                    {
                        case "detail":
                            int vendorID = (int.Parse(Request.QueryString["V691B057A"]));
                            int eventID = (int.Parse(Request.QueryString["E691B057A"]));
                            VendorDetailReport(vendorID, eventID);
                            break;
                        case "summary":
                             vendorID = (int.Parse(Request.QueryString["V691B057A"]));
                             eventID = (int.Parse(Request.QueryString["E691B057A"]));
                            VendorSummaryReport(vendorID, eventID);
                            break;
                        case "ticket":
                            TicketReportTest(); break;
                        default:
                            break;
                    }


                }

            }
            else
            {




            }
        }
        private void SelectQuery(string queryString, string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(
                           connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        GridView1.DataSource = reader;
                        GridView1.DataBind();
                        GridView1.Attributes.Add("class", "table table-bordered table-striped");
                    }
                    else
                    {
                        Label1.Text = "Now rows found in table.";

                    }

                }
            }
            catch (Exception e)
            {
                Label1.Text = "Error:" + e.Message;

            }
        }
        private void TicketReportTest()
        {
            ReportRepository repo = new ReportRepository();

            //var db = new MiidEntities();
            string serverPath = "https://miid.co.za";
             

            List<Models.TicketReportModel> dt = repo.GetTicketReportData(0, serverPath);

           

            pnlReport.Visible = true;

            this.ReportViewer1.Reset();
            this.ReportViewer1.LocalReport.EnableExternalImages = true;
            this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("TicketReport.rdlc");
            ReportDataSource rds = new ReportDataSource("TicketReportDataSet", dt);
            this.ReportViewer1.LocalReport.DataSources.Clear();
            this.ReportViewer1.LocalReport.DataSources.Add(rds);
            this.ReportViewer1.DataBind();
            this.ReportViewer1.LocalReport.Refresh();


           


        }

        private void VendorSummaryReport(int ID, int eventID)
        {
            VendorEventRepository repo = new VendorEventRepository();

            //var db = new MiidEntities();


            List<VendorSummaryReportModel> dt = repo.ReportVendorSummary(eventID, ID);

            ABList<VendorSummaryReportModel> lstP = new ABList<VendorSummaryReportModel>();
            foreach (var v in dt)
            {
                lstP.Add(v);
            }

            pnlReport.Visible = true;

            //this.ReportViewer1.Reset();
            //this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("VendorSummaryReport.rdlc");
            //ReportDataSource rds = new ReportDataSource("DataSet1", dt);
            //this.ReportViewer1.LocalReport.DataSources.Clear();
            //this.ReportViewer1.LocalReport.DataSources.Add(rds);
            //this.ReportViewer1.DataBind();
            //this.ReportViewer1.LocalReport.Refresh();


            DataTable dtPro = lstP.GetDataTable();

            GridView1.DataSource = dtPro;
            GridView1.DataBind();
            GridView1.Attributes.Add("class", "table table-bordered table-striped");
            

        }
        private void VendorDetailReport(int ID, int eventID)
        {
            VendorEventRepository repo = new VendorEventRepository();

            var db = new MiidEntities();


            List<VendorReportModel> dt = repo.GetSingleVendorTransactionsForEvent(eventID, ID);


            ABList<VendorReportModel> lstP = new ABList<VendorReportModel>();
            foreach (var v in dt)
            {
                lstP.Add(v);
            }
            

            DataTable dtPro = lstP.GetDataTable();


            pnlReport.Visible = true;
                      

            GridView1.DataSource = dtPro;
            GridView1.DataBind();
            GridView1.Attributes.Add("class", "table table-bordered table-striped");
        }
    }
}