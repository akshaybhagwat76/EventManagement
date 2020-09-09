using System.Web.Mvc;

namespace MiidWeb.Navigation
{
    public class AuthorizeWithReturnCookieAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var ctx = filterContext.HttpContext;
            NavigationCookies.SetReturnAfterAuthenticationUrl(ctx, ctx.Request.RawUrl);
            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}