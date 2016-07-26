using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{
    public class Project : BaseEntity
    {
        public Project()
        {
            Issues = new HashSet<Issue>();
        }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }        
        public DateTime DateCreate { get; set; }
        public Guid? StateProjectId { get; set; }
        public Guid? TeamId { get; set; }
        public Guid? DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public virtual Team Team { get; set; }
        public virtual StateProject StateProject { get; set; }
        public virtual ICollection<Issue> Issues { get; set; }
    }
}
