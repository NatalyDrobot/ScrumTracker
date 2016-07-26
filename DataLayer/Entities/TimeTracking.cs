using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class TimeTracking : BaseEntity
    {
        
        public string UserId { get; set; }
        public Guid IssueId { get; set; }
        public Guid? TimeTrackingTypeId { get; set; }
        public string Description { get; set; }
        public DateTime DateCreate { get; set; }
        public int SpentTime { get; set; }
        public TimeTrackingType TimeTrackingType { get; set; }
        public virtual User User { get; set; }
        public virtual Issue Issue { get; set; }
    }
}
