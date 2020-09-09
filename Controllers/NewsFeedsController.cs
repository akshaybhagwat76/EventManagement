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
    public class NewsFeedsController : Controller
    {
        private MiidEntities db = new MiidEntities();

        // GET: NewsFeeds
        public ActionResult Index()
        {
            var newsFeeds = db.NewsFeeds.Include(n => n.Status).Include(n => n.EndUser);
            return View(newsFeeds.ToList());
        }

        // GET: NewsFeeds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsFeed newsFeed = db.NewsFeeds.Find(id);
            if (newsFeed == null)
            {
                return HttpNotFound();
            }
            return View(newsFeed);
        }

        // GET: NewsFeeds/Create
        public ActionResult Create()
        {
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code");
            ViewBag.EndUserID = new SelectList(db.EndUsers, "ID", "Surname");
            return View();
        }

        // POST: NewsFeeds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,EndUserID,StatusID,DateCreated")] NewsFeed newsFeed)
        {
            if (ModelState.IsValid)
            {
                db.NewsFeeds.Add(newsFeed);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code", newsFeed.StatusID);
            ViewBag.EndUserID = new SelectList(db.EndUsers, "ID", "Surname", newsFeed.EndUserID);
            return View(newsFeed);
        }

        // GET: NewsFeeds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsFeed newsFeed = db.NewsFeeds.Find(id);
            if (newsFeed == null)
            {
                return HttpNotFound();
            }
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code", newsFeed.StatusID);
            ViewBag.EndUserID = new SelectList(db.EndUsers, "ID", "Surname", newsFeed.EndUserID);
            return View(newsFeed);
        }

        // POST: NewsFeeds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EndUserID,StatusID,DateCreated")] NewsFeed newsFeed)
        {
            if (ModelState.IsValid)
            {
                db.Entry(newsFeed).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code", newsFeed.StatusID);
            ViewBag.EndUserID = new SelectList(db.EndUsers, "ID", "Surname", newsFeed.EndUserID);
            return View(newsFeed);
        }

        // GET: NewsFeeds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsFeed newsFeed = db.NewsFeeds.Find(id);
            if (newsFeed == null)
            {
                return HttpNotFound();
            }
            return View(newsFeed);
        }

        // POST: NewsFeeds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NewsFeed newsFeed = db.NewsFeeds.Find(id);
            db.NewsFeeds.Remove(newsFeed);
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
