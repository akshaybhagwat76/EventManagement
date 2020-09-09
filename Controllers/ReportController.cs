using MiidWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiidWeb.Controllers
{
    public class ReportController : Controller
    {

        public RedirectResult ReportPromoCodes(int id)
        {

            string utl = String.Format(@"~/PromoCodeReport.aspx?EI={0}", id);
            return Redirect(utl);
        }


        // GET: Report
        public ActionResult Index(int id)
        {

            List<SelectListItem> list = new List<SelectListItem>();

            ReportRepository repo = new ReportRepository();
            list.Add(new SelectListItem { Text = "--Summary--", Value = "0" });
            foreach (var s in repo.PosIDsForEvent(id))
            {
                list.Add(new SelectListItem { Text = s, Value = s });
            }
            ViewBag.PosID = list;
            ViewBag.EventID = id;
            return View();
        }
        // POST: Friends/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
      
        public RedirectResult Index(int EventID, string PosID, DateTime? FromDate, DateTime? ToDate, DateTime? FromTime, DateTime? ToTime)
        {

            DateTime? CombineFrom=null;
            if (FromDate != null)
            {
                if (FromTime != null)
                {
                    CombineFrom = DateTime.Parse(FromDate.Value.ToString("yyyy-MM-dd") + " " + FromTime.Value.ToString("HH:mm"));
                }
                else
                {
                    CombineFrom = DateTime.Parse(FromDate.Value.ToString("yyyy-MM-dd"));
                }
            }
            DateTime? CombineTo=null;
            if (ToDate != null)
            {
                if (ToTime != null)
                {
                    CombineTo = DateTime.Parse(ToDate.Value.ToString("yyyy-MM-dd") + " " + ToTime.Value.ToString("HH:mm"));
                }
                else
                {
                    CombineTo = DateTime.Parse(ToDate.Value.ToString("yyyy-MM-dd"));
                }
            }

            string utl = String.Format(@"~/BoxOfficeReport.aspx?EI={0}&PI={1}&FD={2}&TD={3}", EventID,PosID, CombineFrom, CombineTo);
            return Redirect(utl);
            
        }


    }
}
