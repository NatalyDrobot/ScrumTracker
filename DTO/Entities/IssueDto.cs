using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Entities
{
   public class IssueDto:BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }
        public DateTime DateCreate { get; set; }

        public Guid? StateId { get; set; }

        public Guid? PriorityId { get; set; }
        public Guid? SprintId { get; set; }
        public Guid? IssueTypeId { get; set; }
        public Guid? ProjectId { get; set; }
        public string CreatorId { get; set; }

        public string AssigneeId { get; set; }

        public int? Estimate { get; set; }

        public int? SpentTime { get; set; }

        public DateTime LastUpdate { get; set; }
        public Guid? ParentIssueId { get; set; }
        //public virtual ICollection<HistoryDto> Histories { get; set; }
        public int Number { get; set; }
        public IssuePriorityDto Priority { get; set; }

        public IssueStateDto State { get; set; }

        public UserDto Creator { get; set; }

        public UserDto Assignee { get; set; }
        public IssueDto ParentIssue { get; set; }
        public IssueTypeDto IssueType { get; set; }
        public SprintDto Sprint { get; set; }
       public ProjectDto Project { get; set; }
        public ICollection<IssueDto> Children { get; set; }
        public ICollection<TimeTrackingDto> TimeTrackings { get; set; }

    }
}
