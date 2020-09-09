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
    public class NewsFeedCommentsController : Controller
    {
        private MiidEntities db = new MiidEntities();

        // GET: NewsFeedComments
        public ActionResult Index()
        {
            var newsFeedComments = db.NewsFeedComments.Include(n => n.NewsFeed).Include(n => n.Status);
            return View(newsFeedComments.ToList());
        }

        // GET: NewsFeedComments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsFeedComment newsFeedComment = db.NewsFeedComments.Find(id);
            if (newsFeedComment == null)
            {
                return HttpNotFound();
            }
            return View(newsFeedComment);
        }

        // GET: NewsFeedComments/Create
        public ActionResult Create()
        {
            ViewBag.NewsFeedID = new SelectList(db.NewsFeeds, "ID", "ID");
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code");
            return View();
        }

        // POST: NewsFeedComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,StatusID,DateCreated,Comment,NewsFeedID")] NewsFeedComment newsFeedComment)
        {
            if (ModelState.IsValid)
            {
                db.NewsFeedComments.Add(newsFeedComment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.NewsFeedID = new SelectList(db.NewsFeeds, "ID", "ID", newsFeedComment.NewsFeedID);
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code", newsFeedComment.StatusID);
            return View(newsFeedComment);
        }

        // GET: NewsFeedComments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsFeedComment newsFeedComment = db.NewsFeedComments.Find(id);
            if (newsFeedComment == null)
            {
                return HttpNotFound();
            }
            ViewBag.NewsFeedID = new SelectList(db.NewsFeeds, "ID", "ID", newsFeedComment.NewsFeedID);
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code", newsFeedComment.StatusID);
            return View(newsFeedComment);
        }

        // POST: NewsFeedComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,StatusID,DateCreated,Comment,NewsFeedID")] NewsFeedComment newsFeedComment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(newsFeedComment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.NewsFeedID = new SelectList(db.NewsFeeds, "ID", "ID", newsFeedComment.NewsFeedID);
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code", newsFeedComment.StatusID);
            return View(newsFeedComment);
        }

        // GET: NewsFeedComments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NewsFeedComment newsFeedComment = db.NewsFeedComments.Find(id);
            if (newsFeedComment == null)
            {
                return HttpNotFound();
            }
            return View(newsFeedComment);
        }

        // POST: NewsFeedComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NewsFeedComment newsFeedComment = db.NewsFeedComments.Find(id);
            db.NewsFeedComments.Remove(newsFeedComment);
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
