using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MiidWeb;

namespace MiidWeb.Controllers
{
    public class SubdomainController : Controller
    {
        private MiidEntities db = new MiidEntities();

        // GET: Subdomain
        public async Task<ActionResult> Index()
        {
            return View(await db.Subdomains.ToListAsync());
        }

        // GET: Subdomain/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subdomain subdomain = await db.Subdomains.FindAsync(id);
            if (subdomain == null)
            {
                return HttpNotFound();
            }
            return View(subdomain);
        }

        // GET: Subdomain/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Subdomain/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,SubdomainName,CompanyName,Active,Text1,Text2,Text3,Text4,Text5,Text6,EmailFile,EmailTicketPurchase,EmailTicketPurchaseEFT,EmailTicketPurchaseMiiFunds,EmailRegistration,EmailUserUpdate,EmailAccountActive,EmailMiifundsTopup,EmailMiifundsTopupEFT,EmailTicketPurchaseEFTExpired,LoginParaOne,LoginParatwo,HomeParaOne,HomeParatwo,HomeImageOne,HomeImagetwo,HomeImagethree,HomeImageFour,HomeImageOneLink,HomeImageTwoLink,HomeImageThreeLink,HomeImageFourLink,HomeImageOneText,HomeImageTwoText,HomeImageThreeText,HomeImageFourText,HomeCardOneHeading,HomeCardTwoHeading,HomeCardThreeHeading,HomeCardOnePara,HomeCardTwoPara,HomeCardThreePara,CallToActionOne,CallToActionTwo,CallToActionThree,CallToActionAll,PurchaseContainerHeading,PurchaseInfo,PurchaseDetails,PurchaseExtra,CallToActionPurchase,PurchaseTermsOne,PurchaseTermsTwo,PurchaseSoldOut,Currency,PurchaseNoAvailiblity,PurchaseFilter,PurchaseFindTickets,PurchaseRowNumber,PurchaseSeatNumber,PurchaseTotalQunatity,PurchaseSubTotal,PurchaseTotal,MyEventsHeading,MyEventsParaOne,MyEventsParaTwo,MyEventsParaHelpHeading,MyEventsHelpOne,MyEventsHelpTwo,MyEventsHelpThree,MyEventsDownLoadHeading,ButtonDownload,ButtonPending,MyEventsHelpHeadingOne,MyEventsHelpHeadingTwo,MyEventsHelpHeadingThree,CreateFirstHeading,CreateFirstParaOne,CreateFirstFirstName,CreateFirstFirstNameSub,CreateFirstLastName,CreateFirstLastNameSub,CreateFirstCell,CreateFirstCellSub,CreateFirstID,CreateFirstIDSub,CreateFirstEmail,CreateFirstSeucrePurchase,Terms,ChooseTenderHeadingOne,ChooseTenderHeadingTwo,ChooseTenderParaOne,ChooseTenderParaTwo,ChooseTenderButtonOne,ChooseTenderButtonTwo,ChooseTenderParaSubOne,ChooseTenderParaSubTwo,PurchaseArea,SeatingPlanImage,ButtonAllEvents,ShowPassword,ForgotpasswordText,RegisterParaOne,VoucherHeading,CardPaymentHeading,CardPaymentPara,ButtonBack,ButtonConfirm,VoucherPara,RegisterPasswordLength,KeyWordSearchText,ManualEftProRata,CustomTerms,PurchaseTermsThree,ManauEFTtopUpRequest,ManauEFTtopUpConfirmed")] Subdomain subdomain)
        {
            if (ModelState.IsValid)
            {
                db.Subdomains.Add(subdomain);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(subdomain);
        }

        // GET: Subdomain/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subdomain subdomain = await db.Subdomains.FindAsync(id);
            if (subdomain == null)
            {
                return HttpNotFound();
            }
            return View(subdomain);
        }

        // POST: Subdomain/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,SubdomainName,CompanyName,Active,Text1,Text2,Text3,Text4,Text5,Text6,EmailFile,EmailTicketPurchase,EmailTicketPurchaseEFT,EmailTicketPurchaseMiiFunds,EmailRegistration,EmailUserUpdate,EmailAccountActive,EmailMiifundsTopup,EmailMiifundsTopupEFT,EmailTicketPurchaseEFTExpired,LoginParaOne,LoginParatwo,HomeParaOne,HomeParatwo,HomeImageOne,HomeImagetwo,HomeImagethree,HomeImageFour,HomeImageOneLink,HomeImageTwoLink,HomeImageThreeLink,HomeImageFourLink,HomeImageOneText,HomeImageTwoText,HomeImageThreeText,HomeImageFourText,HomeCardOneHeading,HomeCardTwoHeading,HomeCardThreeHeading,HomeCardOnePara,HomeCardTwoPara,HomeCardThreePara,CallToActionOne,CallToActionTwo,CallToActionThree,CallToActionAll,PurchaseContainerHeading,PurchaseInfo,PurchaseDetails,PurchaseExtra,CallToActionPurchase,PurchaseTermsOne,PurchaseTermsTwo,PurchaseSoldOut,Currency,PurchaseNoAvailiblity,PurchaseFilter,PurchaseFindTickets,PurchaseRowNumber,PurchaseSeatNumber,PurchaseTotalQunatity,PurchaseSubTotal,PurchaseTotal,MyEventsHeading,MyEventsParaOne,MyEventsParaTwo,MyEventsParaHelpHeading,MyEventsHelpOne,MyEventsHelpTwo,MyEventsHelpThree,MyEventsDownLoadHeading,ButtonDownload,ButtonPending,MyEventsHelpHeadingOne,MyEventsHelpHeadingTwo,MyEventsHelpHeadingThree,CreateFirstHeading,CreateFirstParaOne,CreateFirstFirstName,CreateFirstFirstNameSub,CreateFirstLastName,CreateFirstLastNameSub,CreateFirstCell,CreateFirstCellSub,CreateFirstID,CreateFirstIDSub,CreateFirstEmail,CreateFirstSeucrePurchase,Terms,ChooseTenderHeadingOne,ChooseTenderHeadingTwo,ChooseTenderParaOne,ChooseTenderParaTwo,ChooseTenderButtonOne,ChooseTenderButtonTwo,ChooseTenderParaSubOne,ChooseTenderParaSubTwo,PurchaseArea,SeatingPlanImage,ButtonAllEvents,ShowPassword,ForgotpasswordText,RegisterParaOne,VoucherHeading,CardPaymentHeading,CardPaymentPara,ButtonBack,ButtonConfirm,VoucherPara,RegisterPasswordLength,KeyWordSearchText,ManualEftProRata,CustomTerms,PurchaseTermsThree,ManauEFTtopUpRequest,ManauEFTtopUpConfirmed")] Subdomain subdomain)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subdomain).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(subdomain);
        }

        // GET: Subdomain/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subdomain subdomain = await db.Subdomains.FindAsync(id);
            if (subdomain == null)
            {
                return HttpNotFound();
            }
            return View(subdomain);
        }

        // POST: Subdomain/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Subdomain subdomain = await db.Subdomains.FindAsync(id);
            db.Subdomains.Remove(subdomain);
            await db.SaveChangesAsync();
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
