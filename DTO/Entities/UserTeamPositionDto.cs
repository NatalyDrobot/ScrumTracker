using System;

namespace DTO.Entities
{
    public class UserTeamPositionDto 
    {
        public string UserId { get; set; }

        public Guid? TeamId { get; set; }

        public Guid? PositionId { get; set; }

        public PositionDto Position { get; set; }
        public TeamDto Team { get; set; }
        public UserDto User { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UserTeamPositionDto)obj);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = hash * 16777619 ^ UserId.GetHashCode() ^ TeamId.GetHashCode();
                return hash;
            }
        }

        public bool Equals(UserTeamPositionDto other)
        {
            return this == other;
        }
        public static bool operator ==(UserTeamPositionDto entity, UserTeamPositionDto otherEntity)
        {
            if (ReferenceEquals(entity, null) && ReferenceEquals(otherEntity, null))
            {
                return true;
            }
            if (ReferenceEquals(entity, null) || ReferenceEquals(otherEntity, null))
            {
                return false;
            }
            return entity.Team == otherEntity.Team && entity.User == otherEntity.User;
        }
        public static bool operator !=(UserTeamPositionDto entity, UserTeamPositionDto otherEntity)
        {
            return !(entity == otherEntity);
        }
    }
}
