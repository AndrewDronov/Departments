using Departments.Filters;
using Departments.Models;
using System.Collections.Generic;

namespace Departments.ViewModels
{
    public class DepartmentViewModel
    {
        public DepartmentFilter Filter { get; set; }
        
        public HashSet<Department> Data { get; set; }
    }
}