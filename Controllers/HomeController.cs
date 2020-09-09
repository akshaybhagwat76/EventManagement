using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MiidWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiidWeb.Repositories;
using System.Text;
using MiidWeb.Helpers;
using System.Globalization;

namespace MiidWeb.Controllers
{

    public class HomeController : BaseController
    {
        


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
					GlobalVariables.LoginParaOne = subDomain.LoginParaOne;
					GlobalVariables.LoginParatwo = subDomain.LoginParatwo;
					GlobalVariables.HomeParaOne = subDomain.HomeParaOne;
					GlobalVariables.HomeParaTwo = subDomain.HomeParatwo;
					GlobalVariables.HomeImageOne = subDomain.HomeImageOne;
					GlobalVariables.HomeImagetwo = subDomain.HomeImagetwo;
					GlobalVariables.HomeImagethree = subDomain.HomeImagethree;
					GlobalVariables.HomeImageFour = subDomain.HomeImageFour;
					GlobalVariables.HomeImageOneLink = subDomain.HomeImageOneLink;
					GlobalVariables.HomeImageTwoLink = subDomain.HomeImageTwoLink;
					GlobalVariables.HomeImageThreeLink = subDomain.HomeImageThreeLink;
					GlobalVariables.HomeImageFourLink = subDomain.HomeImageFourLink;
					GlobalVariables.HomeImageOneText = subDomain.HomeImageOneText;
					GlobalVariables.HomeImageTwoText = subDomain.HomeImageTwoText;
					GlobalVariables.HomeImageFourText = subDomain.HomeImageThreeText;
					GlobalVariables.HomeImageFourLink = subDomain.HomeImageFourText;
					GlobalVariables.HomeCardOneHeading = subDomain.HomeCardOneHeading;
					GlobalVariables.HomeCardTwoHeading = subDomain.HomeCardTwoHeading;
					GlobalVariables.HomeCardThreeHeading = subDomain.HomeCardThreeHeading;
					GlobalVariables.HomeCardOnePara = subDomain.HomeCardOnePara;
					GlobalVariables.HomeCardTwoPara = subDomain.HomeCardTwoPara;
					GlobalVariables.HomeCardThreePara = subDomain.HomeCardThreePara;
					GlobalVariables.CallToActionOne = subDomain.CallToActionOne;
					GlobalVariables.CallToActionTwo = subDomain.CallToActionTwo;
					GlobalVariables.CallToActionThree = subDomain.CallToActionThree;
					GlobalVariables.CallToActionAll = subDomain.CallToActionAll;

					GlobalVariables.PurchaseContainerHeading = subDomain.PurchaseContainerHeading;
					GlobalVariables.PurchaseInfo = subDomain.PurchaseInfo;
					GlobalVariables.PurchaseDetails = subDomain.PurchaseDetails;
					GlobalVariables.PurchaseExtra = subDomain.PurchaseExtra;
					GlobalVariables.CallToActionPurchase = subDomain.CallToActionPurchase;

					GlobalVariables.PurchaseTermsOne = subDomain.PurchaseTermsOne;
					GlobalVariables.PurchaseTermsTwo = subDomain.PurchaseTermsTwo;
                    GlobalVariables.PurchaseTermsThree = subDomain.PurchaseTermsThree;

                    GlobalVariables.PurchaseSoldOut = subDomain.PurchaseSoldOut;
					GlobalVariables.Currency = subDomain.Currency;
                    GlobalVariables.CustomTerms = subDomain.CustomTerms;

                    GlobalVariables.PurchaseNoAvailiblity = subDomain.PurchaseNoAvailiblity;
					GlobalVariables.PurchaseFilter = subDomain.PurchaseFilter;
					GlobalVariables.PurchaseFindTickets = subDomain.PurchaseFindTickets;
					GlobalVariables.PurchaseRowNumber = subDomain.PurchaseRowNumber;
					GlobalVariables.PurchaseSeatNumber = subDomain.PurchaseSeatNumber;
					GlobalVariables.PurchaseTotalQunatity = subDomain.PurchaseTotalQunatity;
					GlobalVariables.PurchaseSubTotal = subDomain.PurchaseSubTotal;
					GlobalVariables.PurchaseTotal = subDomain.PurchaseTotal;
					GlobalVariables.PurchaseArea = subDomain.PurchaseArea;
					GlobalVariables.SeatingPlanImage = subDomain.SeatingPlanImage;

