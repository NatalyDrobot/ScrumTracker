using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO.Entities;

namespace WebSite.Util
{
    public class IssuesFilter
    {
        private delegate IEnumerable<IssueDto> FilterDelegate(IEnumerable<IssueDto> issues, Guid? sprintId);
        private readonly Dictionary<string, FilterDelegate> _filters;
        private readonly string _userId ;
        public IssuesFilter(string userId)
        {
            _userId = userId;
            _filters = new Dictionary<string, FilterDelegate>
            {
                {"in-progress", GetInProgressIssues},
                {"open", GetOpenIssues},
                {"my", GetMyIssues},
                {"all", GetAllIssues},
                {"verified", GetVerifiedIssues},
                {"fixed", GetFixedIssues}
            };
        }

        public IEnumerable<IssueDto> Execute(IEnumerable<IssueDto> issues, Guid? sprintId, string sortFilter)
        {
            return _filters[sortFilter](issues, sprintId);
        }

        private IEnumerable<IssueDto> GetOpenIssues(IEnumerable<IssueDto> issues, Guid? sprintId)
        {
            issues = sprintId != null ? issues.Where(x => x.State.Name == "Open" && x.SprintId == sprintId)
                                        : issues.Where(x => x.State.Name == "Open");
            return issues;
        }
        private IEnumerable<IssueDto> GetInProgressIssues(IEnumerable<IssueDto> issues, Guid? sprintId)
        {
            issues = sprintId != null ? issues.Where(x => x.State.Name == "In Progress" && x.SprintId == sprintId)
                                        : issues.Where(x => x.State.Name == "In Progress");
            return issues;
        }
        private IEnumerable<IssueDto> GetVerifiedIssues(IEnumerable<IssueDto> issues, Guid? sprintId)
        {
            issues = sprintId != null ? issues.Where(x => x.State.Name == "Verified" && x.SprintId == sprintId)
                                        : issues.Where(x => x.State.Name == "Verified");
            return issues;
        }
        private IEnumerable<IssueDto> GetFixedIssues(IEnumerable<IssueDto> issues, Guid? sprintId)
        {
            issues = sprintId != null ? issues.Where(x => x.State.Name == "Fixed" && x.SprintId == sprintId)
                                        : issues.Where(x => x.State.Name == "Fixed");
            return issues;
        }
        private IEnumerable<IssueDto> GetMyIssues(IEnumerable<IssueDto> issues, Guid? sprintId)
        {
            issues = issues.Where(x => x.AssigneeId == _userId);
            return issues;
        }
        private IEnumerable<IssueDto> GetAllIssues(IEnumerable<IssueDto> issues, Guid? sprintId)
        {
            issues = sprintId != null ? issues.Where(x => x.SprintId == sprintId)
                                        : issues;
            return issues;
        }
    
    }
}