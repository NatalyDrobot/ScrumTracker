using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Entities
{
    public class SprintDto : BaseEntity
    {
            public Guid ProjectId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime DateBegin { get; set; }
            public DateTime DateEnd { get; set; }
            public int State { get; set; }
        
            public virtual ProjectDto Project { get; set; }
            public virtual ICollection<IssueDto> Issues { get; set; }
    }
}
