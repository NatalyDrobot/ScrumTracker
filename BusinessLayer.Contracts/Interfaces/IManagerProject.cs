using System;
using System.Collections.Generic;
using DTO.Entities;

namespace BusinessLayer.Contracts.Interfaces
{
    public interface IManagerProject
    {
        IEnumerable<ProjectDto> GetAllProjects();
        void AddProject(ProjectDto project);
        void UpdateProject(ProjectDto project);
        void RemoveProject(ProjectDto project);
        IEnumerable<ProjectDto> GetProjectByName(String name);
        ProjectDto GetProjectById(Guid? id);
        void AddStateProject(StateProjectDto stateProject);
        IEnumerable<StateProjectDto> GetStateProject();
        IEnumerable<StateProjectDto> GetStateProjectByName(String name);
        StateProjectDto GetStateProjectById(Guid? id);
        IEnumerable<ProjectDto> SearchProjectByName(String name);
        IEnumerable<ProjectDto> GetProjectsByTeamId(Guid? teamId);
        IEnumerable<ProjectDto> GetProjectByDepartmentId(Guid? id);
        IEnumerable<ProjectDto> GetUsersProject(Guid userId);
    }
}