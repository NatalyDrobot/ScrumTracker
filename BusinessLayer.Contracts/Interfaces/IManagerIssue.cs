using System;
using System.Collections.Generic;
using DTO.Entities;

namespace BusinessLayer.Contracts.Interfaces
{
    public interface IManagerIssue
    {
        IEnumerable<IssueDto> GetAllIssues();
        void AddIssue(IssueDto issue);
        void UpdateIssue(IssueDto issue);
        void UpdateIssues(IEnumerable<IssueDto> issues);
        void RemoveIssue(IssueDto issue);
        IssueDto GetIssueById(Guid? id);
        int CountIssuesByProjectId(Guid? projectId);
        int CountIssuesInParentIssue(Guid sprintId, Guid issuesId);
        IEnumerable<IssueDto> GetIssuesBySprintId(Guid? sprintId);
        IEnumerable<IssueDto> GetAllIssuesByProjectId(Guid? projectId);
        IEnumerable<IssuePriorityDto> GetAllIssuePriorities();
        IEnumerable<IssueTypeDto> GetAllIssueTypes();
        IEnumerable<IssueStateDto> GetAllIssueStates();
        IssuePriorityDto GetPriorityByName(string name);
        IssueStateDto GetStateByName(string name);
        IssueTypeDto GetTypeByName(string name);
        int CountSprintIssuesByStateAndType(string type, string state, Guid sprintId);
        IssueTypeDto GetTypeById(Guid? typeId);
        IEnumerable<TimeTrackingTypeDto> GetAllTimeTrackingTypes();
        void AddTimeTracking(TimeTrackingDto timeTracking);
        void UpdateTimeTracking(TimeTrackingDto timeTracking);
        void RemoveTimeTracking(TimeTrackingDto timeTracking);
        void RemoveTimeTracking(IEnumerable<TimeTrackingDto> timeTracking);
        TimeTrackingDto GetTimeTrackingById(Guid? id);
    }
}
