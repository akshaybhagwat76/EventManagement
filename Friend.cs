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
    
    public partial class Friend
    {
        public int ID { get; set; }
        public Nullable<int> InitiatingUserID { get; set; }
        public Nullable<int> AcceptingUserID { get; set; }
        public Nullable<System.DateTime> DateRequested { get; set; }
        public Nullable<System.DateTime> DateAccepted { get; set; }
        public Nullable<System.DateTime> DateRejected { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<System.DateTime> DateTimeUpdated { get; set; }
    
        public virtual EndUser EndUser { get; set; }
        public virtual EndUser EndUser1 { get; set; }
    }
}