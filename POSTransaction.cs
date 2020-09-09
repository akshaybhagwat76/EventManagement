//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MiidWeb
{
    using System;
    using System.Collections.Generic;
    
    public partial class POSTransaction
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public Nullable<int> EventID { get; set; }
        public Nullable<int> VendorID { get; set; }
        public Nullable<int> StaffID { get; set; }
        public Nullable<int> DeviceID { get; set; }
        public int EndUserID { get; set; }
        public int POSTagScanID { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<int> ShiftNo { get; set; }
        public string UniqueID { get; set; }
    
        public virtual Device Device { get; set; }
        public virtual EndUser EndUser { get; set; }
    }
}
