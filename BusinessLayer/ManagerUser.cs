using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BusinessLayer.Contracts.Interfaces;
using DTO.Entities;

namespace BusinessLayer
{

    public class ManagerUser : BaseManager, IManagerUser
    {
        public void AddUser(UserDto user)
        {
            _srv.Users.Add(user);
        }



        public void RemoveUser(UserDto user)
        {
            _srv.Users.Remove(user);
        }

        public void UpdateUser(UserDto user)
        {
            _srv.Users.Update(user);
        }

        public IEnumerable<UserDto> GetAllUsers()
        {
            return _srv.Users.GetList().ToArray();
        }

        public UserDto Authenticate(string login, string password)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserDto> GetAllUsersByDepartment(Guid? departmentId)
        {
            //return _srv.Users.GetList().ToArray();
            return _srv.Users.GetWithFilter(u => u.DepartmentId == departmentId && u.IsActivate && u.EmailConfirmed ).ToArray();
        }

        public IEnumerable<UserDto> GetUsersByEmail(String email)
        {
            var listUsers = _srv.Users.GetWithFilter(x => x.Email == email).ToArray();
            return listUsers.Length > 0 ? listUsers : null;
        }
        public IEnumerable<UserDto> FindUsersByEmail(string email)
        {
            var user = _srv.Users.GetWithFilter(u => u.Email.Contains(email)).ToArray();
            return user.Length > 0 ? user: null;
        }

        public UserDto GetUserById(string id)
        {
            return _srv.Users.GetWithFilter(x => x.Id == id).FirstOrDefault();
        }

        private string CalculateHash(string password, string salt)
        {
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(password + salt);
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] hash = algorithm.ComputeHash(saltedHashBytes);
            return Convert.ToBase64String(hash);
        }

    }
}
