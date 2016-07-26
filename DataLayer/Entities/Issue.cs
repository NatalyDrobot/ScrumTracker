using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public class Issue:BaseEntity
    {
        public Issue()
        {
            Comments = new HashSet<Comment>();
            Histories = new HashSet<History>();
            Children = new HashSet<Issue>();
            TimeTrackings = new HashSet<TimeTracking>();
        }
        
        public string Name { get; set; }

        public string Description { get; set; }
        public DateTime DateCreate { get; set; }

        public Guid? StateId { get; set; }

        public Guid? PriorityId { get; set; }
        public Guid? SprintId { get; set; }
        public Guid? IssueTypeId { get; set; }

        public string CreatorId { get; set; }
        public Guid? ProjectId { get; set; }

        public string AssigneeId { get; set; }

        public int? Estimate { get; set; }

        public int? SpentTime { get; set; }
        public DateTime LastUpdate { get; set; }
        public Guid? ParentIssueId { get; set; }
        public int Number { get; set; }
        public virtual ICollection<History> Histories { get; set; }

        public virtual IssuePriority Priority { get; set; }

        public virtual IssueState State { get; set; }

        public virtual User Creator { get; set; }

        public virtual User Assignee { get; set; }
        public virtual Issue ParentIssue { get; set; }
        public virtual IssueType IssueType { get; set; }
        public virtual Sprint Sprint { get; set; }
        public virtual Project Project { get; set; }
        public virtual ICollection<Issue> Children { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<TimeTracking> TimeTrackings { get; set; }

    }
}
