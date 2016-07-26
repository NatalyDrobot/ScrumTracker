using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public class Team : BaseEntity
    {
        public Team()
        {
            UserTeamPositions = new HashSet<UserTeamPosition>();
            Projects = new HashSet<Project>();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid? DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public virtual ICollection<UserTeamPosition> UserTeamPositions { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}
