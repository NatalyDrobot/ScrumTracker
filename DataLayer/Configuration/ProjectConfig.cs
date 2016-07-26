using System.Data.Entity.ModelConfiguration;
using DataLayer.Entities;

namespace DataLayer.Configuration
{
    public class ProjectConfig : EntityTypeConfiguration<Project>
    {
        public ProjectConfig()
        {
            Property(r => r.Name).IsRequired().HasMaxLength(100);
            Property(r => r.Description).HasMaxLength(255);
            Property(r => r.DateCreate).HasColumnType("datetime2");
            Property(r => r.StateProjectId).IsOptional();
            Property(r => r.TeamId).IsOptional();

            HasOptional(t => t.Team).WithMany(c => c.Projects).HasForeignKey
                   (t => t.TeamId).WillCascadeOnDelete(true);
        }
    }
}
