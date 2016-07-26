using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLayer.Contracts.Interfaces;
using DTO.Entities;
using PagedList;
using WebSite.Models;
using WebSite.Models.Team;
using WebSite.Util.Filters;

namespace WebSite.Controllers
{
    [Authorize]
    public class TeamController : BaseController
    {
        private readonly IManagerUser _userManager;
        private readonly IManagerTeam _teamManager;
        private readonly IManagerUserTeamPos _userTeamPosManager;
        private readonly IManagerPosition _positionManager;
        private readonly IManagerProject _projectManager;

        public TeamController(IManagerUser userManager, IManagerTeam teamManager, IManagerUserTeamPos userTeamPosManager,
            IManagerPosition positionManager, IManagerProject projectManager)
        {
            _positionManager = positionManager;
            _userManager = userManager;
            _teamManager = teamManager;
            _userTeamPosManager = userTeamPosManager;
            _projectManager = projectManager;
        }
        
        public ActionResult Index(Guid? id)
        {
            ViewBag.ProjectId = id;  // for assign team to project
           // var users = _userManager.GetAllUsersByDepartment(CurrentUser.DepartmentId).OrderBy(x => x.Email).ToPagedList(1, 6);
            var teams = _teamManager.GetAllTeamsByDepartment(CurrentUser.DepartmentId).OrderByDescending(x => x.DateCreated).ToPagedList(1, 7);
            var userTeamPositions = new List<UserTeamPositionDto>().ToPagedList(1, 6);
            TeamModel model = new TeamModel() {Users = new List<UserDto>().ToPagedList(1,1),Teams = teams, UserTeamPositions = userTeamPositions };
            ViewBag.IsUserAdmin = HttpContext.User.IsInRole("Admin");
            ViewBag.UserPermission = false;
            return View(model);
        }
        [TeamAction]
        public ActionResult ShowTeam(Guid? id)
        {
            //var users = _userManager.GetAllUsersByDepartment(CurrentUser.DepartmentId).OrderBy(x => x.Email).ToPagedList(1, 6);
            var teams = _teamManager.GetAllTeamsByDepartment(CurrentUser.DepartmentId).Where(x => x.Id == id).OrderByDescending(x => x.DateCreated).ToPagedList(1, 7);
            var userTeamPositions = _userTeamPosManager.GetAllUserTeamPos().Where(x => x.TeamId == id).OrderBy(x => x.User.Email).ToPagedList(1, 6);
            TeamModel model = new TeamModel() { Users = new List<UserDto>().ToPagedList(1, 1), Teams = teams, UserTeamPositions = userTeamPositions };
            ViewBag.Positions = new SelectList(_positionManager.GetAllPositions(), "Id", "Name");
            //ViewBag.AccessToEditTeam = HttpContext.User.IsInRole("Admin"); 
            //ViewBag.IsUserAdmin = HttpContext.User.IsInRole("Admin");

            //var userTeamPos = _userTeamPosManager.GetUsersTeamPosByTeamIdAndUserId(id, currentUserId);
            //var positionName = userTeamPos?.Position?.Name;
            return View("Index", model);
        }

        [TeamAction]
        public ActionResult GetUserTeamPostions(string id)
        {
            var userTeamPositions = _userTeamPosManager.GetUsersTeamPosByTeamId(new Guid(id)).OrderBy(x => x.User.Email).ToPagedList(1, 6);
            TeamModel model = new TeamModel() { UserTeamPositions = userTeamPositions };
            ViewBag.Positions = new SelectList(_positionManager.GetAllPositions(), "Id", "Name");
            return PartialView("~/Views/Team/_ListUsersTeamPosPartial.cshtml", model);
        }
        [TeamAction(Access = true)]

        public ActionResult AddUserToTeam(string userId, string teamId)
        {
            OperationStatus operationStatus = new OperationStatus();
            var userTeamPosition = _userTeamPosManager.GetUsersTeamPosByTeamIdAndUserId(new Guid(teamId),userId);
            if (userTeamPosition == null)
            {
                userTeamPosition = new UserTeamPositionDto() { UserId = userId, TeamId = new Guid(teamId) };
                _userTeamPosManager.AddUserTeamPos(userTeamPosition);
                operationStatus.Status = true;
                if (userTeamPosition.TeamId != null) operationStatus.InsertedId = (Guid)userTeamPosition.TeamId;
            }
            return Json(operationStatus, JsonRequestBehavior.AllowGet);

        }

