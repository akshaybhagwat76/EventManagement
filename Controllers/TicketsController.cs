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
using MiidWeb.Repositories;
using MiidWeb.Helpers;
using PagedList;
namespace MiidWeb.Controllers
{
    [Authorize]

    public class TicketsController : BaseController
    {
        private MiidEntities db = new MiidEntities();

        [Authorize(Roles = "Admin")]
        public ActionResult QuickAddTicket()
        {
            QuickAddTicketViewModel model = new QuickAddTicketViewModel();

            model.EventList = new SelectList(db.Events.OrderBy(c => c.EventName), "ID", "EventName");
            model.TicketClassList = new SelectList(db.TicketClasses.OrderBy(c => c.Description), "ID", "Description");

            return View(model);
        }
        private void GetLayoutFiles()
        {

            string host = HttpContext.Request.Url.Host;

            // string company = host.Split('.')[0].ToString();
            string company = host;

            // if (company.Contains("www")) {  company = "training.miid.co.za"; }
           if (company.Contains("localhost")) { company = "miid.co.za"; }

            GlobalVariables.Company = company;

            int subDomainID = LookupHelper.GetSubdomainID(company);
            using (var db = new MiidEntities())
            {
                Subdomain subDomain = db.Subdomains.Find(subDomainID);
                if (subDomain != null)
                {
                    GlobalVariables.SubdomainID = subDomain.ID.ToString();
                    GlobalVariables.Text1 = subDomain.Text1;
                    GlobalVariables.Text2 = subDomain.Text2;
                    GlobalVariables.Text3 = subDomain.Text3;
                    GlobalVariables.Text4 = subDomain.Text4;
                    GlobalVariables.Text5 = subDomain.Text5;
                    GlobalVariables.Text6 = subDomain.Text6;

                }
            }
            if (company == "miid")
            {
                GlobalVariables.LayoutLogin = "~/Views/Shared/_LayoutLogin.cshtml";
                GlobalVariables.LayoutAdmin = "~/Views/Shared/_LayoutAdmin.cshtml";
				
				GlobalVariables.Layout = MiidWeb.Repositories.TicketRepository.GetLayoutFile("Layout");

            }
            else
            {
                GlobalVariables.LayoutLogin = String.Format("~/Views/Shared/_LayoutLogin{0}.cshtml", company);
                GlobalVariables.LayoutAdmin = String.Format("~/Views/Shared/_LayoutAdmin{0}.cshtml", company);
                GlobalVariables.Layout = String.Format("~/Views/Shared/_Layout{0}.cshtml", company);

            }




        }


        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QuickAddTicket(QuickAddTicketViewModel model)
        {
            if (ModelState.IsValid)
            {
                GetLayoutFiles();
                string subdomain = GlobalVariables.Company;

                Ticket ticket = new Ticket();
                ticket.TicketClassID = model.TicketClassID;
                List<TicketViewModel> list = new List<TicketViewModel>();

                int UserID = EndUserRepository.GetByUserName(model.UserName).ID;
                ticket.EndUserID = UserID;

                int x = 0; int Total = 0;

                string UniquePaymentRef = System.Guid.NewGuid().ToString().Replace("-","").Substring(0,10);

                List<string> sessionTickets = new List<string>();
                if (model.Quantity > 0)
                {
                    while (x < model.Quantity)
                    {
                        //UPdate running quantity
                        var ticketClassAffected = db.TicketClasses.Find(ticket.TicketClassID);


                        ticket.StatusID = 5;
                        ticket.TicketNumber = TicketRepository.GenerateTicketNumber(sessionTickets);
                        ticket.TicketPurchasePrice = ticketClassAffected.Price;
                        ticket.UniquePaymentID = UniquePaymentRef;
                        ticket.DatetimePurchased = DateTime.Now;
                        db.Tickets.Add(ticket);
                        db.SaveChanges();

                       
                        ticketClassAffected.RunningQuantity = ticketClassAffected.RunningQuantity - 1;
                        db.Entry(ticketClassAffected).State = EntityState.Modified;
                        db.SaveChanges();

                        sessionTickets.Add(ticket.TicketNumber);
                        x++;
                        var ticketClassVM = new TicketClassViewModel(model.TicketClassID);
                        var ticketVM = new TicketViewModel(ticket.ID);
                        Total += (int) ticketClassVM.TicketClass.Price;
                        list.Add(ticketVM);
                    }
                    string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                    MyMoneyRepository.SendConfirmationMiiFunds_TicketPurchaseWS(UserID, TotalAmountInRands: Total, PaymentDate: DateTime.Now, boughtTickets: list, serverPath: baseUrl, subdomain: subdomain);

                    Growl("MIID ADMIN", "Quick Add Tickets Purchased Successfully.");
                    return RedirectToAction("Index", "Home");
                }
            }


            return RedirectToAction("Index", "Home");
        }

