using System;
using System.Collections.Generic;

namespace DataLayer.Entities
{

    public class Sprint:BaseEntity
    {
        public Sprint()
        {
            Issues = new HashSet<Issue>();
        }
        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public int State { get; set; }
        public virtual Project Project { get; set; }
        public virtual ICollection<Issue> Issues { get; set; }
    }
}
