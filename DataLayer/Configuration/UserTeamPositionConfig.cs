using System.Data.Entity.ModelConfiguration;
using DataLayer.Entities;

namespace DataLayer.Configuration
{
   public class UserTeamPositionConfig: EntityTypeConfiguration<UserTeamPosition>
    {
        public UserTeamPositionConfig()
        {
            Property(r => r.UserId).IsRequired();
            Property(r => r.TeamId).IsRequired();
            Property(r => r.PositionId).IsOptional();

            //HasRequired(t => t.Team).WithMany(c => c.UserTeamPositions).HasForeignKey
            //       (t => t.TeamId).WillCascadeOnDelete(true);
        }
    }
}
