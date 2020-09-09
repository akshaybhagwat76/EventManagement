using Microsoft.Reporting.WebForms;
using MiidWeb.Models;
using MiidWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MiidWeb
{
    public partial class VendorReport2 : System.Web.UI.Page
    {

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
                    int vendorID = (int.Parse(Request.QueryString["V691B057A"]));
                    int eventID = (int.Parse(Request.QueryString["E691B057A"]));

                    switch (Request.QueryString["Type"].ToString())
                    {
                        case "Detail":
                            VendorDetailReport(vendorID, eventID);
                            break;
                        case "Summary":
                            VendorSummaryReport(vendorID, eventID);
                            break;
                        default:
                            break;
                    }


                }

            }
            else
            {




            }
        }



        private void VendorSummaryReport(int ID, int eventID)
        {
            VendorEventRepository repo = new VendorEventRepository();

            var db = new MiidEntities();


            List<VendorSummaryReportModel> dt = repo.ReportVendorSummary(eventID, ID);

            pnlReport.Visible = true;

            this.ReportViewer1.Reset();
            this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("VendorSummaryReport.rdlc");
            ReportDataSource rds = new ReportDataSource("DataSet1", dt);
            this.ReportViewer1.LocalReport.DataSources.Clear();
            this.ReportViewer1.LocalReport.DataSources.Add(rds);
            this.ReportViewer1.DataBind();
            this.ReportViewer1.LocalReport.Refresh();
        }
        private void VendorDetailReport(int ID, int eventID)
        {
            VendorEventRepository repo = new VendorEventRepository();

            var db = new MiidEntities();


            List<VendorReportModel> dt = repo.GetSingleVendorTransactionsForEvent(eventID, ID);

            pnlReport.Visible = true;

            this.ReportViewer1.Reset();
            this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("VendorDetailReport.rdlc");
            ReportDataSource rds = new ReportDataSource("DataSet1", dt);
            this.ReportViewer1.LocalReport.DataSources.Clear();
            this.ReportViewer1.LocalReport.DataSources.Add(rds);
            this.ReportViewer1.DataBind();
            this.ReportViewer1.LocalReport.Refresh();
        }
    }
}