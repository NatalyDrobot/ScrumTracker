using AutoMapper;
using DataLayer.Entities;
using DTO.Entities;

namespace DataLayer
{
    public class MapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.CreateMap<User, UserDto>();
            Mapper.CreateMap<UserDto, User>()
                .ForMember(x => x.Department, opt => opt.Ignore())
                .ForMember(x => x.UserTeamPositions, opt => opt.Ignore())
                .ForMember(x => x.Comments, opt => opt.Ignore());

            Mapper.CreateMap<Department, DepartmentDto>();
            Mapper.CreateMap<DepartmentDto, Department>();

            Mapper.CreateMap<Comment, CommentDto>();
            Mapper.CreateMap<CommentDto, Comment>()
                .ForMember(x => x.Author, opt => opt.Ignore())
                .ForMember(x => x.Issue, opt => opt.Ignore());


            Mapper.CreateMap<Team, TeamDto>();
            Mapper.CreateMap<TeamDto, Team>()
                .ForMember(x => x.Projects, opt => opt.Ignore())
                .ForMember(x => x.UserTeamPositions, opt => opt.Ignore());

            Mapper.CreateMap<Position, PositionDto>();
            Mapper.CreateMap<PositionDto, Position>()
                .ForMember(x => x.UserTeamPositions, opt => opt.Ignore());

            Mapper.CreateMap<UserTeamPosition, UserTeamPositionDto>();
            Mapper.CreateMap<UserTeamPositionDto, UserTeamPosition>()
                .ForMember(x => x.Team, opt => opt.Ignore())
                .ForMember(x => x.User, opt => opt.Ignore())
                .ForMember(x => x.Position, opt => opt.Ignore());

            Mapper.CreateMap<Project, ProjectDto>();
            Mapper.CreateMap<ProjectDto, Project>()
                .ForMember(x => x.StateProject, opt => opt.Ignore())
                .ForMember(x => x.Team, opt => opt.Ignore())
                .ForMember(x => x.Issues, opt => opt.Ignore());


            Mapper.CreateMap<StateProject, StateProjectDto>();
            Mapper.CreateMap<StateProjectDto, StateProject>()
                .ForMember(x => x.Projects, opt => opt.Ignore());

            Mapper.CreateMap<Issue, IssueDto>();
            Mapper.CreateMap<IssueDto, Issue>()
                .ForMember(x => x.Priority, opt => opt.Ignore())
                .ForMember(x => x.State, opt => opt.Ignore())
                .ForMember(x => x.Creator, opt => opt.Ignore())
                .ForMember(x => x.Assignee, opt => opt.Ignore())
                .ForMember(x => x.ParentIssue, opt => opt.Ignore())
                .ForMember(x => x.IssueType, opt => opt.Ignore())
                .ForMember(x => x.Sprint, opt => opt.Ignore())
                .ForMember(x => x.Histories, opt => opt.Ignore())
                .ForMember(x => x.Project, opt => opt.Ignore())
                .ForMember(x => x.TimeTrackings, opt => opt.Ignore());

            Mapper.CreateMap<IssueType, IssueTypeDto>();
            Mapper.CreateMap<IssueTypeDto, IssueType>()
                .ForMember(x => x.Issues, opt => opt.Ignore());

            Mapper.CreateMap<IssuePriority, IssuePriorityDto>();
            Mapper.CreateMap<IssuePriorityDto, IssuePriority>()
                .ForMember(x => x.Issues, opt => opt.Ignore());

            Mapper.CreateMap<IssueState, IssueStateDto>();
            Mapper.CreateMap<IssueStateDto, IssueState>()
                .ForMember(x => x.Issues, opt => opt.Ignore());

            Mapper.CreateMap<Sprint, SprintDto>();
            Mapper.CreateMap<SprintDto, Sprint>()
                .ForMember(x => x.Project, opt => opt.Ignore())
                .ForMember(x => x.Issues, opt => opt.Ignore());

            Mapper.CreateMap<TimeTracking, TimeTrackingDto>();
            Mapper.CreateMap<TimeTrackingDto, TimeTracking>()
                .ForMember(x => x.Issue, opt => opt.Ignore())
                .ForMember(x => x.TimeTrackingType, opt => opt.Ignore())
                .ForMember(x => x.User, opt => opt.Ignore());

            Mapper.CreateMap<TimeTrackingType, TimeTrackingTypeDto>();
            Mapper.CreateMap<TimeTrackingTypeDto, TimeTrackingType>()
                .ForMember(x => x.TimeTrackings, opt => opt.Ignore());

        }
    }
}
