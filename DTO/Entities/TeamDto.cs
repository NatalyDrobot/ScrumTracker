using System;
using System.Collections.Generic;

namespace DTO.Entities
{
   public class TeamDto : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid? DepartmentId { get; set; }
        public DepartmentDto Department { get; set; }
        public virtual ICollection<UserTeamPositionDto> UserTeamPositions { get; set; }
        public virtual ICollection<ProjectDto> Projects { get; set; }
    }
}
