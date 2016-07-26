using DTO.Entities;
using PagedList;

namespace WebSite.Models.Team
{
    public class TeamModel
    {
        public IPagedList<UserDto> Users { get; set; }
        public IPagedList<UserTeamPositionDto> UserTeamPositions { get; set; }
        public IPagedList<TeamDto> Teams { get; set; }
        //public bool UserPermission { get; set; }
    }
}