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

namespace MiidWeb.Controllers
{
    public class FriendsController : Controller
    {
        private MiidEntities db = new MiidEntities();

        // GET: Friends
        public ActionResult Index()
        {
            var friends = db.Friends.Include(f => f.EndUser).Include(f => f.EndUser1);
            return View(friends.ToList());
        }

        
        public ActionResult IndexByEndUser(int? id)
        {
            var friends = EndUserRepository.GetMyFriends((int)id);
                        
            return View(friends);
        }

        // GET: Friends/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Friend friend = db.Friends.Find(id);
            if (friend == null)
            {
                return HttpNotFound();
            }
            return View(friend);
        }

        // GET: Friends/Create
        public ActionResult Create()
        {
            ViewBag.InitiatingUserID = new SelectList(db.EndUsers, "ID", "Surname");
            ViewBag.AcceptingUserID = new SelectList(db.EndUsers, "ID", "Surname");
            return View();
        }

        // POST: Friends/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,InitiatingUserID,AcceptingUserID,DateRequested,DateAccepted,DateRejected,StatusID")] Friend friend)
        {
            if (ModelState.IsValid)
            {
                db.Friends.Add(friend);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InitiatingUserID = new SelectList(db.EndUsers, "ID", "Surname", friend.InitiatingUserID);
            ViewBag.AcceptingUserID = new SelectList(db.EndUsers, "ID", "Surname", friend.AcceptingUserID);
            return View(friend);
        }

        // GET: Friends/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Friend friend = db.Friends.Find(id);
            if (friend == null)
            {
                return HttpNotFound();
            }
            ViewBag.InitiatingUserID = new SelectList(db.EndUsers, "ID", "Surname", friend.InitiatingUserID);
            ViewBag.AcceptingUserID = new SelectList(db.EndUsers, "ID", "Surname", friend.AcceptingUserID);
            return View(friend);
        }

        // POST: Friends/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,InitiatingUserID,AcceptingUserID,DateRequested,DateAccepted,DateRejected,StatusID")] Friend friend)
        {
            if (ModelState.IsValid)
            {
                db.Entry(friend).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InitiatingUserID = new SelectList(db.EndUsers, "ID", "Surname", friend.InitiatingUserID);
            ViewBag.AcceptingUserID = new SelectList(db.EndUsers, "ID", "Surname", friend.AcceptingUserID);
            return View(friend);
        }

        // GET: Friends/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Friend friend = db.Friends.Find(id);
            if (friend == null)
            {
                return HttpNotFound();
            }
            return View(friend);
        }

        // POST: Friends/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Friend friend = db.Friends.Find(id);
            db.Friends.Remove(friend);
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
