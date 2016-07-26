using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DTO.Entities;

namespace WebSite.Models.Issue
{
    public class IssueInfoModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateCreate { get; set; }
        public Guid? StateId { get; set; }
        public Guid? PriorityId { get; set; }
        public Guid? SprintId { get; set; }
        public Guid? IssueTypeId { get; set; }
        public Guid? ProjectId { get; set; }
        public string CreatorId { get; set; }
        public string AssigneeId { get; set; }
        public int? Estimate { get; set; }
        public int? SpentTime {
            get
            {
                if(TimeTrackings!= null && TimeTrackings.Any())
                return TimeTrackings.Sum(x => x.SpentTime);
                return 0;
            }
        }
        public int Number { get; set; }
        public Guid? ParentIssueId { get; set; }
        //public virtual ICollection<HistoryDto> Histories { get; set; }
        public IssuePriorityDto Priority { get; set; }
        public IssueStateDto State { get; set; }
        public UserDto Creator { get; set; }
        public UserDto Assignee { get; set; }
        public IssueDto ParentIssue { get; set; }
        public IssueTypeDto IssueType { get; set; }
        public SprintDto Sprint { get; set; }
        public ProjectDto Project { get; set; }
        public int CountIssues { get; set; }
        public ICollection<IssueDto> Children { get; set; }
        public string Key;
        public IEnumerable<CommentDto> CommentsList { get; set; }
        public ICollection<TimeTrackingDto> TimeTrackings { get; set; }

    }
    public class CommentViewModel
    {
        public string Avatar { get; set; }
        public string CommmentId { get; set; }
        public DateTime Date { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Comment { get; set; }
        public Guid IssueId { get; set; }


    }
}
