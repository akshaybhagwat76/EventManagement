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
    
    public partial class Region
    {
        public Region()
        {
            this.Events = new HashSet<Event>();
        }
    
        public int ID { get; set; }
        public string RegionName { get; set; }
        public string Description { get; set; }
        public string CountryCode { get; set; }
        public Nullable<System.DateTime> DateTimeUpdated { get; set; }
    
        public virtual ICollection<Event> Events { get; set; }
    }
}
