using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MiidWeb;
using MiidWeb.Models;

namespace MiidWeb.Controllers
{
    public class EndUsersControllerBAK : Controller
    {
        private MiidEntities db = new MiidEntities();

        // GET: EndUsers
        public ActionResult Index()
        {
            var endUsers = db.EndUsers.Include(e => e.AccountType).Include(e => e.Status);
            return View(endUsers.ToList());
        }

        // GET: EndUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EndUser endUser = db.EndUsers.Find(id);

            EndUserViewModel endUserVM = new EndUserViewModel();



            if (endUser == null)
            {
                return HttpNotFound();
            }

            endUserVM.EndUser = endUser;
            endUserVM.CalendarViewModel = new CalendarViewModel(endUser.ID, DateTime.Now, 1);    
            

            return View(endUserVM);
        }

        public ActionResult AddNewsFeedComment(int id)
        {
            return null;
        }

        [HttpPost]
        public ActionResult AddNewsFeedComment(FormCollection form)
        {
            var f = form;
            string NewsFeedComment = form["NewsFeedComment"];
            int NewsFeedID = int.Parse(form["NewsFeedID"]);
            int UserID = int.Parse(form["UserID"]);

            var newsFeedComment = new NewsFeedComment
                {
                    CommenterUserID = UserID,
                    Comment = NewsFeedComment,
                    NewsFeedID = NewsFeedID,
                    DateCreated = DateTime.Now,
                    StatusID = 1
                };
            
            db.NewsFeedComments.Add(newsFeedComment);
            db.SaveChanges();

            return RedirectToAction("EndUserProfile", new { id = UserID });
        }

        
 [HttpPost]
        public ActionResult AddNewsItem(FormCollection form)
        {
            var f = form;
            string NewsFeedHeadline = form["NewsFeedHeadline"];
           
            int UserID = int.Parse(form["UserID"]);

            var newsFeed = new NewsFeed
                {
                    EndUserID = UserID,
                    Headline = NewsFeedHeadline,
                    DateCreated = DateTime.Now,
                    StatusID = 1
                };

            db.NewsFeeds.Add(newsFeed);
            db.SaveChanges();

            return RedirectToAction("EndUserProfile", new { id = UserID });
        }

        [HttpPost]
        public ActionResult MultipleButtonCancel()
        {
        
            return null;
        }

        public ActionResult EndUserProfile(int? id, int loggedInUserID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EndUser endUser = db.EndUsers.Find(id);

            EndUserViewModel endUserVM = new EndUserViewModel();


            if (endUser == null)
            {
                return HttpNotFound();
            }

            endUserVM.EndUser = endUser;
            endUserVM.CalendarViewModel = new CalendarViewModel(endUser.ID, DateTime.Now,1);
            endUserVM.NewsFeedViewModel = new NewsFeedViewModel(endUser.ID, 10, loggedInUserID);//Take 10 latest newsfeeditems
            endUserVM.CurrentEvents = db.Events.ToList();
            endUserVM.Friends = db.Friends.Include(x=>x.EndUser).Include(x=>x.EndUser1).Where(x => x.InitiatingUserID == endUser.ID || x.AcceptingUserID == endUser.ID).ToList();//and status ok

            return View(endUserVM);
        }

        public ActionResult PrevNextMonth(int? id, string PrevNext, DateTime CurrentMonth)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EndUser endUser = db.EndUsers.Find(id);

            EndUserViewModel endUserVM = new EndUserViewModel();


            if (endUser == null)
            {
                return HttpNotFound();
            }

            endUserVM.EndUser = endUser;

            DateTime NewMonth = CurrentMonth;

            if (PrevNext == "Next")
            {
                NewMonth = NewMonth.AddMonths(1);
            }
            else
            {
                NewMonth = NewMonth.AddMonths(-1);
            }

