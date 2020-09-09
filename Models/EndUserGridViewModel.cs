using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiidWeb.Models
{
    public class EndUserGridViewModel
    {
                    // Data properties
            public IEnumerable<EndUserViewModel> Users { get; set; }




        public string FirstNameSearch { get; set; }
        public string SurnameSearch { get; set; }
        public string IdNumberSearch { get; set; }
    }
}