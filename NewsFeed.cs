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
    
    public partial class NewsFeed
    {
        public NewsFeed()
        {
            this.NewsFeedComments = new HashSet<NewsFeedComment>();
        }
    
        public int ID { get; set; }
        public Nullable<int> EndUserID { get; set; }
        public Nullable<int> StatusID { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string Headline { get; set; }
        public Nullable<System.DateTime> DateTimeUpdated { get; set; }
    
        public virtual EndUser EndUser { get; set; }
        public virtual Status Status { get; set; }
        public virtual ICollection<NewsFeedComment> NewsFeedComments { get; set; }
    }
}
