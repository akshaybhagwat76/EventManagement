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
    public class FeeConfigurationsController : Controller
    {
        private MiidEntities db = new MiidEntities();

        // GET: FeeConfigurations
        public ActionResult Index()
        {
            return View(db.FeeConfigurations.ToList());
        }

        // GET: FeeConfigurations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeeConfiguration feeConfiguration = db.FeeConfigurations.Find(id);
            if (feeConfiguration == null)
            {
                return HttpNotFound();
            }
            return View(feeConfiguration);
        }

        // GET: FeeConfigurations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FeeConfigurations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TransactionType,PaymentType,MinimumCharge,PercentageCharge,FixedFee,Active")] FeeConfiguration feeConfiguration)
        {
            if (ModelState.IsValid)
            {
                db.FeeConfigurations.Add(feeConfiguration);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(feeConfiguration);
        }

        // GET: FeeConfigurations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeeConfiguration feeConfiguration = db.FeeConfigurations.Find(id);
            if (feeConfiguration == null)
            {
                return HttpNotFound();
            }
            return View(feeConfiguration);
        }

        // POST: FeeConfigurations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TransactionType,PaymentType,MinimumCharge,PercentageCharge,FixedFee,Active")] FeeConfiguration feeConfiguration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feeConfiguration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(feeConfiguration);
        }

        // GET: FeeConfigurations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeeConfiguration feeConfiguration = db.FeeConfigurations.Find(id);
            if (feeConfiguration == null)
            {
                return HttpNotFound();
            }
            return View(feeConfiguration);
        }

        // POST: FeeConfigurations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FeeConfiguration feeConfiguration = db.FeeConfigurations.Find(id);
            db.FeeConfigurations.Remove(feeConfiguration);
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