					GlobalVariables.MyEventsHeading = subDomain.MyEventsHeading;
					GlobalVariables.MyEventsParaOne = subDomain.MyEventsParaOne;
					GlobalVariables.MyEventsParaTwo = subDomain.MyEventsParaTwo;
					GlobalVariables.MyEventsParaHelpHeading = subDomain.MyEventsParaHelpHeading;
					GlobalVariables.MyEventsHelpOne = subDomain.MyEventsHelpOne;
					GlobalVariables.MyEventsHelpTwo = subDomain.MyEventsHelpTwo;
					GlobalVariables.MyEventsHelpThree = subDomain.MyEventsHelpThree;
					GlobalVariables.MyEventsDownLoadHeading = subDomain.MyEventsDownLoadHeading;
					GlobalVariables.ButtonDownload = subDomain.ButtonDownload;
					GlobalVariables.ButtonPending = subDomain.ButtonPending;
					GlobalVariables.MyEventsHelpHeadingOne = subDomain.MyEventsHelpHeadingOne;
					GlobalVariables.MyEventsHelpHeadingTwo = subDomain.MyEventsHelpHeadingTwo;
					GlobalVariables.MyEventsHelpHeadingThree = subDomain.MyEventsHelpHeadingThree;

					GlobalVariables.CreateFirstHeading = subDomain.CreateFirstHeading;
					GlobalVariables.CreateFirstParaOne = subDomain.CreateFirstParaOne;
					GlobalVariables.CreateFirstFirstName = subDomain.CreateFirstFirstName;
					GlobalVariables.CreateFirstFirstNameSub = subDomain.CreateFirstFirstNameSub;
					GlobalVariables.CreateFirstLastName = subDomain.CreateFirstLastName;
					GlobalVariables.CreateFirstLastNameSub = subDomain.CreateFirstLastNameSub;
					GlobalVariables.CreateFirstCell = subDomain.CreateFirstCell;
					GlobalVariables.CreateFirstCellSub = subDomain.CreateFirstCellSub;
					GlobalVariables.CreateFirstID = subDomain.CreateFirstID;
					GlobalVariables.CreateFirstIDSub = subDomain.CreateFirstIDSub;
                    GlobalVariables.CreateFirstEmail = subDomain.CreateFirstEmail;
					GlobalVariables.CreateFirstSeucrePurchase = subDomain.CreateFirstSeucrePurchase;
					GlobalVariables.Terms = subDomain.Terms;

					GlobalVariables.ChooseTenderHeadingOne = subDomain.ChooseTenderHeadingOne;
					GlobalVariables.ChooseTenderHeadingTwo = subDomain.ChooseTenderHeadingTwo;
					GlobalVariables.ChooseTenderParaOne = subDomain.ChooseTenderParaOne;
					GlobalVariables.ChooseTenderParaTwo = subDomain.ChooseTenderParaTwo;
					GlobalVariables.ChooseTenderButtonTwo = subDomain.ChooseTenderButtonTwo;
					GlobalVariables.ChooseTenderButtonTwo = subDomain.ChooseTenderButtonTwo;
					GlobalVariables.ChooseTenderParaSubOne = subDomain.ChooseTenderParaSubOne;
					GlobalVariables.ChooseTenderParaSubTwo = subDomain.ChooseTenderParaSubTwo;

					GlobalVariables.ButtonAllEvents = subDomain.ButtonAllEvents;
					GlobalVariables.ShowPassword = subDomain.ShowPassword;
					GlobalVariables.ForgotpasswordText = subDomain.ForgotpasswordText;
					GlobalVariables.RegisterParaOne = subDomain.RegisterParaOne;
					GlobalVariables.RegisterPasswordLength = subDomain.RegisterPasswordLength;

