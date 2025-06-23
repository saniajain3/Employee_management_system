using System.Linq.Expressions;

namespace EmployeeManagementSystem.Data
{
    public interface IRepository<T, TKey> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> filter);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<T?> FindByIdAsync(TKey id);
        Task AddAsync(T entity);
        void Update(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(TKey id);


        Task DeleteByPISNoAsync(long PIS_No);

        Task<int> SaveChangesAsync();
    }
}
