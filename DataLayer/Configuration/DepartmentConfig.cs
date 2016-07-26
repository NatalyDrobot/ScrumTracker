using DataLayer.Entities;
using System.Data.Entity.ModelConfiguration;

namespace DataLayer
{
    internal class DepartmentConfig : EntityTypeConfiguration<Department>
    {
        public DepartmentConfig()
        {
            Property(d => d.Name).IsRequired().HasMaxLength(50);
            Property(d => d.Description).HasMaxLength(255);
        }
    }
}