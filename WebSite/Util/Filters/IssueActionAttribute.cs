using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace WebSite.Util.Filters
{
    public class IssueActionAttribute : FilterAttribute, IActionFilter
    {
        
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var test = filterContext.RequestContext.HttpContext.Request.HttpMethod.ToString();
            
            Debug.WriteLine(test);
            var user = filterContext.RequestContext.HttpContext.User.Identity.GetUserId();
            Debug.WriteLine(user);
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }
    }
}