        public ActionResult AutocompleteTeamSearch(string term)
        {
            var model = _teamManager.GetAllTeamsByDepartment(CurrentUser.DepartmentId).Where(x => x.Name.Contains(term)).OrderByDescending(x => x.DateCreated)
                .Select(x => new { value = x.Name })
                .Distinct();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTeams(string term, int? page)
        {
            int pageSize = 7;
            int pageNumber = (page ?? 1);
            var teams = _teamManager.GetAllTeamsByDepartment(CurrentUser.DepartmentId).OrderByDescending(x => x.DateCreated).ToPagedList(pageNumber, pageSize);

            TeamModel model = new TeamModel() { Teams = teams };
            return PartialView("~/Views/Team/_ListTeamsPartial.cshtml", model);
        }
        public ActionResult SearchTeams(string term, int? page)
        {
            int pageSize = 7;
            int pageNumber = (page ?? 1);
            IPagedList<TeamDto> teams;
            if (term != null)
                teams = _teamManager.GetAllTeamsByDepartment(CurrentUser.DepartmentId).Where(a => a.Name.Contains(term)).OrderByDescending(x => x.DateCreated).ToPagedList(pageNumber, pageSize);
            else
            {
                teams = _teamManager.GetAllTeamsByDepartment(CurrentUser.DepartmentId).OrderByDescending(x => x.DateCreated).ToPagedList(pageNumber, pageSize);
            }
            ViewBag.IsUserAdmin = HttpContext.User.IsInRole("Admin");
            ViewBag.TeamFilter = term;
            ViewBag.TeamPage = pageNumber;

            TeamModel model = new TeamModel() { Teams = teams };
            return PartialView("~/Views/Team/_ListTeamsPartial.cshtml", model);
        }

        
        public ActionResult AutocompleteUserSearch(string term)
        {
            var model = _userManager.GetAllUsersByDepartment(CurrentUser.DepartmentId).Where(x => x.Email.Contains(term)).OrderBy(x => x.Email)
                .Select(x => new { value = x.Email })
                .Distinct();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [TeamAction]
        public ActionResult SearchUsers(string term, string teamId, int? page)
        {
            var teamUsers = new List<UserDto>();
            if (teamId != null)
            {
                teamUsers = _teamManager.GetTeamById(new Guid(teamId)).UserTeamPositions.Select(x => x.User).ToList();
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var availableUsers  = _userManager.GetAllUsersByDepartment(CurrentUser.DepartmentId).Except(teamUsers);
            IPagedList<UserDto> users;
            if (term != null)
                users = availableUsers.Where(a => a.Email.Contains(term)).OrderBy(x => x.Email).ToPagedList(pageNumber, pageSize);
            else
            {
                users = availableUsers.OrderBy(x => x.Email).ToPagedList(pageNumber, pageSize);
            }
            ViewBag.UserFilter = term;
            ViewBag.UserPage = pageNumber;
            ViewBag.TeamIdFilter = teamId;
            TeamModel model = new TeamModel() { Users = users };
            
            return PartialView("~/Views/Team/_ListUsersPartial.cshtml", model);
        }
        
        public ActionResult EditTeamInfo(string id)
        {
            var team = _teamManager.GetTeamById(new Guid(id));

            EditTeamModel model = new EditTeamModel() { Id = team.Id, Name = team.Name, Description = team.Description };
            return PartialView("~/Views/Team/_EditTeamModalPartial.cshtml", model);
        }
        
        public ActionResult EditTeam(EditTeamModel model)
        {
            OperationStatus operationStatus = new OperationStatus();
            if (ModelState.IsValid)
            {
                var team = _teamManager.GetTeamById(model.Id);
                team.Name = model.Name;
                team.Description = model.Description;
                _teamManager.UpdateTeam(team);
                operationStatus.Status = true;
            }
            else
            {
                operationStatus.Status = false;
                operationStatus.Message = "Something wrong";
            }
            return Json(operationStatus, JsonRequestBehavior.AllowGet);
        }
        [TeamAction]
        public ActionResult DeleteTeamInfo(string id)
        {
            var team = _teamManager.GetAllTeamsByDepartment(CurrentUser.DepartmentId).First(x => x.Id == new Guid(id));
            DeleteTeamModel model = new DeleteTeamModel() { Id = team.Id };
            return PartialView("~/Views/Team/_DeleteTeamModalPartial.cshtml", model);
        }

        public ActionResult DeleteTeam(DeleteTeamModel model)
        {
            OperationStatus operationStatus = new OperationStatus();
            var team = _teamManager.GetTeamById(model.Id);
            if (team != null)
            {
                _teamManager.RemoveTeam(team);
                operationStatus.Status = true;
            }
            return Json(operationStatus, JsonRequestBehavior.AllowGet);
        }
        [TeamAction]
        public ActionResult DeleteUserTeamPosInfo(string userId, string teamId)
        {
            var userTeamPos = _userTeamPosManager.GetUsersTeamPosByTeamIdAndUserId(new Guid(teamId),userId);
            DeleteUserTeamPosModel model = new DeleteUserTeamPosModel() { UserId = userTeamPos.UserId, TeamId = userTeamPos.TeamId };
            return PartialView("~/Views/Team/_DeleteUserTeamPosModalPartial.cshtml", model);
        }

        public ActionResult DeleteUserTeamPos(DeleteUserTeamPosModel model)
        {
            OperationStatus operationStatus = new OperationStatus();
            var userTeamPos = _userTeamPosManager.GetUsersTeamPosByTeamIdAndUserId(model.TeamId, model.UserId);
            if (userTeamPos != null)
            {
                _userTeamPosManager.RemoveUserTeamPos(userTeamPos);
                operationStatus.Status = true;
            }
            return Json(operationStatus, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddTeam(AddTeamModel model)
        {
            OperationStatus operationStatus = new OperationStatus();
        
            TeamDto team = new TeamDto() { Name = model.Name, Description = model.Description, DateCreated = DateTime.Now, DepartmentId = CurrentUser.DepartmentId};
            ViewBag.TeamId = team.Id;
            _teamManager.AddTeam(team);
            operationStatus.Status = true;
            if (!HttpContext.User.IsInRole("Admin"))
            {
                var positionId = _positionManager.GetTeamByName(Resources.Resource.PM).First().Id;
                var userTeamPosition = new UserTeamPositionDto() {UserId = CurrentUserId, TeamId = team.Id, PositionId = positionId};
                _userTeamPosManager.AddUserTeamPos(userTeamPosition);
            }
            return Json(operationStatus, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckName(string name)
        {
            bool res;
            var team = _teamManager.GetTeamByName(name);
            if (team != null)
                res = false;
            else res = true;
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [TeamAction]
        public ActionResult EditUserTeamPos(string userId, string teamId, string positionId)
        {
            OperationStatus operationStatus = new OperationStatus();
            var userTeamPos =
                _userTeamPosManager.GetUsersTeamPosByTeamIdAndUserId(new Guid(teamId), userId);
            if (userTeamPos != null)
            {
                userTeamPos.PositionId = new Guid(positionId);
                _userTeamPosManager.UpdateUserTeamPos(userTeamPos);
                operationStatus.Status = true;
            }
            return Json(operationStatus, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AssignTeamByProject(Guid? projectId, Guid? teamId) 
        {
            try
            {
                var project = _projectManager.GetProjectById(projectId);
                var team = _teamManager.GetTeamById(teamId);
                project.Team = team;
                _projectManager.UpdateProject(project);
                Session["MessageForTeam"] = $"The team '{team.Name}' was assigned to project '{project.Name}' successfully!";
                return RedirectToAction("Index","Project");
            }
            catch
            {
                return View("Index");
            }
            
        }
    }
}