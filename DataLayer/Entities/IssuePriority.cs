using System.Collections.Generic;

namespace DataLayer.Entities
{
    public  class IssuePriority:BaseEntity
    {
        public IssuePriority()
        {
            Issues = new HashSet<Issue>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Issue> Issues { get; set; }
    }
}
