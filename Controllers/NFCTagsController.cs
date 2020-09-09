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
using MiidWeb.Helpers;
using MiidWeb.Models;

namespace MiidWeb.Controllers
{
  [Authorize]
  public class NFCTagsController : BaseController
  {
    private MiidEntities db = new MiidEntities();

    // GET: NFCTags

    [Authorize(Roles = "Admin")]
    public ActionResult Index()
    {
      var nFCTags = db.NFCTags.Include(n => n.EndUser).Include(n => n.Status);
      return View(nFCTags.ToList());
    }

    public ActionResult IndexByUserID(int?id=0)
    {

            if ((UserHelper.UserEmailConfirmed(User.Identity.Name)) == "Pending")
            {
                return RedirectToAction("ConfirmEmailLink", "EndUsers", new { email = User.Identity.Name });
            }

            int LoggedInUserID = Helpers.UserHelper.UserID(User.Identity.Name);
            int ParentID = Helpers.UserHelper.UserID(User.Identity.Name);
            ViewBag.ParentID = ParentID;


            if (id > 0)
            {
                //this will make it the child tag scren
                LoggedInUserID = id ?? 0;
                ViewBag.ShowStuff = true;

            }
            else
            {

                ViewBag.ShowStuff = false;
            }
            ViewBag.EndUserID = LoggedInUserID;

      int cancelledStatusID = Helpers.StatusHelper.StatusID("NFCTag", "Cancelled");


      List<NFCTagViewModel> model = new List<NFCTagViewModel>();

      var nFCTags = db.NFCTags.Include(n => n.EndUser).Include(n => n.Status).Where(x => x.EndUserID == LoggedInUserID && x.StatusID != cancelledStatusID).ToList();

          

            foreach (var tag in nFCTags)
            {
                model.Add(new NFCTagViewModel(tag));
            }

      return View(model);
    }

    // GET: NFCTags/Details/5
    public ActionResult Details(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      NFCTag nFCTag = db.NFCTags.Find(id);
      if (nFCTag == null)
      {
        return HttpNotFound();
      }
      return View(nFCTag);
    }

    // GET: NFCTags/Create
    [Authorize(Roles = "Admin")]
    public ActionResult Create()
    {
      ViewBag.EndUserID = new SelectList(db.EndUsers, "ID", "Surname");
      ViewBag.StatusID = new SelectList(db.Status.Where(x => x.Context == "NFCTag"), "ID", "Code");
      return View();
    }

    // POST: NFCTags/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "ID,TagNumber,EndUserID,ActivationDate,StatusID,ActivationCode,TagPin")] NFCTag nFCTag)
    {
      if (ModelState.IsValid)
      {
        db.NFCTags.Add(nFCTag);
        db.SaveChanges();
        return RedirectToAction("Index");
      }

      ViewBag.EndUserID = new SelectList(db.EndUsers, "ID", "Surname", nFCTag.EndUserID);
      ViewBag.StatusID = new SelectList(db.Status.Where(x => x.Context == "NFCTag"), "ID", "Code");


      return View(nFCTag);
    }

    public ActionResult CreateByUser(int? id=0)
    {
      ViewBag.EndUserID = Helpers.UserHelper.UserID(User.Identity.Name);
            if (id > 0) { ViewBag.EndUserID = id; }

      ViewBag.StatusID = new SelectList(db.Status.Where(x => x.Context == "NFCTag"), "ID", "Code");
      return View();
    }

