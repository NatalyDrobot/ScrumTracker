using System;
using System.Collections.Generic;

namespace DTO.Entities
{
   public class ProjectDto : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime DateCreate { get; set; }
        public Guid? StateProjectsId => StateProject?.Id;
        public Guid? TeamId => Team?.Id;
        public Guid DepartmentId { get; set; }
        public DepartmentDto Department { get; set; }
        public StateProjectDto StateProject { get; set; }
        public TeamDto Team { get; set; }
       public ICollection<IssueDto> Issues { get; set; }
    }
}
