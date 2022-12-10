using Departments.Filters;
using Departments.Models;
using Departments.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Departments.Services.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IRepository<Department> _fileRepository;
        private readonly IRepository<Department> _dbRepository;
        private readonly ILogger<DepartmentService> _logger;

        public DepartmentService(IEnumerable<IRepository<Department>> repositories, ILogger<DepartmentService> logger)
        {
            _logger = logger;
            _fileRepository = repositories.SingleOrDefault(s => s.GetType() == typeof(FileDepartmentRepository));
            _dbRepository = repositories.SingleOrDefault(s => s.GetType() == typeof(DbDepartmentRepository));
        }

        public Department FindById(int departmentId)
        {
            Department department = null;

            try
            {
                department = _dbRepository.FindById(departmentId);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

            return department;
        }
        
        public HashSet<Department> GetAll(DepartmentFilter filter = null)
        {
            try
            {   
                //In real project, we should pass query to repository before fetching all data
                var departments = _dbRepository.GetAll();
                
                if (filter != null)
                {
                    if (!string.IsNullOrEmpty(filter.Name))
                    {
                        departments = departments.Where(d => d.Name.Contains(filter.Name)).ToHashSet();
                    }
                }

                return departments;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

            return new HashSet<Department>();
        }

        public bool Create(Department department)
        {
            try
            {
                _dbRepository.Create(department);
                // If we need to create model in file repository, uncomment code below.
                //_fileRepository.Create(department);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }

            return true;
        }

        public bool Update(Department department)
        {
            try
            {
                _dbRepository.Update(department);
                // If we need to update model in file repository, uncomment code below.
                //_fileRepository.Update(department);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }

            return true;
        }

        public bool Remove(Department department)
        {
            try
            {
                _dbRepository.Remove(department);

                // If we need to remove model from file repository, uncomment code below.
                //_fileRepository.Remove(department);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }

            return true;
        }

        public void Synchronize()
        {
            var fileDepartments = _fileRepository.GetAll();
            var dbDepartments = _dbRepository.GetAll();

            foreach (var fileDepartment in fileDepartments)
            {
                if (!dbDepartments.TryGetValue(fileDepartment, out var dbDepartment))
                {
                    try
                    {
                        _dbRepository.Create(fileDepartment);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);
                        continue;
                    }

                    // set id in file repository from db
                    _fileRepository.Update(fileDepartment);
                }
                else
                {
                    if (fileDepartment.ParentId != dbDepartment.ParentId)
                    {
                        try
                        {
                            dbDepartment.ParentId = fileDepartment.ParentId;
                            _dbRepository.Update(dbDepartment);
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e.Message);
                        }
                    }
                }
            }
        }
    }
}