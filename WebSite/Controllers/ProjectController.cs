using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BusinessLayer.Contracts.Interfaces;
using DTO.Entities;
using Resources;
using WebSite.Models.Project;
using WebSite.Util.Filters;

namespace WebSite.Controllers
{
    [Authorize]
    public class ProjectController : BaseController
    {
        private readonly IManagerIssue _issueManager;
        private readonly IManagerProject _projectManager;
        private readonly IManagerTeam _teamManager;
        private readonly IManagerSprint _sprintManager;
        private readonly IManagerUserTeamPos _userTeamPosManager;

        public ProjectController(IManagerProject projectManager, IManagerTeam teamManager, IManagerSprint sprintManager,
            IManagerIssue issueManager, IManagerUserTeamPos userTeamPosManager)
        {
            _projectManager = projectManager;
            _teamManager = teamManager;
            _sprintManager = sprintManager;
            _issueManager = issueManager;
            _userTeamPosManager = userTeamPosManager;
        }

        // GET: Projects
        [ProjectAction]
        public ActionResult Index()
        {
            ViewBag.Message = Session["MessageForTeam"];
            Session["MessageForTeam"] = null;
            var isAdmin = HttpContext.User.IsInRole("Admin");
            var projects = isAdmin ? _projectManager.GetProjectByDepartmentId(CurrentUser.Department.Id)
                                   : _projectManager.GetUsersProject(Guid.Parse(CurrentUserId));
            return View(MappingDtoToModels(projects));
        }

        // GET: Projects with filter
        [ProjectAction]
        public ActionResult FilterProjectsByName(string id)
        {
            var isAdmin = HttpContext.User.IsInRole("Admin");
            var projects = isAdmin
                ? _projectManager.SearchProjectByName(id).Where(x => x.Department.Id == CurrentUser.DepartmentId)
                : _projectManager.GetUsersProject(Guid.Parse(CurrentUserId)).Where(x => x.Name.Contains(id));
            return projects == null ? View("Index", null) : View("Index", MappingDtoToModels(projects));
        }

        public ActionResult AutocompleteProjectName(string term)
        {
            var isAdmin = HttpContext.User.IsInRole("Admin");
            var projects = isAdmin ? _projectManager.SearchProjectByName(term).Where(x => x.Department.Id == CurrentUser.DepartmentId).Select(a => new { value = a.Name }).Distinct()
                                   : _projectManager.GetUsersProject(Guid.Parse(CurrentUserId)).Where(x => x.Name.Contains(term)).Select(a => new { value = a.Name }).Distinct();
            return Json(projects, JsonRequestBehavior.AllowGet);
        }

        // GET: Projects with filter
        [ProjectAction]
        public ActionResult GetProjectsByTeamId(Guid? id)
        {
            var projects = _projectManager.GetProjectsByTeamId(id);
            return projects == null ? View("Index", null) : View("Index", MappingDtoToModels(projects));
        }

