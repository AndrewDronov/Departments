using System.Collections.Generic;

namespace Departments.Services.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity FindById(int id);
        HashSet<TEntity> GetAll();
        void Create(TEntity item);
        void Update(TEntity item);
        void Remove(TEntity item);
    }
}