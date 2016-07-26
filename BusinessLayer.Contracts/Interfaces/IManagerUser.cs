using System;
using System.Collections.Generic;
using DTO.Entities;

namespace BusinessLayer.Contracts.Interfaces
{
    public interface IManagerUser
    {
        void AddUser(UserDto user);
        void RemoveUser(UserDto user);
        void UpdateUser(UserDto user);
        UserDto GetUserById(string id);
        IEnumerable<UserDto> GetAllUsers();
        IEnumerable<UserDto> GetUsersByEmail(string email);
        UserDto Authenticate(string login, string password);
        IEnumerable<UserDto> GetAllUsersByDepartment(Guid? departmentId);
        IEnumerable<UserDto> FindUsersByEmail(string email);
    }
}