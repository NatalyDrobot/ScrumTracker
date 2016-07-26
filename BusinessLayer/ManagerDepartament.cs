using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Contracts.Interfaces;
using DTO.Entities;

namespace BusinessLayer
{
    public class ManagerDepartament:BaseManager, IManagerDepartament
    {
        public void AddDepartment(DepartmentDto department)
        {
            _srv.Departments.Add(department);
        }

        public IEnumerable<DepartmentDto> GetAllDepartaments()
        {
            return _srv.Departments.GetList().ToArray();
        }
        public IEnumerable<DepartmentDto> GetDepartmentByName(String name)
        {
            var listDepartment = _srv.Departments.GetWithFilter(x => x.Name == name).ToArray();
            return listDepartment.Length > 0 ? listDepartment : null;
        }


    }
}
