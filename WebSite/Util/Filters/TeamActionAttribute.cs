using System;
using System.Web;
using System.Web.Mvc;
using BusinessLayer;
using Microsoft.AspNet.Identity;

namespace WebSite.Util.Filters
{
    public class TeamActionAttribute : FilterAttribute, IActionFilter
    {
        public bool Access { get; set; }
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var test = filterContext.RequestContext.HttpContext.Request.QueryString;
            string id = "";
            if (!test.HasKeys())
            {
                id = filterContext.RequestContext.HttpContext.Request.Form["teamId"];
                if (filterContext.RequestContext.HttpContext.Request.RequestContext.RouteData.Values.ContainsKey("id"))
                {
                    id = filterContext.RequestContext.HttpContext.Request.RequestContext.RouteData.Values["id"].ToString();

                }
            }
            else
            {
                id = test["id"] ?? test["teamId"];

            }
            var currentUserId = filterContext.RequestContext.HttpContext.User.Identity.GetUserId();
            var userTeamPosManager = new ManagerUserTeamPos();
            var userTeamPos = userTeamPosManager.GetUsersTeamPosByTeamIdAndUserId(new Guid(id), currentUserId);
            var positionName = userTeamPos?.Position?.Name;
            var isAdmin = filterContext.RequestContext.HttpContext.User.IsInRole("Admin");
            if (isAdmin || positionName == "Project Manager")
            {
                filterContext.Controller.ViewBag.UserPermission = true;
                filterContext.Controller.ViewBag.TeamPmId = currentUserId;
                filterContext.Controller.ViewBag.IsUserAdmin = isAdmin;
            }
            else
            {
                filterContext.Controller.ViewBag.UserPermission = false;
                filterContext.Controller.ViewBag.TeamPmId = null;
                filterContext.Controller.ViewBag.IsUserAdmin = false;
                if (Access)
                    throw new HttpException(404, "Permission");
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}