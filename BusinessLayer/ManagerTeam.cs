using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Contracts.Interfaces;
using DTO.Entities;

namespace BusinessLayer
{
    public class ManagerTeam : BaseManager, IManagerTeam
    {
        public IEnumerable<TeamDto> GetAllTeams()
        {
            return _srv.Teams.GetList().ToArray();
        }
                
        public void AddTeam(TeamDto team)
        {
            _srv.Teams.Add(team);
        }

        public void UpdateTeam(TeamDto team)
        {
            _srv.Teams.Update(team);
        }

        public void RemoveTeam(TeamDto team)
        {
            _srv.Teams.Remove(team);
        }

        public IEnumerable<TeamDto> GetTeamByName(string name)
        {
            var listUsers = _srv.Teams.GetWithFilter(x => x.Name == name).ToArray();
            return listUsers.Length > 0 ? listUsers : null;
        }

        public TeamDto GetTeamById(Guid? id)
        {
            return _srv.Teams.GetWithFilter(x => x.Id == id).FirstOrDefault();
        }

        public IEnumerable<TeamDto> GetUsersTeams(string userId)
        {
            var userListPosition =  _srv.UserTeamPositions.GetWithFilter(x => x.UserId == userId);
            List<TeamDto> teamList = new List<TeamDto>();

            foreach(var i in userListPosition)
            {
                teamList.Add(i.Team);
            }

            return teamList;
        }

        public IEnumerable<TeamDto> GetAllTeamsByDepartment(Guid? departmentId)
        {
            return _srv.Teams.GetWithFilter(u => u.DepartmentId == departmentId).ToArray();
        }
    }
}
