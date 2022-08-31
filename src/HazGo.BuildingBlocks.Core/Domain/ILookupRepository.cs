namespace HazGo.BuildingBlocks.Core.Domain
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using HazGo.BuildingBlocks.Core.Models;

    public interface ILookupRepository<TEntity>
    {
         Task<LookupRecord[]> GetLookupRecordsAsync(
             Expression<Func<TEntity, bool>> where = null);
    }
}
