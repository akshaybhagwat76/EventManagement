using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MiidWeb;
using MiidWeb.Repositories;
using MiidWeb.Models;
using System.IO;
using MiidWeb.Helpers;
using System.Text;

namespace MiidWeb.Controllers
{
    [Authorize]
    public class EventOrganisersController : BaseController
    {
        private MiidEntities db = new MiidEntities();
        private void GetLayoutFiles()
        {

            string host = HttpContext.Request.Url.Host;

            //  string company = host.Split('.')[0].ToString();
            string company = host;

            //  if (company.Contains("www")) {  company = "training.miid.co.za"; }
           if (company.Contains("localhost")) { company = "www.lulatickets.miid.co.za"; }


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

        public ActionResult EOTicketReport(int? EventID=0, string ticketnumber = null, string firstname = null, string surname = null, string idnumber=null, DateTime? fromdate = null, DateTime? todate= null, DateTime? ticketstartdate=null)
        {
            List<EOTicketReport> list = EventOrganiserRepository.GetEOTicketReport(EventID ?? 0,fromdate, todate, firstname, surname, idnumber, ticketnumber ,ticketstartdate);

            ViewBag.EventID = EventID;
            StringBuilder sb = new StringBuilder();
            foreach (var i in list)
            {
                sb.Append(String.Format("{0},", i.TicketID));

            }

            ViewBag.TicketIDList = sb.ToString();
            return View(list);
        }


		public ActionResult EORemarketingReport(int? EventID = 0, string ticketnumber = null, string firstname = null, string surname = null, string idnumber = null, DateTime? fromdate = null, DateTime? todate = null, DateTime? ticketstartdate = null)
		{
			List<EORemarketingReport> list = EventOrganiserRepository.GetRemarketinReport(EventID ?? 0, fromdate, todate, firstname, surname, idnumber, ticketnumber, ticketstartdate);

			ViewBag.EventID = EventID;
			StringBuilder sb = new StringBuilder();
			

			ViewBag.TicketIDList = sb.ToString();
			return View(list);
		}

		

		public EmptyResult ExportSearchResults(string TicketIDList="")
        {
            string csv = ExportCsvRepository.ExportCSV("EOTicketReportExport", true, TicketIDList);
            //Download the CSV file.
            DownloadCsv(csv);

            return null;
        }

        private void DownloadCsv(string csv)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", String.Format("attachment;filename=TicketReport_{0}.csv", DateTime.Now.ToString("yyyy-MM-dd")));
            Response.Charset = "";
            Response.ContentType = "application/text";
            Response.Output.Write(csv);
            Response.Flush();
            Response.End();
        }

		public EmptyResult ExportRemarketingResults(string TicketIDList = "")
		{
			string csv = ExportCsvRepository.ExportCSV("EORemarketingReportExport", true, TicketIDList);
			//Download the CSV file.
			DownloadCsv2(csv);

			return null;
		}

		private void DownloadCsv2(string csv)
		{
			Response.Clear();
			Response.Buffer = true;
			Response.AddHeader("content-disposition", String.Format("attachment;filename=TicketReport_{0}.csv", DateTime.Now.ToString("yyyy-MM-dd")));
			Response.Charset = "";
			Response.ContentType = "application/text";
			Response.Output.Write(csv);
			Response.Flush();
			Response.End();
		}



		[HttpPost]
        public ActionResult EOTicketReport(FormCollection form, int EventID)
        {
            
                 int UsedStatusID = StatusHelper.StatusID("Ticket", "Used");
                 

            int x = 1;

            int PermissionCount = int.Parse(form["hdnRowCount"].ToString());

            while (x <= PermissionCount)
            {
                int TicketID = int.Parse(form["TicketID_" + x.ToString()]);
                           
                var Selected = form["chk_" + x.ToString()];
               

                var bt = db.Tickets.Find(TicketID);


                if (Selected == "true,false" || Selected == "on") //then true
                {                     
                        bt.StatusID = UsedStatusID;
                        bt.DatetimeRedeemed = DateTime.Now;
                        db.Entry(bt).State = EntityState.Modified;
                        db.SaveChanges();
                 
                }
               
                x++;
            }
            
            return RedirectToAction("EOTicketReport", new { EventID = EventID });
        }
		







