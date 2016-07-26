using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Contracts.Interfaces;
using DTO.Entities;

namespace BusinessLayer
{
    public class ManagerSprint : BaseManager, IManagerSprint
    {
        public IEnumerable<SprintDto> GetAllSprints()
        {
            return _srv.Sprints.GetList().ToArray();
        }

        public void AddSprint(SprintDto sprint)
        {
            _srv.Sprints.Add(sprint);
        }

        public void UpdateSprint(SprintDto sprint)
        {
            _srv.Sprints.Update(sprint);
        }

        public void RemoveSprint(SprintDto sprint)
        {
            _srv.Sprints.Remove(sprint);
        }

        public IEnumerable<SprintDto> GetSprintsByProjectId(Guid? projectId)
        {
            var listSprint = _srv.Sprints.GetWithFilter(x => x.ProjectId == projectId).ToArray();
            return listSprint.Length > 0 ? listSprint : null;
        }

        public int CountSprintsByProjectId(Guid? projectId)
        {
            var sprints = _srv.Sprints.GetWithFilter(x => x.ProjectId == projectId).ToArray();
            return sprints.Length;
        }

        public SprintDto GetSprintById(Guid? id)
        {
            return _srv.Sprints.GetWithFilter(x => x.Id == id).FirstOrDefault();
        }

        public IEnumerable<SprintDto> GetSprintByName(String name)
        {
            var listSprint = _srv.Sprints.GetWithFilter(x => x.Name == name).ToArray();
            return listSprint.Length > 0 ? listSprint : null;
        }

        public IEnumerable<SprintDto> GetTeamSprints(string id)
        {
          
            var userTeams = _srv.UserTeamPositions.GetWithFilter(x => x.UserId == id);
            List<ProjectDto> projectList = new List<ProjectDto>();
            List<SprintDto> sprintList = new List<SprintDto>();

            foreach(var i in userTeams)
            {
                projectList.AddRange(_srv.Projects.GetWithFilter(p => p.TeamId == i.TeamId));
            }

            foreach(var project in projectList)
            {
                sprintList.Add(_srv.Sprints.GetWithFilter(x => x.ProjectId == project.Id).FirstOrDefault());
            }
            
            return sprintList.ToArray();
        }
    }
}
