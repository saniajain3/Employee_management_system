using EmployeeManagementSystem.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
namespace EmployeeManagementSystem.Data
{
    public class Repository<T,TKey> : IRepository<T,TKey> where T : class
    {
        private readonly AppDbContext dbContext;
        protected DbSet<T> dbSet;
        public Repository(AppDbContext dbContext) {
            dbSet= dbContext.Set<T>();
            this.dbContext = dbContext;
        }
        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task DeleteAsync(TKey id)
        {
            var entity = await FindByIdAsync(id);
            dbSet.Remove(entity);
        }

        public async Task<T> FindByIdAsync(TKey id)
        {
            var entity = await dbSet.FindAsync(id);
            return entity;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            if (typeof(T) == typeof(Department))
            {
                var departments = await dbContext.Departments
                    .Include(d => d.Employees)
                    .ToListAsync();

                return departments as IEnumerable<T>;
            }

            return await dbContext.Set<T>().ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAll(Expression<Func<T,bool>>filter)
        {
            if (typeof(T) == typeof(Department))
            {
                var departments = await dbContext.Departments
                    .Include(d => d.Employees)
                    .Where(filter as Expression<Func<Department, bool>>)
                    .ToListAsync();

                return departments as IEnumerable<T>;
            }

            return await dbContext.Set<T>().AsQueryable().Where(filter).ToListAsync();
        }   


        public async Task<int> SaveChangesAsync()
        {
            return(await dbContext.SaveChangesAsync());
        }


        public void Update(T entity)
        {
            dbSet.Update(entity);

        }
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.AnyAsync(predicate);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task UpdateAsync(T entity)
        {
            dbSet.Update(entity);
            await Task.CompletedTask; 
        }
        public async Task DeleteByPISNoAsync(long PIS_No)
        {
            if (typeof(T) == typeof(Employee))
            {
                var employee = await dbContext.Set<Employee>().FirstOrDefaultAsync(e => e.PIS_No == PIS_No);
                if (employee != null)
                {
                    dbContext.Set<Employee>().Remove(employee);
                }
            }
        }


    }
}
