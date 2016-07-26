using System.Web.Mvc;
using BusinessLayer.Contracts.Interfaces;
using WebSite.Models;
using System;
using System.Collections.Generic;
using WebSite.Util.Filters;
using System.Runtime.InteropServices;
using System.Linq;

namespace WebSite.Controllers
{
    [RequireHttps]
    [Authorize]
    
    public class HomeController : BaseController
    {
        private readonly IManagerProject mngrProject;
        private readonly IManagerTeam mngrTeam;
        private readonly IManagerSprint mngrSprint;
        private readonly IManagerIssue mngrIssue;

        public HomeController(IManagerProject projectManager, IManagerTeam teamManager, IManagerSprint sprintManager, IManagerIssue issueManager)
        {
            mngrProject = projectManager;
            mngrTeam = teamManager;
            mngrSprint = sprintManager;
            mngrIssue = issueManager;
        }

        public ActionResult Index()
        {

            List<DashBoardView> vm = new List<DashBoardView>();

            var teamsList = mngrTeam.GetUsersTeams(CurrentUserId);
      
            var projectList = mngrProject.GetUsersProject(Guid.Parse(CurrentUserId)).Where(x=>x.StateProject.Name == "Open");

            if (projectList != null)
            {
                foreach (var project in projectList)
                {                   

                    var sprints = mngrSprint.GetSprintsByProjectId(project.Id);

                    if (sprints != null)
                    {
                        foreach (var sprint in sprints.Where(x => x.State == 1))
                        {
                            DashBoardView item = new DashBoardView();
                            item.ProjectName = project.Name;
                            item.ProjectId = project.Id;
                            item.SprintId = sprint.Id;
                            item.SprintName = sprint.Name;
                            item.EndDate = sprint.DateEnd;
                            item.IssueList = mngrIssue.GetAllIssuesByProjectId(project.Id)
                                                .Where(x => x.IssueType.Name.Equals("Task"));
                                               // .OrderBy(x=>x.);                            

                            vm.Add(item);
                        }
                    }
                }
            }

            return View(vm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GetChartInfo(Guid sprintId)
        {
            var issuesList = mngrIssue.GetIssuesBySprintId(sprintId);
            var sprintInfo = mngrSprint.GetSprintById(sprintId);

            List<string> timeArr = new List<string>();
            List<double> idealSprintLine = new List<double>();
            List<int> actualLine = new List<int>();
            int totalIssues = sprintInfo.Issues.Count;

            DateTime sprintStartAt = sprintInfo.DateBegin;

           

            while(sprintStartAt <= sprintInfo.DateEnd)
            {
                timeArr.Add(sprintStartAt.ToShortDateString());
                sprintStartAt = sprintStartAt.AddDays(1);

                // stop adding actual effort from the future
                if (sprintStartAt <= DateTime.Now)
                {
                    var leftItems = totalIssues - issuesList.Where(x => x.State.Name.Equals("Fixed") && x.LastUpdate.Day == sprintStartAt.Day).Count();
                    totalIssues = leftItems;
                    actualLine.Add(leftItems);
                }
            }

            double step = (double)sprintInfo.Issues.Count / timeArr.Count;

            for(double i = sprintInfo.Issues.Count; i > step; i -= step)
            {
                idealSprintLine.Add(i);
            }
                        

            BurnDownChartView vm = new BurnDownChartView();
            vm.SprintDates = timeArr;
            vm.IdealTasks = idealSprintLine;
            vm.ActualTasks = actualLine;
                            

            // .OrderBy(x=>x.LastUpdate)
            // .Select(x=>x.LastUpdate);

            return new JsonResult()
            {
                Data = vm
            };

        }

    }
}