            endUserVM.CalendarViewModel = new CalendarViewModel(endUser.ID, NewMonth,1);
            
            
            return View("Details", endUserVM);
        }

       


        // GET: EndUsers/Create
        public ActionResult Create()
        {
            ViewBag.AccountTypeID = new SelectList(db.AccountTypes, "ID", "Code");
            ViewBag.ParentEndUserID = new SelectList(db.EndUsers, "ID", "Surname");
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code");
            return View();
        }

        // POST: EndUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Surname,Firstname,DateOfBirth,AccountTypeID,IDNumber,Age,Sex,RaceID,StreetAddress,Suburb,City,PostalCode,Telephone,Cell,Email,StatusID,StartDate,AccountBalance,IsChildUser,ParentEndUserID,NextOfKin,NextOfKinTelephone,Medication,MedicalAidCompany,MedicalAidNumber,Alergies,KnownConditions")] EndUser endUser)
        {
            if (ModelState.IsValid)
            {
                db.EndUsers.Add(endUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountTypeID = new SelectList(db.AccountTypes, "ID", "Code", endUser.AccountTypeID);
            
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code", endUser.StatusID);
            return View(endUser);
        }


        // GET: EndUsers/Create
        public ActionResult CreateFirst(string Email)
        {
            ViewBag.Email = Email;
            ViewBag.AccountTypeID = new SelectList(db.AccountTypes, "ID", "Code");
            ViewBag.ParentEndUserID = new SelectList(db.EndUsers, "ID", "Surname");
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code");
            return View();
        }

        // POST: EndUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFirst([Bind(Include = "ID,Surname,Firstname,DateOfBirth,AccountTypeID,IDNumber,Age,Sex,RaceID,StreetAddress,Suburb,City,PostalCode,Telephone,Cell,Email,StatusID,StartDate,AccountBalance,IsChildUser,ParentEndUserID,NextOfKin,NextOfKinTelephone,Medication,MedicalAidCompany,MedicalAidNumber,Alergies,KnownConditions")] EndUser endUser)
        {
            if (ModelState.IsValid)
            {
                db.EndUsers.Add(endUser);
                db.SaveChanges();
                return RedirectToAction("EndUserProfile", new {id=endUser.ID });
            }

            ViewBag.AccountTypeID = new SelectList(db.AccountTypes, "ID", "Code", endUser.AccountTypeID);
            
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code", endUser.StatusID);
            return View(endUser);
        }

        
        // GET: EndUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EndUser endUser = db.EndUsers.Find(id);
            if (endUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountTypeID = new SelectList(db.AccountTypes, "ID", "Code", endUser.AccountTypeID);
          
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code", endUser.StatusID);
            return View(endUser);
        }

        // POST: EndUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Surname,Firstname,DateOfBirth,AccountTypeID,IDNumber,Age,Sex,RaceID,StreetAddress,Suburb,City,PostalCode,Telephone,Cell,Email,StatusID,StartDate,AccountBalance,IsChildUser,ParentEndUserID,NextOfKin,NextOfKinTelephone,Medication,MedicalAidCompany,MedicalAidNumber,Alergies,KnownConditions")] EndUser endUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(endUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountTypeID = new SelectList(db.AccountTypes, "ID", "Code", endUser.AccountTypeID);
        
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code", endUser.StatusID);
            return View(endUser);
        }


        // GET: EndUsers/Edit/5
        public ActionResult MiiDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EndUser endUser = db.EndUsers.Find(id);
            if (endUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountTypeID = new SelectList(db.AccountTypes, "ID", "Code", endUser.AccountTypeID);
         
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code", endUser.StatusID);
            return View(endUser);
        }

        // POST: EndUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MiiDetails([Bind(Include = "ID,Surname,Firstname,DateOfBirth,AccountTypeID,IDNumber,Age,Sex,RaceID,StreetAddress,Suburb,City,PostalCode,Telephone,Cell,Email,StatusID,StartDate,AccountBalance,IsChildUser,ParentEndUserID,NextOfKin,NextOfKinTelephone,Medication,MedicalAidCompany,MedicalAidNumber,Alergies,KnownConditions")] EndUser endUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(endUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountTypeID = new SelectList(db.AccountTypes, "ID", "Code", endUser.AccountTypeID);
          
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code", endUser.StatusID);
            return View(endUser);
        }




        // GET: EndUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EndUser endUser = db.EndUsers.Find(id);
            if (endUser == null)
            {
                return HttpNotFound();
            }
            return View(endUser);
        }

        // POST: EndUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EndUser endUser = db.EndUsers.Find(id);
            db.EndUsers.Remove(endUser);
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
