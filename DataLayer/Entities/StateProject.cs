using System.Collections.Generic;

namespace DataLayer.Entities
{
    public class StateProject : BaseEntity
    {
        public StateProject()
        {
            Projects = new HashSet<Project>();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
    }
}
