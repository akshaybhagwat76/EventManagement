using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MiidWeb.Repositories;
//using Microsoft.Reporting.WebForms.Internal.Soap.ReportingServices2005.Execution;
using System.Net;
using System.Configuration;
using System.Web.Services.Protocols;
using System.IO;
using Microsoft.Reporting.WebForms;

//using Microsoft.Reporting.WebForms;
//using Microsoft.Reporting.WebForms.Internal.Soap.ReportingServices2005.Execution;

namespace MiidWeb.api
{
    public class ReportApi
    {

        public string PopulateTicketReport(int ticketId, string serverPath, string drivepath, string subdomain)
        {
            string TicketNumber = "";

            try
            {
                using (var db = new MiidEntities())
                {
                    var tc = db.Tickets.Find(ticketId);
                    var tcl = db.TicketClasses.Find(tc.TicketClassID);
                    var ev = db.Events.Find(tcl.EventID);
                    var sub = db.Subdomains.Find(ev.SubdomainID);
                    if (sub != null) { subdomain = sub.SubdomainName.ToLower(); }

                    TicketNumber = tc.TicketNumber ?? "";
                }


                ReportViewer reportViewer = new ReportViewer();
                Microsoft.Reporting.WebForms.Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string filenameExtension;

                reportViewer.LocalReport.EnableExternalImages = true;
                reportViewer.LocalReport.ReportPath = ("TicketReport.rdlc");

                //DANIEL COMMENTED OUT BECAUSE ALL RDLC's need to be replaced
               if (!String.IsNullOrEmpty(subdomain))
              {
                    reportViewer.LocalReport.ReportPath = String.Format("TicketReport_{0}.rdlc", subdomain);
               }

                ReportRepository repo = new ReportRepository();

                List<Models.TicketReportModel> dt = repo.GetTicketReportData(ticketId, serverPath);

                ReportDataSource rds = new ReportDataSource("TicketReportDataSet", dt);
                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.DataSources.Add(rds);
                //  reportViewer.DataBind();
                reportViewer.LocalReport.Refresh();



                byte[] bytes = reportViewer.LocalReport.Render(
                    "PDF", null, out mimeType, out encoding, out filenameExtension,
                    out streamids, out warnings);

                string TicketPath = String.Format("{0}\\MiiDTicketPDFs\\Ticket_{1}_{2}.pdf", drivepath, ticketId, TicketNumber);

                using (FileStream fs = new FileStream(TicketPath, FileMode.Create))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }

                string TicketPathForDownload = String.Format("{0}\\MiiDTicketPDFsForDownload\\Ticket_{1}_{2}.pdf", drivepath, ticketId, TicketNumber);

                using (FileStream fs = new FileStream(TicketPathForDownload, FileMode.Create))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }


                return TicketPath;
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
                if (ex.InnerException != null)
                    mes = mes + ex.InnerException.Message;
                throw;
                return mes;
            }

        }

        public string PopulateTicketReportMulti(int ticketId, string serverPath, string drivepath, string subdomain)
        {
            string TicketNumber = "";

            try
            {
                using (var db = new MiidEntities())
                {
                    var tc = db.Tickets.Find(ticketId);
                    var tcl = db.TicketClasses.Find(tc.TicketClassID);
                    var ev = db.Events.Find(tcl.EventID);
                    var sub = db.Subdomains.Find(ev.SubdomainID);
                    if (sub != null) { subdomain = sub.SubdomainName.ToLower(); }

                    TicketNumber = tc.TicketNumber??"";
                }
              

                ReportViewer reportViewer = new ReportViewer();
                Microsoft.Reporting.WebForms.Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string filenameExtension;

                reportViewer.LocalReport.EnableExternalImages = true;
                reportViewer.LocalReport.ReportPath = ("TicketReport.rdlc");

                //DANIEL Removed CUSTOMISED rdlc because they all need to be changed
                if (!String.IsNullOrEmpty(subdomain))
                {
                    reportViewer.LocalReport.ReportPath = String.Format("TicketReport_{0}.rdlc", subdomain);
                }

                ReportRepository repo = new ReportRepository();

                List<Models.TicketReportModel> dt = repo.GetTicketReportDataMulti(ticketId, serverPath);

                ReportDataSource rds = new ReportDataSource("TicketReportDataSet", dt);
                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.DataSources.Add(rds);
                //  reportViewer.DataBind();
                reportViewer.LocalReport.Refresh();



                byte[] bytes = reportViewer.LocalReport.Render(
                    "PDF", null, out mimeType, out encoding, out filenameExtension,
                    out streamids, out warnings);

                string TicketPath = String.Format("{0}\\MiiDTicketPDFs\\MultipleTicket_{1}_{2}.pdf", drivepath, ticketId, TicketNumber);

                using (FileStream fs = new FileStream(TicketPath, FileMode.Create))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }

                string TicketPathForDownload = String.Format("{0}\\MiiDTicketPDFsForDownload\\MultipleTicket_{1}_{2}.pdf", drivepath, ticketId, TicketNumber);

                using (FileStream fs = new FileStream(TicketPathForDownload, FileMode.Create))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }


                return TicketPath;
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
                if(ex.InnerException!=null)
                    mes = mes + ex.InnerException.Message;
                throw;
                return mes;
            }

        }

    
    }
}