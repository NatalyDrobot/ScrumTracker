using DataLayer.Entities;
using System.Data.Entity;
using DataLayer.Configuration;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DataLayer
{
    public  class StickersContext : IdentityDbContext<User>
    {
        static StickersContext()
        {
            Database.SetInitializer<StickersContext>(new StickersDbInitializer());
        }

        public StickersContext()
            : base("StickersConnectionString")
        { }

        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<StateProject> StateProjects { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<History> Histories { get; set; }
        public virtual DbSet<IssuePriority> IssuePriorities { get; set; }
        public virtual DbSet<Sprint> Sprints { get; set; }
        public virtual DbSet<IssueState> IssueStates { get; set; }
        public virtual DbSet<Issue> Issues { get; set; }
        public virtual DbSet<IssueType> IssueTypes { get; set; }
        public virtual DbSet<TimeTracking> TimeTrackings { get; set; }
        public virtual DbSet<TimeTrackingType> TimeTrackingTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new UserConfig());
            modelBuilder.Configurations.Add(new CommentConfig());
            modelBuilder.Configurations.Add(new DepartmentConfig());
            modelBuilder.Configurations.Add(new ProjectConfig());
            modelBuilder.Configurations.Add(new TeamConfig());
            modelBuilder.Configurations.Add(new UserTeamPositionConfig());
            modelBuilder.Configurations.Add(new IssueConfig());
            modelBuilder.Configurations.Add(new IssueTypeConfig());
            modelBuilder.Configurations.Add(new StateConfig());
            modelBuilder.Configurations.Add(new PriorityConfig());
            modelBuilder.Configurations.Add(new TimeTrackingConfig());

        }

        public static StickersContext Create()        
        {
            return  new StickersContext();
        }
    }
}
