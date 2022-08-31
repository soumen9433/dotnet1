namespace HazGo.BuildingBlocks.Persistence.EF
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using HazGo.BuildingBlocks.Core.Common;
    using HazGo.BuildingBlocks.Core.Domain;
    using Microsoft.EntityFrameworkCore;

    public abstract class RepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
        where TEntity : EntityBase<TPrimaryKey>
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        protected RepositoryBase(DbContext context)
        {
            _dbContext = context ?? throw new ArgumentException(nameof(context));
            _dbSet = _dbContext.Set<TEntity>();
        }

        public virtual void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Add(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public virtual void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> where = null)
        {
            return await _dbSet.AsNoTracking().AnyAsync(where);
        }

        public virtual async Task<TEntity> GetByIdAsync(TPrimaryKey id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetByIdAsync(IEnumerable<TPrimaryKey> id)
        {
            return await _dbSet.Where(x => id.Contains(x.Id)).ToArrayAsync();
        }

        public virtual void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public virtual void Update(IEnumerable<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public void SoftDelete<TSoftDeleteEntity>(TEntity entity)
            where TSoftDeleteEntity : ISoftDeleteEntity
        {
            ((ISoftDeleteEntity)entity).StatusId = EntityStatus.Deleted;
        }

        public async Task<TEntity> FindFirstOrDefaultAsync(Expression<Func<TEntity, bool>> where)
        {
            return await this._dbSet.FirstOrDefaultAsync(where);
        }

        public async Task<long> CountAsync(Expression<Func<TEntity, bool>> where)
        {
            return await _dbSet.LongCountAsync(where);
        }

        public async Task<IEnumerable<TModel>> SelectAsync<TModel>(Expression<Func<TEntity, TModel>> selector, Expression<Func<TEntity, bool>> where)
            where TModel : class
        {
            return await _dbSet.AsNoTracking().Where(where).Select(selector).ToArrayAsync();
        }

        protected async Task<LookupRecord[]> GetLookupRecords<LookupRecord>(Expression<Func<TEntity, LookupRecord>> selector, Expression<Func<TEntity, bool>> where = null)
        {
            if (where != null)
            {
                return await _dbSet.AsNoTracking().Where(where).Select(selector).ToArrayAsync();
            }
            else
            {
                return await _dbSet.AsNoTracking().Select(selector).ToArrayAsync();
            }
        }

        public IQueryable<TEntity> SearchQueryAsync(Expression<Func<TEntity, bool>> where = null)
        {
            if (where != null)
            {
                return _dbSet.AsNoTracking().Where(where);
            }
            else
            {
                return _dbSet.AsNoTracking();
            }
        }
    }
}
