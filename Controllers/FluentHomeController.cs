using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MiidWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MiidWeb.Repositories;

namespace MiidWeb.Controllers
{
    
    public class FluentHomeController : Controller
{
    public ActionResult Index()
    {
        var model = new MyViewModel
        {
            StartDate = DateTime.Now.AddDays(2),
            DateToCompareAgainst = DateTime.Now
        };
        return View(model);
    }

    [HttpPost]
    public ActionResult Index(MyViewModel model)
    {
        return View(model);
    }
}
}