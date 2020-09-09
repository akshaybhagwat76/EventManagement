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

namespace MiidWeb.Controllers
{
    public class VendorsController : Controller
    {
        private MiidEntities db = new MiidEntities();

        // GET: Vendors
        public ActionResult Index()
        {
            return View(db.Vendors.ToList());
        }

        // GET: Vendors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // GET: Vendors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vendors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IDNumber,Name,Telephone,Email,ContactName,ContactTelephone,ContactEmail,SecondaryContactName,SecondaryContactTelephone,SecondaryContactEmail,StartDate,Description,Bank,BranchCode,AccountNumber,BankType")] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                db.Vendors.Add(vendor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vendor);
        }


        public ActionResult AddVendorToEvent(int? eventid)
        {
            ViewBag.EventID = eventid;

            var Event = db.Events.Find(eventid);

            ViewBag.EventName = Event.EventName;
            ViewBag.ErrorMessage = "";
            return View();
        }

        // POST: Vendors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddVendorToEvent(VendorViewModel vendor)
        {
            if (ModelState.IsValid)
            {
                Vendor Vendor = new Vendor();
                var Vendors = db.Vendors.Where(x=>x.VendorCode == vendor.VendorCode);
                if (Vendors.Count() > 0)
                {
                    Vendor = Vendors.First();


                    var VendorEvent = new VendorEvent();
                    VendorEvent.EventID = vendor.EventID;
                    VendorEvent.VendorID = Vendor.ID;

                    db.VendorEvents.Add(VendorEvent);
                    db.SaveChanges();
                    return RedirectToAction("EditByEO", "Events", new { id = vendor.EventID });
                }
                ViewBag.ErrorMessage = "Vendor Not Found.";

                ViewBag.EventID = vendor.EventID;

                var Event = db.Events.Find(vendor.EventID);

                ViewBag.EventName = Event.EventName;

                return View(vendor);
            }

            return View(vendor);
        }


        public ActionResult RemoveFromEvent(int? id, int? eventid)
        {
            
            var ves = db.VendorEvents.Where(x=>x.VendorID == id && x.EventID == eventid);

            if (ves.Count() > 0)
            {
                var ve = db.VendorEvents.Find(ves.First().ID);
                db.VendorEvents.Remove(ve);
                db.SaveChanges();

            }

            return RedirectToAction("EditByEO", "Events", new { id = eventid });
        }

        // POST: Vendors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveFromEvent(VendorViewModel vendor)
        {
            if (ModelState.IsValid)
            {
                Vendor Vendor = new Vendor();
                var Vendors = db.Vendors.Where(x => x.VendorCode == vendor.VendorCode);
                if (Vendors.Count() > 0)
                {
                    Vendor = Vendors.First();


                    var VendorEvent = new VendorEvent();
                    VendorEvent.EventID = vendor.EventID;
                    VendorEvent.VendorID = Vendor.ID;

                    db.VendorEvents.Add(VendorEvent);
                    db.SaveChanges();
                    return RedirectToAction("EditByEO", "Events", new { id = vendor.EventID });
                }
                ViewBag.ErrorMessage = "Vendor Not Found.";

                ViewBag.EventID = vendor.EventID;

                var Event = db.Events.Find(vendor.EventID);

                ViewBag.EventName = Event.EventName;

                return View(vendor);
            }

            return View(vendor);
        }






        // GET: Vendors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // POST: Vendors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IDNumber,Name,Telephone,Email,ContactName,ContactTelephone,ContactEmail,SecondaryContactName,SecondaryContactTelephone,SecondaryContactEmail,StartDate,Description,Bank,BranchCode,AccountNumber,BankType")] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vendor);
        }

        // GET: Vendors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // POST: Vendors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vendor vendor = db.Vendors.Find(id);
            db.Vendors.Remove(vendor);
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
