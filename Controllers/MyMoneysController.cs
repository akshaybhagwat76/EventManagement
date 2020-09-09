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
using MiidWeb.Repositories;
using MiidWeb.Helpers;
using System.Text;

namespace MiidWeb.Controllers
{
    
  
    
    public class MyMoneysController : BaseController
    {
        private MiidEntities db = new MiidEntities();


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




        }

        #region Admin Functions
        // GET: MyMoneys
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var myMoneys = db.MyMoneys.Include(m => m.TransactionType).Include(m => m.EndUser);
            return View(myMoneys.ToList());
        }

        #endregion

        #region End User Functions
        [Authorize]
        public ActionResult IndexByUserID(string growl = "")
        {
            if ((UserHelper.UserEmailConfirmed(User.Identity.Name)) == "Pending")
            {
                return RedirectToAction("ConfirmEmailLink", "EndUsers", new { email = User.Identity.Name });
            }

            int LoggedInUserID = Helpers.UserHelper.UserID(User.Identity.Name);

            var myMoneys = db.MyMoneys.Include(m => m.TransactionType).Include(m => m.EndUser).Where(x => x.EndUserID == LoggedInUserID).OrderByDescending(x => x.ID);

            decimal myBalance = MyMoneyRepository.MyAvailableFunds(LoggedInUserID);
            ViewBag.MyBalance = myBalance;
            ViewBag.LoggedInUserID = LoggedInUserID;

            return View("Index");
        }

        [Authorize]
        public ActionResult ViewDetailedBalance(int? id = 0)
        {
            int LoggedInUserID = Helpers.UserHelper.UserID(User.Identity.Name);

            if (id > 0) { LoggedInUserID = id??0; }

            var user =  EndUserRepository.GetByUserID(LoggedInUserID);
            ViewBag.UserName = String.Format("{0} {1}", user.Firstname, user.Surname);

            var myMoneys = db.MyMoneys.Include(m => m.TransactionType).Include(m => m.EndUser).Where(x => x.EndUserID == LoggedInUserID).OrderByDescending(x => x.TransactionDate).ToList();


            List<MyMoneyViewModel> model = new List<MyMoneyViewModel>();

            foreach (var m in myMoneys)
            {
                model.Add(new MyMoneyViewModel(m));
            }

            return View("DetailedBalance", model);
        }


        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        #region Ticket Purchases

        public ActionResult ChooseTender(string sessionid, string purpose)
        {
            PurchaseTicketViewModel pvModel = (PurchaseTicketViewModel)Session[sessionid];

            ViewBag.ID = sessionid;
            ViewBag.Purpose = purpose;
            ViewBag.PendingTicketsTotalCost = pvModel.PendingTicketsTotalCost;
            ViewBag.DiscountAmount = pvModel.DiscountAmount;
            ViewBag.costofTotalPurchase = (decimal)(pvModel.TicketClasses.Sum(x => x.TotalCost));
            decimal costOfTotalPurchase = (decimal)(pvModel.TicketClasses.Sum(x => x.TotalCost));
            return View(pvModel);
        }
        public ActionResult ConfirmTender(int id, string sid)
        {


            if (id == (int)Enums.TenderTypeEnum.MiiFunds)
                return RedirectToAction("FinaliseMiifundsTicketPayment", new { id = sid });

            if (id == (int)Enums.TenderTypeEnum.ManualEFT)
                return RedirectToAction("ManualEFT", new { id = sid, pps = "Ticket Purchase" });

            if (id == (int)Enums.TenderTypeEnum.InstantEFT)
                return RedirectToAction("InstantEFT", new { id = sid, pps = "Ticket Purchase" });

            if (id == (int)Enums.TenderTypeEnum.CardPayment)
                return RedirectToAction("CardPayment", new { id = sid, pps = "Ticket Purchase" });

            if (id == (int)Enums.TenderTypeEnum.PayGenius)
                return RedirectToAction("CardPaymentPg", new { id = sid, pps = "Ticket Purchase" });

            return View("ChooseTender");

        }
        public ActionResult FinaliseMiifundsTicketPayment(string id)//Pay for "Pending Payment" tickets for this event and session id
        {
          
            PurchaseTicketViewModel pvModel = (PurchaseTicketViewModel)Session[id];
            ViewBag.DiscountAmount = pvModel.DiscountAmount;
            decimal costOfTotalPurchase = (decimal)(pvModel.TicketClasses.Sum(x => x.TotalCost));
            decimal AdminFee = Helpers.ConfiguredFeeHelper.FeeAmount("MiiFundsTicketPurchase", costOfTotalPurchase, int.Parse(GlobalVariables.SubdomainID));
            ViewBag.AdminFee = AdminFee;

            ViewBag.ID = id;
            ViewBag.EventID = pvModel.Event.ID;

            int LoggedInUserID = Helpers.UserHelper.UserID(User.Identity.Name);
            ViewBag.LoggedInUserID = LoggedInUserID;

            return View(pvModel);
        }

       


        [HttpPost]
        public ActionResult FinaliseMiifundsTicketPayment(FormCollection form)
        {
            int EventID = int.Parse(form["EventID"].ToString());
            int UserID = UserHelper.UserID(User.Identity.Name);
            GetLayoutFiles();
            string subdomain = GlobalVariables.Company;



            //if (TicketRepository.HasPurchasedATicketForThisEvent(UserID, EventID))
            //{
            //    Growl("Alert.", "You have already purchased a ticket and are unable to go back to this page.");

            //    return RedirectToAction("Index", "Home");
            //}

            int LoggedInUserID = Helpers.UserHelper.UserID(User.Identity.Name);

            string id = form["hdnSessionID"].ToString();
            

            PurchaseTicketViewModel pvModel = (PurchaseTicketViewModel)Session[id];

            PurchaseTicketResultViewModel purchaseTicketsResult = TicketRepository.PurchaseTicketsWithMiiFunds(pvModel, LoggedInUserID);

            if (form["acceptTerms"] == null || form["acceptTerms"].ToString() != "on")
            {

                Growl("Mii Funds Ticket Purchase Failed", "Please accept terms and conditions");
                return RedirectToAction("FinaliseMiifundsTicketPayment", "MyMoneys", new { id = id });
            }
            else
            {

                if (!purchaseTicketsResult.HasErrors)
                {
                    if (pvModel.DiscountedTicketClassID != null)
                    {
                        TicketRepository.PromoCodeUpdate(pvModel.PromoCode, UserID, pvModel.DiscountedTicketClassID ?? 0);
                    }

                    if (db.MyMoneys.Where(x => x.EndUserID == LoggedInUserID) != null && db.MyMoneys.Where(x => x.EndUserID == LoggedInUserID).Count() > 0)
                    {
                        pvModel.MyMoneyCurrentBalance = db.MyMoneys.Where(x => x.EndUserID == (int)LoggedInUserID).OrderByDescending(x => x.ID).Take(1).First().RunningBalance;
                    }
                    else
                    {
                        pvModel.MyMoneyCurrentBalance = 0;
                    }

                    Growl("Mii Funds Ticket Purchase", "Successful! Enjoy the Event.");
                    //Send Email Notifications

                    string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority +  Request.ApplicationPath.TrimEnd('/') + "/";
                    MyMoneyRepository.SendConfirmationMiiFunds_TicketPurchase(LoggedInUserID, purchaseTicketsResult.TotalTicketCost, DateTime.Now, purchaseTicketsResult.purchasedTickets, baseUrl, subdomain);

                    
                    return RedirectToAction("MiiEvents", "Events");
                }
                else
                {
                    Growl("Mii Funds Ticket Purchase Failed", purchaseTicketsResult.ErrorList[0].Description.ToString());
                    return RedirectToAction("MiiEvents", "Events");
                }
            }
        }


        public ActionResult CancelTicket(int id)
        {
            if (id == 0)
            {
                id = Helpers.UserHelper.UserID(User.Identity.Name);
            }
            TicketRepository.CancelTickets(id);
            
            return RedirectToAction("MiiEvents", "Events");
        }


        public ActionResult PurchaseTicketsFail()
        {
           
            return View(Session["PurchaseTicketResultViewModel"]);
        }



        #endregion


        #region Manual EFT topups and ticket purchases
        public ActionResult ManualEFT(string id = "", string pps = "")
        {
            PurchaseTicketViewModel pvModel = new PurchaseTicketViewModel();
        
            if (pps == "Ticket Purchase")
            {
                pvModel = (PurchaseTicketViewModel)Session[id];
            }
            decimal discount = pvModel.DiscountAmount;
            ViewBag.DiscountAmount = discount;

            ManualEFTViewModel model = new ManualEFTViewModel(UserHelper.UserID(User.Identity.Name), pps, pvModel, id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManualEFT(ManualEFTViewModel model)
        {
            decimal fee = ConfiguredFeeHelper.FeeAmount("ManualEFT", model.TotalAmountInRands, int.Parse(GlobalVariables.SubdomainID));

            model.ConfiguredFeeAmount = fee;
            model.TotalAmountInRands += fee;
            model.ActualAmount = model.TotalAmountInRands - fee;

            Session.Add("ManualEFT", model);
            ViewBag.DiscountAmount = model.PurchaseTicketViewModel.DiscountAmount;
            return RedirectToAction("ManualEFTConfirm", model);

        }

        public ActionResult ManualEFTConfirm()
        {

            ManualEFTViewModel model = (ManualEFTViewModel)Session["ManualEFT"];
            if (model.Purpose == "Ticket Purchase")
            {
                model.PurchaseTicketViewModel = (PurchaseTicketViewModel)Session[model.PurchaseSessionID];
                ViewBag.DiscountAmount = model.PurchaseTicketViewModel.DiscountAmount;
            }
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManualEFTConfirm(ManualEFTViewModel model)
        {


            bool error = false;

            if (model.ReferenceInSafePlace == false)
            {
                ModelState.AddModelError("ReferenceInSafePlaceError", "Please confirm you have used the correct reference number & amount");
                error = true;
                Growl("MiiD", "Please confirm you have used the correct reference number & amount");
            }

            if (model.TermsAndConditions == false)
            {
                ModelState.AddModelError("TermsAndConditionsError", "Please accept terms and conditions");
                Growl("MiiD", "Please accept terms and conditions");
                error = true;
            }

            if (error)
                goto here;

            string purpose = model.Purpose;
            int TransactionTypeID = 0;
            PurchaseTicketResultViewModel ConfirmationMessage = new PurchaseTicketResultViewModel();
            if (purpose == "Ticket Purchase")
            {
                TransactionTypeID = StatusHelper.TransactionTypeID("Manual EFT Ticket Purchase");
                PurchaseTicketViewModel pvModel = (PurchaseTicketViewModel)Session[model.PurchaseSessionID];

                if (pvModel.DiscountedTicketClassID != null)
                {
                    TicketRepository.PromoCodeUpdate(pvModel.PromoCode, model.EndUserID, pvModel.DiscountedTicketClassID ?? 0);
                }

                ConfirmationMessage = TicketRepository.ReserveTicketsForDirectPurchase(pvModel, model.EndUserID, model.UniquePaymentID);
                model.UniquePaymentID = ConfirmationMessage.UniquePaymentID;

                if (ConfirmationMessage.HasErrors)
                {
                    Session["PurchaseTicketResultViewModel"] = ConfirmationMessage;
                    // return PurchaseTicketsFail();

                }
                else
                {
                    Growl("Ticket Reservation", "Manual EFT Request Received");
                    MyMoneyRepository.SendConfirmationManualEFT_TicketReservation(model.EndUserID, model.TotalAmountInRands, model.UniquePaymentID, model.PaymentDate, ConfirmationMessage.reservedTickets);
                }
            }
            else
            {
                TransactionTypeID = StatusHelper.TransactionTypeID("Manual EFT Topup");
                Growl("Mii-Funds Topup", "Manual EFT Request Received");
                MyMoneyRepository.SendConfirmationManualEFT(model.EndUserID, model.TotalAmountInRands, model.UniquePaymentID, model.PaymentDate, model.ConfiguredFeeAmount);

            }

            if (!BankTransactionRepository.DoesUniquePaymentIDExist(model.UniquePaymentID))
            {
                model.SaveAsBankTransaction();
                
                return View("ManualEFTConfirmSuccess", model);

            }
            else
            {
                if (ConfirmationMessage.HasErrors)
                {
                    Session["PurchaseTicketResultViewModel"] = (ConfirmationMessage);
                    return View("PurchaseTicketsFail");

                    //return RedirectToAction("PurchaseTicketsFail", ConfirmationMessage); //Daniel check this

                }
                return null;
            }


          

        here:


            ManualEFTViewModel model2 = (ManualEFTViewModel)Session["ManualEFT"];
            if (model2.Purpose == "Ticket Purchase")
            {
                model2.PurchaseTicketViewModel = (PurchaseTicketViewModel)Session[model.PurchaseSessionID];
            }
            return View(model2);

        }

        public ActionResult ManualEFTConfirmSuccess()
        {

            ManualEFTViewModel model = (ManualEFTViewModel)Session["ManualEFT"];

            return View(model);
        }


        #endregion

        #region Card Payment topups and ticket purchases

        public ActionResult CardPayment(string id = "", string pps = "")
        {
            PurchaseTicketViewModel pvModel = new PurchaseTicketViewModel();
            CardPaymentViewModel model = null;

            if (pps == "Ticket Purchase")
            {
                pvModel = (PurchaseTicketViewModel)Session[id];
                model = new CardPaymentViewModel(UserHelper.UserID(User.Identity.Name), pps, pvModel, id);

            }
            else
            {
                int TopUpAmount = 0;// int.Parse(Session["TopUpAmount"].ToString());
                model = new CardPaymentViewModel(UserHelper.UserID(User.Identity.Name), pps, TopUpAmount, id);
            }



            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void CardPayment(CardPaymentViewModel model)
        {
            if (model.Purpose == "Ticket Purchase")
            {
                var pvModel = (PurchaseTicketViewModel)Session[model.PurchaseSessionID];
               
                var ConfirmationMessage = TicketRepository.ReserveTicketsForDirectPurchase(pvModel, model.EndUserID, model.UniquePaymentID);
                model.UniquePaymentID = ConfirmationMessage.UniquePaymentID;

                if (ConfirmationMessage.HasErrors)
                {
                    RedirectToAction("PurchaseTicketsFail", ConfirmationMessage); //Daniel DAniel check this

                }

            }
            
            model.AdminFee = Helpers.ConfiguredFeeHelper.FeeAmount("CardPayment", (decimal)model.TotalAmountInRands, int.Parse(GlobalVariables.SubdomainID));

            model.SaveAsBankTransaction();
            if (model.Purpose == "Ticket Purchase")
            {
                Response.Redirect("~/CardPaymentOrder.aspx?on=" + model.UniquePaymentID + "&pps=tckt");
            }
            else
            {
                Response.Redirect("~/CardPaymentOrder.aspx?on=" + model.UniquePaymentID + "&pps=topup");
            }
        }
        //
        public ActionResult CardPaymentPg(string id = "", string pps = "")
        {
            PurchaseTicketViewModel pvModel = new PurchaseTicketViewModel();
            CardPaymentViewModel model = null;

            if (pps == "Ticket Purchase")
            {
                pvModel = (PurchaseTicketViewModel)Session[id];
                model = new CardPaymentViewModel(UserHelper.UserID(User.Identity.Name), pps, pvModel, id);

            }
            else
            {
                int TopUpAmount = 0;// int.Parse(Session["TopUpAmount"].ToString());
                model = new CardPaymentViewModel(UserHelper.UserID(User.Identity.Name), pps, TopUpAmount, id);
            }



            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public void CardPaymentPg(CardPaymentViewModel model)
        {
            if (model.Purpose == "Ticket Purchase")
            {
                var pvModel = (PurchaseTicketViewModel)Session[model.PurchaseSessionID];

                var ConfirmationMessage = TicketRepository.ReserveTicketsForDirectPurchase(pvModel, model.EndUserID, model.UniquePaymentID);
                model.UniquePaymentID = ConfirmationMessage.UniquePaymentID;

                if (ConfirmationMessage.HasErrors)
                {
                    RedirectToAction("PurchaseTicketsFail", ConfirmationMessage); //Daniel DAniel check this

                }

            }

            model.AdminFee = Helpers.ConfiguredFeeHelper.FeeAmount("CardPayment", (decimal)model.TotalAmountInRands, int.Parse(GlobalVariables.SubdomainID));

        
            if (model.Purpose == "Ticket Purchase")
            {

                int CentsAmount = (int)(model.TotalAmountInRands * 100.00M) + (int)(model.AdminFee * 100.00M);
                EndUser endUser = EndUserRepository.GetByUserID(model.EndUserID);
                //Paygenius
                var request = PayGeniusRepository.AssembleRequest(amount: CentsAmount, description: model.Purpose, reference: model.UniquePaymentID,
                    email: endUser.Email, name: endUser.Firstname, surname: endUser.Surname, 
                    addressLineOne: endUser.StreetAddress, city: endUser.City, 
                    postCode: endUser.PostalCode,                    
                    country: endUser.Country
                                         );
                var response = PayGeniusRepository.SendToPaygenius(request);

                model.SaveAsBankTransaction(response.reference);
                
                

                Response.Redirect(response.redirectUrl);
            }
            else
            {
             //   Response.Redirect("~/CardPaymentOrder.aspx?on=" + model.UniquePaymentID + "&pps=topup");
            }
        }

        //
        public ActionResult TopupFundsMenu(int? EventID = 0)
        {
            //New DS
            if (!User.Identity.IsAuthenticated)
            {



                string cookievalue = EventID.ToString();
                var cookie = new HttpCookie("TopupFundsForEvent", cookievalue);
                cookie.Expires = DateTime.Now.AddMinutes(15.0);
                Response.AppendCookie(cookie);

              
                Growl("Mi-iD", "Please login / register to topup funds.");

                return RedirectToAction("LoginAlternative", "LoginAlternative");


            }



            if (EndUserRepository.IsUserUpToDate(UserHelper.UserID(User.Identity.Name)))
            {
                if (EndUserRepository.IsUserIDValid(UserHelper.UserID(User.Identity.Name)))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("CreateFirst", "EndUsers", new { Email = User.Identity.Name, IsCashless = "true" });
                }
            }
            else
            {
               // Growl("Mi-iD", "Please complete a few more details before topping up Mii-Funds.");
                return RedirectToAction("CreateFirst", "EndUsers", new { Email = User.Identity.Name, Cookie = "MiiFunds Topup" });

            }

           
        }

        public ActionResult InstantEFT(string id = "", string pps = "")
        {
            PurchaseTicketViewModel pvModel = new PurchaseTicketViewModel();

            if (pps == "Ticket Purchase")
            {
                pvModel = (PurchaseTicketViewModel)Session[id];
            }

            InstantEFTViewModel model = new InstantEFTViewModel(UserHelper.UserID(User.Identity.Name), pps, pvModel, id);

            return View(model);
        }

        //Paygenius




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InstantEFT(InstantEFTViewModel model)
        {    
            decimal fee = ConfiguredFeeHelper.FeeAmount("InstantEFT", model.TotalAmountInRands, int.Parse(GlobalVariables.SubdomainID));
            decimal discount =  model.PurchaseTicketViewModel.DiscountAmount;

            int payfastfixed = int.Parse(ConfigRepo.Get("PayfastFixed"));
            int PayfastVat = int.Parse(ConfigRepo.Get("PayfastVat"));

            var fixedamount = (fee + payfastfixed);
            var vat = (fixedamount * PayfastVat) /100;
            
            var newfee = fixedamount + vat;

            ViewBag.DiscountAmount = discount;
            //LogHelper.Log(String.Format("FeeAmount={0}", fee), "InstantEFT");

            model.ConfiguredFeeAmount = newfee;
            model.TotalAmountInRands += newfee;
            model.ActualAmount =  model.TotalAmountInRands - newfee;


            Session.Add("InstantEFT", model);
            return RedirectToAction("InstantEFTConfirm", model);

        }

        public ActionResult InstantEFTConfirm(string id = "", string pps = "")
        {    

            InstantEFTViewModel model = (InstantEFTViewModel)Session["InstantEFT"];
            if (model.Purpose == "Ticket Purchase")
            {
                model.PurchaseTicketViewModel = (PurchaseTicketViewModel)Session[model.PurchaseSessionID];
            }
            int LoggedInUserID = Helpers.UserHelper.UserID(User.Identity.Name);
            ViewBag.LoggedInUserID = LoggedInUserID;

            decimal discount = model.PurchaseTicketViewModel.DiscountAmount;

            ViewBag.DiscountAmount = discount;

            return View(model);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectResult InstantEFTConfirm(InstantEFTViewModel model)
        {
            try
            {
                PurchaseTicketResultViewModel ConfirmationMessage = new PurchaseTicketResultViewModel();
                if (model.Purpose == "Ticket Purchase")
                {
                    var pvModel = (PurchaseTicketViewModel)Session[model.PurchaseSessionID];

                    ConfirmationMessage = TicketRepository.ReserveTicketsForDirectPurchase(pvModel, model.EndUserID,model.UniquePaymentID);
                    model.UniquePaymentID = ConfirmationMessage.UniquePaymentID;


                   
                    if (pvModel.DiscountedTicketClassID != null)
                    {
                        TicketRepository.PromoCodeUpdate(pvModel.PromoCode, model.EndUserID, pvModel.DiscountedTicketClassID??0);
                    }

                }
                if (!BankTransactionRepository.DoesUniquePaymentIDExist(model.UniquePaymentID))
                {
                    model.SaveAsBankTransaction();
                    string utl = String.Format(@"~/PayfastOrder.aspx?on={0}", model.UniquePaymentID);

                    return Redirect(utl);
                   
                }
                else
                {
                    if (ConfirmationMessage.HasErrors)
                    {
                        Session["PurchaseTicketResultViewModel"]= (ConfirmationMessage);
                        return Redirect("/MyMoneys/PurchaseTicketsFail");

                        //return RedirectToAction("PurchaseTicketsFail", ConfirmationMessage); //Daniel check this

                    }
                    return null;
                }

            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine(e.Message);
                if (e.InnerException != null)
                {
                    sb.AppendLine(e.InnerException.Message);
                    if (e.InnerException.InnerException != null)
                    {
                        sb.AppendLine(e.InnerException.InnerException.Message);
                    }
                }
                Response.Write(sb.ToString());
                return null;
            }

        }



        #endregion


        public ActionResult TransferFunds()
        {
            TransferMiiFundsViewModel model = new TransferMiiFundsViewModel(UserHelper.UserID(User.Identity.Name));

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TransferFunds(TransferMiiFundsViewModel model)
        {

            var user = db.EndUsers.Include(x => x.NFCTags).Where(x => x.ID == model.EndUserID).First();
           

          

               bool success= MyMoneyRepository.TransferMiiFunds(model.FromAccountID, model.ToAccountID, model.TransferAmount, model.EndUserID);

                if (success)
                {

                    Growl("Mii-Funds", "Mii-Funds transfer successful.");
                    return RedirectToAction("Index", "MyMoneys");
                }
                else
                {
                    Growl("Mii-Funds", "An error occurred transfering Mii-funds.");

                    model = new TransferMiiFundsViewModel(UserHelper.UserID(User.Identity.Name));

                    return View(model);
                }
           

        }



        #region Withdrawal MiiFunds
        public ActionResult RequestWithdrawal()
        {
            MiiFundsRequestWithdrawalViewModel model = new MiiFundsRequestWithdrawalViewModel(UserHelper.UserID(User.Identity.Name));

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestWithdrawal(MiiFundsRequestWithdrawalViewModel model)
        {

            var user = db.EndUsers.Include(x => x.NFCTags).Where(x => x.ID == model.EndUserID).First();
            if (!model.TermsAndConditions)
            {
                Growl("Mii-Funds", "Please accept terms and conditions.");

                return View(model);
            }


          //  if (model.IDNumber != user.IDNumber)
            //{

                // ViewBag.NotificationInvalidCredentials = "Invalid credentials. Please check and retry.";

              //  Growl("Mii-Funds", "Invalid ID Number. Please check and retry.");

                //return View(model);
           // }





            if (model.RequestedAmount <= model.AdminFee)
            {

                //ViewBag.NotificationMinAmount = "Amount requested below minimum allowed. Please retry or cancel.";


                Growl("Mii-Funds", "Amount requested below minimum allowed. Please retry or cancel.");

                return View(model);
            }

            if (model.RequestedAmount > (model.AvailableFunds ?? 0.00M))
            {

                //                ViewBag.NotificationInsufficientFunds = "Insufficient funds. Please retry or cancel.";
                Growl("Mii-Funds", "Insufficient funds. Please retry or cancel.");

                return View(model);
            }
            else
            {
                //All Good
                Session.Add("RequestWithdrawal", model);
                return RedirectToAction("RequestWithdrawalBankDetails", model);
            }

        }


        public ActionResult RequestWithdrawalBankDetails()
        {
            MiiFundsRequestWithdrawalViewModel model = (MiiFundsRequestWithdrawalViewModel)Session["RequestWithdrawal"];
                      
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestWithdrawalBankDetails(MiiFundsRequestWithdrawalViewModel model)
        {
            try
            {

                SiteUserRepository.UpdateBankDetails(model);


                Session.Add("RequestWithdrawal", model);



                return RedirectToAction("RequestWithdrawalConfirm");
            }
            catch (Exception ex)
            {
                Growl("MIID", "Compulsory fields are empty.");
                return View(model);
            }
        }




        public ActionResult RequestWithdrawalConfirm()
        {
            MiiFundsRequestWithdrawalViewModel model = (MiiFundsRequestWithdrawalViewModel)Session["RequestWithdrawal"];

            //double check available funds with fresh db call
            model.AvailableFunds = MyMoneyRepository.MyAvailableFunds(model.EndUserID);

            model.AdminFee = ConfiguredFeeHelper.FeeAmount("MiiFundsWithdrawal", model.RequestedAmount, int.Parse(GlobalVariables.SubdomainID));
         
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestWithdrawalConfirm(MiiFundsRequestWithdrawalViewModel model)
        {
            var UniquePaymentID = MiidWeb.Helpers.PaymentHelper.UniqueRefNo(model.FirstName, model.Surname);

            BankTransaction bt = new BankTransaction
            {
                AdminFee = model.AdminFee,
                Amount = model.RequestedAmount,
                ConfirmationDate = null,
                DepositorName = model.FirstName + " " + model.Surname,
                EndUserID = model.EndUserID,
                StatusID = StatusHelper.StatusID("MiiFunds Withdrawal Request", "Pending Admin Payout"),
                TransactionDate = DateTime.Now,
                Description = "MiiFunds Withdrawal Request",
                TransactionTypeID = StatusHelper.TransactionTypeID("MiiFunds Withdrawal"),
                UpdatedByUserName = "End User",
                Reference = UniquePaymentID

            };

            db.BankTransactions.Add(bt);
            db.SaveChanges();
            //Db Alert
            ExportCsvRepository.ExportCSVAndEmail("AreThereMoreTicketsThanBank", bt.Reference ?? "", "info@miid.co.za", String.Format("WARNING. duplicate transaction: {0}", bt.Reference ?? ""), "See attachment", String.Format("AreThereMoreTicketsThanBank_{0}.csv", DateTime.Now.ToString("yyyyMMddHHmmss")));

            //Update / Create a MiiFunds Transaction
            decimal lastBalance = 0.00M;
            if (db.MyMoneys.Where(k => k.EndUserID == bt.EndUserID).OrderByDescending(y => y.ID).Count() > 0)
            {
                MyMoney last = db.MyMoneys.Where(k => k.EndUserID == bt.EndUserID).OrderByDescending(y => y.ID).First();
                lastBalance = last.RunningBalance ?? 0.00M;
            }


            MyMoney money = new MyMoney
            {
                Amount = -(model.RequestedAmount + model.AdminFee),
                DateTimeUpdated = DateTime.Now,
                Description = bt.Description,
                EndUserID = bt.EndUserID,
                RunningBalance = lastBalance - (model.RequestedAmount + model.AdminFee),
                TransactionTypeID = StatusHelper.TransactionTypeID("Miifunds Withdrawal"),
                TransactionDate = DateTime.Now,
                EmailNotificationStatus = "P",//pending
                Reference = UniquePaymentID

            };

            db.MyMoneys.Add(money);
            db.SaveChanges();
            db.Entry(money).GetDatabaseValues();




            MyMoneyRepository.SendConfirmationMiifundsWithdrawalRequest(model.EndUserID, model.RequestedAmount, bt.ID, (decimal)(model.AdminFee??0.00M));
            Helpers.EmailHelper.SendMail("donavan.wallis@miid.co.za", "Pre-Paid Fund Withdraw Request", "Check withdraw list, customer has requested a refund");
            Helpers.EmailHelper.SendMail("jonathan.wallis@miid.co.za", "Pre-Paid Fund Withdraw Request", "Check withdraw list, customer has requested a refund");
            Helpers.EmailHelper.SendMail("info@miid.co.za", "Pre-Paid Fund Withdraw Request", "Check withdraw list, customer has requested a refund");

            return View("WithdrawalRequestConfirmation", model);
        }





        #endregion

    }
}
