using Departments.Models;
using System.Threading.Tasks;

namespace Departments.Services.Interfaces
{
    public interface IRandomStatusService
    {
        public Task<DepartmentStatus?> GetRandomStatusAsync();
    }
}