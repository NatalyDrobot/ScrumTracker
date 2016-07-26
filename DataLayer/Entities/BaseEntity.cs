using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
    }
}
