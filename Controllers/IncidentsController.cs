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
    public class IncidentsController : Controller
    {
        private MiidEntities db = new MiidEntities();

        // GET: Incidents
        public ActionResult Index()
        {
            var incidents = db.Incidents.Include(i => i.Event).Include(i => i.Staff).Include(i => i.Status);
            return View(incidents.ToList());
        }

        // GET: Incidents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incident incident = db.Incidents.Find(id);
            if (incident == null)
            {
                return HttpNotFound();
            }
            return View(incident);
        }

        // GET: Incidents/Create
        public ActionResult Create()
        {
            ViewBag.EventID = new SelectList(db.Events, "ID", "EventName");
            ViewBag.StaffID = new SelectList(db.Staffs, "ID", "Code");
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code");
            return View();
        }

        // POST: Incidents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IncidentDateTime,StatusID,EventID,StaffID")] Incident incident)
        {
            if (ModelState.IsValid)
            {
                db.Incidents.Add(incident);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventID = new SelectList(db.Events, "ID", "EventName", incident.EventID);
            ViewBag.StaffID = new SelectList(db.Staffs, "ID", "Code", incident.StaffID);
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code", incident.StatusID);
            return View(incident);
        }

        // GET: Incidents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incident incident = db.Incidents.Find(id);
            if (incident == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventID = new SelectList(db.Events, "ID", "EventName", incident.EventID);
            ViewBag.StaffID = new SelectList(db.Staffs, "ID", "Code", incident.StaffID);
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code", incident.StatusID);
            return View(incident);
        }

        // POST: Incidents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IncidentDateTime,StatusID,EventID,StaffID")] Incident incident)
        {
            if (ModelState.IsValid)
            {
                db.Entry(incident).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventID = new SelectList(db.Events, "ID", "EventName", incident.EventID);
            ViewBag.StaffID = new SelectList(db.Staffs, "ID", "Code", incident.StaffID);
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code", incident.StatusID);
            return View(incident);
        }

        // GET: Incidents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incident incident = db.Incidents.Find(id);
            if (incident == null)
            {
                return HttpNotFound();
            }
            return View(incident);
        }

        // POST: Incidents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Incident incident = db.Incidents.Find(id);
            db.Incidents.Remove(incident);
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
