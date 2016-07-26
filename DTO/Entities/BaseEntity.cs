using System;

namespace DTO.Entities
{
    public class BaseEntity
    {
        private Guid _id;
        public Guid Id {
            get
            {
                if(_id == Guid.Empty)
                {
                    _id = Guid.NewGuid();         
                }
                return _id;
            }
            private set { _id = value; }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BaseEntity)obj);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = hash * 16777619 ^ Id.GetHashCode();
                return hash;
            }
        }

        public bool Equals(BaseEntity other)
        {
            return this == other;
        }
        public static bool operator ==(BaseEntity entity, BaseEntity otherEntity)
        {
            if (ReferenceEquals(entity, null) && ReferenceEquals(otherEntity, null))
            {
                return true;
            }
            if (ReferenceEquals(entity, null) || ReferenceEquals(otherEntity, null))
            {
                return false;
            }
            return entity.Id == otherEntity.Id;
        }
        public static bool operator !=(BaseEntity entity, BaseEntity otherEntity)
        {
            return !(entity == otherEntity);
        }
    }
}