		// GET: EventOrganisers
		[Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var eventOrganisers = db.EventOrganisers.Include(e => e.Status);
            return View(eventOrganisers.ToList());
        }
        [Authorize(Roles = "Admin")]
        public ActionResult IndexList()
        {
            var eventOrganisers = db.EventOrganisers.Include(e => e.Status);
            return View(eventOrganisers.ToList());
        }
        [AllowAnonymous]
        public ActionResult ThankYou()
        {
            return View();
        }
        // GET: EventOrganisers/Details/5
        public ActionResult Details(int? id)
        {
            //UserModel user = EndUserRepository.GetUser(User.Identity.Name);
            var eo = EventOrganiserRepository.GetEventOrganiserByEmail(User.Identity.Name);
           

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventOrganiser eventOrganiser = db.EventOrganisers.Find(id);
            if (eventOrganiser == null)
            {
                return HttpNotFound();
            }
            return View(eventOrganiser);
        }
        public ActionResult MyDetails()
        {
            //UserModel user = EndUserRepository.GetUser(User.Identity.Name);
            var eventOrganiser = EventOrganiserRepository.GetEventOrganiserByEmail(User.Identity.Name);

            if (eventOrganiser == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EventOrganiserViewModel eovm = new EventOrganiserViewModel(eventOrganiser.ID);
            return View(eovm);
            //return View("MyDetailsNoTickets", eovm);
        }

        //[Authorize(Roles = "Admin")]
        // GET: EventOrganisers/Create
        public ActionResult Create()
        {
            ViewBag.StatusID = new SelectList(db.Status.Where(x => x.Context == "EO"), "ID", "Code");
            return View();
        }

        // POST: EventOrganisers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CompanyName,ContactName,Telephone,Email,SecondaryContactName,SecondaryContactTelephone,SecondaryContactEmail,StartDate,Bank,BankAccountNumber,BankAccountHolder,BranchCode,BankType,DateCreated,DateUpdated,StatusID")] EventOrganiser eventOrganiser)
        {

            eventOrganiser.StartDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.EventOrganisers.Add(eventOrganiser);
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = eventOrganiser.ID});
            }

