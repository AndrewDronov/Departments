using Departments.Filters;
using Departments.Models;
using System.Collections.Generic;

namespace Departments.Services.Interfaces
{
    public interface IDepartmentService
    {
        public Department FindById(int departmentId);
        
        public HashSet<Department> GetAll(DepartmentFilter filter = null);
        
        public bool Create(Department department);
        
        public bool Update(Department department);
        
        public bool Remove(Department department);
        void Synchronize();
    }
}