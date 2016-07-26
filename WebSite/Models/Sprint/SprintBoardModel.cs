using System;
using System.Collections.Generic;
using WebSite.Models.Issue;

namespace WebSite.Models.Sprint
{
    public class SprintBoardModel
    {
        public Guid SprintId { get; set; }
        public Guid? ParentIssueId { get; set; }
        public bool IsTaskOrBug { get; set; }
        public IEnumerable<IssueInfoModel> IssueOpen { get; set; }
        public IEnumerable<IssueInfoModel> IssueReopened { get; set; }
        public IEnumerable<IssueInfoModel> IssueInProgress { get; set; }
        public IEnumerable<IssueInfoModel> IssueFixed { get; set; }
        public IEnumerable<IssueInfoModel> IssueVerified { get; set; }
    }
}