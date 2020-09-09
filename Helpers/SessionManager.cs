using MiidWeb.Controllers;
using MiidWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiidWeb.Helpers
{
    public static class SessionManager
    {
        private static void EnsureUserID()
        {
            //if youve lost the session = logoff
            //if youve lost both the EndUser and the EventOrganiser  = logoff
            if (HttpContext.Current.Session == null || (HttpContext.Current.Session["UserID"] == null))// && HttpContext.Current.Session["EventOrganiser"] == null))
            {
                //HttpContext.Current.Session.Abandon();
                HttpContext.Current.Response.Redirect("~/Account/Logoff", true);
            }
        }

     
        public static string UserID
        {
            get
            {
                //EnsureUserID();
                if (HttpContext.Current.Session["UserID"] == null)
                    HttpContext.Current.Session["UserID"] = "";
                return (string)HttpContext.Current.Session["UserID"];
            }
            set
            {
                HttpContext.Current.Session["UserID"] = value;
            }
        }

        public static string Country
        {
            get
            {
                //EnsureUserID();
                if (HttpContext.Current.Session["Country"] == null)
                    HttpContext.Current.Session["Country"] = "";
                return (string)HttpContext.Current.Session["Country"];
            }
            set
            {
                HttpContext.Current.Session["Country"] = value;
            }
        }

        public static UserModel EndUser
        {
            get
            {
                EnsureUserID();
                if (HttpContext.Current.Session["EndUser"] == null)
                    HttpContext.Current.Session["EndUser"] = "";
                return (UserModel)HttpContext.Current.Session["EndUser"];
            }
            set
            {
                HttpContext.Current.Session["EndUser"] = value;
            }
        }

    }
}