using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MiidWeb;

namespace MiidWeb.Controllers
{
    public class EventOrganisersControllerBak : Controller
    {
        private MiidEntities db = new MiidEntities();

        // GET: EventOrganisers
        public ActionResult Index()
        {
            return View(db.EventOrganisers.ToList());
        }

        public ActionResult ThankYou()
        {
            return View();
        }

        // GET: EventOrganisers/Details/5
        public ActionResult Details(int? id)
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

        // GET: EventOrganisers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EventOrganisers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CompanyName,ContactName,Telephone,Email,SecondaryContactName,SecondaryContactTelephone,SecondaryContactEmail,StartDate,Bank,BankAccountNumber,BankAccountHolder,BranchCode,BankType")] EventOrganiser eventOrganiser)
        {
            if (ModelState.IsValid)
            {
                db.EventOrganisers.Add(eventOrganiser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(eventOrganiser);
        }


        // GET: EventOrganisers/Create
        public ActionResult Register()
        {
            return View();
        }

        // POST: EventOrganisers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "ID,CompanyName,ContactName,Telephone,Email,SecondaryContactName,SecondaryContactTelephone,SecondaryContactEmail,StartDate,Bank,BankAccountNumber,BankAccountHolder,BranchCode,BankType")] EventOrganiser eventOrganiser)
        {
            if (ModelState.IsValid)
            {
                db.EventOrganisers.Add(eventOrganiser);
                db.SaveChanges();
                //Email MIID ADMIN

                return RedirectToAction("ThankYou");
            }

            return View(eventOrganiser);
        }




        // GET: EventOrganisers/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: EventOrganisers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CompanyName,ContactName,Telephone,Email,SecondaryContactName,SecondaryContactTelephone,SecondaryContactEmail,StartDate,Bank,BankAccountNumber,BankAccountHolder,BranchCode,BankType")] EventOrganiser eventOrganiser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventOrganiser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eventOrganiser);
        }

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
