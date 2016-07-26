using System.Collections.Generic;

namespace DTO.Entities
{
   public class PositionDto:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<UserTeamPositionDto> UserTeamPositions { get; set; }
    }
}
