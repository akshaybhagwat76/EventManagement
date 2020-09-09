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
using System.IO;
using MiidWeb.Repositories;
using Microsoft.Owin.Security;
using System.Data.Entity.Validation;
using System.Web.Security;
using System.Web.Configuration;
using MiidWeb.Helpers;
using PagedList;
using System.Configuration;

namespace MiidWeb.Controllers
{
    [Authorize]

    public class EndUsersController : BaseController
    {
        private MiidEntities db = new MiidEntities();


        private int GetSubdomainID()
        {
            string host = HttpContext.Request.Url.Host;

            //   string company = host.Split('.')[0].ToString();
            string company = host;

            //  if (company.Contains("www")) {  company = "training.miid.co.za"; }
             if (company.Contains("localhost")) { company = "miid.co.za"; }
            if (company.ToLower().Contains("gigCulture.miid.co.za")) { company = "GigCulture.miid.co.za"; }
            if (company.ToLower().Contains("farmyardpark.miid.co.za")) { company = "FarmYardPark.miid.co.za"; }
            if (company.ToLower().Contains("www.lulatickets.co.za")) { company = "lulatickets.co.za"; }
            if (company.ToLower().Contains("lulatickets.co.za")) { company = "lulatickets.co.za"; }


            int subDomainID = LookupHelper.GetSubdomainID(company);

            return subDomainID;

        }

        [Authorize(Roles = "Admin")]
        // GET: EndUsers
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";


            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var endUsers = db.EndUsers.Include(e => e.AccountType).Include(e => e.Status);
            var tickets = db.Tickets.Include(t => t.TicketClass).Include(t => t.EndUser);

            if (!String.IsNullOrEmpty(searchString))
            {
                endUsers = endUsers.Where(s => s.Firstname.Contains(searchString)
                                       || s.Surname.Contains(searchString)
                                       || s.Cell.Contains(searchString)
                                       || s.Email.Contains(searchString)
                                       || s.IDNumber.Contains(searchString)

                                       );


            }

            switch (sortOrder)
            {
                case "name_desc":
                    endUsers = endUsers.OrderByDescending(s => s.Firstname);
                    break;
                case "Date":
                    endUsers = endUsers.OrderBy(s => s.Surname);
                    break;
                case "date_desc":
                    endUsers = endUsers.OrderByDescending(s => s.ID);
                    break;
                default:  // Name ascending 
                    endUsers = endUsers.OrderBy(s => s.Email);
                    break;
            }

