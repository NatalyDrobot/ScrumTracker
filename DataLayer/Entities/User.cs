using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DataLayer.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DataLayer
{
    public class User: IdentityUser
    {

        public User()
        {
            UserTeamPositions = new HashSet<UserTeamPosition>();
            Comments = new HashSet<Comment>();
        }
      
        public string LastName { get; set; }
        public string Firstname { get; set; }
        public string Avatar { get; set; }
        public Guid? DepartmentId { get; set; }
        public bool IsActivate { get; set; }
        public string Culture { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<UserTeamPosition> UserTeamPositions { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

}
