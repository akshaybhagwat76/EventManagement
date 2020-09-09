using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace MiidWeb.Controllers
{
       [Authorize(Roles = "Admin")]
    public class CMsController : Controller
    {
        private MiidEntities db = new MiidEntities();

        // GET: CMs
        public ActionResult Index()
        {
            return View(db.CMS.ToList());
        }

        // GET: CMs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CM cM = db.CMS.Find(id);
            if (cM == null)
            {
                return HttpNotFound();
            }
            return View(cM);
        }

        // GET: CMs/Create
           [ValidateInput(false)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: CMs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "ID,PagePath,Paral,Para2,Para3,Para4,DateTimeCreated")] CM cM)
        {
            if (ModelState.IsValid)
            {
                db.CMS.Add(cM);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cM);
        }

        // GET: CMs/Edit/5
           [ValidateInput(false)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CM cM = db.CMS.Find(id);
            if (cM == null)
            {
                return HttpNotFound();
            }
            return View(cM);
        }

        // POST: CMs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ID,PagePath,Paral,Para2,Para3,Para4,DateTimeCreated")] CM cM)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cM).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cM);
        }

        // GET: CMs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CM cM = db.CMS.Find(id);
            if (cM == null)
            {
                return HttpNotFound();
            }
            return View(cM);
        }

        // POST: CMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CM cM = db.CMS.Find(id);
            db.CMS.Remove(cM);
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
