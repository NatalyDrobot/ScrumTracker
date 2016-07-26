using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLayer.Contracts.Interfaces;
using DTO.Entities;
using PagedList;
using WebSite.Models;
using WebSite.Models.Issue;
using WebSite.Util;
using WebSite.Util.Filters;

namespace WebSite.Controllers
{
    [Authorize]
    public class IssueController : BaseController
    {
        private readonly IManagerIssue _issueManager;
        private readonly IManagerProject _projectManager;
        private readonly IManagerTeam _teamManager;
        private readonly IManagerComment _commentManager;
        private readonly IssuesFilter _issueFilter;

        public IssueController(IManagerIssue issueManager, IManagerProject projectManager, IManagerTeam teamManager,IManagerComment commentManager)
        {
            _issueFilter = new IssuesFilter(CurrentUserId);
            _issueManager = issueManager;
            _projectManager = projectManager;
            _teamManager = teamManager;
            _commentManager = commentManager;
        }

        // GET: Issue
        public ActionResult Index(Guid? id, Guid? issueId, Guid? sprintId, string stateName)
        {
            var projects = _projectManager.GetProjectByDepartmentId(CurrentUser.Department.Id);
            var model = new IssueModel() { Issues = new List<IssueDto>().ToPagedList(1, 7) };
            if (projects == null) return View(model);
            var projectDtos = projects as IList<ProjectDto> ?? projects.ToList();
            Guid? projectId = id ?? projectDtos.First().Id;
            ViewBag.Projects = new SelectList(projectDtos, "Id", "Name", projectId);
            IEnumerable<IssueDto> issues;
            if (issueId == null)
            {

                issues =
                    _issueManager.GetAllIssues()
                        .Where(x => x.ProjectId == projectId)
                        .OrderByDescending(x => x.DateCreate);
                if (sprintId != null)
                {
                    issues = _issueFilter.Execute(issues, sprintId,stateName);
                }
            }
           
            else
            {
                var issue = _issueManager.GetIssueById(issueId);
                issues = new List<IssueDto> {issue};
                ViewBag.IssueFilter = issue.Name;
            }
            model.Issues = issues.ToPagedList(1,7);
            ViewBag.ProjectId = projectId;
            Permission(projectDtos.First());
            return View(model);
        }

