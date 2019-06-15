using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Enterprise.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Enterprise.DAL
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAsync();
        Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> predicate, IEnumerable<string> includeProperties = null);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetAsync(int id);
        Task<TEntity> GetAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task RemoveAsync(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties);
        Task<IQueryable<TEntity>> Include(params Expression<Func<TEntity, object>>[] includes);
    }
    public class Repository<TEntity>
        : IRepository<TEntity> where TEntity : BaseDbModel, new()
    {
        protected ApplicationDbContext _applicationDbContext;
        protected int _userId;

        public Repository(ApplicationDbContext applicationDbContext,
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor
            )
        {
            _applicationDbContext = applicationDbContext;
            _userId = String.IsNullOrEmpty(httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value) 
                ? 0 
                : Int32.Parse(httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
        public async Task<IQueryable<TEntity>> Include(params Expression<Func<TEntity, object>>[] includes)
        {
            IIncludableQueryable<TEntity, object> query = null;

            if (includes.Length > 0)
            {
                query = _applicationDbContext.Set<TEntity>().AsQueryable().Include(includes[0]);
            }
            for (int queryIndex = 1; queryIndex < includes.Length; ++queryIndex)
            {
                query = query.Include(includes[queryIndex]);
            }

            return query ?? _applicationDbContext.Set<TEntity>().AsQueryable();
        }
        public async Task AddAsync(TEntity entity)
        {
            //entity.Id = _userId;
            await _applicationDbContext.Set<TEntity>().AddAsync(entity);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.Run(() =>
            {
                return _applicationDbContext.Set<TEntity>().Where(x => x.Id == _userId).Any(predicate);
            });
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = _applicationDbContext.Set<TEntity>().AsQueryable();

            foreach (var property in includeProperties)
            {
                query = query.Include(property);
            }
            var entity = await query.ToListAsync();
            return entity;
        }
        public async Task<IEnumerable<TEntity>> GetAsync()
        {
            return await _applicationDbContext.Set<TEntity>().Where(e => e.Id == _userId).ToListAsync();
        }

        public async Task<TEntity> GetAsync(int id)
        {
            var entity = await _applicationDbContext.Set<TEntity>().FindAsync(id);
            if (entity.Id != _userId)
            {
                throw new UnauthorizedAccessException();
            }
            return entity;
        }

        public async Task<TEntity> GetAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = _applicationDbContext.Set<TEntity>().AsQueryable();

            foreach (var property in includeProperties)
            {
                query = query.Include(property);
            }

            var entity = await query.FirstOrDefaultAsync(x => x.Id == id);

            return entity;
        }

        public async Task RemoveAsync(TEntity entity)
        {
            _applicationDbContext.Set<TEntity>().Remove(entity);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> predicate, IEnumerable<string> includeProperties = null)
        {
            var query = _applicationDbContext.Set<TEntity>().AsQueryable();

            if (includeProperties != null)
            {
                foreach (var property in includeProperties)
                {
                    query = query.Include(property);
                }
            }
            return await query.Where(x => x.Id == _userId).Where(predicate).ToListAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _applicationDbContext.Set<TEntity>().Update(entity);
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
