using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace DataLayer.Configuration
{
   public class TimeTrackingConfig: EntityTypeConfiguration<TimeTracking>
    {
        public TimeTrackingConfig()
        {
            HasKey(x => x.Id);
            Property(r => r.Description).HasMaxLength(200).IsOptional();
        }
    }
}
