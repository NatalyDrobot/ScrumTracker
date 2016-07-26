using System.Collections.Generic;

namespace DTO.Entities
{
  public class StateProjectDto : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<ProjectDto> Projects { get; set; }
    }
}
