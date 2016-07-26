using System;

namespace DataLayer.Entities
{
    public class History:BaseEntity
    {
        public Guid StickerId { get; set; }
        public string UserId { get; set; }
        public string OldState { get; set; }
        public string NewState { get; set; }
        public string TypeEvent { get; set; }
        public virtual Issue Issue { get; set; }
        public virtual User User { get; set; }
    }
}
