using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MiidWeb.Models;

namespace MiidWeb.Helpers
{
    public static class Extensions
    {
        public static EndUser GetSessionEndUser(this HttpContext current)
        {
            return (EndUser)(current != null ? current.Session["__EndUser"] : null);
        }

        public static void SetSessionRegisteredUser(this HttpContext current, EndUser EndUser)
        {
            current.Session.Add("__EndUser", EndUser);
        }
    }
}