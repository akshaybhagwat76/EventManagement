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
    
    public partial class Staff
    {
        public Staff()
        {
            this.Incidents = new HashSet<Incident>();
        }
    
        public int ID { get; set; }
        public string Code { get; set; }
        public Nullable<int> AccessLevelID { get; set; }
        public string IDNumber { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string StreetAddress { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Cell { get; set; }
        public string Tell { get; set; }
        public string Email { get; set; }
        public string NextKinName { get; set; }
        public string NextKinTelephone { get; set; }
        public string NextKinEmail { get; set; }
        public Nullable<int> StaffTypeID { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<System.DateTime> DateTimeUpdated { get; set; }
        public Nullable<int> CurrentVendorID { get; set; }
        public Nullable<int> CurrentDevicePin { get; set; }
        public string CurrentStaffCode { get; set; }
        public Nullable<int> CurrentEventID { get; set; }
        public Nullable<int> CurrentDeviceID { get; set; }
    
        public virtual Device Device { get; set; }
        public virtual Event Event { get; set; }
        public virtual ICollection<Incident> Incidents { get; set; }
        public virtual StaffType StaffType { get; set; }
        public virtual Status Status { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
}
