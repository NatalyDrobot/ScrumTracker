using System.Collections.Generic;

namespace DataLayer.Entities
{
    public class TimeTrackingType:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<TimeTracking> TimeTrackings { get; set; }
    }
}