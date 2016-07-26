using System.Collections.Generic;

namespace DTO.Entities
{
    public class IssuePriorityDto:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<IssueDto> Issues { get; set; }
    }
}