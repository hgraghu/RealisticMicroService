using Microsoft.AspNetCore.Mvc;
using ProductService.DataAccess;
using System.Linq.Expressions;

namespace ProductService.Interfaces
{
    public interface IGenericController<T> where T : class
    {
        Task<IActionResult> Create(T entity);
        Task<IActionResult> Delete(T entity);
        Task<IActionResult> FirstOrDefault(Expression<Func<T, bool>> predicate);
        Task<IActionResult> GetAll(EntityPagination pagination);
        Task<IActionResult> GetById(int id);
        Task<IActionResult> GetAllByParam(Expression<Func<T, bool>> predicate);
        Task<IActionResult> Update(T entity);
    }
}
