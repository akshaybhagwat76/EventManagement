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
using MiidWeb.Models;
using System.Configuration;
using MiidWeb.Helpers;
using System.Data.Entity.Validation;

namespace MiidWeb.Controllers
{
    [Authorize]
    public class TicketClassesController : BaseController
    {
        private MiidEntities db = new MiidEntities();
        [Authorize(Roles = "Admin")]
        // GET: TicketClasses
        public ActionResult Index()
        {
            var ticketClasses = db.TicketClasses.Include(t => t.Event);
            return View(ticketClasses.ToList());
        }

        // GET: TicketClasses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketClass ticketClass = db.TicketClasses.Find(id);
            if (ticketClass == null)
            {
                return HttpNotFound();
            }
            return View(ticketClass);
        }

        
        public ActionResult CreateForEvent(int eventID)
        {
            ViewBag.EventID = eventID;

            var Event = db.Events.Find(eventID);

            ViewBag.EventName = Event.EventName;

            var eo = EventOrganiserRepository.GetEventOrganiserByEmail(User.Identity.Name);

            if (eo.ID != Event.EventOrganiserID)
            {
                return RedirectToAction("Index",  "Home");
            }
            int baseYear = int.Parse(ConfigRepo.Get("baseYear").ToString());
            TicketClassViewModel tc = new TicketClassViewModel(eventID, true, baseYear);
            tc.TicketClass = new TicketClass();
            tc.TicketClass.StartDate = Event.StartDateTime;
            tc.TicketClass.EndDate = Event.EndDateTime;

            return View(tc);
        }


        public ActionResult CreateMultiForEvent(int eventID)
        {
            ViewBag.EventID = eventID;

            var Event = db.Events.Find(eventID);

            ViewBag.EventName = Event.EventName;

            var eo = EventOrganiserRepository.GetEventOrganiserByEmail(User.Identity.Name);

            if (eo.ID != Event.EventOrganiserID)
            {
                return RedirectToAction("Index", "Home");
            }
            int baseYear = int.Parse(ConfigRepo.Get("baseYear").ToString());
            TicketClassViewModel tc = new TicketClassViewModel(eventID, true, baseYear);
            tc.TicketClass = new TicketClass();
            tc.TicketClass.StartDate = Event.StartDateTime;
            tc.TicketClass.EndDate = Event.EndDateTime;

            return View(tc);
        }



