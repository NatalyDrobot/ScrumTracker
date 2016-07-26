using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLayer;
using Microsoft.AspNet.Identity;

namespace WebSite.Util.Filters
{
    public class ProjectActionAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.IsAdmin = false;
            filterContext.Controller.ViewBag.ListTeamsPM = null;
            filterContext.Controller.ViewBag.AccessProject = false;
            var isAdmin = filterContext.RequestContext.HttpContext.User.IsInRole("Admin");
            if (!isAdmin)
            {
                var userTeamPosManager = new ManagerUserTeamPos();
                var projectManager = new ManagerProject();
                var userManager = new ManagerUser();

                var currentUserId = filterContext.RequestContext.HttpContext.User.Identity.GetUserId();
                var currentUser = userManager.GetUserById(currentUserId);
                var position = userTeamPosManager.GetAllUserTeamPos()
                    .Where(x => x.UserId == currentUserId && (x.Position.Name == "Project Manager" || x.Position.Name == "Scrum Master"));
                var teams = position.Select(x => x.Team).ToList();
                var listTeamsId = position.Select(x => x.TeamId).ToList();
                if (teams.Count() != 0)
                {
                    filterContext.Controller.ViewBag.ListTeamsPM = teams;
                    var listProjectInDep = projectManager.GetProjectByDepartmentId(currentUser.DepartmentId);
                    List<Guid> access = new List<Guid>();
                    foreach (var item in listProjectInDep.Where(item => item.TeamId != null))
                    {
                        if (listTeamsId.Exists(x => x == item.TeamId)) { access.Add(item.Id);}
                    }
                    filterContext.Controller.ViewBag.AccessProject = access;
                }

                filterContext.Controller.ViewBag.listUsersProjects = projectManager.GetUsersProject(Guid.Parse(currentUserId));
            }
            else
            {
                filterContext.Controller.ViewBag.IsAdmin = true;
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}