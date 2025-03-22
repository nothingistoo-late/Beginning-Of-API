using System.Linq.Expressions;

namespace Final4.IRepository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<bool> AddRangeAsync(List<TEntity> entities);
        public Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null!, params Expression<Func<TEntity, object>>[] includes);
        public Task<List<TEntity>> GetAllHaveFilterAsync(Expression<Func<TEntity, bool>> predicate = null!, params Expression<Func<TEntity, object>>[] includes);
        IQueryable<TEntity> GetQueryable();
        Task<TEntity?> GetByIdAsync(object id, params Expression<Func<TEntity, object>>[] includes);
        Task<bool> Update(TEntity entity);
        Task<bool> Delete(TEntity entity);
        Task<bool> SoftRemoveRangeById(List<Guid> entitiesId);
        Task<bool> SoftRemoveRange(List<TEntity> entities);
        Task<bool> SoftRemove(TEntity entity);

    }
}
