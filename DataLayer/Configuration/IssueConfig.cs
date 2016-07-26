using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace DataLayer.Configuration
{
   public class IssueConfig:EntityTypeConfiguration<Issue>
    {
        public IssueConfig()
        {
            HasKey(t => t.Id);
            Property(d => d.Name).IsRequired().HasMaxLength(75);
            Property(d => d.Description).HasMaxLength(255);
        }
    }
}
