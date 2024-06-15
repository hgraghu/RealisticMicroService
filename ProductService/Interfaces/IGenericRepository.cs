using ProductService.DataAccess;
using ProductService.DTOs;
using System.Linq.Expressions;

namespace ProductService.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        public Task<int> CreateAsync(TEntity entity);
        public Task<GenericResultDto<TEntity>> GetAllAsync(EntityPagination pagination);
        public Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        public Task<TEntity> GetByIdAsync(int id);
        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        public Task UpdateAsync(TEntity entity);
        public Task DeleteAsync(TEntity entity);
    }
}
