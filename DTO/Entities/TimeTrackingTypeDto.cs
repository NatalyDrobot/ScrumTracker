using System.Collections.Generic;

namespace DTO.Entities
{
    public class TimeTrackingTypeDto:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<TimeTrackingDto> TimeTrackings { get; set; }
    }
}