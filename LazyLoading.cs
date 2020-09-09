using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MiidWeb
{
    public class LazyLoading
    {
    }

    public partial class MiidEntities : DbContext
    {
        public MiidEntities(bool LL) : base("name=MiidEntities")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
    }

}