            ViewBag.StatusID = new SelectList(db.Status.Where(x => x.Context == "EO"), "ID", "Code", eventOrganiser.StatusID);
            return View(eventOrganiser);
        }



		public ActionResult EventOrganiserCreate()

		{
			ViewBag.StatusID = new SelectList(db.Status.Where(x => x.Context == "EO"), "ID", "Code");
			return View();

		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EventOrganiserCreate([Bind(Include = "ID,CompanyName,ContactName,Telephone,Email,SecondaryContactName,SecondaryContactTelephone,SecondaryContactEmail,StartDate,Bank,BankAccountNumber,BankAccountHolder,BranchCode,BankType,DateCreated,DateUpdated,StatusID")] EventOrganiser eventOrganiser)
		{

            eventOrganiser.StartDate = DateTime.Now;

			if (ModelState.IsValid)
			{
				db.EventOrganisers.Add(eventOrganiser);
				db.SaveChanges();
				return RedirectToAction("IndexList");
			}

			ViewBag.StatusID = new SelectList(db.Status.Where(x => x.Context == "EO"), "ID", "Code", eventOrganiser.StatusID);
			return View(eventOrganiser);
		}



		// GET: EventOrganisers/Create
		[AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // POST: EventOrganisers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Register([Bind(Include = "ID,CompanyName,ContactName,Telephone,Email,SecondaryContactName,SecondaryContactTelephone,SecondaryContactEmail,StartDate,Bank,BankAccountNumber,BankAccountHolder,BranchCode,BankType,DateCreated,DateUpdated,StatusID")] EventOrganiser eventOrganiser)
        {
            if (ModelState.IsValid)
            {
                //db.EventOrganisers.Add(eventOrganiser);
                //db.SaveChanges();
                //Email MIID ADMIN
                Helpers.EmailHelper.SendMail("info@miid.co.za", "Registration Application: Event Organiser", "A new event organiser applied for registration: " + eventOrganiser.CompanyName + ' ' + eventOrganiser.Email +' '+ eventOrganiser.Telephone +' ' + eventOrganiser.ContactName, null, null, null, ConfigRepo.GetSubdomainID());
				Helpers.EmailHelper.SendMail("lead.ozutb7@zapiermail.com", "Registration Application: Event Organiser", "A new event organiser applied for registration: " + eventOrganiser.CompanyName + ' ' + eventOrganiser.Email + ' ' + eventOrganiser.Telephone + ' ' + eventOrganiser.ContactName, null, null, null, ConfigRepo.GetSubdomainID());

				//Helpers.EmailHelper.SendMail("jonathanmwallis1808@gmail.com", "Registration Application: Event Organiser", "A new event organiser applied for registration: " + eventOrganiser.CompanyName + ' ' + eventOrganiser.Email);
				//Helpers.EmailHelper.SendMail("dsouchon@gmail.com", "Registration Application: Event Organiser", "A new event organiser applied for registration: " + eventOrganiser.CompanyName + ' ' + eventOrganiser.Email);
				//Email Event Organsiser
				Helpers.EmailHelper.SendMail(eventOrganiser.Email, "Mi-iD Registration Application Received: Event Organiser", "Thank you for your application, " + eventOrganiser.ContactName + "! A representative will be in touch with you shortly.", null, null, null, ConfigRepo.GetSubdomainID());

                return RedirectToAction("ThankYou");
            }
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code", eventOrganiser.StatusID);
            return View(eventOrganiser);
        }

        // GET: EventOrganisers/Create
        [AllowAnonymous]
        public ActionResult Register2()
        {
            return View();
        }

        public ActionResult MiiDetailsNew()
        {
            return View();
        }

        public ActionResult BankingDetailsView()

        {    var eventOrganiser = EventOrganiserRepository.GetEventOrganiserByEmail(User.Identity.Name);

            
            if (eventOrganiser == null)
            {
                return HttpNotFound();
            }
            
            return View(eventOrganiser);
            
        }

		

		[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BankingDetailsView([Bind(Include = "ID,CompanyName,ContactName,Telephone,Email,SecondaryContactName,SecondaryContactTelephone,SecondaryContactEmail,StartDate,Bank,BankAccountNumber,BankAccountHolder,BranchCode,BankType,DateCreated,DateUpdated,StatusID,ProfilePicURL,HandHeldLoginPassword")] EventOrganiser eventOrganiser, string imageData, string ProfilePicURL)
        {
            if (!String.IsNullOrEmpty(imageData))
            {
                var bytes = Convert.FromBase64String(imageData);
                var fileName = String.Format("{0}_{1}.png", eventOrganiser.ID.ToString(), eventOrganiser.CompanyName.Trim().Replace(" ", ""));
                var filePath = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                using (var imageFile = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }
                eventOrganiser.ProfilePicURL = fileName;
            }
            else
            {
                eventOrganiser.ProfilePicURL = ProfilePicURL;
            }



            if (ModelState.IsValid)
            {
                db.Entry(eventOrganiser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MyDetails");
            }

            return View(eventOrganiser);
        }

        // POST: EventOrganisers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Register2([Bind(Include = "ID,CompanyName,ContactName,Telephone,Email,SecondaryContactName,SecondaryContactTelephone,SecondaryContactEmail,StartDate,Bank,BankAccountNumber,BankAccountHolder,BranchCode,BankType,DateCreated,DateUpdated,DateTimeUpdated,StatusID")] EventOrganiser eventOrganiser)
        {
            if (ModelState.IsValid)
            {
                //db.EventOrganisers.Add(eventOrganiser);
                //db.SaveChanges();
                //Email MIID ADMIN
                Helpers.EmailHelper.SendMail("info@miid.co.za", "Registration Application: Event Organiser", "A new event organiser applied for registration: " + eventOrganiser.CompanyName + ' ' + eventOrganiser.Email + ' ' + eventOrganiser.Telephone, null, null, null, ConfigRepo.GetSubdomainID());
				Helpers.EmailHelper.SendMail("lead.ozutb7@zapiermail.com", "Registration Application: Event Organiser", "A new event organiser applied for registration: " + eventOrganiser.CompanyName + ' ' + eventOrganiser.Email + ' ' + eventOrganiser.Telephone + ' ' + eventOrganiser.ContactName, null, null, null, ConfigRepo.GetSubdomainID());

				//Helpers.EmailHelper.SendMail("jonathanmwallis1808@gmail.com", "Registration Application: Event Organiser", "A new event organiser applied for registration: " + eventOrganiser.CompanyName + ' ' + eventOrganiser.Email);
				//Helpers.EmailHelper.SendMail("dsouchon@gmail.com", "Registration Application: Event Organiser", "A new event organiser applied for registration: " + eventOrganiser.CompanyName + ' ' + eventOrganiser.Email);
				//Email Event Organsiser
				Helpers.EmailHelper.SendMail(eventOrganiser.Email, "Mi-iD Registration Application Received: Event Organiser", "Thank you for your application, " + eventOrganiser.ContactName + "! A Mi-id representative will be in touch with you shortly.", null, null, null, ConfigRepo.GetSubdomainID());
                eventOrganiser.DateCreated = DateTime.Now;
				db.EventOrganisers.Add(eventOrganiser);
				db.SaveChanges();
				return RedirectToAction("ThankYou");
            }
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code", eventOrganiser.StatusID);
            return View(eventOrganiser);
        }


		


		[Authorize(Roles = "Admin")]
        // GET: EventOrganisers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //EventOrganiser eventOrganiser = db.EventOrganisers.Find(id);
            var model = new EventOrganiserViewModel(id??0);
            model.ResetPasswordTrue = false;
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.StatusID = new SelectList(db.Status.Where(x=>x.Context == "EO"), "ID", "Code", model.EventOrganiser.StatusID);
            return View(model);
        }

        // POST: EventOrganisers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( EventOrganiser eventOrganiser, bool ResetPasswordTrue = false)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventOrganiser).State = EntityState.Modified;
                db.SaveChanges();

                if (ResetPasswordTrue)
                {
                    EventOrganiserRepository.CreateLoginForEventOrganiser(eventOrganiser.Email);//this creates a login and sets password to Password1!
                }
                Growl("Event Organiser Updated", "Email the event organiser to give password: Password1!");

                return RedirectToAction("IndexList");
            }
            ViewBag.StatusID = new SelectList(db.Status.Where(x => x.Context == "EO"), "ID", "Code", eventOrganiser.StatusID);
            return View(eventOrganiser);
        }

		// GET: EventOrganisers/Edit/5
		public ActionResult PinDetails()
		{
			var eventOrganiser = EventOrganiserRepository.GetEventOrganiserByEmail(User.Identity.Name);


			if (eventOrganiser == null)
			{
				return HttpNotFound();
			}

			return View(eventOrganiser);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult PinDetails([Bind(Include = "ID,CompanyName,ContactName,Telephone,Email,SecondaryContactName,SecondaryContactTelephone,SecondaryContactEmail,StartDate,Bank,BankAccountNumber,BankAccountHolder,BranchCode,BankType,DateCreated,DateUpdated,StatusID,ProfilePicURL,HandHeldLoginPassword")] EventOrganiser eventOrganiser, string imageData, string ProfilePicURL)
		{
			if (!String.IsNullOrEmpty(imageData))
			{
				var bytes = Convert.FromBase64String(imageData);
				var fileName = String.Format("{0}_{1}.png", eventOrganiser.ID.ToString(), eventOrganiser.CompanyName.Trim().Replace(" ", ""));
				var filePath = Path.Combine(Server.MapPath("~/Uploads"), fileName);
				using (var imageFile = new FileStream(filePath, FileMode.Create))
				{
					imageFile.Write(bytes, 0, bytes.Length);
					imageFile.Flush();
				}
				eventOrganiser.ProfilePicURL = fileName;
			}
			else
			{
				eventOrganiser.ProfilePicURL = ProfilePicURL;
			}



			if (ModelState.IsValid)
			{
				db.Entry(eventOrganiser).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("MyDetails");
			}

			return View(eventOrganiser);
		}

		// GET: EventOrganisers/Edit/5
		public ActionResult EditMyDetails()
        {
            var eventOrganiser = EventOrganiserRepository.GetEventOrganiserByEmail(User.Identity.Name);

            
            if (eventOrganiser == null)
            {
                return HttpNotFound();
            }
            
            return View(eventOrganiser);
        }

        // POST: EventOrganisers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMyDetails([Bind(Include = "ID,CompanyName,ContactName,Telephone,Email,SecondaryContactName,SecondaryContactTelephone,SecondaryContactEmail,StartDate,Bank,BankAccountNumber,BankAccountHolder,BranchCode,BankType,DateCreated,DateUpdated,StatusID,ProfilePicURL,HandHeldLoginPassword")] EventOrganiser eventOrganiser, string imageData, string ProfilePicURL)
        {
            if (!String.IsNullOrEmpty(imageData))
            {
                var bytes = Convert.FromBase64String(imageData);
                var fileName = String.Format("{0}_{1}.png", eventOrganiser.ID.ToString(), eventOrganiser.CompanyName.Trim().Replace(" ",""));
                var filePath = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                using (var imageFile = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }
                eventOrganiser.ProfilePicURL = fileName;
            }
            else
            {
                eventOrganiser.ProfilePicURL = ProfilePicURL;
            }



            if (ModelState.IsValid)
            {
                db.Entry(eventOrganiser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MyDetails");
            }
            
            return View(eventOrganiser);
        }



        [Authorize(Roles = "Admin")]
        // GET: EventOrganisers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventOrganiser eventOrganiser = db.EventOrganisers.Find(id);
            if (eventOrganiser == null)
            {
                return HttpNotFound();
            }
            return View(eventOrganiser);
        }
        [Authorize(Roles = "Admin")]
        // POST: EventOrganisers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EventOrganiser eventOrganiser = db.EventOrganisers.Find(id);
            db.EventOrganisers.Remove(eventOrganiser);
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
