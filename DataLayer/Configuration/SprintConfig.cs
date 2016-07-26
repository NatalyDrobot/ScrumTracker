using System.Data.Entity.ModelConfiguration;
using DataLayer.Entities;

namespace DataLayer.Configuration
{
    public class SprintConfig : EntityTypeConfiguration<Sprint>
    {
        public SprintConfig()
        {
            Property(r => r.Name).IsRequired().HasMaxLength(30);
            Property(r => r.Description).HasMaxLength(255);
        }
    }
}
