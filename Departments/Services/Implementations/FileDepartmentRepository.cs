using Departments.Models;
using Departments.Services.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Departments.Services.Implementations
{
    public class FileDepartmentRepository : IRepository<Department>
    {
        private const string Path = "Storage/department.json";

        public FileDepartmentRepository()
        {
            if (!File.Exists(Path))
            {
                File.Create(Path).Close();
            }
        }

        public Department FindById(int id)
        {
            var departments = Fetch();
            
            return departments.FirstOrDefault(d => d.DepartmentId == id);
        }

        public HashSet<Department> GetAll()
        {
            return Fetch();
        }

        public void Create(Department item)
        {
            var departments = Fetch();
            
            departments.Add(item);

            SaveChanges(departments);
        }

        public void Update(Department item)
        {
            var departments = Fetch();
            
            departments.Remove(item);
            departments.Add(item);

            SaveChanges(departments);
        }

        public void Remove(Department item)
        {
            var departments = Fetch();
            
            departments.Remove(item);

            SaveChanges(departments);
        }
        
        private HashSet<Department> Fetch()
        {
            var json = File.ReadAllText(Path);
            
           return string.IsNullOrEmpty(json)
                ? new HashSet<Department>()
                : JsonSerializer.Deserialize<HashSet<Department>>(json);
        }

        private void SaveChanges(HashSet<Department> departments)
        {
            var json = JsonSerializer.Serialize(departments, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            });

            File.WriteAllText(Path, json, Encoding.UTF8);
        }
    }
}