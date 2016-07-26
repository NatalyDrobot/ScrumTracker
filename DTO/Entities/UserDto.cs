using System;
using System.Collections.Generic;

namespace DTO.Entities
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string Avatar { get; set; }
        public bool IsActivate { get; set; }
        public bool EmailConfirmed { get; set; }
        public string SecurityStamp { get; set; }
        public string Culture { get; set; }

        public DepartmentDto Department { get; set; }

        public Guid? DepartmentId => Department?.Id;

        public ICollection<UserTeamPositionDto> UserTeamPositions { get; set; }


         public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UserDto)obj);
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

        public bool Equals(UserDto other)
        {
            return this == other;
        }
        public static bool operator ==(UserDto entity, UserDto otherEntity)
        {
            if (ReferenceEquals(entity, null) && ReferenceEquals(otherEntity, null))
            {
                return true;
            }
            if (ReferenceEquals(entity, null) || ReferenceEquals(otherEntity, null))
            {
                return false;
            }
            return entity.Id.Equals(otherEntity.Id);
        }
        public static bool operator !=(UserDto entity, UserDto otherEntity)
        {
            return !(entity == otherEntity);
        }
    }
}