        // POST: TicketClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult CreateForEvent(TicketClassViewModel tcvm, int EventID, string MultipleDates)
        {
            string growl = "";




            if (ModelState.IsValid)
            {
                tcvm.TicketClass.EventID = EventID;
                tcvm.TicketClass.Description = tcvm.TicketClassDescription;
                tcvm.TicketClass.RunningQuantity = tcvm.TicketClass.Quantity;
                tcvm.TicketClass.Price = tcvm.TicketClass.Price;
                tcvm.TicketClass.Code = tcvm.TicketClass.Code;//These match at start
                                                                             //tcvm.TicketClass.StatusID = StatusHelper.StatusID("TicketClass","Active");

                var Event = db.Events.Find(EventID);

                var check1 = db.TicketClasses.Where(x => x.EventID == EventID && x.Description == tcvm.TicketClass.Description);

               if (check1 != null && check1.Count() > 0)
                {

                    growl = "Ticket class already exists.";
                   Growl("MiiD", "You have already used this ticket name. If you did not intend to use this name go to 'edit tickets'");
                   ModelState.AddModelError("StartDateTime", "Please enter a unique description");

                   goto fail;
                }



                else
                {




                    ViewBag.EventName = Event.EventName;

                    if (String.IsNullOrEmpty(MultipleDates))
                    {

                        string[] TimeStr = tcvm.StartDateTimeTime.Split(':');
                        DateTime ticketStartDate = new DateTime(tcvm.StartDateYear, tcvm.StartDateMonth, tcvm.StartDateDay, int.Parse(TimeStr[0]), int.Parse(TimeStr[1]), 0);
                        tcvm.TicketClass.StartDate = ticketStartDate;

                        int failcount = 0;
                        if (DateHelper.DateSequenceViolation((DateTime)Event.StartDateTime, ticketStartDate))
                        {
                            //Invalid start date
                            ModelState.AddModelError("StartDateTime", "Your ticket's date range is not within the same date range of the event that you have created. Either extend the event date range or adjust your ticket start or end date & time.");
                            growl = "Your ticket's date range is not within the same date range of the event that you have created. Either extend the event date range or adjust your ticket start or end date & time";
                            failcount++;
                        }

                        TimeStr = tcvm.EndDateTimeTime.Split(':');
                        DateTime ticketEndDate = new DateTime(tcvm.EndDateYear, tcvm.EndDateMonth, tcvm.EndDateDay, int.Parse(TimeStr[0]), int.Parse(TimeStr[1]), 0);
                        tcvm.TicketClass.EndDate = ticketEndDate;

                        if (DateHelper.DateSequenceViolation(ticketEndDate, (DateTime)Event.EndDateTime))
                        {
                            //Invalid start date
                            ModelState.AddModelError("EndDateTime", "Your ticket's date range is not within the same date range of the event that you have created. Either extend the event date range or adjust your ticket start or end date & time.");
                            growl = "Your ticket's date range is not within the same date range of the event that you have created. Either extend the event date range or adjust your ticket start or end date & time";
                            failcount++;
                        }
                        if (DateHelper.DateSequenceViolation(ticketStartDate, ticketEndDate))
                        {
                            //Invalid start date
                            ModelState.AddModelError("EndDateTime", "Your ticket's date range is not within the same date range of the event that you have created. Either extend the event date range or adjust your ticket start or end date & time.");
                            growl = "Your ticket's date range is not within the same date range of the event that you have created. Either extend the event date range or adjust your ticket start or end date & time";
                            failcount++;
                        }
                   

                    if (failcount > 0)
                        goto fail;
                    }

                    string[] MultipleDatesArray = MultipleDates.Split(',');
                    if (!String.IsNullOrEmpty(MultipleDates) && MultipleDatesArray.Count() > 0)
                    {
                        foreach (var date in MultipleDatesArray)
                        {
                            TicketClass newTC = new TicketClass();
                            newTC.EventID = EventID;
                            newTC.Price = tcvm.TicketClass.Price;
                            newTC.Description = String.Format("{0}  ({1})", tcvm.TicketClassDescription, date);
                            newTC.Code = String.Format("{0}  ({1})", tcvm.TicketClass.Code, date);
                            newTC.Quantity = tcvm.TicketClass.Quantity;
                            newTC.RunningQuantity = tcvm.TicketClass.Quantity;
                            newTC.StartDate = DateTime.Parse(date);
                            newTC.EndDate = DateTime.Parse(date);
                            newTC.StatusID = StatusHelper.StatusID("TicketClass", "Active");
                            newTC.BoxOffice = tcvm.BoxOffice;
                            newTC.IsOnline = tcvm.IsOnline;
                            tcvm.IsOnline = true;
                            db.TicketClasses.Add(newTC);
                            db.SaveChanges();

                        }

                        return RedirectToAction("EditByEOCentral", "Events", new { id = EventID });
                    }
                    else
                    {

                        try
                        {
                            tcvm.TicketClass.StatusID = StatusHelper.StatusID("TicketClass", "Active");
                            tcvm.TicketClass.BoxOffice = tcvm.BoxOffice;
                            tcvm.TicketClass.IsOnline = tcvm.IsOnline;
                            tcvm.IsOnline = true;
                            db.TicketClasses.Add(tcvm.TicketClass);
                            db.SaveChanges();
                            return RedirectToAction("EditByEOCentral", "Events", new { id = EventID });
                        }
                        catch (DbEntityValidationException e)
                        {
                            string em = e.Message;
                        }
                    }
                }
            }

            fail:


            ViewBag.EventID = EventID;

            tcvm = null;

            var Event2 = db.Events.Find(EventID);

            ViewBag.EventName = Event2.EventName;

            var eo = EventOrganiserRepository.GetEventOrganiserByEmail(User.Identity.Name);

            if (eo.ID != Event2.EventOrganiserID)
            {
                return RedirectToAction("Index", "Home");
            }
            int baseYear = int.Parse(ConfigRepo.Get("baseYear").ToString());
            TicketClassViewModel tc = new TicketClassViewModel(EventID, true, baseYear);
            tc.TicketClass = new TicketClass();

            Growl("MiiD", string.IsNullOrWhiteSpace(growl) ? "A field was not set correctly, check the page for hints." : growl);
            return View(tc);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult CreateMultiForEvent(TicketClassViewModel tcvm, int EventID, string MultipleDates)
        {
            string growl = "";




            if (ModelState.IsValid)
            {
                tcvm.TicketClass.EventID = EventID;
                tcvm.TicketClass.Description = tcvm.TicketClassDescription;
                tcvm.TicketClass.RunningQuantity = tcvm.TicketClass.Quantity;
                tcvm.TicketClass.Code = tcvm.TicketClass.Code;
                tcvm.TicketClass.Price = tcvm.TicketClass.Price;//These match at start
                                                                             //tcvm.TicketClass.StatusID = StatusHelper.StatusID("TicketClass","Active");

                var Event = db.Events.Find(EventID);

                var check1 = db.TicketClasses.Where(x => x.EventID == EventID && x.Description == tcvm.TicketClass.Description);

                if (check1 != null && check1.Count() > 0)
                {

                    growl = "Ticket class already exists.";
                    Growl("MiiD", "Please enter a unique description");
                    ModelState.AddModelError("StartDateTime", "Please enter a unique description");

                    goto fail;
                }




                else
                {




                    ViewBag.EventName = Event.EventName;

                    if (String.IsNullOrEmpty(MultipleDates))
                    {

                        string[] TimeStr = tcvm.StartDateTimeTime.Split(':');
                        DateTime ticketStartDate = new DateTime(tcvm.StartDateYear, tcvm.StartDateMonth, tcvm.StartDateDay, int.Parse(TimeStr[0]), int.Parse(TimeStr[1]), 0);
                        tcvm.TicketClass.StartDate = ticketStartDate;

                        int failcount = 0;
                        if (DateHelper.DateSequenceViolation((DateTime)Event.StartDateTime, ticketStartDate))
                        {
                            //Invalid start date
                            ModelState.AddModelError("StartDateTime", "Your ticket's date range is not within the same date range of the event that you have created. Either extend the event date range or adjust your ticket start or end date & time.");
                            growl = "Your ticket's date range is not within the same date range of the event that you have created. Either extend the event date range or adjust your ticket start or end date & time";
                            failcount++;
                        }

                        TimeStr = tcvm.EndDateTimeTime.Split(':');
                        DateTime ticketEndDate = new DateTime(tcvm.EndDateYear, tcvm.EndDateMonth, tcvm.EndDateDay, int.Parse(TimeStr[0]), int.Parse(TimeStr[1]), 0);
                        tcvm.TicketClass.EndDate = ticketEndDate;

                        if (DateHelper.DateSequenceViolation(ticketEndDate, (DateTime)Event.EndDateTime))
                        {
                            //Invalid start date
                            ModelState.AddModelError("EndDateTime", "Your ticket's date range is not within the same date range of the event that you have created. Either extend the event date range or adjust your ticket start or end date & time.");
                            growl = "Your ticket's date range is not within the same date range of the event that you have created. Either extend the event date range or adjust your ticket start or end date & time";
                            failcount++;
                        }
                        if (DateHelper.DateSequenceViolation(ticketStartDate, ticketEndDate))
                        {
                            //Invalid start date
                            ModelState.AddModelError("EndDateTime", "Your ticket's date range is not within the same date range of the event that you have created. Either extend the event date range or adjust your ticket start or end date & time.");
                            growl = "Your ticket's date range is not within the same date range of the event that you have created. Either extend the event date range or adjust your ticket start or end date & time";
                            failcount++;
                        }


                        if (failcount > 0)
                            goto fail;
                    }

                    string[] MultipleDatesArray = MultipleDates.Split(',');
                    if (!String.IsNullOrEmpty(MultipleDates) && MultipleDatesArray.Count() > 0)
                    {
                        //Get times from dropdown values and apply too all ticket classes
                      

                                                                        
                        foreach (var date in MultipleDatesArray)
                        {
                            string[] TimeStr = tcvm.StartDateTimeTime.Split(':');
                            DateTime ticketStartDate = new DateTime(int.Parse(date.Split('-')[0]), int.Parse(date.Split('-')[1]), int.Parse(date.Split('-')[2]), int.Parse(TimeStr[0]), int.Parse(TimeStr[1]), 0);
                            TimeStr = tcvm.EndDateTimeTime.Split(':');
                            DateTime ticketEndDate = new DateTime(int.Parse(date.Split('-')[0]), int.Parse(date.Split('-')[1]), int.Parse(date.Split('-')[2]), int.Parse(TimeStr[0]), int.Parse(TimeStr[1]), 0);

                            TicketClass newTC = new TicketClass();
                            newTC.EventID = EventID;
                            newTC.Price = tcvm.TicketClass.Price;
                            newTC.Description = String.Format("{0}  ({1})", tcvm.TicketClassDescription, date);
                            newTC.Code = String.Format("{0}  ({1})", tcvm.TicketClass.Code, date);
                            newTC.Quantity = tcvm.TicketClass.Quantity;
                            newTC.RunningQuantity = tcvm.TicketClass.Quantity;
                            newTC.StartDate = ticketStartDate;// DateTime.Parse(date);
                            newTC.EndDate = ticketEndDate;// DateTime.Parse(date);
                            newTC.StatusID = StatusHelper.StatusID("TicketClass", "Active");
                            newTC.BoxOffice = tcvm.BoxOffice;
                            newTC.IsOnline = tcvm.IsOnline;
                            tcvm.IsOnline = true;
                            db.TicketClasses.Add(newTC);
                            db.SaveChanges();

                        }

                        return RedirectToAction("EditByEOCentral", "Events", new { id = EventID });
                    }
                    else
                    {

                        try
                        {
                            tcvm.TicketClass.StatusID = StatusHelper.StatusID("TicketClass", "Active");
                            tcvm.TicketClass.BoxOffice = tcvm.BoxOffice;
                            tcvm.TicketClass.IsOnline = tcvm.IsOnline;
                            tcvm.IsOnline = true;
                            db.TicketClasses.Add(tcvm.TicketClass);
                            db.SaveChanges();
                            return RedirectToAction("EditByEOCentral", "Events", new { id = EventID });
                        }
                        catch (DbEntityValidationException e)
                        {
                            string em = e.Message;
                        }
                    }
                }
            }

        fail:


            ViewBag.EventID = EventID;

            tcvm = null;

            var Event2 = db.Events.Find(EventID);

            ViewBag.EventName = Event2.EventName;

            var eo = EventOrganiserRepository.GetEventOrganiserByEmail(User.Identity.Name);

            if (eo.ID != Event2.EventOrganiserID)
            {
                return RedirectToAction("Index", "Home");
            }
            int baseYear = int.Parse(ConfigRepo.Get("baseYear").ToString());
            TicketClassViewModel tc = new TicketClassViewModel(EventID, true, baseYear);
            tc.TicketClass = new TicketClass();

            Growl("MiiD", string.IsNullOrWhiteSpace(growl) ? "A field was not set correctly, check the page for hints." : growl);
            return View(tc);

        }


        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.EventID = new SelectList(db.Events, "ID", "EventName");
            return View();
        }

