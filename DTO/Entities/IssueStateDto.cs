using System.Collections.Generic;

namespace DTO.Entities
{
    public class IssueStateDto:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<IssueDto> Issues { get; set; }
    }
}