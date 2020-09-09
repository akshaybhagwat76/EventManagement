using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiidWeb.Models
{
    public class EventSearchModel
    {
        //public IEnumerable<EventViewModel> Events { get; set; }
        public PagedList.IPagedList<EventViewModel> Events { get; set; }
        // Sorting-related properties
        public string SortBy { get; set; }
        public bool SortAscending { get; set; }
        public string SortExpression
        {
            get
            {   
                return this.SortAscending ? this.SortBy + " asc" : this.SortBy + " desc";

            }
        }

        // Filtering-related properties
        public int? EventCategoryID { get; set; }
        public int? EventOrganiserID{ get; set; }
        public int? RegionID { get; set; }
        public DateTime? EventMonth { get; set; }
        public string KeyWordSearchText { get; set; }

        public IEnumerable EventCategoryList { get; set; }
        
        public IEnumerable EventOrganiserList { get; set; }
        public IEnumerable RegionList { get; set; }

        public IEnumerable EventMonthList { get; set; }
        
    }

 
}