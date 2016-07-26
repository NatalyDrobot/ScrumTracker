using System.Data.Entity.ModelConfiguration;
using DataLayer.Entities;

namespace DataLayer.Configuration
{
    public class TeamConfig : EntityTypeConfiguration<Team>
    {
        public TeamConfig()
        {
            HasKey(t => t.Id);
            Property(r => r.Description).HasMaxLength(200).IsOptional();

            Property(p => p.Name).IsRequired().HasMaxLength(50);
            
        }
    }
}
