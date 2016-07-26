using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Entities
{
    public class TimeTrackingDto:BaseEntity
    {
        public string UserId { get; set; }
        public Guid IssueId { get; set; }
        public Guid? TimeTrackingTypeId { get; set; }
        public string Description { get; set; }
        public DateTime DateCreate { get; set; }
        public int SpentTime { get; set; }
        public TimeTrackingTypeDto TimeTrackingType { get; set; }
        public UserDto User { get; set; }
        public IssueDto Issue { get; set; }
    }
}
