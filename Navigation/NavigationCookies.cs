using System;
using System.Web;

namespace MiidWeb.Navigation
{
    public class NavigationCookies
    {
        private const string ReturnAfterAuthenticationKey = "ReturnAfterAuthentication";
        public static void SetReturnAfterAuthenticationUrl(HttpContextBase context, string url)
        {
            url = VirtualPathUtility.ToAbsolute(url, context.Request.ApplicationPath);
            var cookie = context.Response.Cookies[ReturnAfterAuthenticationKey];
            cookie.Expires = DateTime.Now.AddDays(1);
            cookie.Value = url;
        }
        public static string GetReturnAfterAuthenticationUrl(HttpRequestBase request)
        {
            var cookie = request.Cookies["ReturnAfterAuthentication"];
            if (cookie == null || cookie.Expires > DateTime.Now) return String.Empty;
            return cookie.Value;
        }
    }
}