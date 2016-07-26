using System.Collections.Generic;
namespace DataLayer.Entities
{
    public class Department : BaseEntity
    {
        public Department()
        {
            Users = new HashSet<User>();
            Projects = new HashSet<Project>();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}