        public JsonResult GetTicketClasses(string id)
        {
            List<TicketClass> cats = new List<TicketClass>();
            List<SelectListItem> subdistricts = new List<SelectListItem>();
            var division = db.Events.Find(int.Parse(id));
            cats = db.TicketClasses.Where(x => x.EventID == division.ID).ToList();
            foreach (var c in cats)
            {
                subdistricts.Add(new SelectListItem { Text = c.Description, Value = c.ID.ToString() });
            }
            return Json(new SelectList(subdistricts, "Value", "Text"));
        }

        // GET: Tickets
        [Authorize(Roles = "Admin")]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {


            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";


            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var tickets = db.Tickets.Include(t => t.TicketClass).Include(t => t.EndUser);

            if (!String.IsNullOrEmpty(searchString))
            {
                tickets = tickets.Where(s => s.TicketNumber.Contains(searchString)
                                       || s.UniquePaymentID.Contains(searchString)
                                       || s.EndUser.Firstname.Contains(searchString)
                                       || s.EndUser.Surname.Contains(searchString)
                                       || s.EndUser.IDNumber.Contains(searchString)
                                       || s.EndUser.Email.Contains(searchString)

                                       );


            }

            switch (sortOrder)
            {
                case "name_desc":
                    tickets = tickets.OrderByDescending(s => s.TicketNumber);
                    break;
                case "Date":
                    tickets = tickets.OrderBy(s => s.UniquePaymentID);
                    break;
                case "date_desc":
                    tickets = tickets.OrderByDescending(s => s.DatetimePurchased);
                    break;
                default:  // Name ascending 
                    tickets = tickets.OrderBy(s => s.DatetimeRedeemed);
                    break;
            }

            int pageSize = 30;
            int pageNumber = (page ?? 1);



            return View(tickets.ToPagedList(pageNumber, pageSize));
        }

        // GET: Tickets/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketClassID = new SelectList(db.TicketClasses, "ID", "Code", ticket.TicketClassID);
            ViewBag.EndUserID = new SelectList(db.EndUsers, "ID", "Surname", ticket.EndUserID);
            return View(ticket);
        }


		// POST: Tickets/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Details([Bind(Include = "ID,TicketNumber,EndUserID,StatusID,TicketPurchasePrice,TicketClassID,DatetimePurchased,DatetimeReserved,DatetimeRedeemed,Hash, FirstName")] Ticket ticket)
		{
			if (ModelState.IsValid)
			{
				db.Entry(ticket).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.TicketClassID = new SelectList(db.TicketClasses, "ID", "Code", ticket.TicketClassID);
			ViewBag.EndUserID = new SelectList(db.EndUsers, "ID", "Surname", ticket.EndUserID);
           

            return View(ticket);
		}

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.TicketClassID = new SelectList(db.TicketClasses, "ID", "Code");
            ViewBag.EndUserID = new SelectList(db.EndUsers, "ID", "Surname");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ID,TicketNumber,EndUserID,StatusID,TicketPurchasePrice,TicketClassID,DatetimePurchased,DatetimeReserved,DatetimeRedeemed,Hash,UniquePaymentID")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TicketClassID = new SelectList(db.TicketClasses, "ID", "Code", ticket.TicketClassID);
            ViewBag.EndUserID = new SelectList(db.EndUsers, "ID", "Surname", ticket.EndUserID);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {   

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }

            ViewBag.TicketClassID = new SelectList(db.TicketClasses, "ID", "Code", ticket.TicketClassID);
            ViewBag.EndUserID = new SelectList(db.EndUsers, "ID", "Surname", ticket.EndUserID);
            ViewBag.StatusID = new SelectList
                 (db.Status.Where(u => u.Context.Contains("Ticket")
                 && !u.Code.Contains("Class")
                 
                 
                 ), "ID", "code", ticket.StatusID);



            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ID,TicketNumber,EndUserID,StatusID,TicketPurchasePrice,TicketClassID,DatetimePurchased,DatetimeReserved,DatetimeRedeemed,Hash,UniquePaymentID")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketClassID = new SelectList(db.TicketClasses, "ID", "Code", ticket.TicketClassID);
            ViewBag.EndUserID = new SelectList(db.EndUsers, "ID", "Surname", ticket.EndUserID);
           
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}
