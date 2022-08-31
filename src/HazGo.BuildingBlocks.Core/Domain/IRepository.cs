namespace HazGo.BuildingBlocks.Core.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Generic repository interface")]
    public interface IRepository<TEntity, TPrimaryKey>
        where TEntity : EntityBase<TPrimaryKey>
    {
        void Add(TEntity entity);

        void Add(IEnumerable<TEntity> entities);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        void Delete(IEnumerable<TEntity> entities);

        void SoftDelete<TSoftDeleteEntity>(TEntity entity)
            where TSoftDeleteEntity : ISoftDeleteEntity;

        Task<TEntity> GetByIdAsync(TPrimaryKey id);

        Task<IEnumerable<TEntity>> GetByIdAsync(IEnumerable<TPrimaryKey> id);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> where = null);

        Task<TEntity> FindFirstOrDefaultAsync(Expression<Func<TEntity, bool>> where);

        Task<long> CountAsync(Expression<Func<TEntity, bool>> where);

        Task<IEnumerable<TModel>> SelectAsync<TModel>(Expression<Func<TEntity, TModel>> selector, Expression<Func<TEntity, bool>> where)
            where TModel : class;

        IQueryable<TEntity> SearchQueryAsync(Expression<Func<TEntity, bool>> where = null);
    }
}
