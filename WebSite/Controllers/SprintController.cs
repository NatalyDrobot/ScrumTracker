using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BusinessLayer.Contracts.Interfaces;
using DTO.Entities;
using WebSite.Models.Issue;
using WebSite.Models.Sprint;

namespace WebSite.Controllers
{
    public enum stateSprint { New, Active, Closed }

    [Authorize]
    public class SprintController : BaseController
    {
        private readonly IManagerProject _projectManager;
        private readonly IManagerSprint _sprintManager;
        private readonly IManagerIssue _issueManager;

        public SprintController(IManagerProject projectManager, IManagerSprint sprintManager, IManagerIssue issueManager)
        {
            _projectManager = projectManager;
            _sprintManager = sprintManager;
            _issueManager = issueManager;
        }

        public ActionResult Index(Guid? id)
        {
            ViewBag.IsAdmin = HttpContext.User.IsInRole("Admin");
            var listProjects = ViewBag.IsAdmin ? _projectManager.GetProjectByDepartmentId(CurrentUser.Department.Id)
                                               : _projectManager.GetUsersProject(Guid.Parse(CurrentUserId));
            ViewBag.ListProjects = listProjects != null ? new SelectList(listProjects.ToArray(), "Id", "Name", id) : null;
            return View();
        }

        public JsonResult AccessByCreateSprint(Guid? id)
        {
            var access = false;
            if (!HttpContext.User.IsInRole("Admin"))
            {
                var team = _projectManager.GetProjectById(id).Team;
                if (team != null)
                {
                    if (team.UserTeamPositions.FirstOrDefault(
                        x => x.UserId == CurrentUserId && (x.Position.Name == "Project Manager"
                                                           || x.Position.Name == "Scrum Master")) != null)
                    {
                        access = true;
                    }
                }
            }
            else
            {
                access = true;
            }
            return Json(access, JsonRequestBehavior.AllowGet);
        }

        // GET: Sprint
        public ActionResult GetSprintsByProjectId(Guid? id)
        {
            if (id == null)
                return PartialView("_GetSprintsByProjectId", null);

            ViewBag.Access = AccessByCreateSprint(id).Data;
            IEnumerable<SprintDto> sprints = null;
            try
            {
                sprints = _sprintManager.GetSprintsByProjectId(id);
                sprints = sprints?.OrderByDescending(x => x.DateBegin);
            }
            catch (Exception e)
            {
                Response.Write(e.Message);
            }
            return sprints == null ? PartialView("_GetSprintsByProjectId", null)
                   : PartialView("_GetSprintsByProjectId", MappingDtoToModels(sprints));
        }

        public ActionResult GetIssuesBySprintId(Guid? sprintId)
        {
            var issues = _issueManager.GetIssuesBySprintId(sprintId).Where(x => x.ParentIssueId == null);
            issues = issues?.OrderBy(x => x.Number);
            if (issues.Count() != 0)
            {
                var issuesModel = MappingIssueDtoToModel(issues);
                return PartialView("_GetIssuesBySprintId", issuesModel);
            }
            return PartialView("_GetIssuesBySprintId", null);
        }

