using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities
{
    public class UserTeamPosition 
    {
        [Key, Column(Order = 0)]
        public string UserId { get; set; }
        [Key, Column(Order = 1)]
        public Guid TeamId { get; set; }
        public Guid? PositionId { get; set; }

        public virtual Position Position { get; set; }
        public virtual Team Team { get; set; }
        public virtual User User { get; set; }
    }
}
