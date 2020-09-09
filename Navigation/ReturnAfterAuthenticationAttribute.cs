using System.Web.Mvc;

namespace MiidWeb.Navigation
{
    /// <summary>
    /// After succesfully authenticating or registering, the system redirects the user to the last action flagged with this attribute.
    /// </summary>
    /// <remarks>
    /// This attribute is ignored if the user was already authenticated. 
    /// Currently this is tracked in a cookie.
    /// Only GET requests are tracked.
    /// Uses <see cref="NavigationCookies.SetReturnAfterAuthenticationUrl"/>, so the user
    /// is returned to the last action that sets it. 
    /// An explicit return url takes precendenc over the cookie, which is usually the case when trying
    /// to access an action that requires authorization.
    /// </remarks>
    public class ReturnAfterAuthenticationAttribute : ActionFilterAttribute
    {
        public const string ViewDataIgnoreKey = "IgnoreReturnAfterAuthentication";
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var ctx = filterContext.HttpContext;
            if ((bool?)filterContext.Controller.ViewData[ViewDataIgnoreKey] == true) return;
            if (ctx.User.Identity.IsAuthenticated) return;
            if (filterContext.HttpContext.Request.HttpMethod != "GET") return;
            NavigationCookies.SetReturnAfterAuthenticationUrl(ctx, ctx.Request.RawUrl);
        }
    }
}