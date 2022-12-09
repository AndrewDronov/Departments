using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Departments.Models
{
    public class Department
    {
        [DisplayName("#")]
        public int DepartmentId { get; init; }
        [DisplayName("Название")]
        
        public string Name { get; set; }
        [DisplayName("Статус")]
        public DepartmentStatus Status { get; set; }
        
        [DisplayName("Входит в")]
        public int? ParentId { get; set; }
        
        [DisplayName("Входит в")]
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public Department Parent { get; set; }

        public override int GetHashCode()
        {
            return DepartmentId;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var p = (Department) obj;
            return DepartmentId == p.DepartmentId;
        }
    }

    public enum DepartmentStatus
    {
        Active = 1,
        Blocked = 2
    }
}