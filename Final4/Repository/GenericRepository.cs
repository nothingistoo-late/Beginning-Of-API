using Final4.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using Final4.Data;

namespace Final4.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly ApplicationDBContext _dbContext;
        public GenericRepository(
            ApplicationDBContext context
            ) 
        {
            _dbSet = context.Set<TEntity>();
            _dbContext = context;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            try
            {
                var result = await _dbSet.AddAsync(entity);
                return result.Entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AddRangeAsync(List<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            return true; // Đảm bảo return bool

        }

        public async Task<bool> Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
            return true;
        }

        public Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query.ToListAsync();
        }

        public Task<List<TEntity>> GetAllHaveFilterAsync(Expression<Func<TEntity, bool>>? predicate = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            // Khúc này là để lọc, thích lọc theo bất kì attribute nào của bất kì entity nào cũng được nha ae 
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query.ToListAsync();
        }

        //public async Task<TEntity?> GetByIdAsync(int id,
        //    params Expression<Func<TEntity, object>>[] includes)
        //{
        //    IQueryable<TEntity> query = _dbSet;
        //    if (includes != null)
        //        foreach (var include in includes)
        //            query = query.Include(include);
        //    var result = await query.FirstOrDefaultAsync(x => EF.Property<int?>(x, "Id") == id);
        //    return result;
        //}

        //public async Task<TEntity?> GetByIdAsync(Guid id, params Expression<Func<TEntity, object>>[] includes)
        //{
        //    IQueryable<TEntity> query = _dbSet;
        //    if (includes != null)
        //        foreach (var include in includes)
        //            query = query.Include(include);
        //    var result = await query.FirstOrDefaultAsync(x => EF.Property<Guid?>(x, "Id") == id);
        //    return result;
        //}

        //public async Task<TEntity?> GetByIdAsync(int? id, params Expression<Func<TEntity, object>>[] includes)
        //{
        //    IQueryable<TEntity> query = _dbSet;
        //    if (includes != null)
        //        foreach (var include in includes)
        //            query = query.Include(include);
        //    var result = await query.FirstOrDefaultAsync(x => EF.Property<int?>(x, "Id") == id);
        //    return result;
        //}
        public async Task<TEntity?> GetByIdAsync(object id, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;
            foreach (var include in includes)
                query = query.Include(include);

            if (id is int intId)
                return await query.FirstOrDefaultAsync(x => EF.Property<int>(x, "Id") == intId);

            if (id is Guid guidId)
                return await query.FirstOrDefaultAsync(x => EF.Property<Guid>(x, "Id") == guidId);

            return null;
        }
        public IQueryable<TEntity> GetQueryable()
        {
            return _dbSet;
        }

        public async Task<bool> SoftRemove(TEntity entity)
        {
            _dbSet.Update(entity);
            // await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftRemoveAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            return true;
        }

        public async Task<bool> SoftRemoveRange(List<TEntity> entities)
        {
           
            _dbSet.UpdateRange(entities);
            //  await _dbContext.SaveChangesAsync();
            return true;
        }


        public async Task<bool> SoftRemoveRangeById(List<Guid> entitiesId)
        {
            //var entities = await _dbSet.Where(e => entitiesId.Contains(e.Id)).ToListAsync();
            //_dbContext.UpdateRange(entities);
            //return true;
            throw new NotImplementedException();
        }

        public async Task<bool> Update(TEntity entity)
        {
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateRange(List<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
