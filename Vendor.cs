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
    
    public partial class Vendor
    {
        public Vendor()
        {
            this.Promotions = new HashSet<Promotion>();
            this.Staffs = new HashSet<Staff>();
            this.VendorEvents = new HashSet<VendorEvent>();
        }
    
        public int ID { get; set; }
        public string IDNumber { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string ContactName { get; set; }
        public string ContactTelephone { get; set; }
        public string ContactEmail { get; set; }
        public string SecondaryContactName { get; set; }
        public string SecondaryContactTelephone { get; set; }
        public string SecondaryContactEmail { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public string Description { get; set; }
        public string Bank { get; set; }
        public string BranchCode { get; set; }
        public string AccountNumber { get; set; }
        public string BankType { get; set; }
        public Nullable<System.DateTime> DateTimeUpdated { get; set; }
        public string VendorCode { get; set; }
        public Nullable<int> VendorPin { get; set; }
    
        public virtual ICollection<Promotion> Promotions { get; set; }
        public virtual ICollection<Staff> Staffs { get; set; }
        public virtual ICollection<VendorEvent> VendorEvents { get; set; }
    }
}