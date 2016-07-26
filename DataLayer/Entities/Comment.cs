using System;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities
{
    public class Comment :BaseEntity
    {
        public string AuthorId { get; set; }

        public string Text { get; set; }

        public DateTime DateCreate { get; set; }

        public Guid IssueId { get; set; }

        public virtual Issue Issue { get; set; }

        public virtual User Author { get; set; }
    }
}
