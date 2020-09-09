using MiidWeb.Models;
using MiidWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
//RSA
namespace MiidWeb.Controllers
{
    [Authorize(Roles = "Admin, EventAdmin, QuickAddStaff")]
    public class EventAdminController : BaseController
    {
        private MiidEntities db = new MiidEntities();
        // GET: EventAdmin

        public ActionResult Index(string firstnamesearch = "", string surnamesearch = "", string idnumbersearch = "")//Dashboard
        {
            EndUserGridViewModel model = new EndUserGridViewModel() { FirstNameSearch = "", SurnameSearch = "", IdNumberSearch = "", Users = null };
            List<EndUserViewModel> users = new List<EndUserViewModel>();


            using (var db = new MiidEntities())
            {
                var blanklist = db.EndUsers.Take(0);
                var filteredResults = blanklist;
                bool blank = false;

                if (String.IsNullOrEmpty(firstnamesearch) && String.IsNullOrEmpty(surnamesearch) && String.IsNullOrEmpty(idnumbersearch))
                {
                    filteredResults = blanklist;
                    blank = true;
                }
                else
                {
                    filteredResults = db.EndUsers.Take(50000);
                }

                if (firstnamesearch != null && !String.IsNullOrEmpty(firstnamesearch))
                {
                    filteredResults = filteredResults.Where(p => p.Firstname.ToLower().Contains(firstnamesearch.ToLower()));
                }
                if (surnamesearch != null && !String.IsNullOrEmpty(surnamesearch))
                {
                    filteredResults = filteredResults.Where(p => p.Surname.ToLower().Contains(surnamesearch.ToLower()));
                }
                if (idnumbersearch != null && !String.IsNullOrEmpty(idnumbersearch))
                {
                    filteredResults = filteredResults.Where(p => p.IDNumber.ToLower().Contains(idnumbersearch.ToLower()));
                }

                if (!blank)
                {

                    foreach (var user in filteredResults)
                    {

                        users.Add(new EndUserViewModel(user));
                    }
                    model.Users = users;
                }

            }

            return View(model);
        }


