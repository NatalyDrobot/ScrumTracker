using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using BusinessLayer;
using BusinessLayer.Contracts.Interfaces;
using Microsoft.AspNet.Identity;

namespace WebSite.Util.Filters
{
    public class CultureAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            IManagerUser userManager = new ManagerUser();
            var userId = filterContext.HttpContext.User.Identity.GetUserId();
            var cultureName = userId != null ? userManager.GetUserById(userId).Culture : "en";
            //Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureName);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);
        }
    }
}