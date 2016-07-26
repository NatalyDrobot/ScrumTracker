using DataLayer.Entities;
using DataLayer.Interfaces;
using DataLayer.Repositories;
using DTO.Entities;

namespace DataLayer.Services
{
    public class UnitOfWork 
    {
        static UnitOfWork()
        {
          MapperConfig.RegisterMappings();  
        }

        private static IRepository<User, UserDto> _usersRepo;
        private static IRepository<Department, DepartmentDto> _departmentsRepo;
        private static IRepository<Position, PositionDto> _positionsRepo;
        private static IRepository<Team, TeamDto> _teamsRepo;
        private static IRepository<UserTeamPosition, UserTeamPositionDto> _userTeamPosRepo;
        private static IRepository<Project, ProjectDto> _projectsRepo;
        private static IRepository<StateProject, StateProjectDto> _stateProjectsRepo;
        private static IRepository<Issue, IssueDto> _issuesRepo;
        private static IRepository<IssueState, IssueStateDto> _issueStatesRepo;
        private static IRepository<IssuePriority, IssuePriorityDto> _issuePrioritiesRepo;
        private static IRepository<IssueType, IssueTypeDto> _issueTypesRepo;
        private static IRepository<Sprint, SprintDto> _sprintsRepo;
        private static IRepository<Comment, CommentDto> _commentsRepo;
        private static IRepository<TimeTracking, TimeTrackingDto> _timeTrackingRepo;
        private static IRepository<TimeTrackingType, TimeTrackingTypeDto> _timeTrackingTypesRepo;



        public IRepository<User, UserDto> Users => _usersRepo ?? (_usersRepo = new Repository<User, UserDto>());
        public IRepository<Comment, CommentDto> Comments => _commentsRepo ?? (_commentsRepo = new Repository<Comment, CommentDto>());
        public IRepository<Department, DepartmentDto> Departments => _departmentsRepo ?? (_departmentsRepo = new Repository<Department, DepartmentDto>());
        public IRepository<Position, PositionDto> Positions => _positionsRepo ?? (_positionsRepo = new Repository<Position, PositionDto>());
        public IRepository<Team, TeamDto> Teams => _teamsRepo ?? (_teamsRepo = new Repository<Team, TeamDto> ());
        public IRepository<UserTeamPosition, UserTeamPositionDto> UserTeamPositions => _userTeamPosRepo ?? (_userTeamPosRepo = new Repository<UserTeamPosition, UserTeamPositionDto>());
        public IRepository<Project, ProjectDto> Projects => _projectsRepo ?? (_projectsRepo = new Repository<Project, ProjectDto>());
        public IRepository<StateProject, StateProjectDto> StateProjects => _stateProjectsRepo ?? (_stateProjectsRepo = new Repository<StateProject, StateProjectDto>());
        public IRepository<Issue, IssueDto> Issues => _issuesRepo ?? (_issuesRepo = new Repository<Issue, IssueDto>());
        public IRepository<IssueState, IssueStateDto> IssueStates => _issueStatesRepo ?? (_issueStatesRepo = new Repository<IssueState, IssueStateDto>());
        public IRepository<IssuePriority, IssuePriorityDto> IssuePriorities => _issuePrioritiesRepo ?? (_issuePrioritiesRepo = new Repository<IssuePriority, IssuePriorityDto>());
        public IRepository<IssueType, IssueTypeDto> IssueTypes => _issueTypesRepo ?? (_issueTypesRepo = new Repository<IssueType, IssueTypeDto>());
        public IRepository<Sprint, SprintDto> Sprints => _sprintsRepo ?? (_sprintsRepo = new Repository<Sprint, SprintDto>());
        public IRepository<TimeTracking, TimeTrackingDto> TimeTracking => _timeTrackingRepo ?? (_timeTrackingRepo = new Repository<TimeTracking, TimeTrackingDto>());
        public IRepository<TimeTrackingType, TimeTrackingTypeDto> TimeTrackingTypes => _timeTrackingTypesRepo ?? (_timeTrackingTypesRepo = new Repository<TimeTrackingType, TimeTrackingTypeDto>());
    }
}
