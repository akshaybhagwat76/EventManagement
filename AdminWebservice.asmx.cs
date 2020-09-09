using MiidWeb.api;
using MiidWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace MiidWeb
{
    /// <summary>
    /// Summary description for AdminWebservice
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class AdminWebservice : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }


        [WebMethod]
        public string PopulateTicketReport(int ticketId)
        {
            ReportApi repo = new ReportApi();

            string serverPath = "https://miid.co.za";
            string drivepath = @"d:\danieltemp";
            try
            {
                repo.PopulateTicketReportMulti(ticketId, serverPath, drivepath, subdomain:"miid");

                ReportRepository reportRepository = new ReportRepository();

                foreach (var ticketID in reportRepository.GetAllTicketsInThisPurchase(ticketId))
                {
                    repo.PopulateTicketReport(ticketID, serverPath, drivepath, subdomain: "miid");
                }

            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "success";
        }
    }
}
