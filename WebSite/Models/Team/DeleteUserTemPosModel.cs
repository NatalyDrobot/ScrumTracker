using System;

namespace WebSite.Models.Team
{
    public class DeleteUserTeamPosModel
    {
        public string UserId { get; set; }
        public Guid? TeamId { get; set; }
    }
}