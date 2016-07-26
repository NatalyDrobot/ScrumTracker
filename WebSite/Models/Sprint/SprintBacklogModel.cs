using System;
using System.Collections.Generic;
using WebSite.Models.Issue;

namespace WebSite.Models.Sprint
{
    public class SprintBacklogModel
    {
        public Guid SprintId { get; set; }
        public IEnumerable<IssueInfoModel> Sprint { get; set; }
        public IEnumerable<IssueInfoModel> Backlog { get; set; }
    }
}