    // POST: NFCTags/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult CreateByUser([Bind(Include = "ID,TagNumber,EndUserID,ActivationDate,StatusID,ActivationCode,TagPin")] NFCTag nFCTag)
    {

            ViewBag.EndUserID = Helpers.UserHelper.UserID(User.Identity.Name);
            if (nFCTag.EndUserID > 0) { ViewBag.EndUserID = nFCTag.EndUserID; }

            ViewBag.StatusID = new SelectList(db.Status.Where(x => x.Context == "NFCTag"), "ID", "Code");

            if (ModelState.IsValid)
      {

        //Check if tag exists on database
        var tagExists = db.NFCTags.Where(x => x.TagNumber == nFCTag.TagNumber && x.ActivationCode == nFCTag.ActivationCode);

        if (tagExists.Count() > 0 && (int)(nFCTag.TagPin ?? 0) >= 1000 && (int)(nFCTag.TagPin ?? 0) <= 9999 && tagExists.First().StatusID != StatusHelper.StatusID("NFCTag", "Cancelled"))
        {
          //Check if tag is in use by someone else
          var tag = tagExists.First();
          if (tag.EndUserID == null || tag.EndUserID == nFCTag.EndUserID)
          {

            List<Status> statuses = db.Status.Where(x => x.Context == "NFCtag" && x.Description == "Active").ToList();
            int ActiveTagStatusID = statuses.First().ID;
            //Update the tag with the new pin and user details
            var updateTag = db.NFCTags.Find(tag.ID);
            updateTag.ActivationDate = DateTime.Now;
            updateTag.EndUserID = nFCTag.EndUserID;
            updateTag.DateTimeUpdated = DateTime.Now;
            updateTag.TagPin = nFCTag.TagPin;
            updateTag.StatusID = ActiveTagStatusID;

            db.Entry(updateTag).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("IndexByUserID");
          }
          else
          {
            ViewBag.ErrorMessage = "Tag Number Not Available - Already in Use";
            Growl("Mii Band", "Tag Number Not Available - Already in Use.");
          }

        }
        else
        {
          if (tagExists.Count() <= 0)
          {
            Growl("Mii Band", "Tag Number or Activation Code invalid.");
          }
          if ((int)(nFCTag.TagPin ?? 0) < 1000 || (int)(nFCTag.TagPin ?? 0) > 9999)
          {
            Growl("Mii Band", "Pin must be 4 digits");

          }
          if (tagExists.Count() > 0)
          {
            if (tagExists.First().StatusID == StatusHelper.StatusID("NFCTag", "Cancelled"))
            {
              Growl("Mii Band", "Tag already cancelled.");
            }
          }
        }


        //return RedirectToAction("IndexByUserID");
        return View(nFCTag);
      }

      Growl("Mii Band", "Invalid Entries. Please Check.");

      ViewBag.EndUserID = Helpers.UserHelper.UserID(User.Identity.Name);
      ViewBag.StatusID = new SelectList(db.Status.Where(x => x.Context == "NFCTag"), "ID", "Code");
      return View(nFCTag);
    }


    // GET: NFCTags/Edit/5
    public ActionResult Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      NFCTag nFCTag = db.NFCTags.Find(id);
      if (nFCTag == null)
      {
        return HttpNotFound();
      }
      ViewBag.EndUserID = new SelectList(db.EndUsers, "ID", "Surname", nFCTag.EndUserID);
      ViewBag.StatusID = new SelectList(db.Status.Where(x => x.Context == "NFCTag"), "ID", "Code");
      return View(nFCTag);
    }

    public ActionResult EditByUser(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      NFCTag nFCTag = db.NFCTags.Find(id);

      EndUser user = db.EndUsers.Find(nFCTag.EndUserID);

      NFCTagViewModel model = new NFCTagViewModel
      {
        ID = nFCTag.ID,
        EndUserID = (int)nFCTag.EndUserID,
        IDNumber = user.IDNumber,
        ActivationCode = nFCTag.ActivationCode,
        ActivationDate = (DateTime)nFCTag.ActivationDate,
        Email = user.Email,
        NewTagPin = null,
        StatusID = nFCTag.StatusID ?? 0,
        TagNumber = nFCTag.TagNumber,
        TagPin = nFCTag.TagPin ?? 0
      };
      if (nFCTag == null)
      {
        return HttpNotFound();
      }
      ViewBag.EndUserID = Helpers.UserHelper.UserID(User.Identity.Name);
      return View(model);
    }


