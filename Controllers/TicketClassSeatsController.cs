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
    public class TicketClassSeatsController : Controller
    {
        private MiidEntities db = new MiidEntities();

        // GET: TicketClassSeats
        public ActionResult Index()
        {
            var ticketClassSeats = db.TicketClassSeats.Include(t => t.Ticket).Include(t => t.TicketClassSeatRange);
            return View(ticketClassSeats.ToList());
        }

        // GET: TicketClassSeats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketClassSeat ticketClassSeat = db.TicketClassSeats.Find(id);
            if (ticketClassSeat == null)
            {
                return HttpNotFound();
            }
            return View(ticketClassSeat);
        }

        // GET: TicketClassSeats/Create
        public ActionResult Create()
        {
            ViewBag.TicketID = new SelectList(db.Tickets, "ID", "TicketNumber");
            ViewBag.TicketClassSeatRangeID = new SelectList(db.TicketClassSeatRanges, "ID", "RowNumber");
            return View();
        }

        // POST: TicketClassSeats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TicketClassID,RowNumber,SeatNumber,StatusID,TicketID,TicketClassSeatRangeID")] TicketClassSeat ticketClassSeat)
        {
            if (ModelState.IsValid)
            {
                db.TicketClassSeats.Add(ticketClassSeat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TicketID = new SelectList(db.Tickets, "ID", "TicketNumber", ticketClassSeat.TicketID);
            ViewBag.TicketClassSeatRangeID = new SelectList(db.TicketClassSeatRanges, "ID", "RowNumber", ticketClassSeat.TicketClassSeatRangeID);
            return View(ticketClassSeat);
        }

        // GET: TicketClassSeats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketClassSeat ticketClassSeat = db.TicketClassSeats.Find(id);
            if (ticketClassSeat == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketID = new SelectList(db.Tickets, "ID", "TicketNumber", ticketClassSeat.TicketID);
            ViewBag.TicketClassSeatRangeID = new SelectList(db.TicketClassSeatRanges, "ID", "RowNumber", ticketClassSeat.TicketClassSeatRangeID);
            return View(ticketClassSeat);
        }

        // POST: TicketClassSeats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TicketClassID,RowNumber,SeatNumber,StatusID,TicketID,TicketClassSeatRangeID")] TicketClassSeat ticketClassSeat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketClassSeat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketID = new SelectList(db.Tickets, "ID", "TicketNumber", ticketClassSeat.TicketID);
            ViewBag.TicketClassSeatRangeID = new SelectList(db.TicketClassSeatRanges, "ID", "RowNumber", ticketClassSeat.TicketClassSeatRangeID);
            return View(ticketClassSeat);
        }

        // GET: TicketClassSeats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketClassSeat ticketClassSeat = db.TicketClassSeats.Find(id);
            if (ticketClassSeat == null)
            {
                return HttpNotFound();
            }
            return View(ticketClassSeat);
        }

        // POST: TicketClassSeats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketClassSeat ticketClassSeat = db.TicketClassSeats.Find(id);
            db.TicketClassSeats.Remove(ticketClassSeat);
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
