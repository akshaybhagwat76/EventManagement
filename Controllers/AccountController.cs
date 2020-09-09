using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MiidWeb.Models;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using MiidWeb.Helpers;
using MiidWeb.Repositories;
using System.Web.Security;
using MiidWeb.Navigation;
using System.Text;
using System.Text.RegularExpressions;

namespace MiidWeb.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private ApplicationUserManager _userManager;
        
        private int GetSubdomainID()
        {
            int subDomainID = 0;
            if (HttpContext != null)
            {

                string host = HttpContext.Request.Url.Host;

                //  string company = host.Split('.')[0].ToString();
                string company = host;

                // if (company.Contains("www")) {  company = "training.miid.co.za"; }
                 if (company.Contains("localhost")) { company = "miid.co.za"; }

                subDomainID = LookupHelper.GetSubdomainID(company);
            }
           

            return subDomainID;

        }
        private void GetLayoutFiles()
        {

            string host = HttpContext.Request.Url.Host;

            //  string company = host.Split('.')[0].ToString();
            string company = host;

            //  if (company.Contains("www")) {  company = "training.miid.co.za"; }
           if (company.Contains("localhost")) { company = "miid.co.za"; }
            if (company.ToLower().Contains("gigCulture.miid.co.za")) { company = "GigCulture.miid.co.za"; }
            if (company.ToLower().Contains("farmyardpark.miid.co.za")) { company = "FarmYardPark.miid.co.za"; }
            if (company.ToLower().Contains("www.lulatickets.co.za")) { company = "lulatickets.co.za"; }
            if (company.ToLower().Contains("lulatickets.co.za")) { company = "lulatickets.co.za"; }
            GlobalVariables.Company = company;

            int subDomainID = LookupHelper.GetSubdomainID(company);
            using (var db = new MiidEntities())
            {
                Subdomain subDomain = db.Subdomains.Find(subDomainID);
                if (subDomain != null)
                {
                    GlobalVariables.SubdomainID = subDomain.ID.ToString();
                    GlobalVariables.Text1 = subDomain.Text1;
                    GlobalVariables.Text2 = subDomain.Text2;
                    GlobalVariables.Text3 = subDomain.Text3;
                    GlobalVariables.Text4 = subDomain.Text4;
                    GlobalVariables.Text5 = subDomain.Text5;
                    GlobalVariables.Text6 = subDomain.Text6;

                }
            }
            if (company == "miid")
            {
                GlobalVariables.LayoutLogin = "~/Views/Shared/_LayoutLogin.cshtml";
                GlobalVariables.LayoutAdmin = "~/Views/Shared/_LayoutAdmin.cshtml";
                GlobalVariables.Layout = MiidWeb.Repositories.TicketRepository.GetLayoutFile("Layout");

            }
            else
            {
                GlobalVariables.LayoutLogin = String.Format("~/Views/Shared/_LayoutLogin{0}.cshtml", company);
                GlobalVariables.LayoutAdmin = String.Format("~/Views/Shared/_LayoutAdmin{0}.cshtml", company);
                GlobalVariables.Layout = String.Format("~/Views/Shared/_Layout{0}.cshtml", company);

            }


            GetLogosAndTagLines(company);

        }

        private void GetLogosAndTagLines(string company)
        {
            switch (company)
            {
                case "foundation": GlobalVariables.LogoFile = "../Content/images/logofoundation.jpg"; break;
                case "wrhi": GlobalVariables.LogoFile = "../Content/images/logowrhi.jpg"; break;
                case "righttocare": GlobalVariables.LogoFile = "../Content/images/logorighttocare.png"; break;
                case "match": GlobalVariables.LogoFile = "../Content/images/logomatch.jpg"; break;
                case "khethimpilo": GlobalVariables.LogoFile = "../Content/images/logokhethimpilo.jpg"; break;
                case "anovahealth": GlobalVariables.LogoFile = "../Content/images/logoanovahealth.jpg"; break;
                case "broadreach": GlobalVariables.LogoFile = "../Content/images/logobroadreach.jpg"; break;
                case "nhls": GlobalVariables.LogoFile = "../Content/images/logonhls.jpg"; break;
                default: GlobalVariables.LogoFile = "http://www.sead.co.za/wp-content/uploads/portraitlogolight.png"; break;

            }
            switch (company)
            {
                case "foundation": GlobalVariables.CompanyTagLine = "Health Strategy | Insight | Implementation"; break;
                case "wrhi": GlobalVariables.CompanyTagLine = "Health Strategy | Insight | Implementation"; break;
                case "righttocare": GlobalVariables.CompanyTagLine = "Health Strategy | Insight | Implementation"; break;

                case "match": GlobalVariables.CompanyTagLine = "Maternal Adolescent and Child Health Systems"; break;
                case "khethimpilo": GlobalVariables.CompanyTagLine = "Choose Life for an AIDS Free Generation."; break;
                case "anovahealth": GlobalVariables.CompanyTagLine = "Anova is dedicated to strengthening quality public health services…"; break;
                case "broadreach": GlobalVariables.CompanyTagLine = "Unlock Human & Economic Potential"; break;
                case "nhls": GlobalVariables.CompanyTagLine = "Africa's centre of excellence for innovative laboratory medicine."; break;
                default: GlobalVariables.CompanyTagLine = "Health Strategy | Insight | Implementation"; break;

            }
        }




        public AccountController()
        {


        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager,
    ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager= roleManager;

        }



        private ApplicationRoleManager roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return this.roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set { this.roleManager = value; }
        }


        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            //ViewBag.ReturnUrl = returnUrl;
            //return View();
            TicketRepository.DeleteExpiredReservedTicketsAll();
            TicketRepository.ClearExpiredTicketSeats();//Clears every 15 minutes

            return RedirectToAction("Index", "Home");
        }

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {

            if (!ModelState.IsValid)
            {
                //return View(model);
                //Session["LoginViewModel"] = model;
                string errormessage = "Enter your email address & password";
                ModelState.AddModelError("", errormessage);
                model.ReturnUrl = returnUrl;
                return View(model);

               // Growl("Mi-ID", "Please enter a valid email and password.");
                return RedirectToAction("LoginAlternative", "LoginAlternative");

                //Response.Redirect("\\");
            }
            else
            {
                model.Email = model.Email.Trim();

                switch (UserHelper.UserEmailConfirmed(model.Email))
                {

                    case "Active":

                        if (EndUserRepository.AmIAnEventOrganiser(model.Email))
                        {
                            Session["EventOrganiser"] = EventOrganiserRepository.GetEventOrganiserByEmail(model.Email);
                        }
                        else
                        {
                            Session["UserID"] = UserHelper.UserID(model.Email);
                            Session["EndUser"] = EndUserRepository.GetUser(model.Email, true);
                        }
                        // This doesn't count login failures towards account lockout
                        // To enable password failures to trigger account lockout, change to shouldLockout: true
                        var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);

                        if (EndUserRepository.AmIAnEventOrganiser(model.Email) && EndUserRepository.AmIAnEventOrganiser(model.Email, AmILockedOut: true))
                        {
                            //override signinstatus to locked out
                            result = SignInStatus.LockedOut;
                        }
                        switch (result)
                        {
                            case SignInStatus.Success:
                                //new redirect for DON
                                //return SetAuthCookieAndRedirect(model.Email, model.RememberMe, returnUrl);

                             


                                //check for ticket selected cookie
                                var cookie = Request.Cookies["MiiD_selected_tickets"];
                                if (cookie != null)
                                {
                                    string value = cookie.Value;
                                    string PromoCode = "";
                                    string friendEmailList = "";

                                    var cookie2 = Request.Cookies["MiiD_PromoCode"];
                                    if (cookie2 != null)
                                    {
                                        PromoCode = cookie2.Value;
                                    }

                                    var cookie3 = Request.Cookies["MiiD_FriendEmailList"];
                                    if (cookie3 != null)
                                    {
                                        friendEmailList = cookie3.Value;
                                    }

                                    if (EndUserRepository.IsUserUpToDate(UserHelper.UserID(model.Email)))
                                    {
                                        
                                        return RedirectToAction("PurchaseTickets2", "Events", new { Json = JsonCleaner.RemoveSpaces( value), promocode = PromoCode, friendEmailList = friendEmailList });
                                    }
                                    else
                                    {
                                        // Growl("Mi-iD", "Please complete a few more details before purchasing tickets.");
                                        return RedirectToAction("CreateFirst", "EndUsers", new { Email = model.Email, Cookie = cookie.Value, friendEmailList = friendEmailList });

                                    }

                                }
                                var cashlessCookie = Request.Cookies["TopupFundsForEvent"];
                                if (cashlessCookie != null)
                                {
                                    string value = cashlessCookie.Value;

                                    if (EndUserRepository.IsUserIDValid(UserHelper.UserID(model.Email)))
                                    {
                                        return RedirectToAction("TopupFundsMenu", "MyMoneys", new { EventID = value });
                                    }
                                    else
                                    {
                                        Growl("Mi-iD", "Please check your ID number on your profile before topping up funds.");
                                        return RedirectToAction("CreateFirst", "EndUsers", new { Email = model.Email, IsCashless = "true" });

                                    }

                                }

                                if (Session["EventID"] != null && int.Parse(Session["EventID"].ToString()) != 0)
                                {
                                    return RedirectToAction("Details", "Events", new { id = int.Parse(Session["EventID"].ToString()) });

                                }
                                return RedirectToLocal(returnUrl);
                            case SignInStatus.LockedOut:
                                return View("Lockout");
                            case SignInStatus.RequiresVerification:
                                return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                            case SignInStatus.Failure:
                            default:
                                string errormessage = "Username does not exist";

                                if (EndUserRepository.DoesAspNetUserExist(model.Email))
                                {
                                    errormessage = "Password incorrect.";
                                }
                                ModelState.AddModelError("", errormessage);
                                model.ReturnUrl = returnUrl;
                                return View(model);
                        }


                    case "Pending":

                        var result2 = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
                        switch (result2)
                        {
                            case SignInStatus.Success:

                                return RedirectToAction("ConfirmEmailLink", "EndUsers", new { email = model.Email });
                            case SignInStatus.LockedOut:
                                return View("Lockout");
                            case SignInStatus.RequiresVerification:
                                return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                            case SignInStatus.Failure:
                            default:
                                ModelState.AddModelError("", "Invalid login attempt.");
                                return View(model);
                        }



                    case "Not Registered":

                        var result3 = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
                        switch (result3)
                        {
                            case SignInStatus.Success:

                                return RedirectToAction("CreateFirst", "EndUsers", new { Email = model.Email });
                            case SignInStatus.LockedOut:
                                return View("Lockout");
                            case SignInStatus.RequiresVerification:
                                return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                            case SignInStatus.Failure:
                            default:
                                ModelState.AddModelError("", "Username does not exist.");
                                return View(model);
                        }



                    default:
                        var result4 = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
                        switch (result4)
                        {
                            case SignInStatus.Success:

                                return RedirectToAction("CreateFirst", "EndUsers", new { Email = model.Email });
                            case SignInStatus.LockedOut:
                                return View("Lockout");
                            case SignInStatus.RequiresVerification:
                                return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                            case SignInStatus.Failure:
                            default:
                                ModelState.AddModelError("", "Username does not exist.");
                                return View(model);
                        }


                }

            }
        }

        

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            var user = await UserManager.FindByIdAsync(await SignInManager.GetVerifiedUserIdAsync());
            if (user != null)
            {
                var code = await UserManager.GenerateTwoFactorTokenAsync(user.Id, provider);
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult RegisterNewOrganiser(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }



        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterNewOrganiser(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //csh moved email and confirm email validation to here so we can to lower them before comparing.
                IdentityResult result;

                if (String.IsNullOrEmpty(model.ConfirmEmail))
                {
                    result = new IdentityResult("Please Confirm Email.");
                    AddErrors(result);
                    return View(model);
                }

                if (model.Email.ToLower() != model.ConfirmEmail.ToLower())
                {
                    result = new IdentityResult("The Email and Confirm Email do not match.");
                    AddErrors(result);
                    return View(model);
                }

                model.Email = model.Email.Trim();

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //ds removed 
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    //LogOff2(true, model.Email);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    EndUserRepository.SetEmailConfirmedOnAspNet(model.Email);
                    var enduser = EndUserRepository.GetUser(model.Email, true);
                    SendEmailToNewUser(enduser);

                    //check for ticket selected cookie
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
                        var cookie3 = Request.Cookies["MiiD_FriendEmailList"];
                        string friendEmailList = "";
                        if (cookie3 != null)
                        {
                            friendEmailList = cookie3.Value;
                        }
                        if (EndUserRepository.IsUserUpToDate(UserHelper.UserID(model.Email)))
                        {
                            return RedirectToAction("PurchaseTickets2", "Events", new { Json = JsonCleaner.RemoveSpaces(value), promocode = PromoCode, friendEmailList= friendEmailList });
                        }
                        else
                        {
                            // Growl("Mi-iD", "Please complete a few more details before purchasing tickets.");
                            return RedirectToAction("CreateFirst", "EndUsers", new { Email = model.Email, Cookie = cookie.Value, promocode = PromoCode, friendEmailList = friendEmailList });

                        }

                    }
                    var cashlessCookie = Request.Cookies["TopupFundsForEvent"];
                    if (cashlessCookie != null)
                    {
                        string value = cashlessCookie.Value;
                       
                        if (EndUserRepository.IsUserIDValid(UserHelper.UserID(model.Email)))
                        {
                            return RedirectToAction("TopupFundsMenu", "MyMoneys", new { EventID = value });
                        }
                        else
                        {
                            Growl("Mi-iD", "Please check your ID number on your profile before topping up funds.");
                            return RedirectToAction("CreateFirst", "EndUsers", new { Email = model.Email, IsCashless="true" });

                        }

                    }


                    return RedirectToAction("Index", "Home");
                    //return RedirectToAction("CreateFirst", "EndUsers", new { Email = model.Email, ReturnUrl = model.ReturnUrl });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }



        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //csh moved email and confirm email validation to here so we can to lower them before comparing.
                IdentityResult result;

                if (String.IsNullOrEmpty(model.ConfirmEmail))
                {
                    result = new IdentityResult("Please Confirm Email.");
                    AddErrors(result);
                    return View(model);
                }

                if (model.Email.ToLower() != model.ConfirmEmail.ToLower())
                {
                    result = new IdentityResult("The Email and Confirm Email do not match.");
                    AddErrors(result);
                    return View(model);
                }

                model.Email = model.Email.Trim();


                



                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //ds removed 
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    //LogOff2(true, model.Email);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    EndUserRepository.SetEmailConfirmedOnAspNet(model.Email);
                    var enduser = EndUserRepository.GetUser(model.Email, true);
                    SendEmailToNewUser(enduser);

                    //check for ticket selected cookie
                    string value = "";
                    string PromoCode = "";
                    string friendEmailList = "";

                    var cashlessCookie = Request.Cookies["TopupFundsForEvent"];
                    if (cashlessCookie != null)
                    {
                        value = cashlessCookie.Value;

                        if (EndUserRepository.IsUserIDValid(UserHelper.UserID(model.Email)))
                        {
                            return RedirectToAction("TopupFundsMenu", "MyMoneys", new { EventID = value });
                        }
                        else
                        {
                            Growl("Mi-iD", "Please check your ID number on your profile before topping up funds.");
                            return RedirectToAction("CreateFirst", "EndUsers", new { Email = model.Email, IsCashless = "true" });

                        }

                    }


                  

                    var cookie = Request.Cookies["MiiD_selected_tickets"];

                    if (cookie != null)
                    {
                        value = cookie.Value;
                    }

                    var cookie2 = Request.Cookies["MiiD_PromoCode"];
                    if (cookie2 != null)
                    {
                        PromoCode = cookie2.Value;
                    }
                    var cookie3 = Request.Cookies["MiiD_FriendEmailList"];
                    if (cookie3 != null)
                    {
                        friendEmailList = cookie3.Value;
                    }
                    if (EndUserRepository.IsUserUpToDate(UserHelper.UserID(model.Email)))
                    {
                        return RedirectToAction("PurchaseTickets2", "Events", new { Json = JsonCleaner.RemoveSpaces(value), promocode = PromoCode, friendEmailList = friendEmailList });
                    }
                    else
                    {
                        // Growl("Mi-iD", "Please complete a few more details before purchasing tickets.");
                        return RedirectToAction("CreateFirst", "EndUsers", new { Email = model.Email, Cookie = value, promocode = PromoCode, friendEmailList = friendEmailList });

                    }

                

                    //return RedirectToAction("Index", "Home");
                    //return RedirectToAction("CreateFirst", "EndUsers", new { Email = model.Email, ReturnUrl = model.ReturnUrl });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }



        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult RegisterEventOrganiser()
        {

            return View();
        }



        //
        // POST: /Account/RegisterEvent organiser. this registeres them from the Home page while they are logged out
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterEventOrganiser(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //csh moved email and confirm email validation to here so we can to lower them before comparing.
                IdentityResult result;

                if (String.IsNullOrEmpty(model.ConfirmEmail))
                {
                    result = new IdentityResult("Please Confirm Email.");
                    AddErrors(result);
                    return View(model);
                }

                if (model.Email.ToLower() != model.ConfirmEmail.ToLower())
                {
                    result = new IdentityResult("The Email and Confirm Email do not match.");
                    AddErrors(result);
                    return View(model);
                }

                model.Email = model.Email.Trim();

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //ds removed 
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    //LogOff2(true, model.Email);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");








                    return RedirectToAction("Create", "EventOrganisers");
                    //return RedirectToAction("CreateFirst", "EndUsers", new { Email = model.Email, ReturnUrl = model.ReturnUrl });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View();
        }

        private bool SendEmailToNewUser(UserModel e)
        {
            using (var db = new MiidEntities())
            {

                

                var SUBDOMAIN = db.Subdomains.Find(GetSubdomainID());
                string fileName = Server.MapPath(String.Format(@"~\Content\EmailTemplates\{0}", SUBDOMAIN.EmailRegistration));


                string body1 = System.IO.File.ReadAllText(fileName);

                body1 = body1.Replace("<Firstname>", e.FirstName);
                body1 = body1.Replace("<Email>", e.Email);


                Helpers.EmailHelper.SendMail(e.Email, "MiiD Registration Confirmation", body1.ToString(),null,null,null,SUBDOMAIN.ID);
                return Helpers.EmailHelper.SendMail("newaccounts@miid.co.za", "MiiD Registration Confirmation", body1.ToString(), null, null, null, SUBDOMAIN.ID);
            }
        }


        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword(string ReturnUrl = "")
        {
            ViewBag.ReturnUrl = ReturnUrl;

            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {

            model.Email = model.Email.Trim();

            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");

                EmailHelper.SendMail(model.Email, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>", null, null, null, ConfigRepo.GetSubdomainID());
                return RedirectToAction("ForgotPasswordConfirmation", "Account", new { ReturnUrl = model.ReturnUrl });
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation(string ReturnUrl = "")
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            model.Email = model.Email.Trim();
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("CreateFirst", "EndUsers", new { Email = model.Email });
                //return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        //return RedirectToLocal(returnUrl);
                        return RedirectToAction("CreateFirst", "EndUsers", new { Email = model.Email });
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOff2(bool FromRegistration, string Email)
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("CreateFirst", "EndUsers", new { Email = Email });
        }


        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion


        [Authorize(Roles = "Admin")]
        public ActionResult RoleCreate()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleCreate(string roleName)
        {
            using (var context = new ApplicationDbContext())
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);

                roleManager.Create(new IdentityRole(roleName));
                context.SaveChanges();
            }

            ViewBag.ResultMessage = "Role created successfully !";
            return RedirectToAction("RoleIndex", "Account");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult RoleIndex()
        {
            List<string> roles;
            using (var context = new ApplicationDbContext())
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);

                roles = (from r in roleManager.Roles select r.Name).ToList();
            }

            return View(roles.ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult RoleDelete(string roleName)
        {
            using (var context = new ApplicationDbContext())
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var role = roleManager.FindByName(roleName);

                roleManager.Delete(role);
                context.SaveChanges();
            }

            ViewBag.ResultMessage = "Role deleted succesfully !";
            return RedirectToAction("RoleIndex", "Account");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult RoleAddToUser()
        {
            List<string> roles;
            List<string> users;
            using (var context = new ApplicationDbContext())
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);

                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                users = (from u in userManager.Users select u.UserName).ToList();
                roles = (from r in roleManager.Roles select r.Name).ToList();
            }

            ViewBag.Roles = new SelectList(roles);
            ViewBag.Users = new SelectList(users);
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(string roleName, string userName)
        {
            List<string> roles;
            List<string> users;
            using (var context = new ApplicationDbContext())
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);

                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                users = (from u in userManager.Users select u.UserName).ToList();

                var user = userManager.FindByName(userName);
                if (user == null)
                    throw new Exception("User not found!");

                var role = roleManager.FindByName(roleName);
                if (role == null)
                    throw new Exception("Role not found!");

                if (userManager.IsInRole(user.Id, role.Name))
                {
                    ViewBag.ResultMessage = "This user already has the role specified !";
                }
                else
                {
                    userManager.AddToRole(user.Id, role.Name);
                    context.SaveChanges();

                    ViewBag.ResultMessage = "Username added to the role succesfully !";
                }

                roles = (from r in roleManager.Roles select r.Name).ToList();
            }

            ViewBag.Roles = new SelectList(roles);
            ViewBag.Users = new SelectList(users);
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string userName)
        {
            if (!string.IsNullOrWhiteSpace(userName))
            {
                List<string> userRoles;
                List<string> roles;
                List<string> users;
                using (var context = new ApplicationDbContext())
                {
                    var roleStore = new RoleStore<IdentityRole>(context);
                    var roleManager = new RoleManager<IdentityRole>(roleStore);

                    roles = (from r in roleManager.Roles select r.Name).ToList();

                    var userStore = new UserStore<ApplicationUser>(context);
                    var userManager = new UserManager<ApplicationUser>(userStore);

                    users = (from u in userManager.Users select u.UserName).ToList();

                    var user = userManager.FindByName(userName);
                    if (user == null)
                        throw new Exception("User not found!");

                    var userRoleIds = (from r in user.Roles select r.RoleId);
                    userRoles = (from id in userRoleIds
                                 let r = roleManager.FindById(id)
                                 select r.Name).ToList();
                }

                ViewBag.Roles = new SelectList(roles);
                ViewBag.Users = new SelectList(users);
                ViewBag.RolesForThisUser = userRoles;
            }

            return View("RoleAddToUser");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string userName, string roleName)
        {
            List<string> userRoles;
            List<string> roles;
            List<string> users;
            using (var context = new ApplicationDbContext())
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);

                roles = (from r in roleManager.Roles select r.Name).ToList();

                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                users = (from u in userManager.Users select u.UserName).ToList();

                var user = userManager.FindByName(userName);
                if (user == null)
                    throw new Exception("User not found!");

                if (userManager.IsInRole(user.Id, roleName))
                {
                    userManager.RemoveFromRole(user.Id, roleName);
                    context.SaveChanges();

                    ViewBag.ResultMessage = "Role removed from this user successfully !";
                }
                else
                {
                    ViewBag.ResultMessage = "This user doesn't belong to selected role.";
                }

                var userRoleIds = (from r in user.Roles select r.RoleId);
                userRoles = (from id in userRoleIds
                             let r = roleManager.FindById(id)
                             select r.Name).ToList();
            }

            ViewBag.RolesForThisUser = userRoles;
            ViewBag.Roles = new SelectList(roles);
            ViewBag.Users = new SelectList(users);
            return View("RoleAddToUser");
        }


        ActionResult SetAuthCookieAndRedirect(string userName, bool createPersistentCookie = false, string returnUrl = null)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
            var url = IsValidReturnUrl(returnUrl) ?
                returnUrl :
                NavigationCookies.GetReturnAfterAuthenticationUrl(Request);
            if (string.IsNullOrEmpty(url)) return RedirectToAction("Index", "Home");
            return new RedirectResult(url);
        }

        private bool IsValidReturnUrl(string returnUrl)
        {
            return Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                   && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\");
        }
        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}