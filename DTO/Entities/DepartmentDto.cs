using System.Collections.Generic;

namespace DTO.Entities
{
    public class DepartmentDto:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<UserDto> Users { get; set; }
        public ICollection<DepartmentDto> Departments { get; set; }
    }
}
