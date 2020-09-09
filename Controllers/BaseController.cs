using MiidWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiidWeb.Controllers
{

    public abstract class BaseController : Controller
    {
        private string _subdomain;

        public void Growl(string title, string message)
        {
            TempData["Growl"] = title + ":" + message;
        }

   


      
    }

}