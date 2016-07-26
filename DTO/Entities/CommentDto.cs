using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Entities
{
    public class CommentDto:BaseEntity
    {
        public string AuthorId { get; set; }
        public string Text { get; set; }
        public DateTime DateCreate { get; set; }
        public Guid IssueId { get; set; }
        public  IssueDto Issue { get; set; }
        public  UserDto Author { get; set; }
    }
}
