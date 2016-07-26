using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Contracts.Interfaces;
using DTO.Entities;

namespace BusinessLayer
{
   public class ManagerIssue:BaseManager,IManagerIssue
    {
        public IEnumerable<IssueDto> GetAllIssues()
        {
            return _srv.Issues.GetList().ToArray();
        }

        public void AddIssue(IssueDto issue)
        {
            _srv.Issues.Add(issue);
        }

        public void UpdateIssues(IEnumerable<IssueDto> issues)
        {
            _srv.Issues.Update(issues);
        }

        public void UpdateIssue(IssueDto issue)
        {
            _srv.Issues.Update(issue);
        }

        public void RemoveIssue(IssueDto issue)
        {
            _srv.Issues.Remove(issue);
        }

       public IssueDto GetIssueById(Guid? id)
        {
            return _srv.Issues.GetWithFilter(x => x.Id == id).FirstOrDefault();
        }

        public int CountIssuesByProjectId(Guid? projectId)
        {
            var projects = _srv.Issues.GetWithFilter(x => x.ProjectId == projectId).ToArray();
            return projects.Length;
        }

        public int CountIssuesInParentIssue(Guid sprintId, Guid issuesId)
        {
            var projects = _srv.Issues.GetWithFilter(x => x.Sprint.Id == sprintId && x.ParentIssue.Id == issuesId).ToArray();
            return projects.Length;
        }

        public IEnumerable<IssueDto> GetIssuesBySprintId(Guid? sprintId)
        {
            return _srv.Issues.GetWithFilter(x => x.SprintId == sprintId).ToArray();
        }

        public IEnumerable<IssueDto> GetAllIssuesByProjectId(Guid? projectId)
        {
            return _srv.Issues.GetWithFilter(x => x.ProjectId == projectId).ToArray();
        }

        public IEnumerable<IssuePriorityDto> GetAllIssuePriorities()
        {
            return _srv.IssuePriorities.GetList().ToArray();
        }

        public IEnumerable<IssueTypeDto> GetAllIssueTypes()
        {
            return _srv.IssueTypes.GetList().ToArray();
        }

        public IEnumerable<IssueStateDto> GetAllIssueStates()
        {
            return _srv.IssueStates.GetList().ToArray();
        }

        public IssuePriorityDto GetPriorityByName(string name)
        {
            var priorities = _srv.IssuePriorities.GetWithFilter(x => x.Name == name).ToArray();
            return priorities.Length > 0 ? priorities.First() : null;
        }

        public IssueStateDto GetStateByName(string name)
        {
            var states = _srv.IssueStates.GetWithFilter(x => x.Name == name).ToArray();
            return states.Length > 0 ? states.First() : null;
        }

        public IssueTypeDto GetTypeByName(string name)
        {
            var types = _srv.IssueTypes.GetWithFilter(x => x.Name == name).ToArray();
            return types.Length > 0 ? types.First() : null;
        }

        public int CountSprintIssuesByStateAndType(string type, string state, Guid sprintId)
        {
            return _srv.Issues.GetWithFilter(x => x.IssueType.Name.Equals(type) && x.State.Name == state && x.SprintId == sprintId).Count();
        }
        public IssueTypeDto GetTypeById(Guid? typeId)
        {
            var types = _srv.IssueTypes.GetWithFilter(x => x.Id == typeId).ToArray();
            return types.Length > 0 ? types.First() : null;
        }

        public IEnumerable<TimeTrackingTypeDto> GetAllTimeTrackingTypes()
        {
            return _srv.TimeTrackingTypes.GetList().ToArray();
        }

        public void AddTimeTracking(TimeTrackingDto timeTracking)
        {
            _srv.TimeTracking.Add(timeTracking);
        }
        public void UpdateTimeTracking(TimeTrackingDto timeTracking)
        {
            _srv.TimeTracking.Update(timeTracking);
        }

        public void RemoveTimeTracking(TimeTrackingDto timeTracking)
        {
            _srv.TimeTracking.Remove(timeTracking);
        }
        public void RemoveTimeTracking(IEnumerable<TimeTrackingDto> timeTracking)
        {
            _srv.TimeTracking.Remove(timeTracking);
        }
        public TimeTrackingDto GetTimeTrackingById(Guid? id)
        {
            return _srv.TimeTracking.GetWithFilter(x => x.Id == id).FirstOrDefault();
        }
        //public TimeTrackingDto GetUsersTimeTrackingByIssueIdAndUserId(Guid? issueId, string userId)
        //{
        //    var listTimeTrackings = _srv.TimeTracking.GetWithFilter(x => x.IssueId == issueId && x.UserId == userId).ToArray();
        //    return listTimeTrackings.Length > 0 ? listTimeTrackings.First() : null;
        //}
    }
}