            int pageSize = 30;
            int pageNumber = (page ?? 1);
            return View(endUsers.ToPagedList(pageNumber, pageSize));


        }
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.AccountTypeID = new SelectList(db.AccountTypes, "ID", "Code");
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code");
            return View();
        }
        [Authorize]
        // POST: EndUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ID,Surname,Firstname,DateOfBirth,AccountTypeID,IDNumber,Age,Sex,RaceID,StreetAddress,Suburb,City,PostalCode,Telephone,Cell,Email,StatusID,StartDate,AccountBalance,IsChildUser,NextOfKin,NextOfKinTelephone,Medication,MedicalAidCompany,MedicalAidNumber,Allergies,KnownConditions,AspNetUserID")] EndUser endUser)
        {
            if (ModelState.IsValid)
            {
                endUser.ProfilePicURL = "avatar_blank.png";
                db.EndUsers.Add(endUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountTypeID = new SelectList(db.AccountTypes, "ID", "Code", endUser.AccountTypeID);
            ViewBag.StatusID = new SelectList(db.Status, "ID", "Code", endUser.StatusID);
            return View(endUser);
        }
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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

        [Authorize]
        //NEW METHODS

        // GET: EndUsers/Edit/5
        public ActionResult MiiDetails(int? id)
        {
            if ((UserHelper.UserEmailConfirmed(User.Identity.Name)) == "Pending")
            {
                return RedirectToAction("ConfirmEmailLink", "EndUsers", new { email = User.Identity.Name });
            }

            var LoggedInUserID = Helpers.UserHelper.UserID(User.Identity.Name);
            if (!EndUserRepository.IsUserUpToDate(LoggedInUserID))
            {
                Growl("Message", "Please complete all your profile fields before purchasing event tickets.");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EndUser endUser = db.EndUsers.Find(LoggedInUserID);

            CreateFirstEndUserViewModel v = new CreateFirstEndUserViewModel(endUser);


            string SelectedProvinceId = v.Province;


            if (endUser == null)
            {
                return HttpNotFound();
            }
            //ViewBag.AccountTypeID = new SelectList(db.AccountTypes, "ID", "Code", endUser.AccountTypeID);

            //ViewBag.StatusID = new SelectList(db.Status, "ID", "Code", endUser.StatusID);
            return View(v);
        }
        [Authorize]
        // POST: EndUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MiiDetails(CreateFirstEndUserViewModel v, string imageData)
        {

            // imageData = "";
            try
            {

                bool error = false;
                //if (db.EndUsers.Where(x => x.IDNumber == v.IDNumber && x.ID != v.ID).Count() > 0)
                //{
                //  ModelState.AddModelError("IDNumberError", "ID Number already in use");
                //  error = true;
                //}

                if (!string.IsNullOrWhiteSpace(v.Cell))
                {
                    if (v.Cell.Length < 10)
                    {
                        ModelState.AddModelError("CellError", "Cell number must be 10 digits long");
                        error = true;
                    }
                }
                else
                {
                    ModelState.AddModelError("CellError", "Cell number must be 10 digits long");
                    error = true;

                }
                //    if (!string.IsNullOrWhiteSpace(v.Telephone))
                //   {
                //      if (v.Telephone.Length < 10)
                //      {
                //         ModelState.AddModelError("TelephoneError", "Telephone number must be 10 digits long");
                //          error = true;
                //     }
                //  }
                //   else
                //   {
                //      ModelState.AddModelError("TelephoneError", "Telephone number must be 10 digits long");
                //      error = true;

                //  }

                if (v.TermsAndConditions == false)
                {
                    ModelState.AddModelError("TermsAndConditionsError", "Please accept terms and conditions");
                    error = true;
                }


                if (v.Female == false && v.Male == false)
                {
                    ModelState.AddModelError("SexError", "Please select Gender");
                    error = true;
                }

                //if (v.RaceID == 0)
                //{
                //  ModelState.AddModelError("RaceError", "Please select Race");
                //  error = true;
                //}

                if (error)
                    goto here;

                if (ModelState.IsValid)
                {


                    EndUser e = db.EndUsers.Find(v.ID);
                    if (!String.IsNullOrEmpty(imageData))
                    {
                        var guid = System.Guid.NewGuid();
                        var bytes = Convert.FromBase64String(imageData);
                        var fileName = String.Format("{0}_{1}_{2}_{3}.png", v.ID.ToString(), v.Surname, v.Firstname, guid.ToString().Substring(0, 6));
                        var filePath = Path.Combine(Server.MapPath("~/images"), fileName);
                        using (var imageFile = new FileStream(filePath, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();
                        }

                        e.ProfilePicURL = fileName;
                    }
                    else
                    {
                        e.ProfilePicURL = v.ProfilePicURL;
                    }

                    e.AccountTypeID = 1;
                    e.RaceID = v.RaceID;
                    e.Firstname = v.Firstname;
                    e.Surname = v.Surname;
                    //  e.Telephone = v.Telephone;
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

                    e.Sex = v.Male ? "M" : "F";

                    e.IDNumber = v.IDNumber;
                    e.KnownConditions = v.KnownConditions;

                    e.MedicalAidCompany = v.MedicalAidCompany;
                    e.MedicalAidNumber = v.MedicalAidNumber;
                    e.Medication = v.Medication;
                    e.NextOfKin = v.NextOfKin;
                    e.NextOfKinTelephone = v.NextOfKinTelephone;
                    //e.ProfilePicURL = v.ProfilePicURL;
                    e.NewsletterAccepted = v.Newsletter;
                    e.TermsAndConditionsAccepted = v.TermsAndConditions;


                    db.Entry(e).State = EntityState.Modified;

                    //  ValidateModel(e);

                    db.SaveChanges();

                    Growl("Edit Profile", "Profile updated successfully.");

                    return RedirectToAction("EndUserProfile", "EndUsers", new { ID = e.ID, LoggedInUserID = e.ID });
                }


            }
            catch (System.ComponentModel.DataAnnotations.ValidationException e)
            {

                Growl("Error", e.Message);
            }

        here:

            ViewBag.Email = v.Email;


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

            List<SelectListItem> date11;
            GetNumberDropDown(out date11, 1, 31);
            ViewBag.DateOfBirthDay = date11;


            var date2 = GetMonthDropDown();
            ViewBag.DateOfBirthMonth = date2;


            var date3 = new List<SelectListItem>();
            GetNumberDropDown(out date3, 1931, 2010);
            ViewBag.DateOfBirthYear = date3;


            v.Male = false;
            v.Female = false;


            return View(v);


        }



        // GET: EndUsers/Edit/5
        public ActionResult RegisterChildAccount()
        {


            var LoggedInUserID = Helpers.UserHelper.UserID(User.Identity.Name);



            EndUser endUser = db.EndUsers.Find(LoggedInUserID);
            ViewBag.ParentPhone = endUser.Cell;

            ChildUserViewModel v = new ChildUserViewModel(endUser.ID);



            if (endUser == null)
            {
                return HttpNotFound();
            }

            return View(v);
        }
        [Authorize]
        // POST: EndUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterChildAccount(ChildUserViewModel v, string imageData)
        {

            // imageData = "";
            try
            {


                if (ModelState.IsValid)
                {

                    if (String.IsNullOrEmpty(v.Firstname) || String.IsNullOrEmpty(v.Surname))
                    {
                        Growl("Register Linked Account Error", "Please complete all fields.");

                    }


                    EndUser e = db.EndUsers.Find(v.ID);

                    if (e != null)
                    {


                        e.Firstname = v.Firstname;
                        e.Surname = v.Surname;
                        e.Cell = v.Cell;
                        e.NextOfKin = v.NextOfKin;
                        e.NextOfKinTelephone = v.NextOfKinTelephone;


                        db.Entry(e).State = EntityState.Modified;
                        db.SaveChanges();
                        Growl("Edit Profile", "Profile updated successfully.");
                        return RedirectToAction("IndexByUserID", "MyMoneys");

                    }
                    else
                    {
                        EndUser child = new EndUser();
                        child.ParentUserID = v.ParentUserID;
                        child.NextOfKin = v.NextOfKin;
                        child.NextOfKinTelephone = v.NextOfKinTelephone;
                        child.AccountTypeID = 2;
                        child.Cell = v.Cell;
                        child.Firstname = v.Firstname;
                        child.Surname = v.Surname;
                        child.Email = String.Format("{0}{1}{2}.miid.co.za", v.Firstname.ToLower(), v.Surname.ToLower(), new Random(100).Next());
                        child.Email = child.Email.Replace(" ", "");
                        child.StatusID = 1;
                        db.EndUsers.Add(child);
                        db.SaveChanges();
                        Growl("Edit Profile", "Profile updated successfully.");
                        return RedirectToAction("IndexByUserID", "MyMoneys");
                    }
                    //  ValidateModel(e);






                }


            }
            catch (System.ComponentModel.DataAnnotations.ValidationException e)
            {

                Growl("Error", e.Message);
            }
            catch (Exception ez)
            {
                string e = ez.Message;
                Growl("Error", ez.Message);
                if (ez.InnerException != null)
                {
                    Growl("Error", ez.InnerException.Message);
                }

            }




            return View(v);


        }



        [Authorize]
        // GET: EndUsers/Edit/5
        public ActionResult AdditionalDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var LoggedInUserID = Helpers.UserHelper.UserID(User.Identity.Name);
            EndUser endUser = db.EndUsers.Find(LoggedInUserID);

            CreateFirstEndUserViewModel v = new CreateFirstEndUserViewModel(endUser);


            if (endUser == null)
            {
                return HttpNotFound();
            }
            //ViewBag.AccountTypeID = new SelectList(db.AccountTypes, "ID", "Code", endUser.AccountTypeID);

            //ViewBag.StatusID = new SelectList(db.Status, "ID", "Code", endUser.StatusID);
            return View(v);
        }
        [Authorize]
        // POST: EndUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdditionalDetails(CreateFirstEndUserViewModel v, string imageData)
        {
            try
            {
                // imageData = "";

                if (ModelState.IsValid)
                {



                    EndUser e = db.EndUsers.Find(v.ID);

                    if (!String.IsNullOrEmpty(imageData))
                    {
                        e.ProfilePicURL = imageData;
                    }
                    else
                    {
                        e.ProfilePicURL = v.ProfilePicURL;
                    }

                    e.AccountTypeID = 1;
                    e.RaceID = v.RaceID;
                    e.Firstname = v.Firstname;
                    e.Surname = v.Surname;
                    // e.Telephone = v.Telephone;
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

                    e.Sex = v.Male ? "M" : "F";

                    e.IDNumber = v.IDNumber;
                    e.KnownConditions = v.KnownConditions;

                    e.MedicalAidCompany = v.MedicalAidCompany;
                    e.MedicalAidNumber = v.MedicalAidNumber;
                    e.Medication = v.Medication;
                    e.NextOfKin = v.NextOfKin;
                    e.NextOfKinTelephone = v.NextOfKinTelephone;
                    //e.ProfilePicURL = v.ProfilePicURL;

                    e.NewsletterAccepted = v.Newsletter;
                    e.TermsAndConditionsAccepted = v.TermsAndConditions;

                    db.Entry(e).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("EndUserProfile", "EndUsers", new { ID = e.ID, LoggedInUserID = e.ID });
                }

                return View(v);
            }
            catch (DbEntityValidationException ex)
            {
                var messages = ex.EntityValidationErrors;

            }
            return View(v);
        }

        private CreateFirstEndUserViewModel prepareCreateFirst(string Email)
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
            GetNumberDropDown(out date3, 1931, 2010);
            ViewBag.DateOfBirthYear = date3;

            //user might exist already
            var User = EndUserRepository.GetByUserName(Email);


            CreateFirstEndUserViewModel v = new CreateFirstEndUserViewModel();
            v.Male = false;
            v.Female = false;

            if (User != null)
            {
                v = new CreateFirstEndUserViewModel(User);
            }

            return v;
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



        // GET: EndUsers/Create
        public ActionResult CreateFirst(string Email, string ReturnUrl, string Cookie = "", string promocode = "", string IsCashless = "false", string friendEmailList = "")
        {
            if (EndUserRepository.AmIAnEventOrganiser(Email))
            {
                return RedirectToAction("MyDetails", "EventOrganisers");
            }
            CreateFirstEndUserViewModel v = prepareCreateFirst(Email);
            v.Cookie = Cookie;
            v.PromoCode = promocode;
            v.IsCashless = IsCashless;
            v.friendEmailList = friendEmailList;

            ViewBag.ReturnUrl = ReturnUrl;
            return View(v);
        }




        // POST: EndUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFirst(CreateFirstEndUserViewModel v, string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;

            if (ModelState.IsValid)
            {
                bool error = false;
                if (v.TermsAndConditions == false)
                {
                    ModelState.AddModelError("TermsAndConditionsError", "Please accept terms and conditions");
                    error = true;
                }
                //if (db.EndUsers.Where(x => x.IDNumber == v.IDNumber).Count() > 0)
                //{
                //  ModelState.AddModelError("IDNumberError", "ID Number already in use");
                //  error = true;
                //}

                if (v.Female == false && v.Male == false)
                {
                    ModelState.AddModelError("SexError", "Please select Gender");
                    error = true;
                }

                if (v.Firstname == v.Email)
                {
                    ModelState.AddModelError("FirstnameError", "Please enter your First Name.");
                    error = true;
                }
                if (v.Surname == v.Email)
                {
                    ModelState.AddModelError("SurnameError", "Please enter your Last Name.");
                    error = true;
                }
                if (v.Cell == null || v.Cell.StartsWith("00000") || v.Cell.Length < 9)
                {
                    ModelState.AddModelError("CellError", "Please enter a valid Cell number.");
                    error = true;
                }

                //Daniel removed this for client who doesnt want ID numbers to be compulsory 2018-08-03
                /*if (v.IDNumber == null || v.IDNumber.StartsWith("00000"))
                {
                    ModelState.AddModelError("IDNumberError", "Please enter a ID / Passport number.");
                    error = true;
                }
                */

                //if (v.RaceID == 0)
                //{
                //  ModelState.AddModelError("RaceError", "Please select Race");
                //  error = true;
                //}

                if (error)
                    goto here;

                try
                {
                    EndUser e = db.EndUsers.Find(v.ID);
                    bool BrandNewUser = false;
                    if (e == null)
                    {
                        e = new EndUser();
                        BrandNewUser = true;
                    }

                    e.AccountTypeID = 1;
                    e.RaceID = v.RaceID;
                    e.Firstname = v.Firstname;
                    e.Surname = v.Surname;
                    //  e.Telephone = v.Telephone;
                    e.Cell = v.Cell;
                    e.StreetAddress = v.StreetAddress;
                    e.Suburb = v.Suburb;
                    e.PostalCode = v.PostalCode;
                    e.City = v.City;
                    e.Province = v.Province;
                    e.IDNumber = v.IDNumber;


                    DateTime date1 = new DateTime(v.DateOfBirthYear, v.DateOfBirthMonth, v.DateOfBirthDay);
                    e.DateOfBirth = date1;
                    e.Email = v.Email;

                    e.Sex = v.Male ? "M" : "F";
                    e.NewsletterAccepted = v.Newsletter;
                    e.TermsAndConditionsAccepted = v.TermsAndConditions;
                    e.StatusID = 1;
                    //Newsletter add fields to e.

                    e.EmailVerificationGuid = System.Guid.NewGuid();

                    //ValidateModel(e);

                    if (BrandNewUser)
                    {
                        db.EndUsers.Add(e);
                    }
                    else
                    {

                        db.Entry(e).State = EntityState.Modified;
                    }


                    db.SaveChanges();

                    StringBuilder body = new StringBuilder();

                    if (BrandNewUser)
                    {
                        SendEmailToNewUser(e, body);
                    }
                    else
                    {
                        SendEmailToUpdatingUser(e, body);
                    }
                    //Email MIID ADMIN
                    // Helpers.EmailHelper.SendMail("info@miid.co.za", "Registration Application: EndUser", "A new EndUser applied for registration: " + e.Email);
                    // //Helpers.EmailHelper.SendMail("jonathanmwallis1808@gmail.com", "Registration Application: EndUser", "A new EndUser applied for registration: " + e.Email);
                    //Helpers.EmailHelper.SendMail("dsouchon@gmail.com", "Registration Application: EndUser", "A new EndUser applied for registration: " + e.Email);



                    //Helpers.EmailHelper.SendMail(endUser.Email, "MiiD Registration Confirmation", "test");

                    var result = ViewBag.Email = e.Email;
                    ViewBag.Response = result.ToString();

                    if (!String.IsNullOrEmpty(v.Cookie))
                    {
                        if (v.Cookie == "MiiFunds Topup")
                        {
                            return RedirectToAction("TopupFundsMenu", "MyMoneys");
                        }
                        else
                        {
                            return RedirectToAction("PurchaseTickets2", "Events", new { Json = JsonCleaner.RemoveSpaces(v.Cookie), promocode = v.PromoCode, friendEmailList = v.friendEmailList });

                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (Exception e)
                {
                    string err = e.Message;

                    StringBuilder sb = new StringBuilder();

                    if (e.InnerException != null)
                    {

                        if (e.InnerException.InnerException != null && e.InnerException.InnerException.Message.ToLower().Contains("duplicate"))
                        {
                            sb.Append(e.InnerException.InnerException.Message);
                            Growl("Miid", sb.ToString());
                            return View(v);
                        }
                    }



                }
            }

        here:

            ViewBag.Email = v.Email;


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

            List<SelectListItem> date11;
            GetNumberDropDown(out date11, 1, 31);
            ViewBag.DateOfBirthDay = date11;


            var date2 = GetMonthDropDown();
            ViewBag.DateOfBirthMonth = date2;


            var date3 = new List<SelectListItem>();
            GetNumberDropDown(out date3, 1931, 2010);
            ViewBag.DateOfBirthYear = date3;


            v.Male = false;
            v.Female = false;


            return View(v);
        }

        private bool SendEmailToNewUser(EndUser e, StringBuilder body)
        {
            using (var db = new MiidEntities())
            {
                var SUBDOMAIN = db.Subdomains.Find(int.Parse(GlobalVariables.SubdomainID));
                string fileName = Server.MapPath(String.Format(@"~\Content\EmailTemplates\{0}", SUBDOMAIN.EmailRegistration));



                string body1 = System.IO.File.ReadAllText(fileName);

                body1 = body1.Replace("<Firstname>", e.Firstname);
                body1 = body1.Replace("<Email>", e.Email);
                // body1 = body1.Replace("<CallbackUrl>", callbackUrl);

                Helpers.EmailHelper.SendMail(e.Email, "MiiD Registration Confirmation", body1.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
                return Helpers.EmailHelper.SendMail("newaccounts@miid.co.za", "MiiD Registration Confirmation", body1.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
            }
        }

        private bool SendEmailToUpdatingUser(EndUser e, StringBuilder body)
        {
            //var callbackUrl = Url.Action("ConfirmEmail", "EndUsers", new { id = e.ID, code = e.EmailVerificationGuid }, protocol: Request.Url.Scheme);
            var SUBDOMAIN = db.Subdomains.Find(GetSubdomainID());
            string fileName = Server.MapPath(String.Format(@"~\Content\EmailTemplates\{0}", SUBDOMAIN.EmailUserUpdate));



            string body1 = System.IO.File.ReadAllText(fileName);

            body1 = body1.Replace("<Firstname>", e.Firstname);
            body1 = body1.Replace("<Email>", e.Email);
            // body1 = body1.Replace("<CallbackUrl>", callbackUrl);

            Helpers.EmailHelper.SendMail(e.Email, "MiiD Registration Confirmation", body1.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
            return Helpers.EmailHelper.SendMail("newaccounts@miid.co.za", "MiiD Registration Confirmation", body1.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
        }



        public ActionResult DownloadMyTicket1(int? id)
        {
            var db = new MiidEntities();

            string username = User.Identity.Name;
            int ticketID = id ?? 0;
            int userID = UserHelper.UserID(username);

            var ticket = db.Tickets.Find(ticketID);

            string fileName = String.Format("Ticket_{0}_{1}.pdf", ticket.ID, ticket.TicketNumber);

            string ticketpath = null;

            string drivepath = HttpContext.Server.MapPath("/");

            if (ticket != null && ticket.EndUserID == userID)
            {
                //this is a valid request
                ticketpath = String.Format("{0}\\MiiDTicketPDFs\\Ticket_{1}_{2}.pdf", drivepath, ticket.ID, ticket.TicketNumber);


            }


            if (ticketpath != null)
            {

                string folder = Path.GetFullPath(ticketpath);
                //HttpContext.Response.AddHeader("content-dispostion", "attachment; filename=" + fileName);


                return File(new FileStream(folder, FileMode.Open), "content-disposition", fileName);
            }

            throw new ArgumentException("Invalid file name of file not exist");
        }


        public ActionResult DownloadMyTicket(int? id = 0)
        {
            try
            {
                var db = new MiidEntities();

                string username = User.Identity.Name;
                int ticketID = id ?? 0;
                int userID = UserHelper.UserID(username);

                var ticket = db.Tickets.Find(ticketID);

                string fileName = String.Format("Ticket_{0}_{1}.pdf", ticket.ID, ticket.TicketNumber);

                string ticketpath = null;

                string drivepath = HttpContext.Server.MapPath("/");

                bool debug = bool.Parse(ConfigurationManager.AppSettings["DebugMode"]);

                if (debug == true)
                {
                    drivepath = @"C:\inetpub\wwwroot\training.miid.co.za";
                }
                if (ticket != null && ticket.EndUserID == userID)
                {
                    //this is a valid request

                    ticketpath = String.Format("{0}\\MiiDTicketPDFs\\Ticket_{1}_{2}.pdf", drivepath, ticket.ID, ticket.TicketNumber);

                }


                //if (ticketpath != null)
                //{

                //    string folder = Path.GetFullPath(ticketpath);
                //    //HttpContext.Response.AddHeader("content-dispostion", "attachment; filename=" + fileName);


                //    return File(new FileStream(folder, FileMode.Open), "content-disposition", fileName);
                //}

                // throw new ArgumentException("Invalid file name of file not exist");




                string path = Path.GetFullPath(ticketpath);


             

                if (System.IO.File.Exists(path))
                {
                    Response.AppendHeader("content-disposition", "attachment;filename=" + fileName);


                    System.IO.FileStream stream = new System.IO.FileStream(path, System.IO.FileMode.Open, FileAccess.Read, FileShare.None); // don't use using keyword!
                    return new FileStreamResult(stream, "content-disposition"); // the constructor will fire Dispose() when done
                }
                else
                    Growl("MiiD", "Ticket not found. Please contact support.");
                return RedirectToAction("MiiEvents", "Events");
            }
            catch (Exception e)
            {
                Growl("MiiD", "Ticket not found. Please contact support." + e.Message);
                return RedirectToAction("MiiEvents", "Events");

            }
        }


        public ActionResult ConfirmEmail(int id, string code)
        {


            var endUser = db.EndUsers.Find(id);
            if (endUser != null && endUser.EmailVerificationGuid.ToString().ToLower() == code.ToLower())
            {
                endUser.EmailVerified = true;
                endUser.StatusID = db.Status.Where(x => x.Context == "EndUser" && x.Description == "Active").First().ID;
                db.SaveChanges();

                EndUserRepository.SetEmailConfirmedOnAspNet(endUser.Email);

                var SUBDOMAIN = db.Subdomains.Find(GetSubdomainID());

                string fileName = Server.MapPath(String.Format(@"~\Content\EmailTemplates\{0}", SUBDOMAIN.EmailAccountActive));


                string body = System.IO.File.ReadAllText(fileName);

                body = body.Replace("<Firstname>", endUser.Firstname);
                body = body.Replace("<Email>", endUser.Email);


                Helpers.EmailHelper.SendMail(endUser.Email, "MiiD Account Active", body.ToString(), null, null, null, ConfigRepo.GetSubdomainID());

                return View(endUser);
            }
            else
            {
                return View("ErrorConfirmEmail");
            }

        }

        [Authorize(Roles = "Admin")]
        public ActionResult AddNewsFeedComment(int id)
        {
            return null;
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult MultipleButtonCancel()
        {

            return null;
        }

        [Authorize]
        public ActionResult EndUserProfile(int? id, int? loggedInUserID = 0)
        {


            if (User.IsInRole("QuickAddStaff"))
            {
                return RedirectToAction("Index", "EventAdmin");

            }

            var cookie = Request.Cookies["MiiD_selected_tickets"];
            if (cookie != null)
            {
                string value = cookie.Value;
                string PromoCode = "";
                var cookie2 = Request.Cookies["MiiD_PromoCode"];
                if (cookie2 != null)
                {
                    PromoCode = cookie2.Value;
                }
                string friendEmailList = "";
                var cookie3 = Request.Cookies["MiiD_FriendEmailList"];
                if (cookie3 != null)
                {
                    friendEmailList = cookie3.Value;
                }
                if (EndUserRepository.IsUserUpToDate(UserHelper.UserID(User.Identity.Name)))
                {
                    return RedirectToAction("PurchaseTickets2", "Events", new { Json = JsonCleaner.RemoveSpaces(value), promocode = PromoCode, friendEmailList = friendEmailList });
                }
                else
                {
                    // Growl("Mi-iD", "Please complete a few more details before purchasing tickets.");
                    return RedirectToAction("CreateFirst", "EndUsers", new { Email = User.Identity.Name, Cookie = cookie.Value });

                }

            }



            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if ((UserHelper.UserEmailConfirmed(User.Identity.Name)) == "Pending")
            {
                return RedirectToAction("ConfirmEmailLink", "EndUsers", new { email = User.Identity.Name });
            }
            //Redirect EO's
            if (EndUserRepository.AmIAnEventOrganiser(User.Identity.Name))
            {
                return RedirectToAction("MyDetails", "EventOrganisers");

            }

            var LoggedInUserID = Helpers.UserHelper.UserID(User.Identity.Name);
            EndUser endUser = db.EndUsers.Find(LoggedInUserID);





            EndUserViewModel endUserVM = new EndUserViewModel(endUser);


            if (endUser == null)
            {
                return HttpNotFound();
            }

            // Your attribs will come here.
            bool createFirstComplete = true;
            if (endUser.Firstname.Length == 0 || endUser.Cell.Length == 0 || endUser.Surname.Length == 0)
            {
                createFirstComplete = false;
            }
            //if (endUser.IDNumber.Length == 0 || endUser.Cell.Length == 0 || endUser.Telephone.Length == 0)
            //{
            //  createFirstComplete = false;
            //}
            //if (endUser.Firstname.Length == 0 || endUser.Surname.Length == 0 || endUser.StreetAddress.Length == 0)
            //{
            //  createFirstComplete = false;
            //}
            //if (endUser.Suburb.Length == 0 || endUser.City.Length == 0 || endUser.PostalCode.Length == 0)
            //{
            //  createFirstComplete = false;
            //}

            if (!createFirstComplete)
            {

                string emailadddress = endUser.Email;

                db.EndUsers.Remove(endUser);
                db.SaveChanges();

                return RedirectToAction("CreateFirst", new { Email = emailadddress });
            }

            endUserVM.EndUser = endUser;
            endUserVM.CalendarViewModel = new CalendarViewModel(endUser.ID, DateTime.Now, 1);
            endUserVM.UpcomingEvents = new MyEventsViewModel(endUser.ID, DateTime.Now, 3);//3 months ahead

            endUserVM.NewsFeedViewModel = new NewsFeedViewModel(endUser.ID, 10, loggedInUserID ?? 0);//Take 10 latest newsfeeditems
            endUserVM.CurrentEvents = db.Events.ToList();
            endUserVM.Friends = db.Friends.Include(x => x.EndUser).Include(x => x.EndUser1).Where(x => x.InitiatingUserID == endUser.ID || x.AcceptingUserID == endUser.ID).ToList();//and status ok



            endUserVM.MentionList = new List<Mention>();
            //add friends to lookup
            foreach (var friend in endUserVM.Friends)
            {
                Mention mention = new Mention();
                mention.id = (int)friend.InitiatingUserID;
                mention.name = friend.EndUser1.Firstname + " " + friend.EndUser1.Surname;
                mention.avatar = "/images/" + friend.EndUser1.ProfilePicURL;
                mention.type = "contact";
                endUserVM.MentionList.Add(mention);
            }
            //add events to lookup
            foreach (var ev in endUserVM.UpcomingEvents.Events)
            {
                Mention mention = new Mention();
                mention.id = (int)ev.EventID;
                mention.name = ev.EventName;
                mention.avatar = "/Content/images/" + ev.ImageURL;
                mention.type = "contact";
                endUserVM.MentionList.Add(mention);

            }
            //Check subdomain and use enduserprofile view for that subdomain
            string ViewFile = MiidWeb.Repositories.TicketRepository.GetSubdomainView("EndUserProfile");

            return View(ViewFile, endUserVM);
        }

        [Authorize]
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

        [Authorize]
        public ActionResult PrevNextMonthMini(int? id, string PrevNext, DateTime CurrentMonth, int loggedInUserID)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var LoggedInUserID = Helpers.UserHelper.UserID(User.Identity.Name);
            EndUser endUser = db.EndUsers.Find(LoggedInUserID);

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

            endUserVM.CalendarViewModel = new CalendarViewModel(endUser.ID, NewMonth, 1);

            return View("EndUserProfile", endUserVM);
        }

        [Authorize]
        public ActionResult MiiCalendar(int? id, int? loggedInUserID = 0)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var LoggedInUserID = Helpers.UserHelper.UserID(User.Identity.Name);
            EndUser endUser = db.EndUsers.Find(LoggedInUserID);


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

        [Authorize]
        public ActionResult Edit2()
        {
            UserModel model = new UserModel();
            return View(model);
        }

        [Authorize]
        public ActionResult Edit3(int id)
        {
            EndUserViewModel model = new EndUserViewModel();

            //EndUserJson userResultModel = new EndUserJson();

            //Pirate pirate = new Pirate();

            //userResultModel.last_name = "Souchon";
            //userResultModel.first_name = "Daniel";
            //userResultModel.email = "dsouchon@gmail.com";
            //userResultModel.url = "/images/Koala.jpg";

            //model.EndUserJson = userResultModel;
            //model.Pirate = pirate;

            model.Friends = db.Friends.Include(x => x.EndUser).Include(x => x.EndUser1).Where(x => x.InitiatingUserID == id || x.AcceptingUserID == id).ToList();//and status ok

            model.MentionList = new List<Mention>();

            foreach (var friend in model.Friends)
            {
                Mention mention = new Mention();
                mention.id = (int)friend.InitiatingUserID;
                mention.name = friend.EndUser1.Firstname + " " + friend.EndUser1.Surname;
                mention.avatar = "/images/" + friend.EndUser1.ProfilePicURL;
                mention.type = "contact";
                model.MentionList.Add(mention);
            }
            //{ id:1, name:'Kenneth Auchenberg', 'avatar':'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'type':'contact' },


            return View(model);
        }



        [Authorize]
        public JsonResult GetToken()
        {
            //EndUserJson userResultModel = new EndUserJson();


            //userResultModel.last_name = "Daniel";
            //userResultModel.first_name = "Sowww";
            //userResultModel.email = "dsouchon@gmail.com";
            //userResultModel.url = "/images/Chrysanthemum.jpg";


            Mention mention = new Mention();
            mention.id = 1;
            mention.name = "Daniel Souchon";
            mention.avatar = "/images/Koala.jpg";
            mention.type = "contact";

            return Json(mention, "json", JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult PopulateDetails(UserModel model)
        {
            UserResultModel userResultModel = new UserResultModel();
            if (String.IsNullOrEmpty(model.UserId))
            {
                userResultModel.Message = "UserId can not be blank";
                return Json(userResultModel);
            }

            UserModel user = EndUserRepository.GetUser(model.UserId);

            if (user == null)
            {
                userResultModel.Message = String.Format("No UserId found for {0}", model.UserId);
                return Json(userResultModel);
            }

            userResultModel.LastName = user.LastName;
            userResultModel.FirstName = user.FirstName;
            userResultModel.Message = String.Empty; //success message is empty in this case

            return Json(userResultModel, "json", JsonRequestBehavior.AllowGet);
        }


        [AllowAnonymous]
        public ActionResult ConfirmEmailLink(string email)
        {
            ViewBag.Email = email;

            LoginViewModel lvm = new LoginViewModel();
            lvm.Email = email;

            return View(lvm);
        }

        private void SuperLogOutNow()
        {

            FormsAuthentication.SignOut();
            Session.Abandon();

            // clear authentication cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            // clear session cookie (not necessary for your current problem but i would recommend you do it anyway)
            SessionStateSection sessionStateSection = (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState");
            HttpCookie cookie2 = new HttpCookie(sessionStateSection.CookieName, "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);

            FormsAuthentication.RedirectToLoginPage();


        }


        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ConfirmEmailLink(LoginViewModel model)
        {
            //  SuperLogOutNow();
            return RedirectToAction("ResendEmailConfirmationLink", new { Email = model.Email });
        }

        public ActionResult ResendEmailConfirmationLink(string Email)
        {
            StringBuilder body = new StringBuilder();
            var user = db.EndUsers.Where(x => x.Email == Email).First();

            SendEmailToUpdatingUser(user, body);
            ViewBag.Email = Email;
            //SuperLogOutNow();

            return View("CheckEmail");
        }


        public ActionResult EndUserProfileTakeYouThere()
        {
            return View();
        }
    }
}
