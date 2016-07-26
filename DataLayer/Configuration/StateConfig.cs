using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace DataLayer.Configuration
{
    public class StateConfig : EntityTypeConfiguration<IssueState>
    {
        public StateConfig()
        {
            Property(d => d.Name).IsRequired().HasMaxLength(50);
            Property(d => d.Description).HasMaxLength(255);
        }
    }
}