					GlobalVariables.VoucherHeading = subDomain.VoucherHeading;
					GlobalVariables.VoucherPara = subDomain.VoucherPara;

					GlobalVariables.CardPaymentHeading = subDomain.CardPaymentHeading;
					GlobalVariables.ButtonBack = subDomain.ButtonBack;
					GlobalVariables.ButtonConfirm = subDomain.ButtonConfirm;
					GlobalVariables.CardPaymentPara = subDomain.CardPaymentPara;



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






        [Authorize]
        public ActionResult AppApks()
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index");
            }

            else
            {
                return View();
            }
        }


        public ActionResult Index(string ReturnUrl = null)
        {

            Request.Cookies.Add(new HttpCookie ("subdomainid", ConfigRepo.GetSubdomainID().ToString() ));

            string ViewFile = MiidWeb.Repositories.TicketRepository.GetSubdomainView("Index");

            GetLayoutFiles();

            if (User.Identity.IsAuthenticated)
            {
                if (ReturnUrl != null)
                    return Redirect(ReturnUrl);

                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("AdminIndex");
                }

                if (User.IsInRole("EventAdmin"))
                {
                    return RedirectToAction("Index", "EventAdmin");
                }

                var eo = EventOrganiserRepository.GetEventOrganiserByEmail(User.Identity.Name);
                if (eo != null)
                {
                    return RedirectToAction("MyDetails", "EventOrganisers");
                }

                int EndUserID = Helpers.UserHelper.UserID(User.Identity.Name);
                if (EndUserID != 0)
                {

                    return RedirectToAction("EndUserProfile", "EndUsers", new { id = EndUserID, loggedInUserID = EndUserID });
                }
                else
                {
                    ViewBag.Para1 = CMSRepository.GetContent(@"Home\Index", 1);
                    return View(ViewFile);
                }
            }
            ViewBag.Para1 = CMSRepository.GetContent(@"Home\Index", 1);
            ViewBag.Para2 = CMSRepository.GetContent(@"Home\Index", 2);
            ViewBag.Para3 = CMSRepository.GetContent(@"Home\Index", 3);
            ViewBag.Para4 = CMSRepository.GetContent(@"Home\Index", 4);
            ViewBag.Para5 = CMSRepository.GetContent(@"Home\Index\Part2", 1);

           
            
            return View(ViewFile);
        }

        public ActionResult IndexEO()
        {

            if (User.Identity.IsAuthenticated)
            {


                var eo = EventOrganiserRepository.GetEventOrganiserByEmail(User.Identity.Name);
                if (eo != null)
                {
                    return RedirectToAction("MyDetails", "EventOrganisers");
                }

                return View();
            }

            return View();
        }
        //PaygeniusResponseSuccess
        //PaygeniusResponseCancel
        public ActionResult PaygeniusResponseError(string Ref = "")
        {
          
            string MIIDuniquepaymentID = Ref.Substring(Ref.Length - 12, 12);
            var bt = BankTransactionRepository.GetByUniquePaymentID(MIIDuniquepaymentID);
            //LookupResponse pgStatus = PayGeniusRepository.LookupPaygenius(bt.Description);
            PayGeniusRepository.ProcessPaygeniusOrder(MIIDuniquepaymentID, false);
            ViewBag.Details = false;
            return View("PaygeniusResponse");
        }

        public ActionResult PaygeniusResponseSuccess(string Ref = "")
        {
        
            string MIIDuniquepaymentID = Ref.Substring(Ref.Length - 12, 12);
            string Lookupreference = Ref.Substring(0, Ref.Length - 12);
            var bt = BankTransactionRepository.GetByUniquePaymentID(MIIDuniquepaymentID);
            LookupResponse pgStatus = PayGeniusRepository.LookupPaygenius(Lookupreference);// Ref);
            if (pgStatus.success)
            {
                PayGeniusRepository.ProcessPaygeniusOrder(MIIDuniquepaymentID, true);
                ViewBag.Details = true;
            }
            return View("PaygeniusResponse");
        }

        public ActionResult PaygeniusResponseCancel(string Ref = "")
        {
           
            string MIIDuniquepaymentID = Ref.Substring(Ref.Length - 12, 12);
            var bt = BankTransactionRepository.GetByUniquePaymentID(MIIDuniquepaymentID);
           // LookupResponse pgStatus = PayGeniusRepository.LookupPaygenius(bt.Description);
            PayGeniusRepository.ProcessPaygeniusOrder(MIIDuniquepaymentID, false);
            ViewBag.Details = false;
            return View("PaygeniusResponse");
        }


        [Authorize(Roles = "Admin")]
        public ActionResult AdminIndex()
        {
            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            ContactViewModel model = new ContactViewModel();
            return View(model);
        }

        public ActionResult Help()
        {
            ViewBag.Message = "Your help page.";

            return View();
        }


        public ActionResult TermsConditions()
        {
            ViewBag.Message = "Terms and Conditions";

            return View();
        }
        public ActionResult HelpEventOrganiser()
        {
            ViewBag.Message = "Your help page.";

            return View();
        }


        public ActionResult ForgotPasswordEventOrganiser()
        {
            ViewBag.Message = "Your Forgot Password page.";

            return View();
        }
        public ActionResult ForgotPassword()
        {
            ViewBag.Message = "Your Forgot Password page.";

            return View();
        }
        public ActionResult News()
        {
            ViewBag.Message = "News page.";

            return View();
        }

        public ActionResult ExtraServices()
        {
            ViewBag.Message = "Extra Services";

            return View();
        }

        public ActionResult Logout()
        {
            ViewBag.Message = "Your Logout page.";

            return View();
        }


        public ActionResult FAQ()
        {

            ViewBag.FAQ = CMSRepository.GetContent("FAQ", 1) ?? " Content coming soon...";

            return View();

        }

        public ActionResult ProductSummary()
        {
            return View();
        }

        public ActionResult WhiteLabel()
        {


            return View();
        }

        public ActionResult BoxOffice()
        {


            return View();
        }

		public ActionResult BrandedExperience()
		{


			return View();
		}

		public ActionResult NfcTickets()
        {


            return View();
        }

        public ActionResult RefundsPolicy()
        {
            return View();
        }

        public ActionResult MiiBandExplanation()
        {
            return View();
        }

        public ActionResult RegisterDoc()
        {
            return View();
        }

        public ActionResult TicketingExplanation()
        {
            return View();
        }



        public ActionResult TicketCalculator()
        {
            return View();
        }



        public ActionResult MiiFundsExplanation()
        {
            return View();
        }

        public ActionResult HowItWorks()
        {
            return View();
        }




		//Index pages
		public ActionResult IndexTraining()
		{
			return View();
		}

		public ActionResult Indexbirchwood()
		{
			return View();
		}

		public ActionResult IndexTakeYouThere()
		{
			return View();
		}

		public ActionResult Indexglpros()
		{
			return View();
		}

		public ActionResult Indexsnowflake()
		{
			return View();
		}

		public ActionResult IndexShemurai()
		{
			return View();
		}

		public ActionResult IndexFarmYardPark()
		{
			return View();
		}
		public ActionResult IndexMiidService()
		{
			return View();
		}

		public ActionResult IndexGlobalTickets()
		{
			return View();
		}

		[HttpPost]
        public ActionResult SendContactUsEmail(ContactViewModel contact, string returnUrl)
        {
            StringBuilder body = new StringBuilder();

            body.AppendLine("<html><head></head><body>");
            body.Append("Email received from, " + contact.name + " email:" + contact.email).Append(Environment.NewLine);
            body.Append("Message: ").Append(Environment.NewLine);
            body.Append(contact.message).Append(Environment.NewLine);
            body.AppendLine("</body></html>");

            var result = Helpers.EmailHelper.SendContactUsMail(contact.subject, body.ToString());

            if (result)
            {
                TempData["contactSuccess"] = "1";
                Growl("MiiD", "Thank you ! We will get back to you shortly.");
            }
            else
            {
                TempData["contactSuccess"] = "2";
                Growl("MiiD", "Sorry! There was an error sending mail. Please call us or email directly to info@miid.co.za.");
            }

            return RedirectToAction("Contact");
        }
    }
}