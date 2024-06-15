using Microsoft.EntityFrameworkCore;
using ProductService.DataAccess;
using ProductService.DTOs;
using ProductService.Interfaces;
using System.Linq.Expressions;


namespace ProductService.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntity
    {
        private const int DefaultPageSize = 10; // Set a default page size
        protected readonly ApplicationDbContext context;

        public GenericRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public virtual async Task<int> CreateAsync(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
            return await context.SaveChangesAsync();
        }

        public virtual async Task<GenericResultDto<TEntity>> GetAllAsync(EntityPagination pagination = null)
        {
            var query = context.Set<TEntity>().Where(x => !x.IsDeleted);

            if (pagination != null && !string.IsNullOrEmpty(pagination.SortField))
            {
                var property = typeof(TEntity).GetProperty(pagination.SortField);

                if (property != null)
                {
                    query = pagination.SortOrder == "asc"
                        ? query.OrderBy(e => EF.Property<object>(e, pagination.SortField))
                        : query.OrderByDescending(e => EF.Property<object>(e, pagination.SortField));
                }
            }

            var totalCount = await query.CountAsync();

            var response = new GenericResultDto<TEntity>
            {
                TotalCount = totalCount,
                CurrentPage = pagination?.PageIndex ?? 1,
                PageSize = pagination?.PageSize ?? DefaultPageSize,
            };
            if (pagination?.PageSize > 0)
                response.Items = await query.Skip((pagination?.PageIndex - 1 ?? 0) * (pagination?.PageSize ?? DefaultPageSize))
                                  .Take(pagination?.PageSize ?? DefaultPageSize)
                                  .ToListAsync();
            else
                response.Items = await query.ToListAsync();
            return response;
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await context.Set<TEntity>().Where(predicate).Where(x => x.IsDeleted == false).ToListAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await context.Set<TEntity>().Where(x => x.IsDeleted == false && x.Id == id).FirstOrDefaultAsync();
        }

        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await context.Set<TEntity>().Where(x => x.IsDeleted == false).FirstOrDefaultAsync(predicate);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            context.Set<TEntity>().Update(entity);
            await context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            context.Set<TEntity>().Update(entity);
            await context.SaveChangesAsync();
        }
    }
}