        private void Permission(ProjectDto project)
        {
            ViewBag.Permission = false;
            UserTeamPositionDto userTeamPos = null;

            if (project.Team?.UserTeamPositions != null)
                userTeamPos = project.Team.UserTeamPositions.FirstOrDefault(x => x.UserId == CurrentUserId);
            if (HttpContext.User.IsInRole(Resources.Resource.Admin) || userTeamPos != null)
            {
                ViewBag.Permission = true;
            }
        }
        public ActionResult AddIssue(AddIssueModel model)
        {
            var test = HttpContext.Request.Form;
            OperationStatus operationStatus = new OperationStatus();
            var type = _issueManager.GetTypeByName("Task");
            var priority = _issueManager.GetPriorityByName("Normal");
            var state = _issueManager.GetStateByName("Open");
            var issues = _issueManager.GetAllIssuesByProjectId(model.ProjectId).ToList();
            int maxNumber;
            if(issues.Count > 0)
                maxNumber = issues.Max(x => x.Number)+1;
            else
            {
                maxNumber = 1;
            }
           IssueDto issue = new IssueDto()
            {
                Name = model.Name,
                Description = model.Description,
                DateCreate = DateTime.Now,
                ProjectId = model.ProjectId,
                StateId = state.Id,
                PriorityId = priority.Id,
                IssueTypeId = type.Id,
                Number = maxNumber
            };
            _issueManager.AddIssue(issue);
            operationStatus.Status = true;
            return Json(operationStatus, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddIssueInfo(string id)
        {
            AddIssueModel model = new AddIssueModel() { ProjectId = new Guid(id) };
            return PartialView("~/Views/Issue/_AddIssueModalPartial.cshtml", model);
        }

        public ActionResult DeleteIssueInfo(string id)
        {
            var issue = _issueManager.GetAllIssues().First(x => x.Id == new Guid(id));
            DeleteIssueModel model = new DeleteIssueModel() { Id = issue.Id };
            return PartialView("~/Views/Issue/_DeleteIssueModalPartial.cshtml", model);
        }

        public ActionResult DeleteIssue(DeleteIssueModel model)
        {
            OperationStatus operationStatus = new OperationStatus();
            var issue = _issueManager.GetAllIssues().First(x => x.Id == model.Id);
            if (issue != null)
            {
                _issueManager.RemoveIssue(issue);
                operationStatus.Status = true;
            }
            return Json(operationStatus, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchIssues(string term, int? page, string projectId, string sortFilter, Guid? issueId)
        {
            int pageSize = 7;
            int pageNumber = (page ?? 1);
            IEnumerable<IssueDto> issues;
            if (term != null)
                issues =
                    _issueManager.GetAllIssues()
                        .Where(a => a.ProjectId == new Guid(projectId) && a.Name.Contains(term))
                        .OrderByDescending(x => x.DateCreate);
            else
            {
                issues = _issueManager.GetAllIssues().Where(a => a.ProjectId == new Guid(projectId)).OrderByDescending(x => x.DateCreate);
            }
            if (!string.IsNullOrEmpty(sortFilter))
            {
               issues = _issueFilter.Execute(issues,null, sortFilter);
            }
            Permission(_projectManager.GetProjectById(new Guid(projectId)));
            ViewBag.IssueFilter = term;
            ViewBag.IssuePage = pageNumber;
            ViewBag.ProjectId = projectId;
            ViewBag.IssueSortFilter = sortFilter;
            IssueModel model = new IssueModel() { Issues = issues.ToPagedList(pageNumber,pageSize) };
            return PartialView("~/Views/Issue/_ListIssuesPartial.cshtml", model);
        }

        
        public ActionResult AutocompleteIssueSearch(string term, string projectId)
        {
            
            var model = _issueManager.GetAllIssues().Where(x => x.ProjectId == new Guid(projectId) && x.Name.Contains(term)).OrderBy(x => x.Name)
                .Select(x => new { value = x.Name })
                .Distinct();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IssueInfo(Guid? id)
        {
            ViewBag.IssuePriorities = new SelectList(_issueManager.GetAllIssuePriorities(), "Id", "Name");
            ViewBag.IssueTypes = new SelectList(_issueManager.GetAllIssueTypes(), "Id", "Name");

            ViewBag.IssueStates = new SelectList(_issueManager.GetAllIssueStates(), "Id", "Name");

            ViewBag.TimeTrackingTypes = new SelectList(_issueManager.GetAllTimeTrackingTypes(), "Id", "Name");

            var issue = _issueManager.GetIssueById(id);
            var projectId = issue.ProjectId;
            var teamId = _projectManager.GetProjectById(projectId).TeamId;
            var team = _teamManager.GetTeamById(teamId);
            IEnumerable<UserDto> usersAsignee = new List<UserDto>();
            if (team != null)
                usersAsignee = team.UserTeamPositions.Select(x => x.User);

            IssueInfoModel model = AutoMapperConfig.IssueDtoToIssueModelInfo(issue);
            ViewBag.IssueAsignee = model.AssigneeId != null
                ? new SelectList(usersAsignee, "Id", "Email", model.AssigneeId)
                : new SelectList(usersAsignee, "Id", "Email");
            ViewBag.IssuePriorities = model.PriorityId != null
                ? new SelectList(_issueManager.GetAllIssuePriorities(), "Id", "Name",
                    model.PriorityId)
                : new SelectList(_issueManager.GetAllIssuePriorities(), "Id", "Name");
            ViewBag.IssueTypes = model.IssueTypeId != null
                ? new SelectList(_issueManager.GetAllIssueTypes(), "Id", "Name", model.IssueTypeId)
                : new SelectList(_issueManager.GetAllIssueTypes(), "Id", "Name");
            ViewBag.IssueStates = model.StateId != null
                ? ViewBag.IssueStates = new SelectList(_issueManager.GetAllIssueStates(), "Id", "Name", model.StateId)
                : ViewBag.IssueStates = new SelectList(_issueManager.GetAllIssueStates(), "Id", "Name");

            var comments = _commentManager.GetAll().Where(c=>c.IssueId==issue.Id);
            model.CommentsList = comments;
            Permission(issue.Project);
            return PartialView("~/Views/Issue/_IssueInfoPartial.cshtml", model);
        }

        [HttpPost]
        public ActionResult CreateComment(string text, Guid issueId)
        {
            CommentViewModel model = new CommentViewModel{FirstName=CurrentUser.FirstName,LastName=CurrentUser.LastName,Comment = text, Date= DateTime.Now,IssueId= issueId,Avatar = CurrentUser.Avatar};
            
            CommentDto comment = new CommentDto
            {
                DateCreate = model.Date,
                Text = model.Comment,
                AuthorId = CurrentUserId,
                IssueId = model.IssueId
            };
            _commentManager.Add(comment);
            return PartialView("_SingleComment", model);
        }

        public ActionResult EditIssue(IssueInfoModel model)
        {
            
           OperationStatus operationStatus = new OperationStatus();
            if (ModelState.IsValid)
            {
                var issue = AutoMapperConfig.IssuModelInfoToIssueDto(model);
                _issueManager.UpdateIssue(issue);
                operationStatus.Status = true;
            }
            else
            {
                operationStatus.Status = false;
                operationStatus.Message = "Something wrong";

            }
            return Json(operationStatus, JsonRequestBehavior.AllowGet);
        }

        

        [IssueAction]
        public ActionResult AddParentIssueInfo(string issueId, string projectId)
        {
            var listIssue = _issueManager.GetAllIssuesByProjectId(new Guid(projectId));
            var issue = _issueManager.GetIssueById(new Guid(issueId));
            var children = Recursive(issue);
            var res = listIssue.Except(children).ToList();
            res.Remove(issue);
            return PartialView("~/Views/Issue/_SelectRelativeIssuePartial.cshtml", res);
        }
        [IssueAction]
        public ActionResult AddChildIssueInfo(string issueId, string projectId)
        {
            var listIssue = _issueManager.GetAllIssuesByProjectId(new Guid(projectId)).Where(x => x.ParentIssue == null).ToList();
            var issue = _issueManager.GetIssueById(new Guid(issueId));
            if (issue.ParentIssue != null && issue.ParentIssue.ParentIssue == null)
                listIssue.Remove(issue.ParentIssue);
            listIssue.Remove(issue);
            return PartialView("~/Views/Issue/_SelectRelativeIssuePartial.cshtml", listIssue);
        }

        public ActionResult AddParentIssue(string parentId, string childId)
        {
            OperationStatus status = new OperationStatus();
            var issue = _issueManager.GetIssueById(new Guid(childId));
            if (issue != null)
            {
                issue.ParentIssueId = new Guid(parentId);
                _issueManager.UpdateIssue(issue);
                status.Status = true;
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteParentIssue(string issueId)
        {
            OperationStatus status = new OperationStatus();
            var issue = _issueManager.GetIssueById(new Guid(issueId));
            if (issue != null)
            {
                issue.ParentIssueId = null;
                _issueManager.UpdateIssue(issue);
                status.Status = true;
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<IssueDto> Recursive(IssueDto issue)
        {
            List<IssueDto> inner = new List<IssueDto>();
            foreach (var t in issue.Children)
            {
                inner.Add(t);
                inner = inner.Union(Recursive(t)).ToList();
            }
            return inner;
        }

        public ActionResult AddTimeTracking(string issueId, string comment, int estimate, string timeTrackingTypeId)
        {
            //var timeTrackingType = timeTrackingTypeId != null ? (Guid?) new Guid(timeTrackingTypeId) : null;
            TimeTrackingDto timeTracking = new TimeTrackingDto()
            {
                UserId = CurrentUserId,
                IssueId = new Guid(issueId),
                Description = comment,
                SpentTime = estimate,
                TimeTrackingTypeId = timeTrackingTypeId != "" ? (Guid?)new Guid(timeTrackingTypeId) : null,
                DateCreate = DateTime.Now
            };
            ViewBag.TimeTrackingTypes = new SelectList(_issueManager.GetAllTimeTrackingTypes(), "Id", "Name");

            _issueManager.AddTimeTracking(timeTracking);
            var issue = _issueManager.GetIssueById(new Guid(issueId));
            Permission(issue.Project);
            var model = _issueManager.GetTimeTrackingById(timeTracking.Id);
            return PartialView("~/Views/Issue/_SingleTimeTracking.cshtml", model);
        }
        public ActionResult DeleteTimeTracking(string timeTrackingId)
        {
            OperationStatus operationStatus = new OperationStatus();
            var timeTracking = _issueManager.GetTimeTrackingById(new Guid(timeTrackingId));
            if (timeTracking != null)
            {
                _issueManager.RemoveTimeTracking(timeTracking);
                operationStatus.Status = true;
            }
            return Json(operationStatus, JsonRequestBehavior.AllowGet);
        }
    }
}