    public ActionResult CancelByUser(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      NFCTag nFCTag = db.NFCTags.Find(id);

      EndUser user = db.EndUsers.Find(nFCTag.EndUserID);

      NFCTagViewModel model = new NFCTagViewModel
      {
        ID = nFCTag.ID,
        EndUserID = (int)nFCTag.EndUserID,
        IDNumber = user.IDNumber,
        ActivationCode = nFCTag.ActivationCode,
        ActivationDate = (DateTime)nFCTag.ActivationDate,
        Email = user.Email,
        NewTagPin = null,
        StatusID = nFCTag.StatusID ?? 0,
        TagNumber = nFCTag.TagNumber,
        TagPin = nFCTag.TagPin ?? 0,
        TermsAndConditions = false
      };
      if (nFCTag == null)
      {
        return HttpNotFound();
      }

      return View(model);// RedirectToAction("IndexByUserID");
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult CancelByUser(NFCTagViewModel nFCTag)
    {
      bool correctID = true;
            string DadID = NFCTagRepository.GetMyDadsIDNumber(nFCTag.ID, true);
            string DadEmail = NFCTagRepository.GetMyDadsIDNumber(nFCTag.ID, false);
            //verify id number entered
          //  if (nFCTag.NewIDNumber != DadID)
      //  {
       // correctID = false;
     // }

      if (nFCTag.TermsAndConditions && correctID)
      {

        Session.Add("CurrentTag", nFCTag);

        return RedirectToAction("CancelConfirmed", new { id = nFCTag.ID });
      }
      else
      {
       // if (!correctID)
         // Growl("MiiD", "Incorrect ID Number");
        if (!nFCTag.TermsAndConditions)
          Growl("MiiD", "Please accept terms and conditions to proceed");
        return View(nFCTag);
      }



    }


    public ActionResult CancelConfirmed(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      NFCTagViewModel nFCTag = (NFCTagViewModel)Session["CurrentTag"];
      if (nFCTag == null)
      {
        return HttpNotFound();
      }
      return View(nFCTag);
    }

    // POST: NFCTags/Delete/5
    [HttpPost, ActionName("CancelConfirmed")]
    [ValidateAntiForgeryToken]
    public ActionResult CancelConfirmed(NFCTagViewModel nFCTag)
    {


      if (ModelState.IsValid)
      {

        var nfc = db.NFCTags.Find(nFCTag.ID);


        nfc.StatusID = StatusHelper.StatusID("NFCtag", "Cancelled");
        db.Entry(nfc).State = EntityState.Modified;
        db.SaveChanges();

        return RedirectToAction("IndexByUserID");

      }
      ViewBag.EndUserID = Helpers.UserHelper.UserID(User.Identity.Name);

      return View("CancelByUser", new { id = nFCTag.ID });
    }




    public ActionResult ReactivateByUser(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      NFCTag nFCTag = db.NFCTags.Find(id);


      nFCTag.StatusID = StatusHelper.StatusID("NFCtag", "Active");
      db.Entry(nFCTag).State = EntityState.Modified;
      db.SaveChanges();

      if (nFCTag == null)
      {
        return HttpNotFound();
      }

      return RedirectToAction("IndexByUserID");
    }




    // POST: NFCTags/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "ID,TagNumber,EndUserID,ActivationDate,StatusID,ActivationCode,TagPin")] NFCTag nFCTag)
    {
      if (ModelState.IsValid)
      {
        db.Entry(nFCTag).State = EntityState.Modified;
        db.SaveChanges();
        return RedirectToAction("Index");
      }
      ViewBag.EndUserID = new SelectList(db.EndUsers, "ID", "Surname", nFCTag.EndUserID);

      ViewBag.StatusID = new SelectList(db.Status.Where(x => x.Context == "NFCTag"), "ID", "Code", nFCTag.StatusID);
      return View(nFCTag);
    }

    // POST: NFCTags/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult EditByUser(NFCTagViewModel nFCTag)
    {
      bool secureid = true;
      bool secureemail = true;
      bool passwordmatch = true;

      if (ModelState.IsValid)
      {
                //verify id number entered
                //string DadID = NFCTagRepository.GetMyDadsIDNumber(nFCTag.ID, true);
                string DadEmail = NFCTagRepository.GetMyDadsIDNumber(nFCTag.ID, false);

                //if (nFCTag.NewIDNumber != DadID)
               // {
                   // secureid = false;
                   // ViewBag.NotificationIncorrectIDNumber = "Incorrect ID Number";
                //}


        //verify email entered
        if (nFCTag.NewEmail != DadEmail)
        {
          secureemail = false;

          ViewBag.NotificationIncorrectEmail = "Incorrect Email";
        }
        //verify password match
        if (nFCTag.NewTagPin != nFCTag.ConfirmTagPin)
        {
          passwordmatch = false;

          ViewBag.NotificationPinMismatch = "Does not match new pin";
        }

        if (secureemail && passwordmatch)
        {

          var nfc = db.NFCTags.Find(nFCTag.ID);
          nfc.TagPin = nFCTag.NewTagPin;

          db.Entry(nfc).State = EntityState.Modified;
          db.SaveChanges();
          return View("PinChangeConfirmation");// RedirectToAction("IndexByUserID");
        }
        else
        {
          ViewBag.Notification = "Tag Reset Unsuccesful";

        }
      }
      ViewBag.EndUserID = Helpers.UserHelper.UserID(User.Identity.Name);


      return View(nFCTag);
    }


    // GET: NFCTags/Delete/5
    public ActionResult Delete(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      NFCTag nFCTag = db.NFCTags.Find(id);
      if (nFCTag == null)
      {
        return HttpNotFound();
      }
      return View(nFCTag);
    }

    // POST: NFCTags/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
      NFCTag nFCTag = db.NFCTags.Find(id);
      db.NFCTags.Remove(nFCTag);
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