        public ActionResult MoveIssueInSprint(Guid? sprintId, Guid issueId)
        {
            var issue = _issueManager.GetIssueById(issueId);
            if (issue.SprintId != sprintId)
            {
                issue.SprintId = sprintId;
                if (issue.Children != null)
                {
                    foreach (var item in issue.Children)
                    {
                        item.SprintId = sprintId;
                        item.Sprint = sprintId != null ? _sprintManager.GetSprintById(sprintId) : null;
                    }
                    _issueManager.UpdateIssues(issue.Children);
                }
                _issueManager.UpdateIssue(issue);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MoveIssueInBoard(Guid issueId, string nameState)
        {
            var issue = _issueManager.GetIssueById(issueId);
            var state = _issueManager.GetStateByName(nameState);
            if (state != null)
            {
                issue.State = state;
                issue.StateId = state.Id;
                _issueManager.UpdateIssue(issue);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StartSprint(Guid id)
        {
            var errorActive = 0; // error activing sprint - when project has already contained active sprint
            var errorClosed = 0; // error closing sprint - when sprint contains not fixed issues
            var sprint = _sprintManager.GetSprintById(id);
            try
            {
                if (sprint.State == (int)stateSprint.New)
                {
                    var checkActiveSprint = _sprintManager.GetSprintsByProjectId(sprint.ProjectId)
                                            .FirstOrDefault(a => a.Id != id && a.State == (int)stateSprint.Active);
                    if (checkActiveSprint == null)
                    {
                        sprint.State = (int)stateSprint.Active;
                        _sprintManager.UpdateSprint(sprint);
                    }
                    else
                    {
                        errorActive = 1;
                    }
                }
                else
                {
                    var countIssues = _issueManager.GetIssuesBySprintId(id).Count(x => x.State.Name != "Fixed");
                    if (countIssues == 0)
                    {
                        sprint.State = (int)stateSprint.Closed;
                        _sprintManager.UpdateSprint(sprint);
                    }
                    else
                    {
                        errorClosed = 1;
                    }
                }
            }
            catch
            {
                // ignored
            }
            var list = new List<int> { sprint.State, errorActive, errorClosed };
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CountIssues(Guid sprintId)
        {
            var countIssues = new List<int>();
            var sprint = MappingDtoToModels(_sprintManager.GetSprintById(sprintId));
            countIssues.Add(sprint.CountAllIssues);
            countIssues.Add(sprint.CountTaskOpen);
            countIssues.Add(sprint.CountTaskInProgess);
            countIssues.Add(sprint.CountTaskFixed);
            countIssues.Add(sprint.CountTaskVerified);
            return Json(countIssues, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowPlanning(Guid sprintId, Guid projectId)
        {
            var backlogProject = _issueManager.GetAllIssuesByProjectId(projectId)
                                .Where(x => x.State.Name != "Fixed" && x.Sprint == null);
            var backlogSprint = _issueManager.GetIssuesBySprintId(sprintId);
            backlogProject = backlogProject?.OrderBy(x => x.Number);
            backlogSprint = backlogSprint?.OrderBy(x => x.Number);
            var planning = new SprintBacklogModel()
            {
                SprintId = sprintId,
                Backlog = MappingIssueDtoToModel(backlogProject),
                Sprint = MappingIssueDtoToModel(backlogSprint)
            };
            return PartialView(planning);
        }

        public ActionResult ShowAgileBoard(Guid sprintId, Guid issueId)
        {
            SprintBoardModel board;
            var issue = _issueManager.GetIssueById(issueId);
            if (issue.IssueType.Name == "Task" || issue.IssueType.Name == "Bug")
            {
                IList<IssueDto> list = new List<IssueDto>();
                list.Add(issue);
                board = new SprintBoardModel()
                {
                    SprintId = sprintId,
                    ParentIssueId = null,
                    IsTaskOrBug = true,
                    IssueOpen = MappingIssueDtoToModel(list.Where(x => x.State.Name == "Open")),
                    IssueReopened = MappingIssueDtoToModel(list.Where(x => x.State.Name == "Reopened")),
                    IssueInProgress = MappingIssueDtoToModel(list.Where(x => x.State.Name == "In Progress")),
                    IssueFixed = MappingIssueDtoToModel(list.Where(x => x.State.Name == "Fixed")),
                    IssueVerified = MappingIssueDtoToModel(list.Where(x => x.State.Name == "Verified"))
                };
            }
            else
            {
                var allIssues = _issueManager.GetIssuesBySprintId(sprintId).Where(x => x.ParentIssueId == issueId);
                var issueDtos = allIssues as IssueDto[] ?? allIssues.ToArray();
                board = new SprintBoardModel()
                {
                    SprintId = sprintId,
                    ParentIssueId = issueId,
                    IsTaskOrBug = false,
                    IssueOpen = MappingIssueDtoToModel(issueDtos.Where(x => x.State.Name == "Open").OrderBy(x => x.Number)),
                    IssueReopened = MappingIssueDtoToModel(issueDtos.Where(x => x.State.Name == "Reopened").OrderBy(x => x.Number)),
                    IssueInProgress = MappingIssueDtoToModel(issueDtos.Where(x => x.State.Name == "In Progress").OrderBy(x => x.Number)),
                    IssueFixed = MappingIssueDtoToModel(issueDtos.Where(x => x.State.Name == "Fixed").OrderBy(x => x.Number)),
                    IssueVerified = MappingIssueDtoToModel(issueDtos.Where(x => x.State.Name == "Verified").OrderBy(x => x.Number))
                };
            }
            return PartialView(board);
        }

        public ActionResult AddIssueInSprint(Guid sprintId, string typeName, Guid? parentIssueId)
        {
            var type = _issueManager.GetTypeByName(typeName);
            var model = new AddIssueModel
            {
                SprintId = sprintId,
                IssueTypeId = type.Id,
                ParentIssueId = parentIssueId
            };
            if (typeName != "Task")
            {
                //ViewBag.IssueTypeId = new SelectList(_issueManager.GetAllIssueTypes().Where(x => x.Name != "Task"), "Id", "Name", type.Id);
                ViewBag.IssueTypeId = new SelectList(_issueManager.GetAllIssueTypes(), "Id", "Name", type.Id);
                ViewBag.propertydisable = false;
            }
            else
            {
                ViewBag.IssueTypeId = new SelectList(_issueManager.GetAllIssueTypes().Where(x => x.Name == "Task"), "Id", "Name", type.Id);
                ViewBag.propertydisable = true;
            }
            ViewBag.TypeName = typeName;

            return PartialView("_AddIssueInSprint", model);
        }

        [HttpPost]
        public ActionResult AddIssueInSprint(AddIssueModel model)
        {
            var type = _issueManager.GetTypeById(model.IssueTypeId);
            var priority = _issueManager.GetPriorityByName("Normal");
            var state = _issueManager.GetStateByName("Open");
            var sprint = _sprintManager.GetSprintById(model.SprintId);
            var parent = _issueManager.GetIssueById(model.ParentIssueId);
            var issues = _issueManager.GetAllIssuesByProjectId(sprint.ProjectId).ToList();
            int maxNumber;
            if (issues.Count > 0)
            { maxNumber = issues.Max(x => x.Number) + 1; }
            else
            { maxNumber = 1; }
            var issue = new IssueDto()
            {
                Name = model.Name,
                Number = maxNumber,
                Description = model.Description,
                DateCreate = DateTime.Now,
                ProjectId = sprint.ProjectId,
                SprintId = model.SprintId,
                Sprint = sprint,
                StateId = state.Id,
                State = state,
                PriorityId = priority.Id,
                Priority = priority,
                IssueTypeId = type.Id,
                IssueType = type,
                ParentIssueId = model.ParentIssueId,
                ParentIssue = parent
            };
            try
            {
                _issueManager.AddIssue(issue);
                return Json(new { sprintId = model.SprintId, issueId = model.ParentIssueId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                // ignored
            }

            ViewBag.IssueTypeId = new SelectList(_issueManager.GetAllIssueTypes(), "Id", "Name", type.Id);
            return PartialView("_AddIssueInSprint", model);
        }

        public JsonResult CheckName(Guid? id, Guid projectId, string name)
        {
            bool res = true;
            var sprintTemp = _sprintManager.GetSprintByName(name);
            if (sprintTemp == null) return Json(res, JsonRequestBehavior.AllowGet);

            var sprint = sprintTemp.FirstOrDefault(s => s.ProjectId == projectId);
            if (sprint != null)
            {
                res = false;
                if (id != null) // if - edit project
                {
                    if (sprint.Id == id)
                    {
                        res = true;
                    }
                }
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckDate(DateTime dateBegin, Guid projectId, Guid? id, DateTime dateEnd)
        {
            bool res = !(dateBegin >= dateEnd);
            var lastDate = _sprintManager.GetSprintsByProjectId(projectId);
            if (lastDate == null) return Json(res, JsonRequestBehavior.AllowGet);

            var sprintDtos = lastDate as SprintDto[] ?? lastDate.ToArray();
            var sprints = sprintDtos.Where(b => b.Id != id);

            if (!sprints.Any()) return Json(res, JsonRequestBehavior.AllowGet);

            if (id != null) // edit
            {
                var sprintEdit = _sprintManager.GetSprintById(id);
                if (sprintEdit.DateBegin == dateBegin && sprintEdit.DateEnd == dateEnd)
                {
                    res = true;
                }
                else
                {
                    DateTime? maxDate = sprintDtos.Max(a => a.DateEnd);
                    if (dateBegin <= maxDate)
                    {
                        res = false; // error 
                    }
                }
            }
            else // create
            {
                DateTime? maxDate = sprintDtos.Max(a => a.DateEnd);
                if (dateBegin <= maxDate)
                {
                    res = false; // error 
                }
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        // GET: Sprint/Create        
        public ActionResult Create(Guid id)
        {
            DateTime maxDate;
            var lastDate = _sprintManager.GetSprintsByProjectId(id);
            if (lastDate != null)
            {
                var sprintDtos = lastDate as SprintDto[] ?? lastDate.ToArray();
                maxDate = sprintDtos.Max(a => a.DateEnd).AddDays(1);
            }
            else
            {
                maxDate = DateTime.Now;
            }

            var sprint = new SprintModel
            {
                ProjectId = id,
                NameProject = _projectManager.GetProjectById(id).Name,
                DateBegin = maxDate,
                DateEnd = maxDate.AddDays(14)
            };
            return PartialView("Create", sprint);
        }

        // POST: Sprint/Create
        [HttpPost]
        public ActionResult Create(SprintModel sprint)
        {
            //sprint.NameProject = _projectManager.GetProjectById(sprint.ProjectId).Name;
            try
            {
                if (ModelState.IsValid)
                {
                    var sprintDto = new SprintDto()
                    {
                        Name = sprint.Name,
                        Description = sprint.Description,
                        DateBegin = sprint.DateBegin,
                        DateEnd = sprint.DateEnd,
                        ProjectId = sprint.ProjectId,
                        State = 0 // new
                    };
                    _sprintManager.AddSprint(sprintDto);
                    return RedirectToAction("Index", new { id = sprint.ProjectId });
                }
            }
            catch
            {
                // ignored
            }
            return View(sprint);
        }

        // GET: Sprint/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sprint = _sprintManager.GetSprintById(id);
            return PartialView(MappingDtoToModels(sprint));
        }

        // POST: Sprint/Edit/5
        [HttpPost]
        public ActionResult Edit(SprintModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var sprint = _sprintManager.GetSprintById(model.Id);
                    if (sprint != null)
                    {
                        sprint.Name = model.Name;
                        sprint.Description = model.Description;
                        sprint.DateBegin = model.DateBegin;
                        sprint.DateEnd = model.DateEnd;
                        sprint.ProjectId = model.ProjectId;

                        _sprintManager.UpdateSprint(sprint);
                        return RedirectToAction("Index", new { id = model.ProjectId });
                    }
                }
                return PartialView(model);
            }
            catch
            {
                return PartialView(model);
            }
        }

        // GET: Sprint/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sprint = _sprintManager.GetSprintById(id);
            if (sprint == null)
            {
                return HttpNotFound();
            }
            return View(MappingDtoToModels(sprint));
        }

        // POST: Sprint/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                var sprint = _sprintManager.GetSprintById(id);
                _sprintManager.RemoveSprint(sprint);
                return RedirectToAction("Index", new { id = sprint.ProjectId });
            }
            catch
            {
                return View();
            }
        }

        private SprintModel MappingDtoToModels(SprintDto sprint)
        {
            var model = new SprintModel()
            {
                Id = sprint.Id,
                Name = sprint.Name,
                Description = sprint.Description,
                DateBegin = sprint.DateBegin,
                DateEnd = sprint.DateEnd,
                ProjectId = sprint.Project.Id,
                NameProject = sprint.Project.Name,
                State = sprint.State,
                StateName = Enum.GetName(typeof(stateSprint), sprint.State),
                CountAllIssues = _issueManager.GetIssuesBySprintId(sprint.Id).Count(),
                CountIssues = _issueManager.GetIssuesBySprintId(sprint.Id).Count(x => x.IssueType.Name == "Story"),
                CountTaskOpen = _issueManager.GetIssuesBySprintId(sprint.Id).Count(x => x.State.Name == "Open"),
                CountTaskInProgess = _issueManager.GetIssuesBySprintId(sprint.Id).Count(x => x.State.Name == "In Progress"),
                CountTaskFixed = _issueManager.GetIssuesBySprintId(sprint.Id).Count(x => x.State.Name == "Fixed"),
                CountTaskVerified = _issueManager.GetIssuesBySprintId(sprint.Id).Count(x => x.State.Name == "Verified")
            };
            return model;
        }

        private IEnumerable<SprintModel> MappingDtoToModels(IEnumerable<SprintDto> sprints)
        {
            var models = new List<SprintModel>();
            foreach (var sprint in sprints)
            {
                var model = MappingDtoToModels(sprint);
                models.Add(model);
            }
            return models;
        }

        public IssueInfoModel MappingIssueDtoToModel(IssueDto issue)
        {
            var model = new IssueInfoModel()
            {
                Id = issue.Id,
                Key = issue.Project.Code,
                Name = issue.Name,
                Number = issue.Number,
                Description = issue.Description,
                Priority = issue.Priority,
                Assignee = issue.Assignee,
                Estimate = issue.Estimate,
                State = issue.State,
                IssueType = issue.IssueType,
                Sprint = issue.Sprint,
                ParentIssue = issue.ParentIssue,
                Creator = issue.Creator,
                Project = issue.Project,
                DateCreate = issue.DateCreate
            };
            if (issue.Sprint != null)
            {
                model.CountIssues = _issueManager.CountIssuesInParentIssue(issue.Sprint.Id, issue.Id);
            }
            return model;
        }

        public IEnumerable<IssueInfoModel> MappingIssueDtoToModel(IEnumerable<IssueDto> issues)
        {
            var issueModels = new List<IssueInfoModel>();
            foreach (var issue in issues)
            {
                var model = MappingIssueDtoToModel(issue);
                issueModels.Add(model);
            }
            return issueModels;
        }

    }
}
