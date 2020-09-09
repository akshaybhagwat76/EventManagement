using MiidWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiidWeb.Models
{
    public class EndUserViewModel
    {
        public EndUser EndUser { get; set; }

        public EndUser ID { get; set; }

        public List<EndUserViewModel> MyChildren { get; set; }

        public string Status { get; set; }
        public CalendarViewModel CalendarViewModel { get; set; }

        public MyEventsViewModel UpcomingEvents { get; set; }
        public NewsFeedViewModel NewsFeedViewModel { get; set; }

        public List<Event> CurrentEvents { get; set; }
        public List<Friend> Friends { get; set; }

        public string FriendList { get; set; }

        public EndUserJson EndUserJson { get; set; }

        public Pirate Pirate { get; set; }

        public List<Mention> MentionList { get; set; }

        public decimal AvailableBalance { get; set; }

        public bool HaveActiveBand { get; set; }

        public string TagNumber { get; set; }

        public int TagID { get; set; }

        public string TagStatus { get; set; }

        public bool IsCreditTransaction { get; set; }



        public EndUserViewModel()
        {

        }
        public EndUserViewModel(EndUser EndUser)
        {
            try
            {
                this.EndUser = EndUser;
                this.MyChildren = new List<EndUserViewModel>();

                using (var db = new MiidEntities())
                {
                    var dbMyChildren = db.EndUsers.Where(x => x.ParentUserID == EndUser.ID).ToList();

                    foreach (var kid in dbMyChildren)
                    {
                        this.MyChildren.Add(new EndUserViewModel(kid));

                    }

                    var MyMoneys = db.MyMoneys.Where(x => x.EndUserID == EndUser.ID).OrderByDescending(v => v.ID);


                    var Status = db.Status.Find(EndUser.StatusID);
                    this.Status = Status.Description;

                    if (MyMoneys != null && MyMoneys.Count() > 0)
                    {
                        this.AvailableBalance = (decimal)(MyMoneys.First().RunningBalance ?? 0);
                    }
                    else
                    {
                        this.AvailableBalance = 0.00M;
                    }

                    int ActiveStatusID = StatusHelper.StatusID("NFCTag", "Active");
                    var Tags = db.NFCTags.Where(x => x.EndUserID == EndUser.ID && x.StatusID == ActiveStatusID);
                    var NonActiveTags = db.NFCTags.Where(x => x.EndUserID == EndUser.ID && x.StatusID != ActiveStatusID);
                    if (Tags != null && Tags.Count() > 0)
                    {
                        this.HaveActiveBand = true;
                        this.TagNumber = Tags.First().TagNumber;
                        this.TagStatus = "Active";
                        this.TagID = Tags.First().ID;
                    }
                    else
                    {
                        this.HaveActiveBand = false;

                    }
                    if (NonActiveTags != null && NonActiveTags.Count() > 0 && this.HaveActiveBand == false)
                    {
                        this.TagNumber = NonActiveTags.First().TagNumber;
                        this.TagStatus = db.Status.Find(NonActiveTags.First().StatusID).Description;
                        this.TagStatus = "Can Tag";
                    }
                    else
                    {
                        if (HaveActiveBand == false)
                        {
                            this.TagStatus = "No Tags";
                        }


                    }
                }
            }
            catch (Exception ee)
            {

            }
        }
    }

    public class Mention
    {
        //{ id:1, name:'Kenneth Auchenberg', 'avatar':'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'type':'contact' },
        public int id { get; set; }
        public string name { get; set; }

        public string avatar { get; set; }

        public string type { get; set; }
    }

    public class EndUserJson
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string url { get; set; }

        //"first_name": "Arthur",
        //           "last_name": "Godfrey",
        //           "email": "arthur_godfrey@nccu.edu",
        //           "url": "https://si0.twimg.com/sticky/default_profile_images/default_profile_2_normal.png"
    }

    public class Pirate
    {
        public string label { get; set; }
        public PirateValue value { get; set; }

        public Pirate()
        {
            this.label = "<img class=\"dropdown\" src=\"../../images/2_Souchon_Daniel.png\">Daniel Souchon";
            this.value = new PirateValue();
        }
        //label: '<img class="dropdown" src="../../images/2_Souchon_Daniel.png">Daniel Souchon', value: { value: 'pirate1', content: '<span class="pirate"><a href="http://en.wikipedia.org/wiki/Calico_Jack_Rackham"><img class="inline" src="../../images/2_Souchon_Daniel.png">Daniel Souchon</a></span>' }
    }

    public class PirateValue
    {
        public string value { get; set; }
        public string content { get; set; }

        public PirateValue()
        {
            this.value = "pirate1";
            this.content = "<span class=\"pirate\"><a href=\"http://en.wikipedia.org/wiki/Calico_Jack_Rackham\"><img class=\"inline\" src=\"../../images/2_Souchon_Daniel.png\">Daniel Souchon</a></span>";
        }
    }

    public class ChildUserViewModel
    {
        public int ID { get; set; }
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string Email { get; set; }
        public int? ParentUserID { get; set; }
        public string Cell { get; set; }

        public string NextOfKin { get; set; }

        public string NextOfKinTelephone { get; set; }

        public ChildUserViewModel()
        {

        }

        public ChildUserViewModel(int ParentUserID)

        {
            this.ParentUserID = ParentUserID;
        }
        public ChildUserViewModel(EndUser e)
        {
            this.ID = e.ID;
          
            this.Surname = e.Surname;
            this.Firstname = e.Firstname;
            this.ParentUserID = e.ParentUserID;
            this.Email = e.Email;
            this.Cell = e.Cell;
            this.NextOfKin = e.NextOfKin;
            this.NextOfKinTelephone = e.NextOfKinTelephone;
        }

    }


    public class CreateFirstEndUserViewModel
    {

        public CreateFirstEndUserViewModel()
        {
        }
        public CreateFirstEndUserViewModel(EndUser e)
        {




            this.ID = e.ID;
            this.Suburb = e.Suburb;
            this.Surname = e.Surname;
            this.Firstname = e.Firstname;
            this.Allergies = e.Allergies;
            this.Cell = e.Cell;
            this.City = e.City;

            if (e.DateOfBirth != null)
            {
                string dbod = ((DateTime)e.DateOfBirth).ToString("dd");
                dbod = ((DateTime)e.DateOfBirth).ToString("d");

                this.DOB = e.DateOfBirth;
                this.DateOfBirthDay = int.Parse(((DateTime)e.DateOfBirth).ToString("dd"));
                this.DateOfBirthMonth = int.Parse(((DateTime)e.DateOfBirth).ToString("MM"));
                this.DateOfBirthYear = int.Parse(((DateTime)e.DateOfBirth).ToString("yyyy"));
            }
            this.Email = e.Email;
            this.Female = e.Sex == "F";
            this.IDNumber = e.IDNumber;
            this.KnownConditions = e.KnownConditions;
            this.Male = e.Sex == "M";
            this.MedicalAidCompany = e.MedicalAidCompany;
            this.MedicalAidNumber = e.MedicalAidNumber;
            this.Medication = e.Medication;
            this.Newsletter = e.NewsletterAccepted ?? false;
            this.NextOfKin = e.NextOfKin;
            this.NextOfKinTelephone = e.NextOfKinTelephone;
            this.PostalCode = e.PostalCode;
            this.ProfilePicURL = e.ProfilePicURL;
            this.Province = e.Province;
            this.RaceID = e.RaceID ?? 0;
            this.StreetAddress = e.StreetAddress;
            this.Suburb = e.Suburb;
            this.Telephone = e.Telephone;
            this.TermsAndConditions = e.TermsAndConditionsAccepted ?? false;

            var list = new List<SelectListItem>();

            list.Add(new SelectListItem() { Value = "0", Text = "Race" });
            list.Add(new SelectListItem() { Value = "1", Text = "African" });
            list.Add(new SelectListItem() { Value = "2", Text = "Caucasian" });
            list.Add(new SelectListItem() { Value = "3", Text = "Coloured" });
            list.Add(new SelectListItem() { Value = "4", Text = "Indian" });
            list.Add(new SelectListItem() { Value = "5", Text = "Other" });
            list.Add(new SelectListItem() { Value = "6", Text = "Not specified" });


            this.Races = list;

            var raceitem = new SelectListItem();
            if (list.Where(x => x.Value == this.RaceID.ToString()) != null)
            {
                this.Race = list.Where(x => x.Value == this.RaceID.ToString()).First().Text;
            }
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

            this.Provinces = provinceList;

            this.DobMonths = GetMonthDropDown();
            this.DobDays = GetNumberDropDown(1, 31);
            this.DobYears = GetNumberDropDown(1960, 2005);


            if (e.Firstname == e.Email) this.Firstname = null;
            if (e.Surname == e.Email) this.Surname = null;
            if (e.Cell != null)
            {
                if (e.Cell.StartsWith("00000")) this.Cell = null;
            }
            if (e.IDNumber != null)
            {
                if (e.IDNumber != null && e.IDNumber.StartsWith("00000")) this.IDNumber = null;
            }


        }

        public IEnumerable<SelectListItem> Races { get; set; }
        public IEnumerable<SelectListItem> Provinces { get; set; }
        public IEnumerable<SelectListItem> DobDays { get; set; }
        public IEnumerable<SelectListItem> DobMonths { get; set; }
        public IEnumerable<SelectListItem> DobYears { get; set; }

        public int ID { get; set; }
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string IDNumber { get; set; }
        public int RaceID { get; set; }

        

        public string IsCashless { get; set; }
       
    public string Race { get; set; }

        public string StreetAddress { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Telephone { get; set; }
        public string Cell { get; set; }
        public string Email { get; set; }

        public DateTime? DOB { get; set; }
        public int DateOfBirthDay { get; set; }
        public int DateOfBirthMonth { get; set; }
        public int DateOfBirthYear { get; set; }

        public bool Male { get; set; }
        public bool Female { get; set; }
        public bool TermsAndConditions { get; set; }
        public bool Newsletter { get; set; }
        public string NextOfKin { get; set; }
        public string NextOfKinTelephone { get; set; }
        public string Medication { get; set; }
        public string MedicalAidCompany { get; set; }
        public string MedicalAidNumber { get; set; }
        public string Allergies { get; set; }
        public string KnownConditions { get; set; }
        public string ProfilePicURL { get; set; }

        public string Cookie { get; set; }
        public string PromoCode { get; set; }

        public string friendEmailList { get; set; }

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

        private static List<SelectListItem> GetNumberDropDown(int x, int y)
        {
            List<SelectListItem> date1 = new List<SelectListItem>();

            while (x <= y)
            {
                date1.Add(new SelectListItem() { Value = x.ToString(), Text = x.ToString() });
                x++;
            }

            return date1;
        }
    }

}