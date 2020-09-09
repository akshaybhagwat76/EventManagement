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
    [Authorize]
    public class ConfigurationsController : Controller
    {
        private MiidEntities db = new MiidEntities();

        // GET: Configurations
        public ActionResult Index()
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index");
            }
            var configurations = db.Configurations.Include(c => c.Subdomain);
            return View(configurations.ToList());
        }

        // GET: Configurations/Details/5
        public ActionResult Details(int? id)
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Configuration configuration = db.Configurations.Find(id);
            if (configuration == null)
            {
                return HttpNotFound();
            }
            return View(configuration);
        }

        // GET: Configurations/Create
        public ActionResult Create()
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index");
            }
            ViewBag.SubdomainID = new SelectList(db.Subdomains, "ID", "SubdomainName");
            return View();
        }

        // POST: Configurations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,SubdomainID,Key,Value")] Configuration configuration)
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                db.Configurations.Add(configuration);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SubdomainID = new SelectList(db.Subdomains, "ID", "SubdomainName", configuration.SubdomainID);
            return View(configuration);
        }

        // GET: Configurations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Configuration configuration = db.Configurations.Find(id);
            if (configuration == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubdomainID = new SelectList(db.Subdomains, "ID", "SubdomainName", configuration.SubdomainID);
            return View(configuration);
        }

        // POST: Configurations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SubdomainID,Key,Value")] Configuration configuration)
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                db.Entry(configuration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SubdomainID = new SelectList(db.Subdomains, "ID", "SubdomainName", configuration.SubdomainID);
            return View(configuration);
        }

        // GET: Configurations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Configuration configuration = db.Configurations.Find(id);
            if (configuration == null)
            {
                return HttpNotFound();
            }
            return View(configuration);
        }

        // POST: Configurations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index");
            }
            Configuration configuration = db.Configurations.Find(id);
            db.Configurations.Remove(configuration);
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
