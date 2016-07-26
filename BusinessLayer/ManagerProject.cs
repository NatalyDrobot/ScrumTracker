using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Contracts.Interfaces;
using DataLayer.Entities;
using DTO.Entities;

namespace BusinessLayer
{
    //public struct TakeSkipValue
    //{
    //    public int CountOutput { get; set; }
    //    public int PageNumber { get; set; }

    //    public int Skip { get { return CountOutput * (PageNumber - 1); } }
    //    public int Take { get { return CountOutput; } }

    //    public string OrderBy { get; set; } // парсить нужно
    //}

    public class ManagerProject : BaseManager, IManagerProject
    {
        public IEnumerable<ProjectDto> GetAllProjects()
        {
            return _srv.Projects.GetList().ToArray();
        }

        public IEnumerable<ProjectDto> GetProjectByDepartmentId(Guid? id)
        {
            var list = _srv.Projects.GetWithFilter(x => x.DepartmentId == id).ToArray();
            return list.Length > 0 ? list : null;
        }
        //public IEnumerable<ProjectDto> GetAllProjects(TakeSkipValue listProject)
        //{
        //    //return ServiceDal.Projects.GetList().Skip(listProject.Skip).Take(listProject.Take).OrderBy().ToList();
        //    return _srv.Projects.GetList().Skip(listProject.Skip).Take(listProject.Take).ToArray();
        //}

        public void AddProject(ProjectDto project)
        {
            _srv.Projects.Add(project);
        }

        public void UpdateProject(ProjectDto project)
        {
            _srv.Projects.Update(project);
        }

        public void RemoveProject(ProjectDto project)
        {
            _srv.Projects.Remove(project);
        }

        public IEnumerable<ProjectDto> GetProjectByName(String name)
        {
            var listProject = _srv.Projects.GetWithFilter(x => x.Name == name).ToArray();
            return listProject.Length > 0 ? listProject : null;
        }

        public IEnumerable<ProjectDto> SearchProjectByName(String name)
        {
            var listProject = _srv.Projects.GetWithFilter(x => x.Name.Contains(name)).ToArray();
            return listProject.Length > 0 ? listProject : null;
        }

        public IEnumerable<ProjectDto> GetProjectsByTeamId(Guid? teamId)
        {
            //IQueryable<IssueDto> Issues = from 
            //var query = from i in Issues
            //            join ;
            var listProject = _srv.Projects.GetWithFilter(x => x.TeamId == teamId).ToArray();
            return listProject.Length > 0 ? listProject : null;
        }

        public ProjectDto GetProjectById(Guid? id)
        {
            return _srv.Projects.GetWithFilter(x => x.Id == id).FirstOrDefault();
        }

        // STATEPROJECT

        public void AddStateProject(StateProjectDto stateProject)
        {
            _srv.StateProjects.Add(stateProject);
        }

        public IEnumerable<StateProjectDto> GetStateProject()
        {
            return _srv.StateProjects.GetList().ToArray();
        }

        public IEnumerable<StateProjectDto> GetStateProjectByName(String name)
        {
            var listState = _srv.StateProjects.GetWithFilter(x => x.Name == name).ToArray();
            return listState.Length > 0 ? listState : null;
        }

        public IEnumerable<ProjectDto> GetUsersProject(Guid userId)
        {
            List<ProjectDto> projectList = new List<ProjectDto>();

            var teamsIds = _srv.UserTeamPositions.GetList().Where(tp => tp.User.Id == userId.ToString()).Select(tp=>tp.TeamId);

            foreach (var teamId in teamsIds )
            {
                projectList.AddRange(_srv.Projects.GetWithFilter(x=>x.TeamId == teamId));
            }
            return projectList.ToArray();
        }

        public StateProjectDto GetStateProjectById(Guid? id)
        {
            return _srv.StateProjects.GetWithFilter(x => x.Id == id).FirstOrDefault();
        }

    }
}
