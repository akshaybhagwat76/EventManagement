using MiidWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MiidWeb.Repositories
{
  public class VendorRepository
  {

    public string GetNewVendorCode()
    {

      string VendorCode = "";
      var intVendorCode = 0;

      using (MiidEntities db = new MiidEntities())
      {
        if (db.Vendors.Count() > 0)
        {
          var devices = db.Vendors.OrderByDescending(x => x.ID).ToList();

          intVendorCode = int.Parse(devices.First().VendorCode) + 1;
        }
        else
        {
          intVendorCode = 100;
        }

        VendorCode = intVendorCode.ToString();
      }

      return VendorCode;

    }

    public static void SendEODReport(int VendorID, int EventID, string emailBody, int shiftNo, DateTime closingTime)
    {
      //Send mail to admin

      StringBuilder adminbody = new StringBuilder();
      adminbody.AppendLine("<html><body>");

      using (var db = new MiidEntities())
      {
        var Vendor = db.Vendors.Find(VendorID);
        var Event = db.Events.Find(EventID);


        adminbody.AppendLine(String.Format("Close Shift Report for {0}:", Vendor.Name));
        adminbody.AppendLine(String.Format("Event {0}:", Event.EventName));
        adminbody.AppendLine(String.Format("Shift {0}:", shiftNo));
        adminbody.AppendLine(String.Format("Closing Time {0}:", closingTime.ToString("yyyy-MM-dd hh:mm:ss")));

        adminbody.AppendLine(emailBody);


        adminbody.AppendLine("</body></html>");

        EmailHelper.SendMail(Vendor.Email, "EOD Report", adminbody.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
        EmailHelper.SendMail("info@miid.co.za", "EOD Report", adminbody.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
      }

    }

    public static void SendVendorCode(Vendor vendor)
    {
      //Send mail to admin

      StringBuilder adminbody = new StringBuilder();
      using (var db = new MiidEntities())
      {



        adminbody.AppendLine(String.Format("Hi, {0}", "MiiD Finance Admin"));
        adminbody.AppendLine(String.Format("Vendor Added: Contact: {0}  Code: {1} - Email:{2}", vendor.ContactName, vendor.VendorCode, vendor.Email));


        adminbody.AppendLine(String.Format("Details are:"));
        adminbody.AppendLine(String.Format("IDNumber:{0}", vendor.IDNumber));
        adminbody.AppendLine(String.Format("Name:{0}", vendor.Name));
        adminbody.AppendLine(String.Format("Telephone:{0}", vendor.Telephone));
        adminbody.AppendLine(String.Format("Email:{0}", vendor.Email));
        adminbody.AppendLine(String.Format("ContactName:{0}", vendor.ContactName));
        adminbody.AppendLine(String.Format("ContactTelephone:{0}", vendor.ContactTelephone));
        adminbody.AppendLine(String.Format("ContactEmail:{0}", vendor.ContactEmail));
        adminbody.AppendLine(String.Format("SecondaryContactName:{0}", vendor.SecondaryContactName));
        adminbody.AppendLine(String.Format("SecondaryContactTelephone:{0}", vendor.SecondaryContactTelephone));
        adminbody.AppendLine(String.Format("SecondaryContactEmail:{0}", vendor.SecondaryContactEmail));
        adminbody.AppendLine(String.Format("StartDate:{0}", vendor.StartDate));
        adminbody.AppendLine(String.Format("Description:{0}", vendor.Description));
        adminbody.AppendLine(String.Format("Bank:{0}", vendor.Bank));
        adminbody.AppendLine(String.Format("BranchCode:{0}", vendor.BranchCode));
        adminbody.AppendLine(String.Format("AccountNumber:{0}", vendor.AccountNumber));
        adminbody.AppendLine(String.Format("BankType:{0}", vendor.BankType));
        adminbody.AppendLine(String.Format("DateTimeUpdated:{0}", vendor.DateTimeUpdated));
        adminbody.AppendLine(String.Format("VendorCode:{0}", vendor.VendorCode));
        adminbody.AppendLine(String.Format("VendorPin:{0}", vendor.VendorPin));


        EmailHelper.SendMail("logs@miid.co.za", String.Format("Mii-Funds withdrawal request"), adminbody.ToString(), null, null, null, ConfigRepo.GetSubdomainID());

      }



      StringBuilder userbody = new StringBuilder();

      using (var db = new MiidEntities())
      {

        userbody.AppendLine(String.Format("Hi, {0}", vendor.ContactName));
        userbody.AppendLine(String.Format("You have been added as vendor on MiiD system."));


        userbody.AppendLine(String.Format("Your Details are:"));
        userbody.AppendLine(String.Format("IDNumber:{0}", vendor.IDNumber));
        userbody.AppendLine(String.Format("Name:{0}", vendor.Name));
        userbody.AppendLine(String.Format("Telephone:{0}", vendor.Telephone));
        userbody.AppendLine(String.Format("Email:{0}", vendor.Email));
        userbody.AppendLine(String.Format("ContactName:{0}", vendor.ContactName));
        userbody.AppendLine(String.Format("ContactTelephone:{0}", vendor.ContactTelephone));
        userbody.AppendLine(String.Format("ContactEmail:{0}", vendor.ContactEmail));
        userbody.AppendLine(String.Format("SecondaryContactName:{0}", vendor.SecondaryContactName));
        userbody.AppendLine(String.Format("SecondaryContactTelephone:{0}", vendor.SecondaryContactTelephone));
        userbody.AppendLine(String.Format("SecondaryContactEmail:{0}", vendor.SecondaryContactEmail));
        userbody.AppendLine(String.Format("StartDate:{0}", vendor.StartDate));
        userbody.AppendLine(String.Format("Description:{0}", vendor.Description));
        userbody.AppendLine(String.Format("Bank:{0}", vendor.Bank));
        userbody.AppendLine(String.Format("BranchCode:{0}", vendor.BranchCode));
        userbody.AppendLine(String.Format("AccountNumber:{0}", vendor.AccountNumber));
        userbody.AppendLine(String.Format("BankType:{0}", vendor.BankType));
        userbody.AppendLine(String.Format("DateTimeUpdated:{0}", vendor.DateTimeUpdated));
        userbody.AppendLine(String.Format("VendorCode:{0}", vendor.VendorCode));
        userbody.AppendLine(String.Format("VendorPin:{0}", vendor.VendorPin));

        userbody.AppendLine(String.Format("Thank you!"));
        userbody.AppendLine(String.Format("MiiD Team"));

        EmailHelper.SendMail(vendor.Email, "MiiD Vendor Details", userbody.ToString(), null, null, null, ConfigRepo.GetSubdomainID());
      }
    }
  }
}