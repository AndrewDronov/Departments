using System.ComponentModel;

namespace Departments.Filters
{
    public class DepartmentFilter
    {
        [DisplayName("Название")]
        public string Name { get; set; }
    }
}