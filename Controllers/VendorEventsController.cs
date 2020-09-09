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
    [Authorize(Roles = "Admin")]
    public class VendorEventsController : BaseController
    {
        private MiidEntities db = new MiidEntities();

        // GET: VendorEvents
        public ActionResult Index()
        {
            var vendorEvents = db.VendorEvents.Include(v => v.Event).Include(v => v.Vendor);
            return View(vendorEvents.ToList());
        }

        // GET: VendorEvents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VendorEvent vendorEvent = db.VendorEvents.Find(id);
            if (vendorEvent == null)
            {
                return HttpNotFound();
            }
            return View(vendorEvent);
        }

        // GET: VendorEvents/Create
        public ActionResult Create()
        {
            ViewBag.EventID = new SelectList(db.Events, "ID", "EventName");
            ViewBag.VendorID = new SelectList(db.Vendors, "ID", "IDNumber");
            return View();
        }

        // POST: VendorEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,VendorID,EventID")] VendorEvent vendorEvent)
        {
            if (ModelState.IsValid)
            {
                db.VendorEvents.Add(vendorEvent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventID = new SelectList(db.Events, "ID", "EventName", vendorEvent.EventID);
            ViewBag.VendorID = new SelectList(db.Vendors, "ID", "IDNumber", vendorEvent.VendorID);
            return View(vendorEvent);
        }

        // GET: VendorEvents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VendorEvent vendorEvent = db.VendorEvents.Find(id);
            if (vendorEvent == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventID = new SelectList(db.Events, "ID", "EventName", vendorEvent.EventID);
            ViewBag.VendorID = new SelectList(db.Vendors, "ID", "IDNumber", vendorEvent.VendorID);
            return View(vendorEvent);
        }

        // POST: VendorEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,VendorID,EventID")] VendorEvent vendorEvent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendorEvent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventID = new SelectList(db.Events, "ID", "EventName", vendorEvent.EventID);
            ViewBag.VendorID = new SelectList(db.Vendors, "ID", "IDNumber", vendorEvent.VendorID);
            return View(vendorEvent);
        }

        // GET: VendorEvents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VendorEvent vendorEvent = db.VendorEvents.Find(id);
            if (vendorEvent == null)
            {
                return HttpNotFound();
            }
            return View(vendorEvent);
        }

        // POST: VendorEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VendorEvent vendorEvent = db.VendorEvents.Find(id);
            db.VendorEvents.Remove(vendorEvent);
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
