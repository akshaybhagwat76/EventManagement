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
    public class TicketClassSeatRangesController : Controller
    {
        private MiidEntities db = new MiidEntities();

        // GET: TicketClassSeatRanges
        public ActionResult Index()
        {
            var ticketClassSeatRanges = db.TicketClassSeatRanges.Include(t => t.TicketClass);
            return View(ticketClassSeatRanges.ToList());
        }

        // GET: TicketClassSeatRanges/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketClassSeatRange ticketClassSeatRange = db.TicketClassSeatRanges.Find(id);
            if (ticketClassSeatRange == null)
            {
                return HttpNotFound();
            }
            return View(ticketClassSeatRange);
        }

        // GET: TicketClassSeatRanges/Create
        public ActionResult Create()
        {
            ViewBag.TicketClassID = new SelectList(db.TicketClasses, "ID", "Code");
            return View();
        }

        // POST: TicketClassSeatRanges/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TicketClassID,RowNumber,FromSeatNumber,ToSeatNumber")] TicketClassSeatRange ticketClassSeatRange)
        {
            if (ModelState.IsValid)
            {
                db.TicketClassSeatRanges.Add(ticketClassSeatRange);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TicketClassID = new SelectList(db.TicketClasses, "ID", "Code", ticketClassSeatRange.TicketClassID);
            return View(ticketClassSeatRange);
        }

        // GET: TicketClassSeatRanges/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketClassSeatRange ticketClassSeatRange = db.TicketClassSeatRanges.Find(id);
            if (ticketClassSeatRange == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketClassID = new SelectList(db.TicketClasses, "ID", "Code", ticketClassSeatRange.TicketClassID);
            return View(ticketClassSeatRange);
        }

        // POST: TicketClassSeatRanges/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TicketClassID,RowNumber,FromSeatNumber,ToSeatNumber")] TicketClassSeatRange ticketClassSeatRange)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketClassSeatRange).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketClassID = new SelectList(db.TicketClasses, "ID", "Code", ticketClassSeatRange.TicketClassID);
            return View(ticketClassSeatRange);
        }

        // GET: TicketClassSeatRanges/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketClassSeatRange ticketClassSeatRange = db.TicketClassSeatRanges.Find(id);
            if (ticketClassSeatRange == null)
            {
                return HttpNotFound();
            }
            return View(ticketClassSeatRange);
        }

        // POST: TicketClassSeatRanges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketClassSeatRange ticketClassSeatRange = db.TicketClassSeatRanges.Find(id);
            db.TicketClassSeatRanges.Remove(ticketClassSeatRange);
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
