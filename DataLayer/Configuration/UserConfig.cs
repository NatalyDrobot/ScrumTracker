using System.Data.Entity.ModelConfiguration;

namespace DataLayer
{
    internal class UserConfig : EntityTypeConfiguration<User>
    {
        public UserConfig()
        {
            ToTable("Users");
        }
    }
}