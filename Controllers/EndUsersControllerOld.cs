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
using System.Text;


namespace MiidWeb.Controllers
{
    [Authorize]
    public class EndUsersController : Controller
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
            if (endUser == null)
            {
                return HttpNotFound();
            }
            return View(endUser);
        }

        // GET: EndUsers/Create
        public ActionResult Create()
        {
            ViewBag.AccountTypeID = new SelectList(db.AccountTypes, "ID", "Code");
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code");
            return View();
        }

        // POST: EndUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Surname,Firstname,DateOfBirth,AccountTypeID,IDNumber,Age,Sex,RaceID,StreetAddress,Suburb,City,PostalCode,Telephone,Cell,Email,StatusID,StartDate,AccountBalance,IsChildUser,NextOfKin,NextOfKinTelephone,Medication,MedicalAidCompany,MedicalAidNumber,Allergies,KnownConditions,AspNetUserID")] EndUser endUser)
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
        public ActionResult Edit([Bind(Include = "ID,Surname,Firstname,DateOfBirth,AccountTypeID,IDNumber,Age,Sex,RaceID,StreetAddress,Suburb,City,PostalCode,Telephone,Cell,Email,StatusID,StartDate,AccountBalance,IsChildUser,NextOfKin,NextOfKinTelephone,Medication,MedicalAidCompany,MedicalAidNumber,Allergies,KnownConditions,AspNetUserID")] EndUser endUser)
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


        //NEW METHODS

        // GET: EndUsers/Edit/5
        public ActionResult MiiDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            

            EndUser endUser = db.EndUsers.Find(id);

            CreateFirstEndUserViewModel v = new CreateFirstEndUserViewModel(endUser);


            if (endUser == null)
            {
                return HttpNotFound();
            }
            //ViewBag.AccountTypeID = new SelectList(db.AccountTypes, "ID", "Code", endUser.AccountTypeID);

            //ViewBag.StatusID = new SelectList(db.Status, "ID", "Code", endUser.StatusID);
            return View(v);
        }

        // POST: EndUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MiiDetails(CreateFirstEndUserViewModel v)
        {
            if (ModelState.IsValid)
            {

                EndUser e = db.EndUsers.Find(v.ID);

                e.AccountTypeID = 1;
                e.RaceID = v.RaceID;
                e.Firstname = v.Firstname;
                e.Surname = v.Surname;
                e.Telephone = v.Telephone;
                e.Cell = v.Cell;
                e.StreetAddress = v.StreetAddress;
                e.Suburb = v.Suburb;
                e.PostalCode = v.PostalCode;
                e.City = v.City;
                e.Province = v.Province;

                DateTime date1 = new DateTime(v.DateOfBirthYear, v.DateOfBirthMonth, v.DateOfBirthDay);
                e.DateOfBirth = date1;

                e.Allergies = v.Allergies;
                                                
                e.Suburb = v.Suburb;
                                
                e.Sex = v.Male ? "M":"F";
                
                e.IDNumber = v.IDNumber;
                e.KnownConditions = v.KnownConditions;
                
                e.MedicalAidCompany = v.MedicalAidCompany;
                e.MedicalAidNumber = v.MedicalAidNumber;
                e.Medication = v.Medication;
                e.NextOfKin = v.NextOfKin;
                e.NextOfKinTelephone = v.NextOfKinTelephone;
                e.ProfilePicURL = v.ProfilePicURL;
                


                db.Entry(e).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("EndUserProfile", "EndUsers", new {ID = e.ID, LoggedInUserID = e.ID });
            }
            
            return View(v);
        }

        public ActionResult AdditionalDetails(int? id)
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
        public ActionResult AdditionalDetails([Bind(Include = "ID,Surname,Firstname,DateOfBirth,AccountTypeID,IDNumber,Age,Sex,RaceID,StreetAddress,Suburb,City,PostalCode,Telephone,Cell,Email,StatusID,StartDate,AccountBalance,IsChildUser,ParentEndUserID,NextOfKin,NextOfKinTelephone,Medication,MedicalAidCompany,MedicalAidNumber,Alergies,KnownConditions")] EndUser endUser)
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

        // GET: EndUsers/Create
        public ActionResult CreateFirst(string Email)
        {
            ViewBag.Email = Email;


            var list = new List<SelectListItem>();
          
            list.Add(new SelectListItem() { Value = "0", Text = "Race" });
            list.Add(new SelectListItem() { Value = "1", Text = "African" });
            list.Add(new SelectListItem() { Value = "2", Text = "Caucasian" });
            list.Add(new SelectListItem() { Value = "3", Text = "Coloured" });
            list.Add(new SelectListItem() { Value = "4", Text = "Indian" });
            list.Add(new SelectListItem() { Value = "5", Text = "Other" });
            list.Add(new SelectListItem() { Value = "6", Text = "Not specified" });
            
            
            ViewBag.RaceID = list;

            var provinceList = new List<SelectListItem>();


            provinceList.Add(new SelectListItem() { Value = "0", Text = "Province" });
            provinceList.Add(new SelectListItem() { Value = "1", Text = "Western Cape" });
            provinceList.Add(new SelectListItem() { Value = "2", Text = "Eastern Cape" });
            provinceList.Add(new SelectListItem() { Value = "3", Text = "Northern Cape" });
            provinceList.Add(new SelectListItem() { Value = "4", Text = "Gauteng" });
            provinceList.Add(new SelectListItem() { Value = "5", Text = "Limpopo" });
            provinceList.Add(new SelectListItem() { Value = "6", Text = "Mpumalanga" });
            provinceList.Add(new SelectListItem() { Value = "7", Text = "KwaZulu-Natal" });
            provinceList.Add(new SelectListItem() { Value = "8", Text = "North-West" });
            provinceList.Add(new SelectListItem() { Value = "9", Text = "Freestate" });

            ViewBag.Province = provinceList;

            List<SelectListItem> date1;
            GetNumberDropDown(out date1, 1, 31);
            ViewBag.DateOfBirthDay = date1;


            var date2 = GetMonthDropDown();
            ViewBag.DateOfBirthMonth = date2;
            
                        
            var date3 = new List<SelectListItem>();
            GetNumberDropDown(out date3, 1961, 2005);
            ViewBag.DateOfBirthYear = date3;

            CreateFirstEndUserViewModel v = new CreateFirstEndUserViewModel();
            v.Male = false;
            v.Female = false;

            return View(v);
        }

        private static List<SelectListItem> GetMonthDropDown()
        {
            var date2 = new List<SelectListItem>();
            int x = 1;

            while (x <= 12)
            {

                DateTime d = new DateTime(2000, x, 1);
                date2.Add(new SelectListItem() { Value = x.ToString(), Text = d.ToString("MMMM") });
                x++;
            }
            return date2;
        }

        private static void GetNumberDropDown(out List<SelectListItem> date1, int x, int y)
        {
            date1 = new List<SelectListItem>();
                        
            while (x <= y)
            {
                date1.Add(new SelectListItem() { Value = x.ToString(), Text = x.ToString() });
                x++;
            }
        }

        // POST: EndUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFirst(CreateFirstEndUserViewModel v)
        {
            if (ModelState.IsValid)
            {

                EndUser e = new EndUser();

                e.AccountTypeID = 1;
                e.RaceID = v.RaceID;
                e.Firstname = v.Firstname;
                e.Surname = v.Surname;
                e.Telephone = v.Telephone;
                e.Cell = v.Cell;
                e.StreetAddress = v.StreetAddress;
                e.Suburb = v.Suburb;
                e.PostalCode = v.PostalCode;
                e.City = v.City;
                e.Province = v.Province;
 
                DateTime date1 = new DateTime(v.DateOfBirthYear, v.DateOfBirthMonth, v.DateOfBirthDay);
                e.DateOfBirth = date1;
                e.Email = v.Email;
                
                e.Sex = v.Male ? "M" : "F";
                e.NewsletterAccepted = v.Newsletter;
                e.TermsAndConditionsAccepted = v.TermsAndConditions;

                //Newsletter add fields to e.

                e.EmailVerificationGuid = System.Guid.NewGuid();
                db.EndUsers.Add(e);
                db.SaveChanges();

                StringBuilder body = new StringBuilder();

                var callbackUrl = Url.Action("ConfirmEmail", "EndUsers", new { id = e.ID, code = e.EmailVerificationGuid }, protocol: Request.Url.Scheme);

                body.AppendLine("<html><head></head><body>");
                body.AppendLine("Thank you for your registering, " + e.Firstname + "!");
                body.AppendLine("Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                body.AppendLine("</body></html>");

                Helpers.EmailHelper.SendMail(e.Email, "MiiD Registration Confirmation", body.ToString());

                //Email MIID ADMIN
                Helpers.EmailHelper.SendMail("donavanwallis@hotmail.com", "Registration Application: EndUser", "A new EndUser applied for registration: " + e.Email);
                Helpers.EmailHelper.SendMail("jonathan.wallis@miid.co.za", "Registration Application: EndUser", "A new EndUser applied for registration: " + e.Email);
                Helpers.EmailHelper.SendMail("dsouchon@gmail.com", "Registration Application: EndUser", "A new EndUser applied for registration: " + e.Email);
        


                //Helpers.EmailHelper.SendMail(endUser.Email, "MiiD Registration Confirmation", "test");


                return View("CheckEmail");
            }

         
            return View(v);
        }

        public ActionResult ConfirmEmail(int id, string code)
        {
            var endUser = db.EndUsers.Find(id);
            if (endUser != null && endUser.EmailVerificationGuid.ToString() == code)
            {
                endUser.EmailVerified = true;
                endUser.StatusID = db.Status.Where(x => x.Context == "EndUser" && x.Description == "Active").First().ID;
                db.SaveChanges();

                return View(endUser);
            }
            else
            {
                return View("ErrorConfirmEmail");
            }
        
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
            int CommenterUserID = int.Parse(form["CommenterUserID"]);
            int UserID = int.Parse(form["UserID"]);

            var newsFeedComment = new NewsFeedComment
            {
                CommenterUserID = CommenterUserID,
                Comment = NewsFeedComment,
                NewsFeedID = NewsFeedID,
                DateCreated = DateTime.Now,
                StatusID = 1
            };

            db.NewsFeedComments.Add(newsFeedComment);
            db.SaveChanges();

            return RedirectToAction("EndUserProfile", new { id = UserID, loggedInUserID = CommenterUserID });
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

            return RedirectToAction("EndUserProfile", new { id = UserID, loggedInUserID = UserID });
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
            endUserVM.CalendarViewModel = new CalendarViewModel(endUser.ID, DateTime.Now, 1);
            endUserVM.UpcomingEvents = new MyEventsViewModel(endUser.ID, DateTime.Now, 3);//3 months ahead

            endUserVM.NewsFeedViewModel = new NewsFeedViewModel(endUser.ID, 10, loggedInUserID);//Take 10 latest newsfeeditems
            endUserVM.CurrentEvents = db.Events.ToList();
            endUserVM.Friends = db.Friends.Include(x => x.EndUser).Include(x => x.EndUser1).Where(x => x.InitiatingUserID == endUser.ID || x.AcceptingUserID == endUser.ID).ToList();//and status ok

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

            endUserVM.CalendarViewModel = new CalendarViewModel(endUser.ID, NewMonth, 1);
            endUserVM.UpcomingEvents = new MyEventsViewModel(endUser.ID, NewMonth, 12);//12 months ahead



            return View("MiiCalendar", endUserVM);
        }

        public ActionResult PrevNextMonthMini(int? id, string PrevNext, DateTime CurrentMonth, int loggedInUserID)
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
            endUserVM.EndUser = endUser;
            
            endUserVM.NewsFeedViewModel = new NewsFeedViewModel(endUser.ID, 10, loggedInUserID);//Take 10 latest newsfeeditems
            endUserVM.CurrentEvents = db.Events.ToList();
            endUserVM.Friends = db.Friends.Include(x => x.EndUser).Include(x => x.EndUser1).Where(x => x.InitiatingUserID == endUser.ID || x.AcceptingUserID == endUser.ID).ToList();//and status ok

            endUserVM.CalendarViewModel = new CalendarViewModel(endUser.ID, NewMonth,1);

            return View("EndUserProfile", endUserVM);
        }

        public ActionResult MiiCalendar(int? id, int loggedInUserID)
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
            ViewBag.EndUser = endUser.Firstname + " " + endUser.Surname;
            endUserVM.EndUser = endUser;



            endUserVM.CalendarViewModel = new CalendarViewModel(endUser.ID, DateTime.Now, 1);
            endUserVM.UpcomingEvents = new MyEventsViewModel(endUser.ID, DateTime.Now, 12);//3 months ahead


            return View(endUserVM);
        }

    }
}
