using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DTO.Entities;
using WebSite.Models;

namespace WebSite.Controllers.Mappers
{
    public static class ManageMapper
    {
        public static ProfileViewModel ToProfileViewModel(this UserDto user)
        {
            return new ProfileViewModel
            {
                Id = user.Id,
                Avatar = user.Avatar,
                LastName = user.LastName,
                FirstName = user.FirstName,
                Email = user.Email,
                Department = user.Department?.Name,
                EmailConfirmed = user.EmailConfirmed
                
            };
        }

        public static IEnumerable <ProfileViewModel> ToProfileViewModel(this ICollection<UserDto> users)
        {
            return users.Select(u => new ProfileViewModel
            {
                Id = u.Id,
                Avatar = u.Avatar,
                LastName = u.LastName,
                FirstName = u.FirstName,
                Email = u.Email,
                Department = u.Department?.Name,
                EmailConfirmed = u.EmailConfirmed


            }).ToList();
        }
    }
}