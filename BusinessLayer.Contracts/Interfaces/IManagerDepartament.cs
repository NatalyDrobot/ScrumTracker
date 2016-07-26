using System;
using System.Collections.Generic;
using DTO.Entities;

namespace BusinessLayer.Contracts.Interfaces
{
    public interface IManagerDepartament
    {
        IEnumerable<DepartmentDto> GetAllDepartaments();

        IEnumerable<DepartmentDto> GetDepartmentByName(String name);

        void AddDepartment(DepartmentDto department);
    }
}