using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.DataAccess;
using ProductService.Interfaces;
using ProductService.Models;
using System.Linq.Expressions;
using System.Security.Claims;

namespace ProductService.Controllers
{
    public class GenericController<T> : ControllerBase, IGenericController<T> where T : class, IEntity
    {
        private readonly IGenericRepository<T> repository;
        public GenericController(IGenericRepository<T> repo)
        {
            repository = repo;
        }

        [HttpPost]
        [Authorize()]
        public virtual async Task<IActionResult> Create(T entity)
        {
            entity.CreatedBy = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            entity.CreatedAt = DateTime.Now;
            var id = await repository.CreateAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = id }, entity);
        }

        [HttpDelete]
       [Authorize()]
        public virtual async Task<IActionResult> Delete(T entity)
        {
            entity.DeletedBy = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            entity.DeletedAt = DateTime.Now;
            entity.IsDeleted = true;
            await repository.DeleteAsync(entity);
            return NoContent();
        }

        [HttpGet("firstordefault")]
       [Authorize()]
        public virtual async Task<IActionResult> FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            var entity = await repository.FirstOrDefaultAsync(predicate);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpGet("getall")]
       [Authorize()]
        public virtual async Task<IActionResult> GetAll([FromQuery] EntityPagination pagination)
        {
            try
            {
                var entities = await repository.GetAllAsync(pagination);
                return Ok(entities);
            }
            catch (Exception)
            {
                // Log the exception

                // Return a generic error response
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
       [Authorize()]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await repository.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpGet("getbyparams")]
       [Authorize()]
        public virtual async Task<IActionResult> GetAllByParam(Expression<Func<T, bool>> predicate)
        {
            var entity = await repository.FindAsync(predicate);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpPut]
       [Authorize()]
        public async Task<IActionResult> Update(T entity)
        {
            entity.UpdatedBy = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            entity.UpdatedAt = DateTime.Now;
            await repository.UpdateAsync(entity);
            return NoContent();
        }
    }

    [ApiController]
    [Route("api/v1/product")]
    public class ProductController : GenericController<Product>
    {
        public ProductController(IGenericRepository<Product> repository) : base(repository)
        {
        }
    }
}
