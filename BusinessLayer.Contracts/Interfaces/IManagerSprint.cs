using System;
using System.Collections.Generic;
using DTO.Entities;

namespace BusinessLayer.Contracts.Interfaces
{
    public interface IManagerSprint
    {
        IEnumerable<SprintDto> GetAllSprints();
        void AddSprint(SprintDto sprint);
        void UpdateSprint(SprintDto sprint);
        void RemoveSprint(SprintDto sprint);
        IEnumerable<SprintDto> GetSprintsByProjectId(Guid? projectId);
        SprintDto GetSprintById(Guid? id);
        int CountSprintsByProjectId(Guid? projectId);
        IEnumerable<SprintDto> GetSprintByName(String name);

    }
}