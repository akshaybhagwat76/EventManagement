using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiidWeb.Models
{
    public class VendorReportModel
    {
        public int ID { get; set; }
        public DateTime EndTime { get; set; }
        public string EventName { get; set; }
        public string VendorName { get; set; }
        public string VendorCode{ get; set; }
        public string VendorContactName { get; set; }
        public string VendorTelephone { get; set; }
        public string VendorEmail { get; set; }
        public int ShiftNo { get; set; }
        public string StaffName { get; set; }
        public string DeviceCode { get; set; }
        public decimal Amount { get; set; }

        
    }

    public class VendorSummaryReportModel
    {
        public int ID { get; set; }
        public DateTime ShiftStartTime { get; set; }
        public DateTime ShiftEndTime { get; set; }
        public string EventName { get; set; }
        public string VendorName { get; set; }
        public string VendorCode { get; set; }
        public string VendorContactName { get; set; }
        public string VendorTelephone { get; set; }
        public string VendorEmail { get; set; }
        public int ShiftNo { get; set; }
        public decimal Amount { get; set; }


    }
}