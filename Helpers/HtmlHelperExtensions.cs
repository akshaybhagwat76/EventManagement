using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiidWeb.Helpers
{
    public static class HtmlHelperExtensions
    {
        #region Resolve

        public static string Resolve(this HtmlHelper html, string path)
        {
            try
            {
                return VirtualPathUtility.ToAbsolute(path);
            }
            catch (Exception)
            {
                return "";
            }
        }

        #endregion
    }
}