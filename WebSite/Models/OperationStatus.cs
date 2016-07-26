using System;

namespace WebSite.Models
{
    public class OperationStatus
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public Guid InsertedId { get; set; }
    }
}