        public JsonResult CheckName(Guid? id, string name)
        {
            var res = true;
            var projectTemp = _projectManager.GetProjectByName(name);
            if (projectTemp != null)
            {
                res = false;
                var projectExist = projectTemp.FirstOrDefault();
                if (id != null)  // if - edit project
                {
                    if (projectExist.Id == id)
                    {
                        res = true;
                    }
                }
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            ViewBag.StateProjectsId = new SelectList(_projectManager.GetStateProject(), "Id", "Name");
            var isAdmin = HttpContext.User.IsInRole("Admin");
            var teams = isAdmin ? _teamManager.GetAllTeamsByDepartment(CurrentUser.DepartmentId)
                                : _teamManager.GetUsersTeams(CurrentUserId);
            var teamDtos = teams as TeamDto[] ?? teams.ToArray();
            ViewBag.TeamId = !teamDtos.Any() ? new SelectList(teamDtos, "Id", "Name") 
                                : new SelectList(teamDtos, "Id", "Name", teamDtos.First().Id);
            return View();
        }

        // POST: Projects/Create
        [HttpPost]
        public ActionResult Create(ProjectModel project)
        {
            if (ModelState.IsValid ) 
            {
                try
                {
                    var state = _projectManager.GetStateProjectById(project.StateProjectsId);
                    var team = _teamManager.GetTeamById(project.TeamId);
                    project.DateCreate = DateTime.Today;
                    var projectDto = new ProjectDto()
                    {
                        Name = project.Name,
                        Description = project.Description,
                        Code = project.Code,
                        DateCreate = project.DateCreate,
                        StateProject = state,
                        Team = team,
                        DepartmentId = CurrentUser.Department.Id
                    };
                    _projectManager.AddProject(projectDto);
                    return RedirectToAction("Index");
                }
                catch 
                {
                    // ignored
                }
            }
            ViewBag.StateProjectsId = new SelectList(_projectManager.GetStateProject(), "Id", "Name", project.StateProjectsId);
            var isAdmin = HttpContext.User.IsInRole("Admin");
            var teams = isAdmin ? _teamManager.GetAllTeamsByDepartment(CurrentUser.DepartmentId)
                                : _teamManager.GetUsersTeams(CurrentUserId);
            var teamDtos = teams as TeamDto[] ?? teams.ToArray();
            ViewBag.TeamId = !teamDtos.Any() ? new SelectList(teamDtos, "Id", "Name")
                                : new SelectList(teamDtos, "Id", "Name", project.TeamId);
            return PartialView(project);
        }

        // GET: Projects/Edit/5
        [ProjectAction]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var project = _projectManager.GetProjectById(id);
            var model = MappingDtoToModels(project);
            var isAdmin = HttpContext.User.IsInRole("Admin");
            var teams = isAdmin ? _teamManager.GetAllTeamsByDepartment(CurrentUser.DepartmentId)
                                : _teamManager.GetUsersTeams(CurrentUserId);
            var teamDtos = teams as TeamDto[] ?? teams.ToArray();

            if (project.Team == null)
            {
                ViewBag.TeamId = new SelectList(teamDtos, "Id", "Name");
            }
            else
            {
                model.TeamId = project.Team.Id;
                ViewBag.TeamId = new SelectList(teamDtos, "Id", "Name", model.TeamId);
            }

            ViewBag.StateProjectsId = new SelectList(_projectManager.GetStateProject(), "Id", "Name", model.StateProjectsId);
            return PartialView(model);
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ProjectAction]
        public ActionResult Edit(ProjectModel model)
        {
            if (ModelState.IsValid )
            {
                var project = _projectManager.GetProjectById(model.Id);

                if (project != null)
                {
                    project.Name = model.Name;
                    project.Code = model.Code;
                    project.Description = model.Description;
                    project.StateProject = _projectManager.GetStateProjectById(model.StateProjectsId);
                    project.Team = _teamManager.GetTeamById(model.TeamId);

                    _projectManager.UpdateProject(project);
                    return RedirectToAction("Index");
                }
            }
            ViewBag.StateProjectsId = new SelectList(_projectManager.GetStateProject(), "Id", "Name", model.StateProjectsId);
            var isAdmin = HttpContext.User.IsInRole("Admin");
            var teams = isAdmin ? _teamManager.GetAllTeamsByDepartment(CurrentUser.DepartmentId)
                                : _teamManager.GetUsersTeams(CurrentUserId);
            var teamDtos = teams as TeamDto[] ?? teams.ToArray();
            ViewBag.TeamId = !teamDtos.Any() ? new SelectList(teamDtos, "Id", "Name")
                                : new SelectList(teamDtos, "Id", "Name", model.TeamId);
            return View(model);
        }

        // GET: Projects/Delete/5
        [ProjectAction]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var project = _projectManager.GetProjectById(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(MappingDtoToModels(project));
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ProjectAction]
        public ActionResult DeleteConfirmed(Guid id)
        {
            try
            {
                var project = _projectManager.GetProjectById(id);
                _projectManager.RemoveProject(project);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private ProjectModel MappingDtoToModels(ProjectDto project)
        {
            if (project == null) return null;

            var model = new ProjectModel()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Code = project.Code,
                DateCreate = project.DateCreate,
                StateProjectsId = project.StateProject.Id,
                NameStateProject = project.StateProject.Name,
                CountSprints = _sprintManager.CountSprintsByProjectId(project.Id),
                CountIssues = _issueManager.CountIssuesByProjectId(project.Id),
                DepartmentId = project.DepartmentId,
            };
            if (project.Team != null)
            {
                model.TeamId = project.Team.Id;
                model.NameTeamProject = project.Team.Name;

                var members = _userTeamPosManager.GetUsersTeamPosByTeamId(project.Team.Id);
                if (members != null)
                {
                    model.CountMembers = _userTeamPosManager.GetUsersTeamPosByTeamId(project.Team.Id).Count();
                }
            }
            else
            {
                model.TeamId = null;
            }
            return model;
        }

        private IEnumerable<ProjectModel> MappingDtoToModels(IEnumerable<ProjectDto> projects)
        {
            return projects?.Select(project => MappingDtoToModels(project)).ToList();
        }
    }
}
