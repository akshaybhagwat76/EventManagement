using MiidWeb.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiidWeb.Models
{
    public class VendorViewModel
    {
        public Vendor Vendor { get; set; }
        public int EventID { get; set; }

        public int VendorID { get; set; }
        public string EventName { get; set; }
        public string VendorCode { get; set; }

        public string VendorName { get; set; }

        public decimal RevenueForEvent { get; set; }

        
        public VendorViewModel() 
        { }

        public VendorViewModel(string VendorCode, int EventID)
        { 
           
             var db = new MiidEntities();

             var vendors = db.Vendors.Where(x => x.VendorCode.ToLower() == VendorCode.ToLower());
             if (vendors.Count() > 0)
             {
                 

                 var Vendor = vendors.First();
                 this.Vendor = Vendor;
                 this.VendorID = Vendor.ID;
                 this.VendorCode = Vendor.VendorCode;
                 this.VendorName = Vendor.Name;

                 var postrans = db.POSTransactions.Where(x => x.VendorID == Vendor.ID && x.EventID == EventID);

                 this.RevenueForEvent = 0.00M;

                 if (postrans!= null && postrans.Count() > 0)
                 {
                     this.RevenueForEvent = postrans.Sum(x => x.Amount)??0.00M;
                 }


             }

             this.EventID = EventID;


           
        }

        public VendorViewModel(int VendorID)
        {

            var db = new MiidEntities();

            var vendor = db.Vendors.Find(VendorID);

            this.Vendor = vendor;
            


        }



    }

    public class VendorListViewModel
    {
        private MiidEntities db = new MiidEntities();

        public int EventID { get; set; }
        public List<VendorViewModel> Vendors { get; set; }
        public IEnumerable<SelectListItem> VendorDropDownList { get; set; }

        public int SelectedVendorID { get; set; }
        public string SelectedReportType { get; set; }

        public IEnumerable<SelectListItem> ReportTypes { get; set; }


        public VendorListViewModel()
        { 
        
        }


        public VendorListViewModel(int EventID)
        {
            int baseYear = int.Parse(ConfigRepo.Get("baseYear").ToString());
            this.EventID = EventID;

            this.Vendors = new List<VendorViewModel>();

            var vendorevents = db.VendorEvents.Where(x => x.EventID == EventID);


            foreach (var ev in vendorevents)
            {
                this.Vendors.Add(new VendorViewModel((int)ev.VendorID));
            }

            this.VendorDropDownList = GetVendorDropDownList(this.Vendors);
            this.ReportTypes = GetVendorReportTypes();

            this.SelectedVendorID = 0;
            this.SelectedReportType = "Report Type";
        }




        private static IEnumerable<SelectListItem> GetVendorDropDownList(List<VendorViewModel> vendors)
        {
            List<SelectListItem> dlist = new List<SelectListItem>();

            dlist.Add(new SelectListItem() { Value = "0", Text = "Choose Vendor" });

            foreach(var v in vendors)
            {
                dlist.Add(new SelectListItem() { Value = v.Vendor.ID.ToString(), Text = v.Vendor.Name.ToString() });
                
            }

            return dlist;
        }
        private IEnumerable<SelectListItem> GetVendorReportTypes()
        {
            List<SelectListItem> dlist = new List<SelectListItem>();
            dlist.Add(new SelectListItem() { Value = "Summary", Text = "Summary Report" });
            dlist.Add(new SelectListItem() { Value = "Detail", Text = "Detail Report" });

            return dlist;

        }
    }

}