        // POST: TicketClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ID,Code,EventID,Description,Quantity,Price")] TicketClass ticketClass)
        {
            if (ModelState.IsValid)
            {
                ticketClass.RunningQuantity = ticketClass.Quantity; //These match at start
                db.TicketClasses.Add(ticketClass);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventID = new SelectList(db.Events, "ID", "EventName", ticketClass.EventID);
            return View(ticketClass);
        }

        // GET: TicketClasses/Edit/5

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }





            TicketClass ticketClass = db.TicketClasses.Find(id);
            if (ticketClass == null)
            {
                return HttpNotFound();
            }

            var eo = EventOrganiserRepository.GetEventOrganiserByEmail(User.Identity.Name);

            var Event = db.Events.Find(ticketClass.EventID);

            if (eo.ID != Event.EventOrganiserID)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.EventID = ticketClass.EventID;
            ViewBag.Event = db.Events.Find(ticketClass.EventID).EventName;
            TicketClassViewModel tvm = new TicketClassViewModel((int)id);
            //tvm.TicketClass = ticketClass;
            if (User.IsInRole("Admin"))
            {

                return View(tvm);
            }
            else
            {
                return View("EditByEO", tvm);
            }
        }

        // POST: TicketClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(TicketClassViewModel tcvm, int EventID)
        {

            string growl = "";
            if (ModelState.IsValid)
            {


                if (User.IsInRole("Admin"))
                {
                    //Check new qty
                    if (tcvm.TicketClass.Quantity < tcvm.OldQuantity && tcvm.TicketClass.Quantity < tcvm.TicketsSold)//Top Down
                    {
                        //disallow
                        Growl("MiiD Ticket Class Edit Error", String.Format("New quantity cannot be lower than number of tickets already sold -  ({0}).", tcvm.TicketsSold.ToString()));
                        ViewBag.EventID = tcvm.EventID;
                        goto fail;

                    }

                    var Event = db.Events.Find(EventID);

                    ViewBag.EventName = Event.EventName;

                    string[] TimeStr = tcvm.StartDateTimeTime.Split(':');
                    DateTime ticketStartDate = new DateTime(tcvm.StartDateYear, tcvm.StartDateMonth, tcvm.StartDateDay, int.Parse(TimeStr[0]), int.Parse(TimeStr[1]), 0);
                    tcvm.TicketClass.StartDate = ticketStartDate;

                    int failcount = 0;
                    if (DateHelper.DateSequenceViolation((DateTime)Event.StartDateTime, ticketStartDate))
                    {
                        //Invalid start date
                        ModelState.AddModelError("StartDateTime", "Start Date Time is before Event Start Date Time.");
                        //                    growl = "Start Date Time is before Event Start Date Time.";
                        growl = "Your ticket's date range is not within the same date range of the event that you have created. Either extend the event date range or adjust your ticket start or end date & time";

                        failcount++;
                    }

                    TimeStr = tcvm.EndDateTimeTime.Split(':');
                    DateTime ticketEndDate = new DateTime(tcvm.EndDateYear, tcvm.EndDateMonth, tcvm.EndDateDay, int.Parse(TimeStr[0]), int.Parse(TimeStr[1]), 0);
                    tcvm.TicketClass.EndDate = ticketEndDate;

                    if (DateHelper.DateSequenceViolation(ticketEndDate, (DateTime)Event.EndDateTime))
                    {
                        //Invalid start date
                        ModelState.AddModelError("EndDateTime", "Ticket End Date Time goes beyond Event End Date Time.");
                        //                  growl = "Ticket End Date Time goes beyond Event End Date Time.";
                        growl = "Your ticket's date range is not within the same date range of the event that you have created. Either extend the event date range or adjust your ticket start or end date & time";

                        failcount++;
                    }

                    if (failcount > 0)
                        goto fail;


                    tcvm.TicketClass.BoxOffice = tcvm.BoxOffice;
                    
                    tcvm.TicketClass.IsOnline = tcvm.IsOnline;
                    

                    tcvm.TicketClass.EventID = EventID;
                    db.Entry(tcvm.TicketClass).State = EntityState.Modified;
                    db.SaveChanges();
                    Growl("MiiD", "Ticket Class Updated Successfully.");


                }
                else
                {
                    tcvm.TicketClass.DateTimeUpdated = DateTime.Now;
                    
                    tcvm.TicketClass.RunningQuantity = tcvm.RunningQuantity;
                    tcvm.TicketClass.EventID = EventID;
                    tcvm.TicketClass.BoxOffice = tcvm.BoxOffice;
                    tcvm.TicketClass.IsOnline = tcvm.IsOnline;
                    
                  


                    db.Entry(tcvm.TicketClass).State = EntityState.Modified;
                    db.SaveChanges();
                    Growl("MiiD", "Ticket Class Updated Successfully.");

                }
                return RedirectToAction("EditByEOCentral", "Events", new { id = EventID });
            }
            ViewBag.EventID = tcvm.EventID;
            return View(tcvm);

            fail:


            ViewBag.EventID = EventID;
            int TCID = tcvm.TicketClass.ID;
            tcvm = null;

            var Event2 = db.Events.Find(EventID);

            ViewBag.EventName = Event2.EventName;

            var eo = EventOrganiserRepository.GetEventOrganiserByEmail(User.Identity.Name);

            if (eo.ID != Event2.EventOrganiserID)
            {
                return RedirectToAction("Index", "Home");
            }

            TicketClassViewModel tc = new TicketClassViewModel(TCID);


            Growl("MiiD", growl);
            return View(tc);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketClass ticketClass = db.TicketClasses.Find(id);
            if (ticketClass == null)
            {
                return HttpNotFound();
            }
            return View(ticketClass);
        }

        // POST: TicketClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketClass ticketClass = db.TicketClasses.Find(id);
            db.TicketClasses.Remove(ticketClass);
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
