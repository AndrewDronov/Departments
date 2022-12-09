using Departments.Models;
using Departments.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Departments.Services.Implementations
{
    public class DbDepartmentRepository : IRepository<Department>
    {
        private readonly DepartmentsContext _context;

        public DbDepartmentRepository(DepartmentsContext context)
        {
            _context = context;
        }

        public Department FindById(int id)
        {
            return _context.Departments
                .Include(d => d.Parent)
                .FirstOrDefault(m => m.DepartmentId == id);
        }

        public HashSet<Department> GetAll()
        {
            return _context.Departments.ToHashSet();
        }

        public void Create(Department item)
        {
            _context.Add(item);
            _context.SaveChanges();
        }

        public void Update(Department item)
        {
            _context.Update(item);
            _context.SaveChanges();
        }

        public void Remove(Department item)
        {
            _context.Remove(item);
            _context.SaveChanges();
        }
    }
}