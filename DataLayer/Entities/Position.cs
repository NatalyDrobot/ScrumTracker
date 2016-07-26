using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
   public class Position:BaseEntity
    {
        public Position()
        {
            UserTeamPositions = new HashSet<UserTeamPosition>();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? TeamId { get; set; }
        public virtual Team Team { get; set; }
        public virtual ICollection<UserTeamPosition> UserTeamPositions { get; set; }
    }
}
