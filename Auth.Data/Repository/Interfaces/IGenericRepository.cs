using System.Linq.Expressions;

namespace Auth.Data.Repository.Interfaces
{
    public interface IGenericRepository<T>
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int id);
        Task<List<T>> GetByFilter(Expression<Func<T, bool>> predicate);
        Task<T> GetFirst(Expression<Func<T, bool>> predicate);
        Task Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
        Task<bool> Any(Expression<Func<T, bool>> predicate);
    }
}
