using System;
using System.Collections.Generic;
using DTO.Entities;

namespace BusinessLayer.Contracts.Interfaces
{
    public interface IManagerTeam
    {
        IEnumerable<TeamDto> GetAllTeams();
        void AddTeam(TeamDto team);
        void UpdateTeam(TeamDto team);
        void RemoveTeam(TeamDto team);
        IEnumerable<TeamDto> GetTeamByName(string name);
        TeamDto GetTeamById(Guid? id);
        IEnumerable<TeamDto> GetUsersTeams(string userId);
        IEnumerable<TeamDto> GetAllTeamsByDepartment(Guid? departmentId);
    }
}