        public ActionResult FindUserFilterable(string firstnamesearch = "", string surnamesearch = "", string idnumbersearch = "")//Search Form
        {
            try
            {
                EndUserGridViewModel model = new EndUserGridViewModel() { FirstNameSearch = "", SurnameSearch = "", IdNumberSearch = "", Users = null };
                List<EndUserViewModel> users = new List<EndUserViewModel>();
                model.Users = new List<EndUserViewModel>();

                using (var db = new MiidEntities())
                {
                    var blanklist = db.EndUsers.Take(0);
                    var filteredResults = blanklist;
                    bool blank = false;

                    if (String.IsNullOrEmpty(firstnamesearch) && String.IsNullOrEmpty(surnamesearch) && String.IsNullOrEmpty(idnumbersearch))
                    {
                        filteredResults = blanklist;
                        blank = true;
                    }
                    else
                    {
                        filteredResults = db.EndUsers.Take(50000);
                    }

                    if (firstnamesearch != null && !String.IsNullOrEmpty(firstnamesearch))
                    {
                        filteredResults = filteredResults.Where(p => p.Firstname.ToLower().Contains(firstnamesearch.ToLower()));
                    }
                    if (surnamesearch != null && !String.IsNullOrEmpty(surnamesearch))
                    {
                        filteredResults = filteredResults.Where(p => p.Surname.ToLower().Contains(surnamesearch.ToLower()));
                    }
                    if (idnumbersearch != null && !String.IsNullOrEmpty(idnumbersearch))
                    {
                        filteredResults = filteredResults.Where(p => p.IDNumber.ToLower().Contains(idnumbersearch.ToLower()));
                    }

                    if (!blank)
                    {

                        foreach (var user in filteredResults)
                        {
                            var newuser = new EndUserViewModel(user);
                            if(newuser !=null)
                            users.Add(newuser);
                        }
                        model.Users = users.ToList();
                    }
                    return View(model);
                }


            }
            catch (Exception ex)
            {

            }
            return View();
        }
        
        
        public ActionResult AddSiteUser()
        {
            QuickAddEndUser v = new QuickAddEndUser();
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSiteUser(QuickAddEndUser v)
        {
            if (ModelState.IsValid)
            {
                bool error = false;
               

                if (v.Firstname == null)
                {
                    ModelState.AddModelError("First Name", "Please add a first name");
                    Growl("Quick Add Error", "Please add a first name");
                    error = true;
                }


                if (v.Surname == null)
                {
                    ModelState.AddModelError("Surname", "Please add a surname");
                    Growl("Quick Add Error", "Please add a surname");
                    error = true;
                }

              

                if (v.Email == null)
                {
                    ModelState.AddModelError("Emal", "Please add a email address");
                    Growl("Quick Add Error", "Please add email address");
                    error = true;
                }

                if (v.IDNumber == null)
                {
                    ModelState.AddModelError("ID Number", "Please add a an ID number");
                    Growl("Quick Add Error", "Please add ID number. This is needed for pre-paid fund withdraws");
                    error = true;
                }

                if (v.Cell == null)
                {
                    ModelState.AddModelError("Cell", "Please add a cell number");
                    Growl("Quick Add Error", "Please add cell number");
                    error = true;
                }


                if (db.EndUsers.Where(x => x.Email == v.Email).Count() > 0)
                {
                    ModelState.AddModelError("Email", "Email Number already in use");
                   Growl("Quick Add Error", "Email already in use, check email");
                   error = true;
                }

              




                if (error)
                    goto here;

                try
                {

                    string tryCreateLogin = SiteUserRepository.CreateLoginForQuickAddUser(v.Email);
                    if (tryCreateLogin.ToLower().Contains("error"))
                    {
                        ModelState.AddModelError("Username invalid", tryCreateLogin);
                        Growl("Quick Add Error", "UserName invalid");
                        error = true;
                        goto here;
                    }


                    EndUser e = new EndUser();

                    e.AccountTypeID = 1;

                    e.Firstname = v.Firstname;
                    e.Surname = v.Surname;

                    e.Cell = v.Cell;

                    e.IDNumber = v.IDNumber;


                    DateTime date1 = DateTime.Now;//new DateTime(v.DateOfBirthYear, v.DateOfBirthMonth, v.DateOfBirthDay);
                    e.DateOfBirth = date1;
                    e.Email = v.Email;

                    e.Sex = v.Male ? "M" : "F";
                    e.NewsletterAccepted = v.Newsletter;
                    e.TermsAndConditionsAccepted = v.TermsAndConditions;

                    //Newsletter add fields to e.

                    e.EmailVerificationGuid = System.Guid.NewGuid();
                    e.EmailVerified = true;
                    e.StatusID = Helpers.StatusHelper.StatusID("EndUser", "Active");
                    ValidateModel(e);
                    db.EndUsers.Add(e);


                    db.SaveChanges();

                    StringBuilder body = new StringBuilder();

                    //  SendEmailToNewUser(e, body);

                    //Email MIID ADMIN
                    // Helpers.EmailHelper.SendMail("info@miid.co.za", "Registration Application: EndUser", "A new EndUser applied for registration: " + e.Email);
                    // //Helpers.EmailHelper.SendMail("jonathanmwallis1808@gmail.com", "Registration Application: EndUser", "A new EndUser applied for registration: " + e.Email);
                    //Helpers.EmailHelper.SendMail("dsouchon@gmail.com", "Registration Application: EndUser", "A new EndUser applied for registration: " + e.Email);



                    //Helpers.EmailHelper.SendMail(endUser.Email, "MiiD Registration Confirmation", "test");

                    //var result = ViewBag.Email = e.Email;
                    //ViewBag.Response = result.ToString();
                    //return View("CheckEmail");
                    var v1 = new EndUserViewModel(e);

                    return View("AddSiteUserSuccess", v1);
                }
                catch (Exception e)
                {
                    string err = e.Message;
                    Response.Write(e.Message);
                    Response.Write(e.InnerException.Message);
                }
            }

            here:

            ViewBag.Email = v.Email;




            v.Male = false;
            v.Female = false;



            return View(v);
        }

        
        public ActionResult AddSiteUserSuccess(QuickAddEndUser v)//Registration successs
        {
            return View(v);
        }



        
        public ActionResult AddMiiBand(int? id)//Activate Band
        {
            EndUser user = EndUserRepository.GetByUserID(id ?? 0);
            EndUserViewModel model1 = new EndUserViewModel(user);

            ViewBag.UserName = user.Email;
            ViewBag.EndUserID = id;
            ViewBag.UserTagStatusActive = model1.HaveActiveBand;
            NFCTag model = new NFCTag();

            return View(model);
        }
        
        [HttpPost]
        public ActionResult AddMiiBand(NFCTag model)//Activate Band
        {
            EndUser user = EndUserRepository.GetByUserID(model.EndUserID ?? 0);
            EndUserViewModel model1 = new EndUserViewModel(user);

            ViewBag.UserName = user.Email;
            ViewBag.EndUserID = model.EndUserID;
            ViewBag.UserTagStatusActive = model1.HaveActiveBand;

            if (ModelState.IsValid)
            {

                if (NFCTagRepository.AddNewTag(model).ToLower().Contains("error"))
                {

                 

                    Growl("Quick Add Error", "Invalid Tag Number and/or Activation Code");

                    return View(model);
                }
                else
                {
                    return RedirectToAction("AddMiiBandSuccess", new { id = model.EndUserID });
                }

            }
            Growl("Quick Add Error", "Invalid Tag Number and/or Activation Code");
            return View(model);
        }



        
        public ActionResult AddMiiBandSuccess(int? id = 0)//Activate Band Success
        {

            EndUser user = EndUserRepository.GetByUserID(id ?? 0);
            EndUserViewModel model = new EndUserViewModel(user);
            ViewBag.UserName = user.Email;

            return View(model);
        }

        
        public ActionResult EditMiiBand(int? id)//Edit Band
        {
            EndUser user = EndUserRepository.GetByUserID(id ?? 0);
            EndUserViewModel model = new EndUserViewModel(user);
            ViewBag.UserName = user.Email;

            return View(model);
        }

        
        [HttpPost]
        public ActionResult EditMiiBand(EndUserViewModel model, string UserName)//Edit Band
        {
            //Cancel the tag(s) this user has

            NFCTagRepository.CancelMyTags(UserName);

            EndUser user = EndUserRepository.GetByUserName(UserName);
            EndUserViewModel model1 = new EndUserViewModel(user);

            return View("EditMiiBandSuccess", model1);
        }
        
        public ActionResult EditMiiBandSuccess(EndUserViewModel model1)//Edit Success
        {

            return View(model1);
        }

       
        public ActionResult Transactions(int? id = 0)
        {
            EndUser user = EndUserRepository.GetByUserID(id ?? 0);
            EndUserViewModel model = new EndUserViewModel(user);
            ViewBag.UserName = user.Email;



            return View(model);
        }
        
        [HttpPost]
        public ActionResult Transactions(EndUserViewModel model, string UserName, string TransactionAmount)//Edit Band
        {

            try
            {

                EndUser user = EndUserRepository.GetByUserName(UserName);

                //Cancel the tag(s) this user has
                int TransactionAmountInt = int.Parse(TransactionAmount);
                MyMoneyRepository.SaveQuickAddTransaction(TransactionAmountInt, user.ID, model.IsCreditTransaction, User.Identity.Name);

                var modelEUVM = new EndUserViewModel(user);

                ViewBag.IsCreditTransaction = model.IsCreditTransaction;
                ViewBag.TransactionAmount = TransactionAmount;

                return View("TransactionSuccess", modelEUVM);
            }
            catch (Exception e)
            {

                Growl("Error", e.Message);
                return RedirectToAction("Index", "Home");

            }
        }




        
        public ActionResult TransactionSuccess(EndUserViewModel model1)
        {



            return View(model1);
        }



    }
}